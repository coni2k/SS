using System;
using System.Collections.Generic;
using System.Linq;

namespace OSP.SudokuSolver.Engine
{
    public class Square
    {
        #region Members

        private IList<Number> _AvailableNumbers = null;
        private List<Number> _AvailableNumbers2 = new List<Number>();
        private List<Group> _SquareGroups = new List<Group>(3);

        #endregion

        #region Events

        public delegate void SquareEventHandler(Square square);
        internal delegate void NumberBecameUnavailableEventHandler(Number number);

        internal event SquareEventHandler NumberChanging;
        internal event SquareEventHandler NumberChanged;
        internal event Potential.FoundEventHandler PotentialSquareFound;
        internal event NumberBecameUnavailableEventHandler NumberBecameUnavailable;

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
        public IEnumerable<Group> SquareGroups { get { return _SquareGroups; } }

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
        /// The availability will only be determined by looking whether the related squares have (are using) that number or not.
        /// </summary>
        public IEnumerable<Number> AvailableNumbers { get { return _AvailableNumbers; } }

        public List<Number> AvailableNumbers2 { get { return _AvailableNumbers2; } }

        #endregion

        #region Constructors

        internal Square(int id, Number number, Sudoku sudoku, Group horizantolGroup, Group verticalGroup, Group squareGroup)
        {
            this.Id = id;
            this.Update(number, AssignTypes.Initial);
            this.Sudoku = sudoku;

            //Groups
            _SquareGroups.Add(horizantolGroup);
            _SquareGroups.Add(verticalGroup);
            _SquareGroups.Add(squareGroup);

            //Available numbers; assign all number, except zero
            _AvailableNumbers = new List<Number>(this.Sudoku.Size);
            foreach (var availableNumber in this.Sudoku.NumbersExceptZero)
                MakeNumberAvailable(availableNumber);
        }

        #endregion

        #region Methods

        internal void Update(Number number, AssignTypes type)
        {
            //It can be null only for the first time
            if (Number != null)
            {
                //Old number; raise an event to let the sudoku to handle the availability of the other squares
                this.NumberChanging(this);

                //Deassign the square from the old number
                Number.DeassignSquare(this);
            }

            //Assign the new number to this square & let the number know it (there is a cross reference)
            Number = number;
            Number.AssignSquare(this);

            //Set the type
            AssignType = type;

            //Raise an event to let the sudoku to handle the availability of the other squares (again)
            if (NumberChanged != null)
                this.NumberChanged(this);
        }

        /// <summary>
        /// Returns whether the given number is available for the square or not
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public bool IsNumberAvailable(Number number)
        {
            return this.AvailableNumbers.Any(n => n.Equals(number));
        }

        public bool IsNumberAvailable2(Number number)
        {
            return this.AvailableNumbers2.Any(n=> n.Equals(number));
        }

        /// <summary>
        /// Makes the number AVAILABLE in this square
        /// </summary>
        /// <param name="number"></param>
        internal void MakeNumberAvailable(Number number)
        {
            if (number.IsZero)
                throw new ArgumentException("Zero value cannot be in available numbers", "number");

            if (!_AvailableNumbers.Any(n => n.Equals(number)))
                _AvailableNumbers.Add(number);
        }

        /// <summary>
        /// Makes the number UNAVAILABLE in this square
        /// </summary>
        /// <param name="number"></param>
        internal void MakeNumberUnavailable(Number number)
        {
            if (_AvailableNumbers.Any(n => n.Equals(number)))
                _AvailableNumbers.Remove(number);

            if (_AvailableNumbers2.Any(n => n.Equals(number)))
                _AvailableNumbers2.Remove(number);

            if (ValidatePotential(this))
                PotentialSquareFound(new Potential(this, null, _AvailableNumbers[0], PotentialTypes.Square));

            //Raise an event to let the squaregroup to do "potential" check
            if (!number.IsZero)
            {
                if (NumberBecameUnavailable != null)
                    NumberBecameUnavailable(number);
            }
        }

        /// <summary>
        /// Validate 
        /// </summary>
        /// <param name="square"></param>
        /// <returns></returns>
        internal static bool ValidatePotential(Square square)
        {
            return square.IsAvailable && square.AvailableNumbers.Count().Equals(1);
        }

        public override string ToString()
        {
            return Id.ToString();
        }

        #endregion
    }
}