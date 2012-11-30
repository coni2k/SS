namespace SudokuSolver.Engine
{
    /// <summary>
    /// There could be two hint types;
    /// 1. In one group, if all the other squares have a value, the value of the last square becomes clear. This is square type hint.
    /// 2. In one group, if a number can be placed to only one square, because of the other squares is not available for this number, this is group type hint.
    /// </summary>
    public enum HintTypes
    {
        None,
        Square, //Second grade ?
        Group //First grade ?
    }

    /// <summary>
    /// These flags determine whether the value of the square assigned by the user or the sudoku solver
    /// </summary>
    public enum AssignTypes
    {
        Initial = 0,
        User = 1,
        Hint = 2,
        Solver = 3
    }

    public enum GroupTypes
    {
        Square, // Default group
        Horizontal,
        Vertical
    }

    // ?
    public enum SolvingMethods
    {
        Method1
    }
}
