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

        internal delegate void GroupSquareEventHandler(Group group, Square square);
        internal delegate void GroupEventHandler(Group group);

        internal event GroupSquareEventHandler SquareNumberChanging;
        internal event GroupSquareEventHandler SquareNumberChanged;

        internal event GroupSquareEventHandler SquareAvailabilityChanged;

        //internal event GroupEventHandler UpdateCompleted;

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

        internal IEnumerable<Number> RelatedNumbers
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

                //foreach (var s in UsedSquares)
                //{
                //    //foreach (var sg in s.SquareGroups)
                //    //{
                //    //    foreach (var fs in sg.UsedSquares)
                //    //    {
                //            if (!list.Contains(s.Number))
                //                list.Add(s.Number);
                //    //    }
                //    //}
                //}

                //return UsedSquares.Select(us => us.SquareGroups.Select(sg => sg.UsedSquares .Number);
                return list;
            }
        }

        internal IEnumerable<Number> UsedNumbers
        {
            get
            {
                return UsedSquares.Select(s => s.Number);
            }
        }

        internal IEnumerable<Number> AvailableNumbers
        {
            get
            {
                return Sudoku.NumbersExceptZero.Except(UsedNumbers);
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

            //foreach (var relatedSquare in Squares)
            //    relatedSquare.ToggleAvailability(square.Number, this.GroupType, null);
        }

        void Square_NumberChanged(Square square)
        {
            if (SquareNumberChanged != null)
                SquareNumberChanged(this, square);

            //foreach (var relatedSquare in Squares)
            //    relatedSquare.ToggleAvailability(square.Number, this.GroupType, square);

            // TODO ?!

            //if (UpdateCompleted != null)
            //    UpdateCompleted(this);

            //// Check for potential square

            //foreach (var number in AvailableNumbers)
            //{
            //    var list = AvailableSquares.Where(s => s.Availabilities.Any(a => a.Number.Equals(number) && a.IsAvailable()));

            //    if (list.Count().Equals(1))
            //    {
            //        // Get the item from the list
            //        var item = list.Single();

            //        if (PotentialFound != null)
            //        {
            //            System.Diagnostics.Debug.WriteLine("Square.Group_SquareNumberChanged found a potential");
            //            PotentialFound(new Potential(item, this, number, PotentialTypes.Group));
            //        }                
            //    }
            //}

            //var list = AvailableSquares.Where(s => s.Availabilities.Any(a => a.IsAvailable() && AvailableNumbers.Any(an => an.Equals(a.Number)))); // && a.IsAvailable())));
                //return AvailableSquares.Where(s => s.Availabilities.Any(a => a.Number.Equals(number) && a.IsAvailable()));
    
            //if (potentialSquare != null)
            //{
            //    PotentialFound(new Potential(potentialSquare, this, number, PotentialTypes.Group));
            //    return true;
            //}

            //if (list.Count().Equals(1))
            //{
            //    // Get the item from the list
            //    var item = list.Single();

            //    if (PotentialFound != null)
            //        PotentialFound(new Potential(item, this, number, PotentialTypes.Group));
            //}

            ////Check for potential square
            //foreach (var number in RelatedNumbers)
            //{
            //    if (CheckPotentialSquare(number))
            //        System.Diagnostics.Debug.WriteLine("Group.NumberChanged found a potential");
            //}
        }

        void Square_AvailabilityChanged(Square square)
        {
        //    if (CheckPotentialSquare(number))
        //        System.Diagnostics.Debug.WriteLine("Group.Square_AvailabilityChanged found a potential");

            
            if (SquareAvailabilityChanged != null)
                SquareAvailabilityChanged(this, square);

            // Check for potential square removal

            var potentialList = Sudoku.PotentialSquares.Where(p => (p.SquareGroup != null && p.SquareGroup.Equals(this)) && p.PotentialType == PotentialTypes.Group);

            if (potentialList.Count() > 0)
            {
                foreach (var p in potentialList)
                {
                    var list = AvailableSquares.Where(s => s.Availabilities.Any(a => a.Number.Equals(p.Number) && a.IsAvailable()));

                    if (!list.Count().Equals(1))
                    {
                        System.Diagnostics.Debug.WriteLine("Group.Square_AvailabilityChanged found a potential to be REMOVED - Id: {0} - Value: {1}", p.Square.Id.ToString(), p.Number.Value.ToString());
                    }
                }
            }

            // Check for potential square

            foreach (var number in AvailableNumbers)
            {
                var list = AvailableSquares.Where(s => s.Availabilities.Any(a => a.Number.Equals(number) && a.IsAvailable()));

                if (list.Count().Equals(1))
                {
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

        /// <summary>
        /// Checks for potential square
        /// </summary>
        /// <param name="number"></param>
        bool CheckPotentialSquare(Number number)
        {
            Square potentialSquare = GetPotentialSquare(number);

            //If there is a potential, raise an event to let Sudoku class to add this potential to the list
            if (potentialSquare != null)
            {
                PotentialFound(new Potential(potentialSquare, this, number, PotentialTypes.Group));
                return true;
            }

            return false;
        }

        internal Square GetPotentialSquare(Number number)
        {
            //If there is already a square which has this number, ignore this alert
            //TODO Is this really necessary?
            if (Squares.Any(s => s.Number.Equals(number)))
                return null;

            //Get the list of squares which are available
            var list = GetAvailableSquaresForNumber(number);

            //If there is only one, set it as potential
            if (list.Count().Equals(1))
                return list.Single();

            return null;
        }

        public IEnumerable<Square> GetAvailableSquaresForNumber(Number number)
        {
            return AvailableSquares.Where(s => s.Availabilities.Any(a => a.Number.Equals(number) && a.IsAvailable()));
        }

        #endregion
    }
}
