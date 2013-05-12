using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver.Engine;

namespace SudokuSolver.Engine.Test
{
    [TestClass]
    public class SudokuTest
    {
        CaseManager CaseManager
        {
            get { return new CaseManager(); }
        }

        [TestMethod]
        public void TestCase1()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase1();

            // Solve
            sudoku.Solve();

            // Get the control square
            var controlSquare = sudoku.GetSquares().Single(square => square.SquareId == 21);
            
            // Test
            // a. Control square's value
            Assert.IsTrue(controlSquare.SudokuNumber.Value == 9);

            // b. Squares left
            Assert.IsTrue(sudoku.SquaresLeft == 72);
        }

        [TestMethod]
        public void TestCase2()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase2();

            // Solve
            sudoku.Solve();

            // Get the control square
            var controlSquare = sudoku.GetSquares().Single(square => square.SquareId == 61);

            // Test
            // a. Control square's value
            Assert.IsTrue(controlSquare.SudokuNumber.Value == 9);

            // b. Squares left
            Assert.IsTrue(sudoku.SquaresLeft == 72);
        }

        [TestMethod]
        public void TestCase3()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase3();

            // Solve
            sudoku.Solve();

            // Get the control square
            var controlSquare = sudoku.GetSquares().Single(square => square.SquareId == 9);

            // Test
            // a. Control square's value
            Assert.IsTrue(controlSquare.SudokuNumber.Value == 9);

            // b. Squares left
            Assert.IsTrue(sudoku.SquaresLeft == 72);
        }

        [TestMethod]
        public void TestCase4()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase4();

            // Solve
            sudoku.Solve();

            // Get the control square
            var controlSquare = sudoku.GetSquares().Single(square => square.SquareId == 21);

            // Test
            // a. Control square's value
            Assert.IsTrue(controlSquare.SudokuNumber.Value == 9);

            // b. Squares left
            Assert.IsTrue(sudoku.SquaresLeft == 72);
        }

        [TestMethod]
        public void TestCase5()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase5();

            // Solve
            sudoku.Solve();

            // Get the control square
            var controlSquare = sudoku.GetSquares().Single(square => square.SquareId == 1);

            // Test
            // a. Control square's value
            Assert.IsTrue(controlSquare.SudokuNumber.Value == 1);

            // b. Squares left
            Assert.IsTrue(sudoku.SquaresLeft == 76);
        }

        [TestMethod]
        public void TestCase6()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase6();

            // Solve
            sudoku.Solve();

            // Get the control square
            var controlSquare = sudoku.GetSquares().Single(square => square.SquareId == 81);

            // Test
            // a. Control square's value
            Assert.IsTrue(controlSquare.SudokuNumber.Value == 1);

            // b. Squares left
            Assert.IsTrue(sudoku.SquaresLeft == 76);
        }

        [TestMethod]
        public void TestCase7()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase7();

            // Solve
            sudoku.Solve();

            // Get the control squares
            var controlSquare1 = sudoku.GetSquares().Single(square => square.SquareId == 1);
            var controlSquare2 = sudoku.GetSquares().Single(square => square.SquareId == 31);
            var controlSquare3 = sudoku.GetSquares().Single(square => square.SquareId == 54);

            // Test
            // a. Control squares values
            Assert.IsTrue(controlSquare1.SudokuNumber.Value == 1);
            Assert.IsTrue(controlSquare2.SudokuNumber.Value == 2);
            Assert.IsTrue(controlSquare3.SudokuNumber.Value == 2);

            // b. Squares left
            Assert.IsTrue(sudoku.SquaresLeft == 64);
        }

        [TestMethod]
        public void TestCase8()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase8();

            // Solve
            sudoku.Solve();

            // Get the control squares
            var controlSquare1 = sudoku.GetSquares().Single(square => square.SquareId == 1);
            var controlSquare2 = sudoku.GetSquares().Single(square => square.SquareId == 3);
            var controlSquare3 = sudoku.GetSquares().Single(square => square.SquareId == 7);
            var controlSquare4 = sudoku.GetSquares().Single(square => square.SquareId == 9);

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
        public void TestCase9()
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
        public void TestCase10()
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
        public void TestCase11()
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
        public void TestCase12()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase12();
            
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
        public void TestCase13()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase13();

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
        public void TestCase14()
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
                Assert.IsNotInstanceOfType(ex, typeof(UnitTestAssertException));
            }
        }

        [TestMethod]
        public void TestCase15()
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
                Assert.IsNotInstanceOfType(ex, typeof(UnitTestAssertException));
            }
        }

        [TestMethod]
        public void TestCase16()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase16();

            // Test
            // a. ?
            // TODO!

            // Solved
        }

        [TestMethod]
        public void TestCase17()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase17();

            // Test
            // a. ?
            // TODO!

            // Solved
        }

        [TestMethod]
        public void TestCase18()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase18();

            // Test
            // a. ?
            // TODO!
        }
    }
}
