using System.Linq;
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
            get { return _Squares.Where(s => !s.IsAvailable); }
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

        void Square_NumberChanging(Square square)
        {
            foreach (var relatedSquare in Squares)
                relatedSquare.MakeNumberAvailable(square.Number);
        }

        void Square_NumberChanged(Square square)
        {
            foreach (var relatedSquare in Squares)
                relatedSquare.MakeNumberUnavailable(square.Number);

            //Check for potential square
            foreach (var number in RelatedNumbers)
                CheckPotentialSquare(number);
        }

        void Square_NumberBecameUnavailable(Number number)
        {
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
            if (_Squares.Exists(s => s.Number.Equals(number)))
                return null;

            //Get the list of squares which are available
            var list = _Squares.Where(s => s.IsAvailable && s.AvailableNumbers.Contains(number)).ToList();

            //If there is only one, set it as potential
            if (list.Count().Equals(1))
                return list[0];

            return null;
        }

        #endregion
    }
}
