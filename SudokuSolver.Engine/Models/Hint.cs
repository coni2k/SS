using System.Globalization;
namespace SudokuSolver.Engine
{
    public class Hint
    {
        #region - Properties -

        public HintType HintType { get; set; }

        public GroupNumber GroupNumberSource { get; set; }

        #endregion

        #region - Constructors -

        #endregion

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "T: {0} - GNS: {1}", HintType.ToString()[0], GroupNumberSource);
        }
    }
}