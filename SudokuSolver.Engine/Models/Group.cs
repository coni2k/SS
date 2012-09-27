using System.Linq;
using System.Collections.Generic;

namespace SudokuSolver.Engine
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

        internal delegate void GroupSquareEventHandler(Group group, Square square);

        internal event GroupSquareEventHandler SquareNumberChanging;
        internal event GroupSquareEventHandler SquareNumberChanged;
        internal event GroupSquareEventHandler SquareAvailabilityChanged;

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

        internal IEnumerable<Number> UsedNumbers
        {
            get { return UsedSquares.Select(s => s.Number); }
        }

        internal IEnumerable<Number> AvailableNumbers
        {
            get { return Sudoku.GetNumbersExceptZero().Except(UsedNumbers); }
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

        internal void SetSquare(Square square)
        {
            square.NumberChanging += new Square.SquareEventHandler(Square_NumberChanging);
            square.NumberChanged += new Square.SquareEventHandler(Square_NumberChanged);
            square.AvailabilityChanged += new Square.SquareEventHandler(Square_AvailabilityChanged);
            _Squares.Add(square);
        }

        /// <summary>
        /// Makes the old number of the changing square available in the related squares in the group
        /// </summary>
        /// <param name="square"></param>
        void Square_NumberChanging(Square square)
        {
            if (SquareNumberChanging != null)
                SquareNumberChanging(this, square);
        }

        void Square_NumberChanged(Square square)
        {
            if (SquareNumberChanged != null)
                SquareNumberChanged(this, square);
        }

        void Square_AvailabilityChanged(Square square)
        {
            if (SquareAvailabilityChanged != null)
                SquareAvailabilityChanged(this, square);

            // Check for potential square removal

            var potentialList = Sudoku.GetPotentialSquares().Where(p => (p.SquareGroup != null && p.SquareGroup.Equals(this)) && p.PotentialType == PotentialTypes.Group);

            if (potentialList.Count() > 0)
            {
                foreach (var p in potentialList)
                {
                    var list = AvailableSquares.Where(s => s.Availabilities.Any(a => a.Number.Equals(p.Number) && a.IsAvailable));

                    if (!list.Count().Equals(1))
                    {
                        System.Diagnostics.Debug.WriteLine("Group.Square_AvailabilityChanged found a potential to be REMOVED - Id: {0} - Value: {1}", p.Square.Id.ToString(), p.Number.Value.ToString());
                    }
                }
            }

            // Check for potential square

            foreach (var number in AvailableNumbers)
            {
                var list = AvailableSquares.Where(s => s.Availabilities.Any(a => a.Number.Equals(number) && a.IsAvailable));

                if (list.Count().Equals(1))
                {
                    // TODO NEW POTENTIAL (SOLVED?) CODE HERE
                    // this.Update(item.Number., AssignTypes.Potential);
                    // this.Potential_SquareGroup = this;

                    // Get the item from the list
                    var item = list.Single();

                    if (PotentialFound != null)
                    {
                        System.Diagnostics.Debug.WriteLine("Square.Group_SquareNumberChanged found a potential");
                        PotentialFound(new Potential(item, this, number, PotentialTypes.Group));
                    }
                }
            }
        }

        // TOD IS THIS REALLY NECESSARY?
        public IEnumerable<Square> GetAvailableSquaresForNumber(Number number)
        {
            return AvailableSquares.Where(s => s.Availabilities.Any(a => a.Number.Equals(number) && a.IsAvailable));
        }

        public override string ToString()
        {
            return string.Format("Id: {0} - Type: {1}", Id.ToString(), GroupType.ToString());
        }

        #endregion
    }
}
