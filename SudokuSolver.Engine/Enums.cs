namespace SudokuSolver.Engine
{
    /// <summary>
    /// Determine the state of the square
    /// </summary>
    public enum AssignType
    {
        Initial = 0,
        User = 1,
        Hint = 2,
        Solver = 3
    }

    /// <summary>
    /// There could be two hint types;
    /// 1. In one group, if all the other squares have a value, the value of the last square becomes clear. This is square type hint.
    /// 2. In one group, if a number can be placed to only one square, because of the other squares is not available for this number, this is group type hint.
    /// </summary>
    public enum HintType
    {
        // None = 0,
        Square = 1,
        GroupNumberSquare = 2,
        GroupNumberHorizontal = 3,
        GroupNumberVertical = 4
    }

    public enum GroupType
    {
        Square, // Default group
        Horizontal,
        Vertical
    }
}
