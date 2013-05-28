using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SudokuSolver.Engine
{
    public class Square
    {
        private ICollection<Availability> availabilities = null;

        #region Events

        public delegate void SquareEventHandler(Square square);

        internal event SquareEventHandler NumberChanging;
        internal event SquareEventHandler NumberChanged;
        internal event SquareEventHandler AvailabilityChanged;

        // TODO We will come back ye, mi friend!
        internal event Hint.FoundEventHandler HintFound;

        #endregion

        #region Properties

        /// <summary>
        /// Id of the square
        /// </summary>
        public int SquareId { get; private set; }

        /// <summary>
        /// Value of the square
        /// </summary>
        public SudokuNumber SudokuNumber { get; set; }

        /// <summary>
        /// Get the assign type of the square
        /// </summary>
        public AssignTypes AssignType { get; set; }

        /// <summary>
        /// Gets the parent sudoku class
        /// </summary>
        private Sudoku Sudoku { get; set; }

        /// <summary>
        /// Gets the groups that this square assigned to
        /// </summary>
        internal IEnumerable<Group> SquareGroups { get; private set; }

        /// <summary>
        /// Gets whether the square is available or not; if the number of the square is ZERO, it's an available one
        /// </summary>
        public bool IsAvailable
        {
            get { return SudokuNumber != null && SudokuNumber.IsZero; }
        }

        //public Hint Hint { get; set; }

        /// <summary>
        /// Holds the list of available numbers
        /// IMPORTANT This doesn't mean that this square is available or not.
        /// Even if this square has a value, this list may contain other numbers as available.
        /// The availability will only be determined by looking whether the related squares have that number or not.
        /// </summary>
        public IEnumerable<Availability> GetAvailabilities()
        {
            return availabilities;
        }

        /// <summary>
        /// Unavailable ones
        /// </summary>
        public IEnumerable<Availability> GetUsedAvailabilities()
        {
            return GetAvailabilities().Where(x => !x.IsAvailable);
            //get { return GetAvailabilities().Where(x => !x.IsAvailable); }
        }

        #endregion

        #region Constructors

        public Square() { }

        internal Square(int id, Sudoku sudoku,  Group squareTypeGroup, Group horizantalTypeGroup, Group verticalTypeGroup)
        {
            SquareId = id;
            Sudoku = sudoku;
            SudokuNumber = sudoku.GetNumbers().Single(n => n.IsZero); // Zero as initial value
            AssignType = AssignTypes.Initial;

            // Groups
            var groups = new List<Group>(3);
            groups.Add(squareTypeGroup);
            groups.Add(horizantalTypeGroup);
            groups.Add(verticalTypeGroup);
            SquareGroups = groups;

            // Set the square to the groups too (cross)
            squareTypeGroup.SetSquare(this);
            horizantalTypeGroup.SetSquare(this);
            verticalTypeGroup.SetSquare(this);

            // Register the groups' events
            foreach (var group in SquareGroups)
            {
                group.SquareNumberChanging += Group_SquareNumberChanging;
                group.SquareNumberChanged += Group_SquareNumberChanged;
                group.SquareAvailabilityChanged += Group_SquareAvailabilityChanged;
            }

            // Available numbers; assign all numbers, except zero
            availabilities = new List<Availability>(this.Sudoku.Size);
            foreach (var sudokuNumber in Sudoku.GetNumbersExceptZero())
                availabilities.Add(new Availability(this, sudokuNumber));
        }

        #endregion

        #region Methods

        internal void Update(SudokuNumber number, AssignTypes type)
        {
            // Raise an event to let the sudoku to handle the availability of the other squares for the old number
            if (NumberChanging != null)
                NumberChanging(this);

            // Assign the new number to this square & let the number know it (there is a cross reference)
            SudokuNumber = number;

            // Set the type
            AssignType = type;

            // Raise an event to let the sudoku to handle the availability of the other squares for the new number
            if (NumberChanged != null)
                NumberChanged(this);
        }

        /// <summary>
        /// Returns whether the given number is available for the square or not
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public bool IsNumberAvailable(SudokuNumber number)
        {
            return GetAvailabilities().Single(a => a.Number.Equals(number)).IsAvailable;
        }

        void Group_SquareNumberChanging(Group sourceGroup, Square sourceSquare)
        {
            // Make this number available again
            SetAvailability(sourceSquare.SudokuNumber, sourceGroup.GroupType, null);
        }

        void Group_SquareNumberChanged(Group sourceGroup, Square sourceSquare)
        {
            // Make this number unavailable
            SetAvailability(sourceSquare.SudokuNumber, sourceGroup.GroupType, sourceSquare);

            // Tell to other square in the group that this square's availability has changed
            if (AvailabilityChanged != null)
                AvailabilityChanged(this);
        }

        void Group_SquareAvailabilityChanged(Group sourceGroup, Square sourceSquare)
        {
            // If it's not available, nothing to check
            if (!IsAvailable)
                return;

            // TODO !!!

            if (Sudoku.GetHints().Any(p => p.Square.Equals(this) && p.Type == HintTypes.Square))
            {
                // Get the available numbers
                var list = GetAvailabilities().Where(a => a.IsAvailable);

                // If there is only one number left in the list, then we found a new hint
                if (list.Count() != 1)
                {
                    System.Diagnostics.Debug.WriteLine("Square.Group_SquareAvailabilityChanged found a hint to be REMOVED - Id: {0} - Value: {1}", SquareId.ToString(), SudokuNumber.Value.ToString());
                }
            }
            else
            {
                // Get the available numbers
                var list = GetAvailabilities().Where(a => a.IsAvailable);

                // If there is only one number left in the list, then we found a new hint
                if (list.Count() == 1)
                {
                    // TODO NEW HINT CODE HERE
                    // Update(item.Number., AssignTypes.Hint);
                    // Hint_SquareGroup = null;

                    // Get the item from the list
                    var item = list.Single();

                    if (HintFound != null)
                        HintFound(new Hint(this, null, item.Number, HintTypes.Square));
                }
            }
        }

        /// <summary>
        /// Updates the availability of the square
        /// The value of "source" parameter determines whether it's going to be available or not.
        /// If it's null, the number will become available for that type or the other way around.
        /// </summary>
        /// <param name="number"></param>
        /// <param name="type"></param>
        /// <param name="source"></param>
        void SetAvailability(SudokuNumber number, GroupTypes type, Square source)
        {
            // If the old value is zero, then there is nothing to do.
            // Zero value is not used in availabilities list (always available).
            if (number.IsZero)
                return;

            switch (type)
            {
                case GroupTypes.Square:
                    {
                        GetAvailabilities().Single(a => a.Number.Equals(number)).SquareTypeSource = source;
                        break;
                    }
                case GroupTypes.Horizontal:
                    {
                        GetAvailabilities().Single(a => a.Number.Equals(number)).HorizontalTypeSource = source;
                        break;
                    }
                case GroupTypes.Vertical:
                    {
                        GetAvailabilities().Single(a => a.Number.Equals(number)).VerticalTypeSource = source;
                        break;
                    }
            }
        }

        public override string ToString()
        {
            return string.Format("SudokuId: {0} - Number: {1}", SquareId, SudokuNumber.Value);
        }

        #endregion
    }

    ///// <summary>
    ///// To be used to update the square from client-side
    ///// </summary>
    //public class SquareContainer
    //{
    //    public int SquareId { get; set; }
    //    public NumberCon  { get; set; }
    //}
}