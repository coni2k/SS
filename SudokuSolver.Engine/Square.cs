﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace OSP.SudokuSolver.Engine
{
    public class Square
    {
        #region Events

        public delegate void SquareEventHandler(Square square);
        internal delegate void AvailabilityChangedEventHandler(Square square, Number number);

        //internal delegate void GroupSquareEventHandler(Group group, Square square);

        internal event SquareEventHandler NumberChanging;
        internal event SquareEventHandler NumberChanged;

        //internal event GroupSquareEventHandler NumberChanging2;
        //internal event GroupSquareEventHandler NumberChanged2;

        internal event Potential.FoundEventHandler PotentialSquareFound;
        internal event AvailabilityChangedEventHandler AvailabilityChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Id of the square
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Value of the square
        /// </summary>
        public Number Number { get; private set; }

        /// <summary>
        /// Get the assign type of the square
        /// </summary>
        public AssignTypes AssignType { get; private set; }

        /// <summary>
        /// Gets the parent sudoku class
        /// </summary>
        private Sudoku Sudoku { get; set; }

        /// <summary>
        /// Gets the groups that this square assigned to
        /// </summary>
        public IEnumerable<Group> SquareGroups { get; private set; }

        /// <summary>
        /// Gets whether the square is available or not; if the number of the square is ZERO, it's an available one
        /// </summary>
        public bool IsAvailable
        {
            get { return Number.IsZero; }
        }

        /// <summary>
        /// Hold the list of available numbers
        /// IMPORTANT This doesn't mean that this square is available or not.
        /// Even if this square has a value, this list may contain other numbers as available.
        /// The availability will only be determined by looking whether the related squares have that number or not.
        /// </summary>
        public IEnumerable<Availability> Availabilities { get; private set; }

        //public IEnumerable<SquareContainer> RelatedSquares { get; private set; }

        #endregion

        #region Constructors

        internal Square(int id, Sudoku sudoku, Group horizantolTypeGroup, Group verticalTypeGroup, Group squareTypeGroup)
        {
            this.Id = id;
            this.Sudoku = sudoku;
            //this.Sudoku.Initialized += Sudoku_Initialized;

            // Number (zero as initial value)
            var zeroNumber = sudoku.Numbers.Single(n => n.IsZero);
            this.Number = zeroNumber;

            // Set the square to the number too (cross)
            zeroNumber.AssignSquare(this);

            // Assign type
            this.AssignType = AssignTypes.Initial;

            // Groups
            var groups = new List<Group>(3);
            groups.Add(horizantolTypeGroup);
            groups.Add(verticalTypeGroup);
            groups.Add(squareTypeGroup);
            this.SquareGroups = groups;

            // Set the square to the groups too (cross)
            horizantolTypeGroup.SetSquare(this);
            verticalTypeGroup.SetSquare(this);
            squareTypeGroup.SetSquare(this);

            // Register the groups' events
            foreach (var group in SquareGroups)
            {
                group.SquareNumberChanging += Group_SquareNumberChanging;
                group.SquareNumberChanged += Group_SquareNumberChanged;
                group.SquareAvailabilityChanged += Group_SquareAvailabilityChanged;
                // group.UpdateCompleted += Group_UpdateCompleted;
            }

            // Available numbers; assign all numbers, except zero
            var availabilities = new List<Availability>(this.Sudoku.Size);
            foreach (var sudokuNumber in this.Sudoku.NumbersExceptZero)
                availabilities.Add(new Availability(this, sudokuNumber));
            this.Availabilities = availabilities;
        }

        #endregion

        #region Methods

        //void Sudoku_Initialized()
        //{
        //    RegisterRelatedSquareEvents();
        //}

        //void RegisterRelatedSquareEvents()
        //{
        //    var relatedSquares = new List<SquareContainer>();

        //    foreach (var group in this.SquareGroups)
        //    {
        //        foreach (var relatedSquare in group.Squares)
        //        {
        //            if (relatedSquare != this)
        //            {
        //                var container = new SquareContainer(group, relatedSquare);



        //                //relatedSquare.NumberChanging += RelatedSquare_NumberChanging;
        //                //relatedSquare.NumberChanged += RelatedSquare_NumberChanged;

        //                //relatedSquare.NumberChanging += relatedSquare_NumberChanging;

        //                //relatedSquare.NumberChanged += relatedSquare_NumberChanged;

        //                relatedSquares.Add(container);
        //            }
        //        }
        //    }

        //    this.RelatedSquares = relatedSquares;
        //}

        //void RelatedSquare_NumberChanging(Square relatedSquare)
        //{
        //    this.ToggleAvailability(relatedSquare.Number, relatedSquare..GroupType, null);
        //}

        //void RelatedSquare_NumberChanged(Square relatedSquare)
        //{
        //    // TODO
        //}

        internal void Update(Number number, AssignTypes type)
        {
            // Old number; raise an event to let the sudoku to handle the availability of the other squares
            if (NumberChanging != null)
                NumberChanging(this);

            // Deassign the square from the old number
            Number.DeassignSquare(this);

            // Assign the new number to this square & let the number know it (there is a cross reference)
            Number = number;
            Number.AssignSquare(this);

            // Set the type
            AssignType = type;

            // Raise an event to let the sudoku to handle the availability of the other squares (again)
            if (NumberChanged != null)
                NumberChanged(this);
        }

        /// <summary>
        /// Returns whether the given number is available for the square or not
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public bool IsNumberAvailable(Number number)
        {
            return this.Availabilities.Single(a => a.Number.Equals(number)).IsAvailable();
        }

        /// <summary>
        /// Updates the availability of the square
        /// The value of "source" parameter determines whether it's going to be available or not.
        /// If it's null, the number will become available for that type or the other way around.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="type"></param>
        /// <param name="source"></param>
        internal void ToggleAvailability(Number number, GroupTypes type, Square source)
        {
            // If the old value is zero, then there is nothing to do.
            // Zero value is not used in availabilities list (always available).
            if (number.IsZero)
                return;

            switch (type)
            {
                case GroupTypes.Horizontal:
                    this.Availabilities.Single(a => a.Number.Equals(number)).HorizontalTypeSource = source;
                    break;

                case GroupTypes.Vertical:
                    this.Availabilities.Single(a => a.Number.Equals(number)).VerticalTypeSource = source;
                    break;

                case GroupTypes.Square:
                    this.Availabilities.Single(a => a.Number.Equals(number)).SquareTypeSource = source;
                    break;
            }

            // TODO is this check necessary?
            //if (source != null)
            //{
            //    if (ValidatePotential(this))
            //    {
            //        if (PotentialSquareFound != null)
            //            PotentialSquareFound(new Potential(this, null, this.Availabilities.Single(a => a.IsAvailable()).Number, PotentialTypes.Square));
            //    }
            //}

            if (AvailabilityChanged != null)
                AvailabilityChanged(this, number);
        }

        ///// <summary>
        ///// Validate
        ///// </summary>
        ///// <param name="square"></param>
        ///// <returns></returns>
        //internal static bool ValidatePotential(Square square)
        //{
        //    return square.IsAvailable && square.Availabilities.Count(a => a.IsAvailable()).Equals(1);
        //}

        void Group_SquareNumberChanging(Group sourceGroup, Square sourceSquare)
        {
            //if (this != sourceSquare)
            ToggleAvailability(sourceSquare.Number, sourceGroup.GroupType, null);
        }

        void Group_SquareNumberChanged(Group sourceGroup, Square sourceSquare)
        {
            //if (this != sourceSquare)
            ToggleAvailability(sourceSquare.Number, sourceGroup.GroupType, sourceSquare);
        }

        void Group_SquareAvailabilityChanged(Group sourceGroup, Square sourceSquare)
        {
            // Check for potential

            // If it's not available, return already
            if (!this.IsAvailable)
                return;

            // Get the available numbers
            var list = Availabilities.Where(a => a.IsAvailable());

            // If there is only one number left in the list, then we found a new potential
            if (list.Count().Equals(1))
            {
                // Get the item from the list
                var item = list.Single();

                if (PotentialSquareFound != null)
                {
                    System.Diagnostics.Debug.WriteLine("Square.Group_SquareNumberChanged found a potential");
                    PotentialSquareFound(new Potential(this, null, item.Number, PotentialTypes.Square));
                }
            }



        }

        public override string ToString()
        {
            return Id.ToString();
        }

        #endregion
    }
}