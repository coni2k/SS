using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace SudokuSolver.Engine.Tests
{
    [TestClass]
    public class SudokuTest
    {
        [TestMethod]
        public void NormalSize()
        {
            // New sudoku
            var sudoku = new Sudoku();

            // Assert: Total size
            Assert.IsTrue(sudoku.TotalSize == 81);

            // Assert: Squares left
            Assert.IsTrue(sudoku.SquaresLeft == 81);
        }

        [TestMethod]
        public void MiniSize()
        {
            // New sudoku
            var sudoku = SampleCaseManager.MiniSize;

            // Assert: Total size
            Assert.IsTrue(sudoku.TotalSize == 16);

            // Assert: Squares left
            Assert.IsTrue(sudoku.SquaresLeft == 16);
        }

        [TestMethod]
        public void MaxiSize()
        {
            // New sudoku
            var sudoku = SampleCaseManager.MaxiSize;

            // Assert: Total size
            Assert.IsTrue(sudoku.TotalSize == 256);

            // Assert: Squares left
            Assert.IsTrue(sudoku.SquaresLeft == 256);
        }

        [TestMethod]
        public void IsAlreadyAssigned()
        {
            // New sudoku
            var sudoku = new Sudoku();

            // Get the control squares + number
            var squareId1 = sudoku.Squares.Single(square => square.SquareId == 1);
            var squareId9 = sudoku.Squares.Single(square => square.SquareId == 9);
            var squareId21 = sudoku.Squares.Single(square => square.SquareId == 21);
            var squareId61 = sudoku.Squares.Single(square => square.SquareId == 61);
            var number1 = sudoku.Numbers.Single(number => number.Value == 1);

            // Assert: Is already in use? Should be false
            Assert.IsFalse(sudoku.IsNumberAlreadyInUse(squareId1, number1));

            Assert.IsFalse(sudoku.IsNumberAlreadyInUse(squareId9, number1));
            Assert.IsFalse(sudoku.IsNumberAlreadyInUse(squareId21, number1));
            Assert.IsFalse(sudoku.IsNumberAlreadyInUse(squareId61, number1));

            // Update
            sudoku.UpdateSquare(1, 1);

            // Assert: Is already in use? Should be true this time
            Assert.IsFalse(sudoku.IsNumberAlreadyInUse(squareId1, number1));

            Assert.IsTrue(sudoku.IsNumberAlreadyInUse(squareId9, number1));
            Assert.IsTrue(sudoku.IsNumberAlreadyInUse(squareId21, number1));
            Assert.IsTrue(sudoku.IsNumberAlreadyInUse(squareId61, number1));

            // Update the first group to test it against a hint
            sudoku.UpdateSquare(2, 2);
            sudoku.UpdateSquare(3, 3);
            sudoku.UpdateSquare(4, 4);
            sudoku.UpdateSquare(5, 5);
            sudoku.UpdateSquare(6, 6);
            sudoku.UpdateSquare(7, 7);
            sudoku.UpdateSquare(8, 8);

            // Assert: Number value + assign type
            Assert.IsTrue(squareId9.SudokuNumber.Value == 9);
            Assert.IsTrue(squareId9.AssignType == AssignType.Hint);

            // Assert: Is already in use? Should be false against a hint
            var number9 = sudoku.Numbers.Single(number => number.Value == 9);
            Assert.IsFalse(sudoku.IsNumberAlreadyInUse(squareId1, number9));
        }
    }
}
