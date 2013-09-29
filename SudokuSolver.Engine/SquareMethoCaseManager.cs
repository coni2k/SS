using System.Collections.Generic;

namespace SudokuSolver.Engine
{
    public sealed class SquareMethodCaseManager
    {
        SquareMethodCaseManager() { }

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
                sudoku.UpdateSquare(16, 1);

                sudoku.UpdateSquare(21, 9);
                sudoku.UpdateSquare(27, 3);

                sudoku.UpdateSquare(28, 3);
                sudoku.UpdateSquare(30, 1);

                sudoku.UpdateSquare(61, 9);
                sudoku.UpdateSquare(63, 7);

                return sudoku;
            }
        }

        public static Sudoku FourBirdsReverse
        {
            get
            {
                var sudoku = new Sudoku()
                {
                    Title = "Four Birds Reverse",
                    Description = "Remove Id 11 to see whether it removes the correct hints"
                };

                // Update
                sudoku.UpdateSquare(2, 2);
                sudoku.UpdateSquare(4, 4);
                sudoku.UpdateSquare(6, 6);
                sudoku.UpdateSquare(8, 8);

                sudoku.UpdateSquare(10, 7);//
                sudoku.UpdateSquare(11, 5);//
                sudoku.UpdateSquare(16, 1);//

                sudoku.UpdateSquare(21, 9);//
                sudoku.UpdateSquare(26, 5);//
                sudoku.UpdateSquare(27, 3);//

                sudoku.UpdateSquare(28, 3);//
                sudoku.UpdateSquare(30, 1);//

                sudoku.UpdateSquare(61, 9);//
                sudoku.UpdateSquare(63, 7);//

                return sudoku;
            }
        }

        public static Sudoku FourBirdsExtended
        {
            get
            {
                var sudoku = new Sudoku()
                {
                    Title = "Four Birds Extended",
                    Description = ""
                };

                // Update
                sudoku.UpdateSquare(2, 2);
                sudoku.UpdateSquare(4, 4);
                sudoku.UpdateSquare(6, 6);
                sudoku.UpdateSquare(8, 8);

                sudoku.UpdateSquare(10, 7);
                sudoku.UpdateSquare(11, 5);
                sudoku.UpdateSquare(16, 1);

                sudoku.UpdateSquare(21, 9);
                sudoku.UpdateSquare(26, 5);
                sudoku.UpdateSquare(27, 3);

                sudoku.UpdateSquare(28, 3);
                sudoku.UpdateSquare(30, 1);
                sudoku.UpdateSquare(31, 6);
                sudoku.UpdateSquare(34, 2);
                sudoku.UpdateSquare(36, 8);

                sudoku.UpdateSquare(37, 6);
                sudoku.UpdateSquare(38, 9);
                sudoku.UpdateSquare(44, 3);

                sudoku.UpdateSquare(47, 4);
                sudoku.UpdateSquare(52, 9);

                sudoku.UpdateSquare(61, 9);
                sudoku.UpdateSquare(63, 7);

                return sudoku;
            }
        }

        public static Sudoku FourBirdsExtended2
        {
            get
            {
                var sudoku = new Sudoku()
                {
                    Title = "Four Birds Extended 2",
                    Description = ""
                };

                // Update
                sudoku.UpdateSquare(2, 2);
                sudoku.UpdateSquare(4, 4);
                sudoku.UpdateSquare(6, 6);
                sudoku.UpdateSquare(8, 8);

                sudoku.UpdateSquare(10, 7);
                sudoku.UpdateSquare(11, 5);
                sudoku.UpdateSquare(16, 1);

                sudoku.UpdateSquare(21, 9);
                sudoku.UpdateSquare(26, 5);
                sudoku.UpdateSquare(27, 3);

                sudoku.UpdateSquare(28, 3);
                sudoku.UpdateSquare(30, 1);
                sudoku.UpdateSquare(31, 6);
                sudoku.UpdateSquare(32, 9);
                sudoku.UpdateSquare(34, 2);
                sudoku.UpdateSquare(36, 8);

                sudoku.UpdateSquare(37, 6);
                sudoku.UpdateSquare(38, 9);
                sudoku.UpdateSquare(44, 3);

                sudoku.UpdateSquare(47, 4);
                sudoku.UpdateSquare(52, 9);

                sudoku.UpdateSquare(61, 9);
                sudoku.UpdateSquare(63, 7);
                
                return sudoku;
            }
        }

        public static Sudoku FourBirdsFull
        {
            get
            {
                var sudoku = new Sudoku()
                {
                    Title = "Four Birds Full",
                    Description = ""
                };

                // Update
                sudoku.UpdateSquare(2, 2);
                sudoku.UpdateSquare(4, 4);
                sudoku.UpdateSquare(6, 6);
                sudoku.UpdateSquare(8, 8);

                sudoku.UpdateSquare(10, 7);
                sudoku.UpdateSquare(11, 5);
                sudoku.UpdateSquare(12, 6);
                sudoku.UpdateSquare(13, 3);
                sudoku.UpdateSquare(14, 8);
                sudoku.UpdateSquare(15, 9);
                sudoku.UpdateSquare(16, 1);
                sudoku.UpdateSquare(17, 2);
                sudoku.UpdateSquare(18, 4);

                sudoku.UpdateSquare(19, 4);
                sudoku.UpdateSquare(20, 8);
                sudoku.UpdateSquare(21, 9);
                sudoku.UpdateSquare(22, 1);
                sudoku.UpdateSquare(23, 2);
                sudoku.UpdateSquare(24, 7);
                sudoku.UpdateSquare(25, 6);
                sudoku.UpdateSquare(26, 5);
                sudoku.UpdateSquare(27, 3);

                sudoku.UpdateSquare(28, 3);
                sudoku.UpdateSquare(29, 7);
                sudoku.UpdateSquare(30, 1);
                sudoku.UpdateSquare(31, 6);
                sudoku.UpdateSquare(32, 9);
                sudoku.UpdateSquare(33, 5);
                sudoku.UpdateSquare(34, 2);
                sudoku.UpdateSquare(35, 4);
                sudoku.UpdateSquare(36, 8);

                sudoku.UpdateSquare(37, 6);
                sudoku.UpdateSquare(38, 9);
                sudoku.UpdateSquare(39, 2);
                sudoku.UpdateSquare(40, 4);
                sudoku.UpdateSquare(41, 1);
                sudoku.UpdateSquare(42, 8);
                sudoku.UpdateSquare(43, 5);
                sudoku.UpdateSquare(44, 3);
                sudoku.UpdateSquare(45, 7);

                sudoku.UpdateSquare(46, 5);
                sudoku.UpdateSquare(47, 4);
                sudoku.UpdateSquare(48, 8);
                sudoku.UpdateSquare(49, 3);
                sudoku.UpdateSquare(50, 7);
                sudoku.UpdateSquare(51, 2);
                sudoku.UpdateSquare(52, 9);
                sudoku.UpdateSquare(53, 1);
                sudoku.UpdateSquare(54, 6);

                sudoku.UpdateSquare(55, 5);
                sudoku.UpdateSquare(56, 1);
                sudoku.UpdateSquare(57, 2);
                sudoku.UpdateSquare(58, 8);
                sudoku.UpdateSquare(59, 3);
                sudoku.UpdateSquare(60, 4);
                sudoku.UpdateSquare(61, 9);
                sudoku.UpdateSquare(62, 6);
                sudoku.UpdateSquare(63, 7);

                sudoku.UpdateSquare(64, 8);
                sudoku.UpdateSquare(65, 6);
                sudoku.UpdateSquare(66, 3);
                sudoku.UpdateSquare(67, 9);
                sudoku.UpdateSquare(68, 7);
                sudoku.UpdateSquare(69, 1);
                sudoku.UpdateSquare(70, 2);
                sudoku.UpdateSquare(71, 4);
                sudoku.UpdateSquare(72, 5);

                sudoku.UpdateSquare(73, 7);
                sudoku.UpdateSquare(74, 9);
                sudoku.UpdateSquare(75, 4);
                sudoku.UpdateSquare(76, 2);
                sudoku.UpdateSquare(77, 6);
                sudoku.UpdateSquare(78, 5);
                sudoku.UpdateSquare(79, 8);
                sudoku.UpdateSquare(80, 3);
                sudoku.UpdateSquare(81, 1);

                return sudoku;
            }
        }

        public static Sudoku TieBreak
        {
            get
            {
                var sudoku = new Sudoku()
                {
                    Title = "Tie-break",
                    Description = ""
                };

                // Update
                sudoku.UpdateSquare(2, 2);
                sudoku.UpdateSquare(3, 3);
                
                sudoku.UpdateSquare(10, 4);
                sudoku.UpdateSquare(11, 5);
                sudoku.UpdateSquare(12, 6);

                sudoku.UpdateSquare(19, 7);
                sudoku.UpdateSquare(20, 8);

                sudoku.UpdateSquare(80, 3);
                sudoku.UpdateSquare(79, 4);

                sudoku.UpdateSquare(72, 5);
                sudoku.UpdateSquare(71, 6);
                sudoku.UpdateSquare(70, 7);

                sudoku.UpdateSquare(63, 8);
                sudoku.UpdateSquare(62, 9);

                return sudoku;
            }
        }    
    }
}
