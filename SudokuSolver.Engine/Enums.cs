﻿namespace OSP.SudokuSolver.Engine
{
    /// <summary>
    /// There could be two potential squares;
    /// 1. In one group, if all the other squares have a value, the value of the last square becomes clear. This is square type potential.
    /// 2. In one group, if a number can be placed to only one square, because of the other squares is not available for this number, this is group type potential.
    /// </summary>
    public enum PotentialTypes
    {
        None,
        Square,
        Group
    }

    /// <summary>
    /// These flags determine whether the value of the square filled by the user or the sudoku solver
    /// </summary>
    public enum FillTypes
    {
        None,
        User,
        Solver
    }

    public enum GroupTypes
    {
        Horizontal,
        Vertical,
        Square
    }
}
