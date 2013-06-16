using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SudokuSolver.Engine
{
    public class Square
    {
        private ICollection<Group> squareGroups = null;
        private ICollection<Availability> availabilities = null;

        #region - Events -

        public delegate void SquareEventHandler(Square square);

        internal event SquareEventHandler NumberChanging;
        // internal event SquareEventHandler AvailabilityRemoved;
        internal event SquareEventHandler NumberChanged;
        internal event SquareEventHandler AvailabilityAssigned;

        // TODO We will come back ye, mi friend!
        internal event Hint.FoundEventHandler HintFound;

        #endregion

        #region - Properties -

        /// <summary>
        /// Gets the parent sudoku class
        /// </summary>
        public Sudoku Sudoku { get; private set; }

        /// <summary>
        /// Id of the square
        /// </summary>
        public int SquareId { get; private set; }

        /// <summary>
        /// Value of the square
        /// </summary>
        public SudokuNumber SudokuNumber { get; private set; }

        /// <summary>
        /// Get the assign type of the square
        /// </summary>
        public AssignTypes AssignType { get; internal set; }

        public Group SquareTypeGroup { get; private set; }
        public Group HorizantalTypeGroup { get; private set; }
        public Group VerticalTypeGroup { get; private set; }

        /// <summary>
        /// Gets the groups that this square assigned to
        /// </summary>
        internal IEnumerable<Group> SquareGroups
        {
            get
            {
                if (squareGroups == null)
                {
                    squareGroups = new Collection<Group>();
                    squareGroups.Add(SquareTypeGroup);
                    squareGroups.Add(HorizantalTypeGroup);
                    squareGroups.Add(VerticalTypeGroup);
                }

                return squareGroups;
            }
        }

        /// <summary>
        /// Gets whether the square is available or not; if the number of the square is ZERO, it's an available one
        /// </summary>
        public bool IsAvailable
        {
            // get { return SudokuNumber != null && SudokuNumber.IsZero; }
            get { return SudokuNumber.IsZero; }
        }

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

        ///// <summary>
        ///// Unavailable ones
        ///// </summary>
        //public IEnumerable<Availability> GetUsedAvailabilities()
        //{
        //    return GetAvailabilities().Where(availability => !availability.IsAvailable);
        //    //get { return GetAvailabilities().Where(x => !x.IsAvailable); }
        //}

        #endregion

        #region - Constructors -

        internal Square(int id, Sudoku sudoku,  Group squareTypeGroup, Group horizantalTypeGroup, Group verticalTypeGroup)
        {
            SquareId = id;
            Sudoku = sudoku;
            SudokuNumber = sudoku.ZeroNumber; // Zero as initial value
            AssignType = AssignTypes.Initial;

            // Groups
            SquareTypeGroup = squareTypeGroup;
            HorizantalTypeGroup = horizantalTypeGroup;
            VerticalTypeGroup = verticalTypeGroup;

            // Available numbers; assign all numbers, except zero
            availabilities = new Collection<Availability>();
            foreach (var sudokuNumber in Sudoku.GetNumbersExceptZero())
                availabilities.Add(new Availability(this, sudokuNumber));
        }

        internal void RegisterEvents()
        {
            // Register the groups' events
            foreach (var group in SquareGroups)
            {
                group.SquareNumberChanging += Group_SquareNumberChanging;
                // group.SquareAvailabilityRemoved += Group_SquareAvailabilityChanged;
                group.SquareNumberChanged += Group_SquareNumberChanged;
                group.SquareAvailabilityChanged += CheckMethod1;
            }        
        }

        #endregion

        #region - Methods -

        internal void Update(SudokuNumber number, AssignTypes type)
        {
            // Raise an event to let the sudoku to handle the availability of the other squares for the old number
            if (NumberChanging != null)
                NumberChanging(this);

            //// d. Is it available; Checks the related squares in the related groups
            //if (!number.IsZero && SquareGroups.Any(squareGroup => squareGroup.Squares.Any(square => square.SudokuNumber.Equals(number))))
            //    throw new InvalidOperationException("Not a valid assignment, the number is already in use in one of the related groups");

            // Assign the new number to this square & let the number know it (there is a cross reference)
            SudokuNumber = number;

            // Set the type
            AssignType = type;

            // Raise an event to let the sudoku to handle the availability of the other squares for the new number
            //if (NumberChanged != null)
            //    NumberChanged(this);

            // New way
            // Set availabilities of the related squares
            foreach (var group in SquareGroups)
            {
                foreach (var square in group.Squares)
                {
                    square.SetAvailability(SudokuNumber, group.GroupType, this);

                    foreach (var squareGroup in square.SquareGroups)
                    {
                        squareGroup.SetAvailability(SudokuNumber, square);
                    }
                }
            }

            // Method 1; Check whether there is any square that has only one availability left
            
            //var squaresNeedToBeChecked = affectedGroups.SelectMany(group => group.Squares).Distinct().OrderBy(square => square.SquareId);
            //foreach (var square in squaresNeedToBeChecked)
            //    square.CheckMethod1(null, null); // Method 1

            //foreach (var square in squaresNeedToBeChecked)
            //    Console.WriteLine(square);
            // Console.WriteLine("Count: {0}", squaresNeedToBeChecked.Count());
            
            var sqrs = SquareGroups.SelectMany(group => group.Squares).Distinct();
            foreach (var sqr in sqrs)
                sqr.CheckMethod1(null, null);

            foreach (var group in SquareGroups)
            {
                group.SetAvailabilityObsolete(SudokuNumber, this);
            }

            // Get the affected groups
            var affectedGroups = SquareGroups.SelectMany(group => group.Squares.SelectMany(square => square.SquareGroups)).Distinct(); // .OrderBy(group => group.Id).ThenBy(group => (int)group.GroupType);

            // Method 2; Check whether there is any group that has only one square left for any number
            foreach (var group in affectedGroups)
            {
                // group.CheckMethod2();
                
                // group.CheckMethod2Obsolete2();

                group.CheckMethod2New();
                //group.Square_AvailabilityChanged(this); // Method 2
            }

            //foreach (var group in affectedGroups)
            //    Console.WriteLine(group);
            // Console.WriteLine("Type: {0} - Count: {1}", "", affectedGroups.Count());
        }

        /// <summary>
        /// Returns whether the given number is available for the square or not
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public bool IsNumberAvailable(SudokuNumber number)
        {
            return GetAvailabilities().Single(availability => availability.Number.Equals(number)).IsAvailable;
        }

        void Group_SquareNumberChanging(Group sourceGroup, Square sourceSquare)
        {
            // Make this number available again
            SetAvailability(sourceSquare.SudokuNumber, sourceGroup.GroupType, null);

            //if (AvailabilityRemoved != null)
            //    AvailabilityRemoved(this);
        }

        void Group_SquareNumberChanged(Group sourceGroup, Square sourceSquare)
        {
            // Make this number unavailable
            SetAvailability(sourceSquare.SudokuNumber, sourceGroup.GroupType, sourceSquare);

            // Tell to other squares in the group that this square's availability has changed
            if (AvailabilityAssigned != null)
                AvailabilityAssigned(this);
        }

        internal void CheckMethod1(Group sourceGroup, Square sourceSquare)
        {
            Sudoku.Method1Counter++;

            // If it's not available, nothing to check
            if (!IsAvailable)
                return;

            // TODO !!!

            if (Sudoku.GetHints().Any(hint => hint.Square.Equals(this) && hint.Type == HintTypes.Square))
            {
                // Get the available numbers
                var list = GetAvailabilities().Where(availability => availability.IsAvailable);

                // If there is only one number left in the list, then we found a new hint
                if (list.Count() != 1)
                {
                    System.Diagnostics.Debug.WriteLine("Square.Group_SquareAvailabilityChanged found a hint to be REMOVED - Id: {0} - Value: {1}", SquareId.ToString(), SudokuNumber.Value.ToString());
                }
            }
            else
            {
                // Get the available numbers
                var availabilities = GetAvailabilities().Where(availability => availability.IsAvailable);

                // If there is only one number left in the list, then we found a new hint
                if (availabilities.Count() == 1)
                {
                    // TODO NEW HINT CODE HERE
                    // Update(item.Number., AssignTypes.Hint);
                    // Hint_SquareGroup = null;

                    // Get the item from the list
                    var availability = availabilities.Single();

                    if (HintFound != null)
                        HintFound(new Hint(this, null, availability.Number, HintTypes.Square));
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
        internal void SetAvailability(SudokuNumber number, GroupTypes type, Square source)
        {
            // If the old value is zero, then there is nothing to do.
            // Zero value is not used in availabilities list (always available).
            if (number.IsZero)
                return;

            switch (type)
            {
                case GroupTypes.Square:
                    {
                        GetAvailabilities().Single(availability => availability.Number.Equals(number)).SquareTypeSource = source;
                        break;
                    }
                case GroupTypes.Horizontal:
                    {
                        GetAvailabilities().Single(availability => availability.Number.Equals(number)).HorizontalTypeSource = source;
                        break;
                    }
                case GroupTypes.Vertical:
                    {
                        GetAvailabilities().Single(availability => availability.Number.Equals(number)).VerticalTypeSource = source;
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
}