using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SudokuSolver.Engine;

namespace SudokuSolver.Engine.Test
{
    [TestClass]
    public class SudokuTest
    {
        SampleCaseManager caseManager;

        SampleCaseManager CaseManager
        {
            get { return caseManager ?? (caseManager = new SampleCaseManager()); }
        }

        [TestMethod]
        public void SquareMethod_Horizontal()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase_SquareMethod_Horizontal();

            // a. Hint count
            Assert.IsTrue(sudoku.SquareMethodHintSquares.Count() == 1);

            // Get the square
            var square = sudoku.SquareMethodHintSquares.Single();

            // b. Square id
            Assert.IsTrue(square.SquareId == 21);

            // c. Number value
            Assert.IsTrue(square.SudokuNumber.Value == 9);

            // Solve
            sudoku.Solve();

            // d. Hint count
            Assert.IsTrue(!sudoku.SquareMethodHintSquares.Any());

            // e. Square assign type
            Assert.IsTrue(square.AssignType == AssignTypes.Solver);

            // f. Sudoku squares left
            Assert.IsTrue(sudoku.SquaresLeft == 72);
        }

        [TestMethod]
        public void SquareMethod_Vertical()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase_SquareMethod_Vertical();

            // a. Hint count
            Assert.IsTrue(sudoku.SquareMethodHintSquares.Count() == 1);

            // Get the square
            var square = sudoku.SquareMethodHintSquares.Single();

            // b. Square id
            Assert.IsTrue(square.SquareId == 61);

            // c. Number value
            Assert.IsTrue(square.SudokuNumber.Value == 9);

            // Solve
            sudoku.Solve();

            // d. Hint count
            Assert.IsTrue(!sudoku.SquareMethodHintSquares.Any());

            // e. Square assign type
            Assert.IsTrue(square.AssignType == AssignTypes.Solver);

            // f. Sudoku squares left
            Assert.IsTrue(sudoku.SquaresLeft == 72);
        }

        [TestMethod]
        public void SquareMethod_Square()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase_SquareMethod_Square();

            // a. Hint count
            Assert.IsTrue(sudoku.SquareMethodHintSquares.Count() == 1);

            // Get square
            var square = sudoku.SquareMethodHintSquares.Single();

            // b. Square id
            Assert.IsTrue(square.SquareId == 9);

            // c. Number value
            Assert.IsTrue(square.SudokuNumber.Value == 9);

            // Solve
            sudoku.Solve();

            // d. Hint count
            Assert.IsTrue(!sudoku.SquareMethodHintSquares.Any());

            // e. Square assign type
            Assert.IsTrue(square.AssignType == AssignTypes.Solver);

            // f. Sudoku squares left
            Assert.IsTrue(sudoku.SquaresLeft == 72);
        }

        [TestMethod]
        public void SquareMethod_Mixed()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase_SquareMethod_Mixed();

            // a. Hint count
            Assert.IsTrue(sudoku.SquareMethodHintSquares.Count() == 1);

            // Get the square
            var square = sudoku.SquareMethodHintSquares.Single();

            // b. Square id
            Assert.IsTrue(square.SquareId == 1);

            // c. Number value
            Assert.IsTrue(square.SudokuNumber.Value == 9);

            // Solve
            sudoku.Solve();

            // d. Hint count
            Assert.IsTrue(!sudoku.SquareMethodHintSquares.Any());

            // e. Square assign type
            Assert.IsTrue(square.AssignType == AssignTypes.Solver);

            // f. Sudoku squares left
            Assert.IsTrue(sudoku.SquaresLeft == 72);
        }

        [TestMethod]
        public void GroupNumberMethod()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase_GroupNumberMethod();

            // a. Hint count
            Assert.IsTrue(sudoku.GroupNumberMethodHintSquares.Count() == 1);

            // Get the square
            var square = sudoku.GroupNumberMethodHintSquares.Single();

            // b. Square id
            Assert.IsTrue(square.SquareId == 1);

            // c. Hint number value
            Assert.IsTrue(square.SudokuNumber.Value == 1);

            // Solve
            sudoku.Solve();

            // d. Hint count
            Assert.IsTrue(!sudoku.GroupNumberMethodHintSquares.Any());

            // e. Square assign type
            Assert.IsTrue(square.AssignType == AssignTypes.Solver);

            // f. Sudoku squares left
            Assert.IsTrue(sudoku.SquaresLeft == 76);
        }

        [TestMethod]
        public void GroupNumberMethod2()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase_GroupNumberMethod2();

            // a. Hint count
            Assert.IsTrue(sudoku.GroupNumberMethodHintSquares.Count() == 1);

            // Get the square
            var square = sudoku.GroupNumberMethodHintSquares.Single();

            // b. Square id
            Assert.IsTrue(square.SquareId == 81);

            // c. Hint number value
            Assert.IsTrue(square.SudokuNumber.Value == 1);

            // Solve
            sudoku.Solve();

            // d. Hint count
            Assert.IsTrue(!sudoku.GroupNumberMethodHintSquares.Any());

            // e. Square assign type
            Assert.IsTrue(square.AssignType == AssignTypes.Solver);

            // f. Sudoku squares left
            Assert.IsTrue(sudoku.SquaresLeft == 76);
        }

        [TestMethod]
        public void Domino()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase_Domino();

            // a. Hint count
            Assert.IsTrue(sudoku.HintSquares.Count() == 3);

            // b. Square id
            Assert.IsTrue(sudoku.HintSquares.Any(square => square.SquareId == 1));
            Assert.IsTrue(sudoku.HintSquares.Any(square => square.SquareId == 31));

            // c. Number value
            Assert.IsTrue(sudoku.HintSquares.First(square => square.SquareId == 1).SudokuNumber.Value == 1);
            Assert.IsTrue(sudoku.HintSquares.First(square => square.SquareId == 31).SudokuNumber.Value == 2);

            // Solve
            sudoku.Solve();

            // d. Hint count
            Assert.IsTrue(!sudoku.HintSquares.Any());

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
            var sudoku = CaseManager.GetCase_Beauty();

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
        public void HintUpdate()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase_HintUpdate();

            // a. Hint count
            Assert.IsTrue(sudoku.SquareMethodHintSquares.Count() == 1);

            // Get the square
            var square = sudoku.SquareMethodHintSquares.First();

            // b. Hint square value
            Assert.IsTrue(square.SquareId == 21);

            // c. Hint number value; check the number of the hint gets updated
            Assert.IsTrue(square.SudokuNumber.Value == 1);
        }

        [TestMethod]
        public void MiniSize()
        {
            // New sudoku
            var sudoku = CaseManager.GetCase_MiniSize();

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
            var sudoku = CaseManager.GetCase_MaxiSize();

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
            var sudoku = CaseManager.GetCase_RealSudoku();

            // Solve
            sudoku.Solve();

            // Test
            // a. Squares left
            Assert.IsTrue(sudoku.SquaresLeft == 0);
        }

        [TestMethod]
        public void InvalidSudoku()
        {
            // Test
            // a. Invalid sudoku
            try
            {
                var sudoku = CaseManager.GetCase_InvalidSudoku();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
            }
        }

        [TestMethod]
        public void HintsAvailabilityBug()
        {
            // Test
            // a. Invalid sudoku; should not be initialized

            try
            {
                var sudoku = CaseManager.GetCase_ValueRemoveSquareAvailabilityBug();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
            }
        }

        [TestMethod]
        public void HintValueUpdate()
        {
            // Test
            // a. Invalid sudoku; should not be initialized

            try
            {
                var sudoku = CaseManager.GetCase_HintValueUpdate();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
            }
        }

        [TestMethod]
        public void Headache()
        {
            // Test
            // a. Invalid sudoku
            try
            {
                var sudoku = CaseManager.GetCase_Headache();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
            }
        }

        [TestMethod]
        public void Headache2()
        {
            // Test
            // a. Invalid sudoku
            try
            {
                var sudoku = CaseManager.GetCase_Headache2();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
            }
        }

        [TestMethod]
        public void Headache3()
        {
            // Test
            // a. Invalid sudoku
            try
            {
                var sudoku = CaseManager.GetCase_Headache3();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
            }
        }

        [TestMethod]
        public void Headache4()
        {
            // Test
            // a. Invalid sudoku; 
            try
            {
                var sudoku = CaseManager.GetCase_Headache4();
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
            }
        }
    }
}
