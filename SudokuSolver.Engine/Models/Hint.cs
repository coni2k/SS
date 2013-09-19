using System.Globalization;
namespace SudokuSolver.Engine
{
    public class Hint
    {
        #region - Properties -

        public Square Square { get; set; }

        public HintMethod HintType { get; set; }

        public GroupNumber GroupNumberSource { get; set; }

        #endregion

        #region - Constructors -

        public Hint(Square square, HintMethod type, GroupNumber groupNumbersource)
        {
            Square = square;
            HintType = type;
            GroupNumberSource = groupNumbersource;
        }

        #endregion

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "S: {0} - T: {1} - GNS: {2}", Square, HintType.ToString()[0], GroupNumberSource);
        }
    }
}