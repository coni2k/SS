namespace SudokuSolver.Engine
{
    /// <summary>
    /// Determine the state of the square
    /// </summary>
    public enum AssignTypes
    {
        Initial = 0,
        User = 1,
        SquareHint = 2,
        GroupNumberHint = 3,
        Solver = 4
    }

    /// <summary>
    /// There could be two hint types;
    /// 1. In one group, if all the other squares have a value, the value of the last square becomes clear. This is square type hint.
    /// 2. In one group, if a number can be placed to only one square, because of the other squares is not available for this number, this is group type hint.
    /// </summary>
    public enum HintTypes
    {
        Square = 0,
        GroupNumber = 1
    }

    public enum GroupTypes
    {
        Square, // Default group
        Horizontal,
        Vertical
    }
}
