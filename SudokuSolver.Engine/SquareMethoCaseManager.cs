using System.Collections.Generic;

namespace SudokuSolver.Engine
{
    public sealed class SquareMethodCaseManager
    {
        SquareMethodCaseManager() { }

        /// <summary>
        /// Gets sample case; Square method - Mixed
        /// </summary>
        /// <returns></returns>
        public static Sudoku Plain
        {
            get
            {
                var sudoku = new Sudoku()
                {
                    Title = "Square method"
                };

                // Update
                sudoku.UpdateSquare(2, 2);
                sudoku.UpdateSquare(4, 3);
                sudoku.UpdateSquare(10, 4);
                sudoku.UpdateSquare(11, 5);
                sudoku.UpdateSquare(12, 6);
                sudoku.UpdateSquare(28, 7);
                sudoku.UpdateSquare(31, 8);
                sudoku.UpdateSquare(34, 9);

                return sudoku;
            }
        }

        /// <summary>
        /// Gets sample case; Four Birds (Square method - a.k.a. Beauty)
        /// </summary>
        /// <returns></returns>
        public static Sudoku FourBirds
        {
            get
            {
                var sudoku = new Sudoku()
                {
                    Title = "Four Birds",
                    Description = "Update Id 5 with Number 5 for the final touch"
                };

                // Update
                sudoku.UpdateSquare(2, 2);
                sudoku.UpdateSquare(4, 4);
                sudoku.UpdateSquare(6, 6);
                sudoku.UpdateSquare(8, 8);

                sudoku.UpdateSquare(10, 7);
                sudoku.UpdateSquare(12, 9);
                sudoku.UpdateSquare(16, 1);
                sudoku.UpdateSquare(18, 3);

                sudoku.UpdateSquare(28, 3);
                sudoku.UpdateSquare(30, 1);
                sudoku.UpdateSquare(34, 9);
                sudoku.UpdateSquare(36, 7);

                return sudoku;
            }
        }

        /// <summary>
        /// Fifth five; an extended version of four birds
        /// </summary>
        public static Sudoku FifthFive
        {
            get
            {
                var sudoku = new Sudoku()
                {
                    Title = "Fifth five",
                    Description = "Update and clear Id 5 to see whether it says as hint"
                };

                // Update
                sudoku.UpdateSquare(2, 2);
                sudoku.UpdateSquare(4, 4);
                sudoku.UpdateSquare(6, 6);
                sudoku.UpdateSquare(8, 8);

                sudoku.UpdateSquare(10, 7);
                sudoku.UpdateSquare(12, 9);
                sudoku.UpdateSquare(16, 1);
                sudoku.UpdateSquare(18, 3);

                sudoku.UpdateSquare(28, 3);
                sudoku.UpdateSquare(30, 1);
                sudoku.UpdateSquare(34, 9);
                sudoku.UpdateSquare(36, 7);

                sudoku.UpdateSquare(11, 5);
                sudoku.UpdateSquare(26, 5);

                return sudoku;
            }
        }
    }
}
