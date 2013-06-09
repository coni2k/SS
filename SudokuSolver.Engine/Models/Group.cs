using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SudokuSolver.Engine
{
    /// <summary>
    /// Group of squares (for square, horizontal, vertical type)
    /// </summary>
    public class Group
    {
        #region - Members -

        // private ICollection<Square> squares = null;
        private IEnumerable<Square> squares = null;

        #endregion

        #region - Events -

        internal delegate void GroupSquareEventHandler(Group group, Square square);

        internal event GroupSquareEventHandler SquareNumberChanging;
        internal event GroupSquareEventHandler SquareNumberChanged;
        internal event GroupSquareEventHandler SquareAvailabilityChanged;

        internal event Hint.FoundEventHandler HintFound;

        #endregion

        #region - Properties -

        /// <summary>
        /// Id of the group
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Type of the group
        /// </summary>
        public GroupTypes GroupType { get; private set; }

        /// <summary>
        /// Parent sudoku class
        /// </summary>
        private Sudoku Sudoku { get; set; }

        /// <summary>
        /// List of squares that group contains
        /// </summary>
        public IEnumerable<Square> Squares
        {
            get
            {
                return squares;
            }
            internal set
            {
                if (squares != null)
                    throw new InvalidOperationException("Squares already initialized");

                squares = value;

                foreach (var square in squares)
                {
                    square.NumberChanging += new Square.SquareEventHandler(Square_NumberChanging);
                    square.NumberChanged += new Square.SquareEventHandler(Square_NumberChanged);
                    square.AvailabilityChanged += new Square.SquareEventHandler(Square_AvailabilityChanged);
                }
            }
        }

        /// <summary>
        /// List of used (with a value) squares
        /// </summary>
        public IEnumerable<Square> UsedSquares
        {
            get { return Squares.Where(s => !s.IsAvailable); }
        }

        public IEnumerable<Square> AvailableSquares
        {
            get { return Squares.Where(s => s.IsAvailable); }
        }

        internal IEnumerable<SudokuNumber> UsedNumbers
        {
            get { return UsedSquares.Select(s => s.SudokuNumber); }
        }

        internal IEnumerable<SudokuNumber> AvailableNumbers
        {
            get { return Sudoku.GetNumbersExceptZero().Except(UsedNumbers); }
        }

        #endregion

        #region - Constructors -

        internal Group(int id, GroupTypes type, Sudoku sudoku)
        {
            Id = id;
            GroupType = type;
            Sudoku = sudoku;
            // squares = new Collection<Square>();
        }

        #endregion

        #region - Methods -

        //internal void SetSquare(Square square)
        //{
        //    square.NumberChanging += new Square.SquareEventHandler(Square_NumberChanging);
        //    square.NumberChanged += new Square.SquareEventHandler(Square_NumberChanged);
        //    square.AvailabilityChanged += new Square.SquareEventHandler(Square_AvailabilityChanged);
        //    squares.Add(square);
        //}

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

            // Sudoku.GetHints().RemoveAll(p => p.Type == HintTypes.Group && p.SquareGroup.Equals(this) && p.Number.Equals(square.SudokuNumber));

            // TODO ?!
            var hintSquares = Sudoku.GetHintSquares().ToList();
            foreach (var hintSquare in hintSquares)
            {
                // hintSquare.Update(Sudoku.ZeroNumber, AssignTypes.Initial);
            }
        }

        void Square_AvailabilityChanged(Square square)
        {
            if (SquareAvailabilityChanged != null)
                SquareAvailabilityChanged(this, square);

            // Check for hint removal
            // Sudoku.GetHints().RemoveAll(p => p.Type == HintTypes.Group && p.SquareGroup.Equals(this) && AvailableSquares.Count(s => s.GetAvailabilities().Any(a => a.Number.Equals(p.Number) && a.IsAvailable)) != 1);
            
            // TODO !?
            var hintSquares = Sudoku.GetHintSquares().ToList();
            foreach (var hintSquare in hintSquares)
            {
                // hintSquare.Update(Sudoku.ZeroNumber, AssignTypes.Initial);
            }

            // Check for hint
            foreach (var number in AvailableNumbers)
            {
                var possibleSquares = AvailableSquares.Where(s => s.GetAvailabilities().Any(a => a.Number.Equals(number) && a.IsAvailable));

                // If there is only one, we found a new hint
                if (possibleSquares.Count() == 1)
                {
                    // Get the square to be updated
                    var hintSquare = possibleSquares.Single();

                    if (HintFound != null)
                    {
                        System.Diagnostics.Debug.WriteLine("P - Id: {0} - Value: {1} - Type: Group", hintSquare.SquareId.ToString(), number.Value.ToString());
                        HintFound(new Hint(hintSquare, this, number, HintTypes.Group));
                    }

                    hintSquare.Update(number, AssignTypes.Hint);
                }
            }
        }

        // TODO IS THIS REALLY NECESSARY?
        public IEnumerable<Square> GetAvailableSquaresForNumber(SudokuNumber number)
        {
            return AvailableSquares.Where(s => s.GetAvailabilities().Any(a => a.Number.Equals(number) && a.IsAvailable));
        }

        public override string ToString()
        {
            return string.Format("Id: {0} - Type: {1}", Id, GroupType);
        }

        #endregion
    }
}
