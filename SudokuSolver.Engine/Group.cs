﻿using System.Linq;
using System.Collections.Generic;

namespace OSP.SudokuSolver.Engine
{
    /// <summary>
    /// Group of squares (for horizontal, vertical and square groups)
    /// </summary>
    public class Group
    {
        #region Members

        private List<Square> _Squares = null;

        #endregion

        #region Events

        internal event Potential.FoundEventHandler PotentialFound;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the id of the group
        /// Every type has it's own set of Ids; horizontal 1, 2 and vertical 1, 2..
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the type of the group
        /// </summary>
        public GroupTypes GroupType { get; private set; }

        /// <summary>
        /// Gets the parent sudoku class
        /// </summary>
        private Sudoku Sudoku { get; set; }

        /// <summary>
        /// Gets the list of squares which this group is holding
        /// </summary>
        public IEnumerable<Square> Squares { get { return _Squares; } }

        /// <summary>
        /// Gets the list of used squares
        /// </summary>
        public IEnumerable<Square> UsedSquares
        {
            get { return Squares.Where(s => !s.IsAvailable); }
        }

        public IEnumerable<Square> AvailableSquares
        {
            get { return Squares.Where(s => s.IsAvailable); }
        }

        internal List<Number> RelatedNumbers
        {
            get
            {
                //TODO We have to be able to find these numbers with a more proper way ?!
                var list = new List<Number>();

                foreach (var s in Squares)
                {
                    foreach (var sg in s.SquareGroups)
                    {
                        foreach (var fs in sg.UsedSquares)
                        {
                            if (!list.Contains(fs.Number))
                                list.Add(fs.Number);
                        }
                    }
                }

                return list;
            }
        }

        #endregion

        #region Constructors

        internal Group(int id, GroupTypes type, Sudoku sudoku)
        {
            Id = id;
            GroupType = type;
            Sudoku = sudoku;
            _Squares = new List<Square>(Sudoku.Size);
        }

        #endregion

        #region Methods

        internal void AddSquare(Square square)
        {
            square.NumberChanging += new Square.SquareEventHandler(Square_NumberChanging);
            square.NumberChanged += new Square.SquareEventHandler(Square_NumberChanged);
            square.NumberBecameUnavailable += new Square.NumberBecameUnavailableEventHandler(Square_NumberBecameUnavailable);
            _Squares.Add(square);
        }

        /// <summary>
        /// Makes the old number of the changing square available in the related squares in the group
        /// </summary>
        /// <param name="square"></param>
        void Square_NumberChanging(Square square)
        {
            //// If the old value is zero, then there is nothing to do.
            //// Zero value is not used in availabilities list (always available).
            //if (!square.Number.IsZero)
            //{
            //    foreach (var relatedSquare in Squares)
            //        relatedSquare.MakeNumberAvailable(square.Number);
            //}

            //avail - new
            foreach (var relatedSquare in Squares)
                relatedSquare.ToggleAvailability(square.Number, this.GroupType, null);
        }

        void Square_NumberChanged(Square square)
        {
            //foreach (var relatedSquare in Squares)
            //{
            //    relatedSquare.MakeNumberUnavailable(square.Number);
            //}

            //avail - new
            foreach (var relatedSquare in Squares)
                relatedSquare.ToggleAvailability(square.Number, this.GroupType, square);

            //Check for potential square
            foreach (var number in RelatedNumbers)
                CheckPotentialSquare(number);
        }

        void Square_NumberBecameUnavailable(Number number)
        {
            //foreach (var square in Squares)
            //{
            //    if (square.IsNumberAvailable(number) && !square.AvailableNumbers2.Contains(number))
            //        square.AvailableNumbers2.Add(number);
            //}

            CheckPotentialSquare(number);
        }

        /// <summary>
        /// Checks for potential square
        /// </summary>
        /// <param name="number"></param>
        void CheckPotentialSquare(Number number)
        {
            Square potentialSquare = GetPotentialSquare(number);

            //If there is a potential, raise an event to let Sudoku class to add this potential to the list
            if (potentialSquare != null)
                PotentialFound(new Potential(potentialSquare, this, number, PotentialTypes.Group));
        }

        internal Square GetPotentialSquare(Number number)
        {
            //If there is already a square which has this number, ignore this alert
            //TODO Is this really necessary?
            if (Squares.Any(s => s.Number.Equals(number)))
                return null;

            //Get the list of squares which are available
            //var list = Squares.Where(s => s.IsAvailable && s.AvailableNumbers.Any(n => n.Equals(number))).ToList();
            //var list = GetAvailableSquaresForNumber(number);
            var list = GetAvailableSquaresForNumberNew(number);

            //If there is only one, set it as potential
            if (list.Count().Equals(1))
                return list.First();

            return null;
        }

        //public IEnumerable<Square> GetAvailableSquaresForNumber(Number number)
        //{
        //    return AvailableSquares.Where(s => s.AvailableNumbers.Any(n => n.Equals(number)));
        //}

        public IEnumerable<Square> GetAvailableSquaresForNumberNew(Number number)
        {
            return AvailableSquares.Where(s => s.Availabilities.Any(a => a.Number.Equals(number) && a.IsAvailable())); //)); //   .AvailableNumbers.Any(n => n.Equals(number)));
        }

        #endregion
    }
}
