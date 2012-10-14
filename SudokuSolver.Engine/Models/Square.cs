using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SudokuSolver.Engine
{
    public class Square
    {
        private ICollection<Availability> _Availabilities = null;

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
        internal IEnumerable<Group> SquareGroups { get; private set; }

        /// <summary>
        /// Gets whether the square is available or not; if the number of the square is ZERO, it's an available one
        /// </summary>
        public bool IsAvailable
        {
            get { return Number.IsZero; }
        }

        /// <summary>
        /// Holds the list of available numbers
        /// IMPORTANT This doesn't mean that this square is available or not.
        /// Even if this square has a value, this list may contain other numbers as available.
        /// The availability will only be determined by looking whether the related squares have that number or not.
        /// </summary>
        public IEnumerable<Availability> GetAvailabilities()
        {
            return _Availabilities;
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

        internal Square(int id, Sudoku sudoku, Group horizantolTypeGroup, Group verticalTypeGroup, Group squareTypeGroup)
        {
            this.Id = id;
            this.Sudoku = sudoku;
            this.Number = sudoku.GetNumbers().Single(n => n.IsZero); // Zero as initial value
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
            }

            // Available numbers; assign all numbers, except zero
            _Availabilities = new List<Availability>(this.Sudoku.Size);
            foreach (var sudokuNumber in this.Sudoku.GetNumbersExceptZero())
                _Availabilities.Add(new Availability(this, sudokuNumber));
        }

        #endregion

        #region Methods

        internal void Update(Number number, AssignTypes type)
        {
            // Raise an event to let the sudoku to handle the availability of the other squares for the old number
            if (NumberChanging != null)
                NumberChanging(this);

            // Assign the new number to this square & let the number know it (there is a cross reference)
            Number = number;

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
        public bool IsNumberAvailable(Number number)
        {
            return GetAvailabilities().Single(a => a.Number.Equals(number)).IsAvailable;
        }

        void Group_SquareNumberChanging(Group sourceGroup, Square sourceSquare)
        {
            // Make this number available again
            SetAvailability(sourceSquare.Number, sourceGroup.GroupType, null);
        }

        void Group_SquareNumberChanged(Group sourceGroup, Square sourceSquare)
        {
            // Make this number unavailable
            SetAvailability(sourceSquare.Number, sourceGroup.GroupType, sourceSquare);

            // Tell to other square in the group that this square's availability has changed
            if (AvailabilityChanged != null)
                AvailabilityChanged(this);
        }

        void Group_SquareAvailabilityChanged(Group sourceGroup, Square sourceSquare)
        {
            // If it's not available, nothing to check
            if (!this.IsAvailable)
                return;

            // TODO !!!

            if (Sudoku.GetHints().Any(p => p.Square.Equals(this) && p.Type == HintTypes.Square))
            {
                // Get the available numbers
                var list = GetAvailabilities().Where(a => a.IsAvailable);

                // If there is only one number left in the list, then we found a new hint
                if (list.Count() != 1)
                {
                    System.Diagnostics.Debug.WriteLine("Square.Group_SquareAvailabilityChanged found a hint to be REMOVED - Id: {0} - Value: {1}", this.Id.ToString(), this.Number.Value.ToString());
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
                    // this.Update(item.Number., AssignTypes.Hint);
                    // this.Hint_SquareGroup = null;

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
        void SetAvailability(Number number, GroupTypes type, Square source)
        {
            // If the old value is zero, then there is nothing to do.
            // Zero value is not used in availabilities list (always available).
            if (number.IsZero)
                return;

            switch (type)
            {
                case GroupTypes.Horizontal:
                    GetAvailabilities().Single(a => a.Number.Equals(number)).HorizontalTypeSource = source;
                    break;

                case GroupTypes.Vertical:
                    GetAvailabilities().Single(a => a.Number.Equals(number)).VerticalTypeSource = source;
                    break;

                case GroupTypes.Square:
                    GetAvailabilities().Single(a => a.Number.Equals(number)).SquareTypeSource = source;
                    break;
            }
        }

        public override string ToString()
        {
            return string.Format("Id: {0} - Number: {1}", Id.ToString(), Number.Value.ToString());
        }

        #endregion
    }

    /// <summary>
    /// To be used to update the square from client-side
    /// </summary>
    public class SquareContainer
    {
        public int SquareId { get; set; }
        public int Value { get; set; }
    }
}