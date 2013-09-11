using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SudokuSolver.Engine
{
    public sealed class SampleCaseManager
    {
        SampleCaseManager() { }

        /// <summary>
        /// Gets sample case; Square method - Horizontal
        /// </summary>
        /// <returns></returns>
        public static Sudoku SquareMethodHorizontal
        {
            get
            {
                // New sudoku
                var sudoku = new Sudoku()
                {
                    SudokuId = 1,
                    Title = "Square method - Horizontal"
                };

                // Update
                sudoku.UpdateSquare(1, 1);
                sudoku.UpdateSquare(2, 2);
                sudoku.UpdateSquare(3, 3);
                sudoku.UpdateSquare(10, 4);
                sudoku.UpdateSquare(11, 5);
                sudoku.UpdateSquare(12, 6);
                sudoku.UpdateSquare(19, 7);
                sudoku.UpdateSquare(20, 8);

                // Toggle
                sudoku.ToggleReady();

                // Return
                return sudoku;
            }
        }

        /// <summary>
        /// Gets sample case; Square method - Vertical
        /// </summary>
        /// <returns></returns>
        public static Sudoku SquareMethodVertical
        {
            get
            {
                var sudoku = new Sudoku()
                {
                    SudokuId = 2,
                    Title = "Square method - Vertical"
                };

                // Update
                sudoku.UpdateSquare(1, 1);
                sudoku.UpdateSquare(4, 2);
                sudoku.UpdateSquare(7, 3);
                sudoku.UpdateSquare(28, 4);
                sudoku.UpdateSquare(31, 5);
                sudoku.UpdateSquare(34, 6);
                sudoku.UpdateSquare(55, 7);
                sudoku.UpdateSquare(58, 8);

                sudoku.ToggleReady();

                return sudoku;
            }
        }

        /// <summary>
        /// Gets sample case; Square method - Square
        /// </summary>
        /// <returns></returns>
        public static Sudoku SquareMethodSquare
        {
            get
            {
                var sudoku = new Sudoku()
                {
                    SudokuId = 3,
                    Title = "Square method - Square"
                };

                // Update
                sudoku.UpdateSquare(1, 1);
                sudoku.UpdateSquare(2, 2);
                sudoku.UpdateSquare(3, 3);
                sudoku.UpdateSquare(4, 4);
                sudoku.UpdateSquare(5, 5);
                sudoku.UpdateSquare(6, 6);
                sudoku.UpdateSquare(7, 7);
                sudoku.UpdateSquare(8, 8);

                sudoku.ToggleReady();

                return sudoku;
            }
        }

        /// <summary>
        /// Gets sample case; Square method - Mixed
        /// </summary>
        /// <returns></returns>
        public static Sudoku SquareMethodMixed
        {
            get
            {
                var sudoku = new Sudoku()
                {
                    SudokuId = 4,
                    Title = "Square method - Mixed"
                };

                // Update
                sudoku.UpdateSquare(10, 1);
                sudoku.UpdateSquare(11, 2);
                sudoku.UpdateSquare(12, 3);
                sudoku.UpdateSquare(28, 4);
                sudoku.UpdateSquare(31, 5);
                sudoku.UpdateSquare(34, 6);
                sudoku.UpdateSquare(2, 7);
                sudoku.UpdateSquare(4, 8);

                sudoku.ToggleReady();

                return sudoku;
            }
        }

        /// <summary>
        /// Gets sample case; Group number method
        /// </summary>
        /// <returns></returns>
        public static Sudoku GroupNumberMethod
        {
            get
            {
                var sudoku = new Sudoku()
                {
                    SudokuId = 5,
                    Title = "Group number method"
                };

                // Update
                sudoku.UpdateSquare(13, 1);
                sudoku.UpdateSquare(25, 1);
                sudoku.UpdateSquare(29, 1);
                sudoku.UpdateSquare(57, 1);

                //sudoku.ToggleReady();

                return sudoku;
            }
        }

        /// <summary>
        /// Gets sample case; Group number method 2
        /// </summary>
        /// <returns></returns>
        public static Sudoku GroupNumberMethodWithHelp
        {
            get
            {
                var sudoku = new Sudoku()
                {
                    SudokuId = 6,
                    Title = "Group number method with help"
                };

                // Update
                sudoku.UpdateSquare(25, 1);
                sudoku.UpdateSquare(57, 1);
                sudoku.UpdateSquare(69, 1);
                sudoku.UpdateSquare(80, 2);

                // sudoku.ToggleReady();

                return sudoku;
            }
        }

        /// <summary>
        /// Gets sample case; Domino
        /// </summary>
        /// <returns></returns>
        public static Sudoku Domino
        {
            get
            {
                var sudoku = new Sudoku()
                {
                    SudokuId = 7,
                    Title = "Domino"
                };

                // Update
                sudoku.UpdateSquare(4, 3);
                sudoku.UpdateSquare(13, 1);
                sudoku.UpdateSquare(23, 2);
                sudoku.UpdateSquare(7, 4);
                sudoku.UpdateSquare(25, 1);
                sudoku.UpdateSquare(28, 5);
                sudoku.UpdateSquare(29, 1);
                sudoku.UpdateSquare(37, 2);
                sudoku.UpdateSquare(34, 6);
                sudoku.UpdateSquare(55, 7);
                sudoku.UpdateSquare(57, 1);
                sudoku.UpdateSquare(73, 2);
                sudoku.UpdateSquare(58, 8);
                sudoku.UpdateSquare(61, 9);

                return sudoku;
            }
        }

        /// <summary>
        /// Gets sample case; Domino (B-2)
        /// </summary>
        /// <returns></returns>
        public static Sudoku DominoB2
        {
            get
            {
                var sudoku = new Sudoku()
                {
                    SudokuId = 8,
                    Title = "Domino (B-2)"
                };

                // Update
                sudoku.UpdateSquare(2, 2);
                sudoku.UpdateSquare(3, 3);
                sudoku.UpdateSquare(4, 4);
                sudoku.UpdateSquare(5, 5);
                sudoku.UpdateSquare(6, 6);
                sudoku.UpdateSquare(7, 7);
                sudoku.UpdateSquare(8, 8);

                sudoku.UpdateSquare(10, 4);
                sudoku.UpdateSquare(11, 5);
                sudoku.UpdateSquare(12, 6);
                sudoku.UpdateSquare(19, 7);
                sudoku.UpdateSquare(20, 8);

                sudoku.UpdateSquare(28, 2);
                sudoku.UpdateSquare(31, 3);
                sudoku.UpdateSquare(34, 5);
                sudoku.UpdateSquare(55, 6);
                sudoku.UpdateSquare(58, 8);

                sudoku.UpdateSquare(13, 1);
                sudoku.UpdateSquare(29, 1);
                sudoku.UpdateSquare(50, 1);
                sudoku.UpdateSquare(68, 1);
                sudoku.UpdateSquare(73, 1);

                return sudoku;
            }
        }

        /// <summary>
        /// Gets sample case; Domino (B-2)
        /// </summary>
        /// <returns></returns>
        public static Sudoku DominoB2Reverse
        {
            get
            {
                var sudoku = new Sudoku()
                {
                    SudokuId = 9,
                    Title = "Domino (B-2) Reverse"
                };

                // Update
                sudoku.UpdateSquare(2, 2);
                sudoku.UpdateSquare(3, 3);
                sudoku.UpdateSquare(4, 4);
                sudoku.UpdateSquare(5, 5);
                sudoku.UpdateSquare(6, 6);
                sudoku.UpdateSquare(7, 7);
                sudoku.UpdateSquare(8, 8);
                sudoku.UpdateSquare(9, 9);

                sudoku.UpdateSquare(10, 4);
                sudoku.UpdateSquare(11, 5);
                sudoku.UpdateSquare(12, 6);
                sudoku.UpdateSquare(19, 7);
                sudoku.UpdateSquare(20, 8);
                sudoku.UpdateSquare(21, 9);

                sudoku.UpdateSquare(28, 2);
                sudoku.UpdateSquare(31, 3);
                sudoku.UpdateSquare(34, 5);
                sudoku.UpdateSquare(55, 6);
                sudoku.UpdateSquare(58, 8);
                sudoku.UpdateSquare(61, 9);

                sudoku.UpdateSquare(13, 1);
                sudoku.UpdateSquare(27, 1);
                sudoku.UpdateSquare(29, 1);
                sudoku.UpdateSquare(50, 1);
                sudoku.UpdateSquare(63, 1);
                sudoku.UpdateSquare(68, 1);
                sudoku.UpdateSquare(73, 1);

                return sudoku;
            }
        }

        /// <summary>
        /// Gets sample case; Beauty
        /// </summary>
        /// <returns></returns>
        public static Sudoku Beauty
        {
            get
            {
                var sudoku = new Sudoku()
                {
                    SudokuId = 10,
                    Title = "Beauty"
                };

                // Update
                sudoku.UpdateSquare(2, 2);
                sudoku.UpdateSquare(4, 4);
                sudoku.UpdateSquare(5, 5);
                sudoku.UpdateSquare(6, 6);
                sudoku.UpdateSquare(8, 8);
                sudoku.UpdateSquare(10, 9);
                sudoku.UpdateSquare(12, 7);
                sudoku.UpdateSquare(16, 3);
                sudoku.UpdateSquare(18, 1);
                sudoku.UpdateSquare(28, 3);
                sudoku.UpdateSquare(30, 1);
                sudoku.UpdateSquare(34, 9);
                sudoku.UpdateSquare(36, 7);

                return sudoku;
            }
        }

        /// <summary>
        /// Gets sample case; Hint update
        /// </summary>
        /// <returns></returns>
        public static Sudoku HintUpdate
        {
            get
            {
                var sudoku = new Sudoku()
                {
                    SudokuId = 11,
                    Title = "Hint update"
                };

                // Update
                sudoku.UpdateSquare(1, 1);
                sudoku.UpdateSquare(2, 2);
                sudoku.UpdateSquare(3, 3);
                sudoku.UpdateSquare(10, 4);
                sudoku.UpdateSquare(11, 5);
                sudoku.UpdateSquare(12, 6);
                sudoku.UpdateSquare(19, 7);
                sudoku.UpdateSquare(20, 8);

                // At this state, Square Id 21 has Hint for Number 9
                // Change the number of Square Id 1 to 9, which should lead to have a new hint with Number 1
                sudoku.UpdateSquare(1, 9);

                return sudoku;
            }
        }

        /// <summary>
        /// Gets sample case; Hint update
        /// </summary>
        /// <returns></returns>
        public static Sudoku HintUpdate2
        {
            get
            {
                var sudoku = new Sudoku()
                {
                    SudokuId = 12,
                    Title = "Hint update 2"
                };

                // Update
                // Update
                sudoku.UpdateSquare(13, 1);

                sudoku.UpdateSquare(25, 1);
                sudoku.UpdateSquare(26, 2);

                sudoku.UpdateSquare(29, 1);
                sudoku.UpdateSquare(32, 2);

                sudoku.UpdateSquare(57, 1);
                sudoku.UpdateSquare(60, 2);

                // At this state, Square Id 1 has Hint for Number 1
                // Change the number of Square Id 13 to 2, which should lead to have a new hint with Number 2
                sudoku.UpdateSquare(13, 2);

                return sudoku;
            }
        }

        /// <summary>
        /// Gets sample case; Hint update
        /// </summary>
        /// <returns></returns>
        public static Sudoku InvalidHintRemove
        {
            get
            {
                var sudoku = new Sudoku()
                {
                    SudokuId = 13,
                    Title = "Invalid hint remove"
                };

                // Update
                // Update
                sudoku.UpdateSquare(13, 1);

                sudoku.UpdateSquare(22, 2);
                sudoku.UpdateSquare(25, 1);

                sudoku.UpdateSquare(29, 1);
                sudoku.UpdateSquare(30, 2);

                sudoku.UpdateSquare(56, 2);
                sudoku.UpdateSquare(57, 1);

                // Doesnt remove the hints ?!
                sudoku.UpdateSquare(13, 0);

                return sudoku;
            }
        }

        /// <summary>
        /// Gets sample case; Mini size
        /// </summary>
        /// <returns></returns>
        public static Sudoku MiniSize
        {
            get
            {
                var sudoku = new Sudoku(4)
                {
                    SudokuId = 15,
                    Title = "Mini size"
                };

                sudoku.ToggleReady();

                return sudoku;
            }
        }

        /// <summary>
        /// Gets sample case; Maxi size
        /// </summary>
        /// <returns></returns>
        public static Sudoku MaxiSize
        {
            get
            {
                var sudoku = new Sudoku(16)
                {
                    SudokuId = 16,
                    Title = "Maxi size"
                };

                sudoku.ToggleReady();

                return sudoku;
            }
        }

        /// <summary>
        /// Gets sample case; Real-world sudoku
        /// </summary>
        /// <returns></returns>
        public static Sudoku RealSudoku
        {
            get
            {
                var sudoku = new Sudoku()
                {
                    SudokuId = 21,
                    Title = "Real-world sudoku"
                };

                // Update
                sudoku.UpdateSquare(3, 8);
                sudoku.UpdateSquare(10, 3);
                sudoku.UpdateSquare(11, 4);
                sudoku.UpdateSquare(12, 2);
                sudoku.UpdateSquare(19, 9);
                sudoku.UpdateSquare(6, 9);
                sudoku.UpdateSquare(22, 7);
                sudoku.UpdateSquare(7, 4);
                sudoku.UpdateSquare(27, 3);
                sudoku.UpdateSquare(30, 6);
                sudoku.UpdateSquare(37, 4);
                sudoku.UpdateSquare(38, 7);
                sudoku.UpdateSquare(39, 3);
                sudoku.UpdateSquare(46, 2);
                sudoku.UpdateSquare(32, 3);
                sudoku.UpdateSquare(50, 1);
                sudoku.UpdateSquare(36, 2);
                sudoku.UpdateSquare(43, 8);
                sudoku.UpdateSquare(44, 5);
                sudoku.UpdateSquare(45, 1);
                sudoku.UpdateSquare(52, 6);
                sudoku.UpdateSquare(55, 7);
                sudoku.UpdateSquare(75, 8);
                sudoku.UpdateSquare(60, 4);
                sudoku.UpdateSquare(76, 1);
                sudoku.UpdateSquare(63, 3);
                sudoku.UpdateSquare(70, 6);
                sudoku.UpdateSquare(71, 9);
                sudoku.UpdateSquare(72, 7);
                sudoku.UpdateSquare(79, 5);

                sudoku.ToggleReady();

                return sudoku;
            }
        }

        /// <summary>
        /// Gets sample case; Invalid sudoku
        /// </summary>
        /// <returns></returns>
        public static Sudoku InvalidSudoku
        {
            get
            {
                var sudoku = new Sudoku()
                {
                    SudokuId = 51,
                    Title = "Invalid sudoku"
                };

                // Update
                sudoku.UpdateSquare(1, 9);
                sudoku.UpdateSquare(2, 7);
                sudoku.UpdateSquare(3, 3);
                sudoku.UpdateSquare(10, 6);
                sudoku.UpdateSquare(11, 2);
                sudoku.UpdateSquare(12, 8);
                sudoku.UpdateSquare(19, 4);
                sudoku.UpdateSquare(20, 5);

                sudoku.UpdateSquare(21, 1);

                sudoku.UpdateSquare(4, 5);
                sudoku.UpdateSquare(5, 6);
                sudoku.UpdateSquare(6, 8);
                sudoku.UpdateSquare(13, 4);
                sudoku.UpdateSquare(14, 1);
                sudoku.UpdateSquare(15, 3);
                sudoku.UpdateSquare(22, 7);
                sudoku.UpdateSquare(23, 9);

                sudoku.UpdateSquare(24, 2);

                sudoku.UpdateSquare(7, 1);
                sudoku.UpdateSquare(8, 2);

                sudoku.UpdateSquare(9, 4);

                sudoku.UpdateSquare(16, 5);
                sudoku.UpdateSquare(17, 9);

                sudoku.UpdateSquare(18, 7);

                sudoku.UpdateSquare(25, 8);
                sudoku.UpdateSquare(26, 3);

                sudoku.UpdateSquare(27, 6);

                sudoku.UpdateSquare(28, 8);
                sudoku.UpdateSquare(29, 1);
                sudoku.UpdateSquare(30, 5);
                sudoku.UpdateSquare(37, 7);
                sudoku.UpdateSquare(39, 9);

                sudoku.UpdateSquare(47, 4);

                sudoku.UpdateSquare(48, 3);

                sudoku.UpdateSquare(31, 6);
                sudoku.UpdateSquare(32, 4);
                sudoku.UpdateSquare(33, 9);
                sudoku.UpdateSquare(40, 3);
                sudoku.UpdateSquare(41, 5);

                sudoku.UpdateSquare(42, 2);

                sudoku.UpdateSquare(49, 1);

                sudoku.UpdateSquare(50, 7);

                sudoku.UpdateSquare(51, 8);

                sudoku.UpdateSquare(34, 2);

                sudoku.UpdateSquare(35, 3);

                sudoku.UpdateSquare(36, 7);

                sudoku.UpdateSquare(43, 1);

                // sudoku.UpdateSquare(44, 6);

                sudoku.UpdateSquare(45, 4);

                sudoku.UpdateSquare(52, 9);

                sudoku.UpdateSquare(54, 5);

                sudoku.UpdateSquare(55, 4);
                sudoku.UpdateSquare(56, 5);
                sudoku.UpdateSquare(57, 2);
                sudoku.UpdateSquare(64, 8);

                sudoku.UpdateSquare(65, 7);

                sudoku.UpdateSquare(73, 3);

                // sudoku.UpdateSquare(74, 6);

                sudoku.UpdateSquare(75, 9);

                sudoku.UpdateSquare(58, 7);

                sudoku.UpdateSquare(59, 8);

                sudoku.UpdateSquare(60, 6);

                sudoku.UpdateSquare(67, 9);

                sudoku.UpdateSquare(68, 3);

                sudoku.UpdateSquare(69, 1);

                sudoku.UpdateSquare(76, 5);

                sudoku.UpdateSquare(77, 2);

                sudoku.UpdateSquare(78, 4);

                sudoku.UpdateSquare(61, 3);

                sudoku.UpdateSquare(62, 9);

                sudoku.UpdateSquare(63, 1);

                sudoku.UpdateSquare(70, 2);

                sudoku.UpdateSquare(71, 4);

                // sudoku.UpdateSquare(72, 6);

                sudoku.UpdateSquare(80, 8);

                sudoku.UpdateSquare(81, 7);

                sudoku.ToggleReady();

                return sudoku;
            }
        }

        /// <summary>
        /// Gets sample case; Hint's availability bug
        /// </summary>
        /// <returns></returns>
        public static Sudoku HintAvailabilityBug
        {
            get
            {
                var sudoku = new Sudoku()
                {
                    SudokuId = 52,
                    Title = "Hint's availability bug",
                    Description = "Because the availabilities of the hints are not counted (yet)"
                };

                // Update
                sudoku.UpdateSquare(1, 1);
                sudoku.UpdateSquare(2, 2);
                sudoku.UpdateSquare(3, 3);
                sudoku.UpdateSquare(4, 4);
                sudoku.UpdateSquare(5, 5);
                sudoku.UpdateSquare(6, 6);
                sudoku.UpdateSquare(7, 7);
                sudoku.UpdateSquare(8, 8);

                // Since the value of Square Id 9 value is known, this update should not be possible
                // Check this case after start treating hint's availabilities like any other square's availabilities..! - Equal rights to the squares!
                sudoku.UpdateSquare(27, 9);

                return sudoku;
            }
        }

        /// <summary>
        /// Gets sample case; Value remove - square availability buggy
        /// </summary>
        /// <returns></returns>
        public static Sudoku ValueRemoveSquareAvailabilityBug
        {
            get
            {
                var sudoku = new Sudoku()
                {
                    SudokuId = 53,
                    Title = "Value remove - Square availability bug",
                    Description = "Because the availabilities of the hints are not counted (yet)"
                };

                // Update
                sudoku.UpdateSquare(1, 1);
                sudoku.UpdateSquare(2, 2);
                sudoku.UpdateSquare(3, 3);
                sudoku.UpdateSquare(4, 4);
                sudoku.UpdateSquare(5, 5);
                sudoku.UpdateSquare(6, 6);
                sudoku.UpdateSquare(7, 7);
                sudoku.UpdateSquare(8, 8);

                // Since the value of Square Id 9 value is known, this update should not be possible
                // Check this case after start treating hint's availabilities like any other square's availabilities..! - Equal rights to the squares!
                sudoku.UpdateSquare(27, 9);

                return sudoku;
            }
        }

        public static Sudoku HintValueUpdate
        {
            get
            {
                var sudoku = new Sudoku()
                {
                    SudokuId = 54,
                    Title = "Hint value update"
                };

                // Update
                sudoku.UpdateSquare(1, 1);
                sudoku.UpdateSquare(2, 2);
                sudoku.UpdateSquare(3, 3);
                sudoku.UpdateSquare(4, 4);
                sudoku.UpdateSquare(5, 5);
                sudoku.UpdateSquare(6, 6);
                sudoku.UpdateSquare(7, 7);
                sudoku.UpdateSquare(8, 8);

                // This is not a valid action; Square Id 9 is hint now and cannot be changed?
                sudoku.UpdateSquare(9, 0);

                return sudoku;
            }
        }

        /// <summary>
        /// Gets sample case; Headache (1-2-3)
        /// </summary>
        /// <returns></returns>
        public static Sudoku Headache
        {
            get
            {
                var sudoku = new Sudoku()
                {
                    SudokuId = 61,
                    Title = "Headache (1-2-3)"
                };

                // Update
                sudoku.UpdateSquare(1, 1);
                sudoku.UpdateSquare(2, 2);
                sudoku.UpdateSquare(3, 3);
                sudoku.UpdateSquare(13, 1);
                sudoku.UpdateSquare(14, 2);
                sudoku.UpdateSquare(15, 3);

                // This is an invalid assigment - 25 can only have 1-2-3
                sudoku.UpdateSquare(25, 4);

                sudoku.ToggleReady();

                return sudoku;
            }
        }

        /// <summary>
        /// Gets sample case; Headache 2
        /// </summary>
        /// <returns></returns>
        public static Sudoku Headache2
        {
            get
            {
                var sudoku = new Sudoku()
                {
                    SudokuId = 62,
                    Title = "Headache 2"
                };

                // Update
                sudoku.UpdateSquare(1, 1);
                sudoku.UpdateSquare(13, 1);
                sudoku.UpdateSquare(25, 2);
                sudoku.UpdateSquare(26, 3);

                // This is an invalid assigment - should fail
                sudoku.UpdateSquare(27, 4);

                sudoku.ToggleReady();

                return sudoku;
            }
        }

        /// <summary>
        /// Gets sample case; Headache 3
        /// </summary>
        /// <returns></returns>
        public static Sudoku Headache3
        {
            get
            {
                var sudoku = new Sudoku()
                {
                    SudokuId = 63,
                    Title = "Headache 3"
                };

                // Update
                sudoku.UpdateSquare(1, 1);
                sudoku.UpdateSquare(2, 2);
                sudoku.UpdateSquare(3, 3);
                sudoku.UpdateSquare(10, 4);
                sudoku.UpdateSquare(11, 5);
                sudoku.UpdateSquare(12, 6);
                sudoku.UpdateSquare(19, 7);
                sudoku.UpdateSquare(20, 8);
                sudoku.UpdateSquare(28, 4);
                sudoku.UpdateSquare(31, 5);
                sudoku.UpdateSquare(34, 6);

                // This is an invalid assigment - should fail

                sudoku.UpdateSquare(55, 7);
                sudoku.UpdateSquare(58, 8);

                return sudoku;
            }
        }

        /// <summary>
        /// Gets sample case; Headache 4
        /// </summary>
        /// <returns></returns>
        public static Sudoku Headache4
        {
            get
            {
                var sudoku = new Sudoku()
                {
                    SudokuId = 64,
                    Title = "Headache 4" // Try this after fixing CASE 1: ID 5
                };

                // Update
                sudoku.UpdateSquare(1, 1);
                sudoku.UpdateSquare(2, 2);
                sudoku.UpdateSquare(3, 3);
                sudoku.UpdateSquare(10, 4);
                sudoku.UpdateSquare(11, 5);
                sudoku.UpdateSquare(12, 6);
                sudoku.UpdateSquare(19, 7);
                sudoku.UpdateSquare(20, 8);

                // sudoku.UpdateSquare(21, 9);
                sudoku.UpdateSquare(21, 9);

                sudoku.UpdateSquare(4, 4);
                sudoku.UpdateSquare(5, 5);
                sudoku.UpdateSquare(6, 6);
                sudoku.UpdateSquare(7, 7);
                sudoku.UpdateSquare(8, 8);

                // sudoku.UpdateSquare(9, 9);
                sudoku.UpdateSquare(9, 9);

                sudoku.UpdateSquare(13, 1);

                // sudoku.UpdateSquare(14, 2);
                sudoku.UpdateSquare(14, 2);

                sudoku.ToggleReady();

                return sudoku;
            }
        }

        /// <summary>
        /// Gets sample case; Free-style - for general testing
        /// </summary>
        /// <returns></returns>
        public static Sudoku GroupNumberRemoveHintBug
        {
            get
            {
                var sudoku = new Sudoku()
                {
                    SudokuId = 65,
                    Title = "Group number remove hint bug"
                };

                // Update
                sudoku.UpdateSquare(13, 1);
                sudoku.UpdateSquare(25, 1);
                sudoku.UpdateSquare(29, 1);
                sudoku.UpdateSquare(57, 1);

                // When this set, all availabilities should be "available" but that's not the case
                sudoku.UpdateSquare(57, 2);

                return sudoku;
            }
        }

        /// <summary>
        /// Gets sample case; Free-style - for general testing
        /// </summary>
        /// <returns></returns>
        public static Sudoku Freestyle
        {
            get
            {
                var sudoku = new Sudoku(4)
                {
                    SudokuId = 101,
                    Title = "Free-style"
                };

                // Update
                sudoku.UpdateSquare(1, 1);
                sudoku.UpdateSquare(2, 2);
                sudoku.UpdateSquare(5, 3);
                // sudoku.UpdateSquare(1, 1);

                return sudoku;
            }
        }

        public static IEnumerable<Sudoku> SampleCases
        {
            get
            {
                var list = new Collection<Sudoku>();

                //list.Add(SquareMethodHorizontal);
                //list.Add(SquareMethodVertical);
                //list.Add(SquareMethodSquare);
                //list.Add(SquareMethodMixed);
                //list.Add(GroupNumberMethod);
                //list.Add(GroupNumberMethodWithHelp);
                //list.Add(Domino);
                //list.Add(DominoB2);
                //list.Add(DominoB2Reverse);
                list.Add(Beauty);
                //list.Add(HintUpdate);
                //list.Add(HintUpdate2);
                //list.Add(InvalidHintRemove);
                //list.Add(MiniSize);
                //list.Add(MaxiSize);
                //list.Add(RealSudoku);
                //list.Add(InvalidSudoku);
                ////list.Add(Headache);
                ////list.Add(Headache2);
                ////list.Add(Headache3);
                ////list.Add(Headache4);
                ////list.Add(HintAvailabilityBug);
                //list.Add(GroupNumberRemoveHintBug);
                //list.Add(Freestyle);

                return list;
            }
        }
    }
}
