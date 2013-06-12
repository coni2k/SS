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

        private IEnumerable<Square> squares = null;

        #endregion

        #region - Events -

        internal delegate void GroupSquareEventHandler(Group group, Square square);

        internal event GroupSquareEventHandler SquareNumberChanging;
        // internal event GroupSquareEventHandler SquareAvailabilityRemoved;
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
                    square.AvailabilityAssigned += new Square.SquareEventHandler(Square_AvailabilityChanged);
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

            // Sudoku.GetHints().RemoveAll(p => p.Type == HintTypes.Group && p.SquareGroup.Equals(this) && p.Number.Equals(square.SudokuNumber));
        }

        //void Square_AvailabilityRemoved(Square square)
        //{
        //    if (SquareAvailabilityChanged != null)
        //        SquareAvailabilityChanged(this, square);

        //    // Check for hint removal
        //    Sudoku.GetHints().RemoveAll(p => p.Type == HintTypes.Group && p.SquareGroup.Equals(this) && AvailableSquares.Count(s => s.GetAvailabilities().Any(a => a.Number.Equals(p.Number) && a.IsAvailable)) != 1);

        //    //var hintList = Sudoku.GetHints().Where(p => (p.SquareGroup != null && p.SquareGroup.Equals(this)) && p.Type == HintTypes.Group);

        //    //if (hintList.Count() > 0)
        //    //{
        //    //    foreach (var p in hintList)
        //    //    {
        //    //        var list = AvailableSquares.Where(s => s.GetAvailabilities().Any(a => a.Number.Equals(p.Number) && a.IsAvailable));
        //    //        if (list.Count() != 1)
        //    //        {
        //    //            System.Diagnostics.Debug.WriteLine("P (Remove found) - Id: {0} - Value: {1} - Type: Group", p.Square.Id.ToString(), p.Number.Value.ToString());
        //    //        }
        //    //    }
        //    //}

        //    // Check for hint

        //    foreach (var number in AvailableNumbers)
        //    {
        //        var list = AvailableSquares.Where(s => s.GetAvailabilities().Any(a => a.Number.Equals(number) && a.IsAvailable));

        //        if (list.Count() == 1)
        //        {
        //            // TODO NEW HINT CODE HERE
        //            // Update(item.Number., AssignTypes.Hint);
        //            // Hint_SquareGroup = this;

        //            // Get the item from the list
        //            var item = list.Single();

        //            if (HintFound != null)
        //            {
        //                System.Diagnostics.Debug.WriteLine("P - Id: {0} - Value: {1} - Type: Group", item.SquareId.ToString(), number.Value.ToString());
        //                HintFound(new Hint(item, this, number, HintTypes.Group));
        //            }
        //        }
        //    }
        //}

        void Square_NumberChanged(Square changedSquare)
        {
            if (SquareNumberChanged != null)
                SquareNumberChanged(this, changedSquare);

            foreach (var square in Squares)
            {
                square.SetAvailability(changedSquare.SudokuNumber, this.GroupType, changedSquare);
            }

            var affectedGroups = Squares.SelectMany(square => square.SquareGroups); //.Distinct().OrderBy(group => group.Id).ThenBy(group => (int)group.GroupType);
            affectedGroups = affectedGroups.Distinct();
            affectedGroups = affectedGroups.OrderBy(group => group.Id);
            //affectedGroups.ToList().Sort(group => new Comparison<int>( group => group.Id, );

            foreach (var group in affectedGroups)
            {
                Console.WriteLine(group);
                // Console.WriteLine("-");
            }
            Console.WriteLine("Type: {0} - Count: {1}", GroupType, affectedGroups.Count());

            //changedSquare.SquareTypeGroup.Square_AvailabilityChanged(null);
            //changedSquare.HorizantalTypeGroup.Square_AvailabilityChanged(null);
            //changedSquare.VerticalTypeGroup.Square_AvailabilityChanged(null);

            // Availability change completed
            // CheckMethod2();

            //var list = Sudoku.GetHints().Where(p => p.Type == HintTypes.Group && p.SquareGroup.Equals(this));
            //Sudoku.GetHints().remmo .Where(p => p.Type == HintTypes.Group && p.SquareGroup.Equals(this));

            Sudoku.GetHints().RemoveAll(p => p.Type == HintTypes.Group && p.SquareGroup.Equals(this) && p.Number.Equals(changedSquare.SudokuNumber));
        }

        internal void Square_AvailabilityChanged(Square square)
        {
            if (SquareAvailabilityChanged != null)
                SquareAvailabilityChanged(this, square);

            // Check for hint removal
            Sudoku.GetHints().RemoveAll(p => p.Type == HintTypes.Group && p.SquareGroup.Equals(this) && AvailableSquares.Count(s => s.GetAvailabilities().Any(a => a.Number.Equals(p.Number) && a.IsAvailable)) != 1);

            //var hintList = Sudoku.GetHints().Where(p => (p.SquareGroup != null && p.SquareGroup.Equals(this)) && p.Type == HintTypes.Group);

            //if (hintList.Count() > 0)
            //{
            //    foreach (var p in hintList)
            //    {
            //        var list = AvailableSquares.Where(s => s.GetAvailabilities().Any(a => a.Number.Equals(p.Number) && a.IsAvailable));
            //        if (list.Count() != 1)
            //        {
            //            System.Diagnostics.Debug.WriteLine("P (Remove found) - Id: {0} - Value: {1} - Type: Group", p.Square.Id.ToString(), p.Number.Value.ToString());
            //        }
            //    }
            //}

            // Check for hint

            //foreach (var number in AvailableNumbers)
            //{
            //    var list = AvailableSquares.Where(s => s.GetAvailabilities().Any(a => a.Number.Equals(number) && a.IsAvailable));

            //    if (list.Count() == 1)
            //    {
            //        // TODO NEW HINT CODE HERE
            //        // Update(item.Number., AssignTypes.Hint);
            //        // Hint_SquareGroup = this;

            //        // Get the item from the list
            //        var item = list.Single();

            //        if (HintFound != null)
            //        {
            //            System.Diagnostics.Debug.WriteLine("P - Id: {0} - Value: {1} - Type: Group", item.SquareId.ToString(), number.Value.ToString());
            //            HintFound(new Hint(item, this, number, HintTypes.Group));
            //        }
            //    }
            //}

            // Console.WriteLine("CheckMethod2");
            CheckMethod2();

            // Sudoku.Group_Square_AvailabilityChangedCounter++;
        }

        internal void CheckMethod2()
        {
            Sudoku.Method2Counter++;

            foreach (var number in AvailableNumbers)
            {
                var list = AvailableSquares.Where(s => s.GetAvailabilities().Any(a => a.Number.Equals(number) && a.IsAvailable));

                if (list.Count() == 1)
                {
                    // TODO NEW HINT CODE HERE
                    // Update(item.Number., AssignTypes.Hint);
                    // Hint_SquareGroup = this;

                    // Get the item from the list
                    var item = list.Single();

                    if (HintFound != null)
                    {
                        System.Diagnostics.Debug.WriteLine("P - Id: {0} - Value: {1} - Type: Group", item.SquareId.ToString(), number.Value.ToString());
                        HintFound(new Hint(item, this, number, HintTypes.Group));
                    }
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
