using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver.Engine;

namespace SudokuSolver.Engine.Test
{
    [TestClass]
    public class SudokuTest
    {
        CaseManager caseManager;

        CaseManager CaseManager
        {
            get { return caseManager ?? (caseManager = new CaseManager()); }
        }

        [TestMethod]
        public void SquareMethod_Horizontal_8()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase1();

            // a. Hint count
            Assert.IsTrue(sudoku.Hints.Count == 2);

            // Get the hint & square
            var hint = sudoku.Hints.First();

            // b. Hint square value
            Assert.IsTrue(hint.Square.SquareId == 21);

            // c. Hint number value
            Assert.IsTrue(hint.SudokuNumber.Value == 9);

            // Solve
            sudoku.Solve();

            // d. Hint count
            Assert.IsTrue(sudoku.Hints.Count == 1);

            // e. Square assign type
            Assert.IsTrue(hint.Square.AssignType == AssignTypes.Solver);

            // f. Sudoku squares left
            Assert.IsTrue(sudoku.SquaresLeft == 72);
        }

        [TestMethod]
        public void SquareMethod_Verical_8()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase2();

            // a. Hint count
            Assert.IsTrue(sudoku.Hints.Count == 2);

            // Get the hint & square
            var hint = sudoku.Hints.First();

            // b. Hint square value
            Assert.IsTrue(hint.Square.SquareId == 61);

            // c. Hint number value
            Assert.IsTrue(hint.SudokuNumber.Value == 9);

            // Solve
            sudoku.Solve();

            // d. Hint count
            Assert.IsTrue(sudoku.Hints.Count == 1);

            // e. Square assign type
            Assert.IsTrue(hint.Square.AssignType == AssignTypes.Solver);

            // f. Sudoku squares left
            Assert.IsTrue(sudoku.SquaresLeft == 72);
        }

        [TestMethod]
        public void SquareMethod_Square_8()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase3();

            // a. Hint count
            Assert.IsTrue(sudoku.Hints.Count == 2);

            // Get the hint & square
            var hint = sudoku.Hints.First();

            // b. Hint square value
            Assert.IsTrue(hint.Square.SquareId == 9);

            // c. Hint number value
            Assert.IsTrue(hint.SudokuNumber.Value == 9);

            // Solve
            sudoku.Solve();

            // d. Hint count
            Assert.IsTrue(sudoku.Hints.Count == 1);

            // e. Square assign type
            Assert.IsTrue(hint.Square.AssignType == AssignTypes.Solver);

            // f. Sudoku squares left
            Assert.IsTrue(sudoku.SquaresLeft == 72);
        }

        [TestMethod]
        public void SquareMethod_Mixed_8()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase4();

            // a. Hint count
            Assert.IsTrue(sudoku.Hints.Count == 1);

            // Get the hint & square
            var hint = sudoku.Hints.Single();

            // b. Hint square value
            Assert.IsTrue(hint.Square.SquareId == 1);

            // c. Hint number value
            Assert.IsTrue(hint.SudokuNumber.Value == 9);

            // Solve
            sudoku.Solve();

            // d. Hint count
            Assert.IsTrue(sudoku.Hints.Count == 0);

            // e. Square assign type
            Assert.IsTrue(hint.Square.AssignType == AssignTypes.Solver);

            // f. Sudoku squares left
            Assert.IsTrue(sudoku.SquaresLeft == 72);
        }

        [TestMethod]
        public void GroupNumberMethod_Straight_1()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase5();

            // a. Hint count
            Assert.IsTrue(sudoku.Hints.Count == 3);

            // Get the hint & square
            var hint = sudoku.Hints.First();

            // b. Hint square value
            Assert.IsTrue(hint.Square.SquareId == 1);

            // c. Hint number value
            Assert.IsTrue(hint.SudokuNumber.Value == 1);

            // Solve
            sudoku.Solve();

            // d. Hint count
            Assert.IsTrue(sudoku.Hints.Count == 2);

            // e. Square assign type
            Assert.IsTrue(hint.Square.AssignType == AssignTypes.Solver);

            // f. Sudoku squares left
            Assert.IsTrue(sudoku.SquaresLeft == 76);
        }

        [TestMethod]
        public void GroupNumberMethod_Mixed()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase6();

            // a. Hint count
            Assert.IsTrue(sudoku.Hints.Count == 2);

            // Get the hint & square
            var hint = sudoku.Hints.First();

            // b. Hint square value
            Assert.IsTrue(hint.Square.SquareId == 81);

            // c. Hint number value
            Assert.IsTrue(hint.SudokuNumber.Value == 1);

            // Solve
            sudoku.Solve();

            // d. Hint count
            Assert.IsTrue(sudoku.Hints.Count == 1);

            // e. Square assign type
            Assert.IsTrue(hint.Square.AssignType == AssignTypes.Solver);

            // f. Sudoku squares left
            Assert.IsTrue(sudoku.SquaresLeft == 76);
        }

        [TestMethod]
        public void Domino()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase7();

            // a. Hint count
            Assert.IsTrue(sudoku.Hints.Count == 4);

            // b. Hint square value
            Assert.IsTrue(sudoku.Hints.Any(hint => hint.Square.SquareId == 1));
            Assert.IsTrue(sudoku.Hints.Any(hint => hint.Square.SquareId == 31));

            // c. Hint number value
            Assert.IsTrue(sudoku.Hints.First(hint => hint.Square.SquareId == 1).SudokuNumber.Value == 1);
            Assert.IsTrue(sudoku.Hints.First(hint => hint.Square.SquareId == 31).SudokuNumber.Value == 2);

            // Solve
            sudoku.Solve();

            // d. Hint count
            Assert.IsTrue(sudoku.Hints.Count == 5);

            // e. Square assign type
            Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 1).AssignType == AssignTypes.Solver);
            Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 31).AssignType == AssignTypes.Solver);
            Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 54).AssignType == AssignTypes.Solver);

            // b. Squares left
            Assert.IsTrue(sudoku.SquaresLeft == 64);
        }

        [TestMethod]
        public void Beauty()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase8();

            // Solve
            sudoku.Solve();

            // Get the control squares
            var controlSquare1 = sudoku.Squares.Single(square => square.SquareId == 1);
            var controlSquare2 = sudoku.Squares.Single(square => square.SquareId == 3);
            var controlSquare3 = sudoku.Squares.Single(square => square.SquareId == 7);
            var controlSquare4 = sudoku.Squares.Single(square => square.SquareId == 9);

            // Test
            // a. Control squares values
            Assert.IsTrue(controlSquare1.SudokuNumber.Value == 1);
            Assert.IsTrue(controlSquare2.SudokuNumber.Value == 3);
            Assert.IsTrue(controlSquare3.SudokuNumber.Value == 7);
            Assert.IsTrue(controlSquare4.SudokuNumber.Value == 9);

            // b. Squares left
            Assert.IsTrue(sudoku.SquaresLeft == 64);
        }

        [TestMethod]
        public void MiniSize()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase9();

            // Test
            // a. Total size
            Assert.IsTrue(sudoku.TotalSize == 16);

            // b. Squares left
            Assert.IsTrue(sudoku.SquaresLeft == 16);
        }

        [TestMethod]
        public void MaxiSize()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase10();

            // Test
            // a. Total size
            Assert.IsTrue(sudoku.TotalSize == 256);

            // b. Squares left
            Assert.IsTrue(sudoku.SquaresLeft == 256);
        }

        [TestMethod]
        public void RealCase()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase11();

            // Solve
            sudoku.Solve();

            // Test
            // a. Squares left
            Assert.IsTrue(sudoku.SquaresLeft == 0);
        }

        [TestMethod]
        public void InvalidSudoku_Headache()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase12_Headache();
            
            // Test
            // a. Assign an invalid value
            try
            {
                sudoku.UpdateSquare(25, 4);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsNotInstanceOfType(ex, typeof(UnitTestAssertException));
            }
        }

        [TestMethod]
        public void InvalidSudoku_Headache2()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase13_Headache();

            // Test
            // a. Assign an invalid value
            try
            {
                sudoku.UpdateSquare(27, 4);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsNotInstanceOfType(ex, typeof(UnitTestAssertException));
            }
        }

        [TestMethod]
        public void Wrong()
        {
            // Test
            // a. Invalid sudoku
            try
            {
                var sudoku = CaseManager.GetCase14();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(AssertFailedException));
            }
        }

        [TestMethod]
        public void Wrong2()
        {
            // Test
            // a. Invalid sudoku
            try
            {
                var sudoku = CaseManager.GetCase15();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(AssertFailedException));
            }
        }

        [TestMethod]
        public void AvailabilityHintBug()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase17();

            // a. Hint count
            Assert.IsTrue(sudoku.Hints.Count == 2);

            // Get the hint & square
            var hint = sudoku.Hints.First();

            // b. Hint square value
            Assert.IsTrue(hint.Square.SquareId == 21);

            // c. Hint number value
            // TODO Becase currently hint removal doesnt work properly, this fails - check it after fixing hint removal
            Assert.IsTrue(hint.SudokuNumber.Value == 1);

            // Solve
            sudoku.Solve();

            // d. Hint count
            Assert.IsTrue(sudoku.Hints.Count == 1);

            // e. Square assign type
            Assert.IsTrue(hint.Square.AssignType == AssignTypes.Solver);

            // f. Sudoku squares left
            Assert.IsTrue(sudoku.SquaresLeft == 72);
        }

        [TestMethod]
        public void HintsAvailabilityBug()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase18();

            // Test
            // a. Assign an invalid value

            // TODO This is an invalid sudoku
            // Will be solved when we start keeping hints on the squares themselves

            // Since square 9's value is known, this update should not be possible
            // Check this case after start treating hint's availabilities like any other square's availabilities..! - Equal rights to the squares!

            try
            {
                sudoku.UpdateSquare(27, 9);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsNotInstanceOfType(ex, typeof(UnitTestAssertException));
            }
        }
    }
}
