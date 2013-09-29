using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace SudokuSolver.Engine.Tests
{
    [TestClass]
    public class SquareMethodTests
    {
        [TestMethod]
        public void Plain()
        {
            // New sudoku
            var sudoku = SquareMethodCaseManager.Plain;

            // Assert: Hint count
            Assert.IsTrue(sudoku.SquareMethodHints.Count() == 1);

            // Get the square
            var square = sudoku.SquareMethodHints.Single();

            // Assert: Square id
            Assert.IsTrue(square.SquareId == 1);

            // Assert: Number value + assign type
            Assert.IsTrue(square.SudokuNumber.Value == 1);
            Assert.IsTrue(square.AssignType == AssignType.Hint);

            // Update the hint to initial
            sudoku.UpdateSquare(1, 1);

            // Assert: Number value + assign type
            Assert.IsTrue(square.SudokuNumber.Value == 1);
            Assert.IsTrue(square.AssignType == AssignType.Initial);

            // Update it back to hint
            sudoku.UpdateSquare(1, 0);

            // Assert: Number value + assign type
            Assert.IsTrue(square.SudokuNumber.Value == 1);
            Assert.IsTrue(square.AssignType == AssignType.Hint);

            // Update id 2 to break the hint
            sudoku.UpdateSquare(2, 0);

            // Assert: Number value + assign type
            Assert.IsTrue(square.SudokuNumber.Value == 0);
            Assert.IsTrue(square.AssignType == AssignType.Initial);

            // And update it back again for a final test
            sudoku.UpdateSquare(2, 2);

            // Assert: Number value + assign type
            Assert.IsTrue(square.SudokuNumber.Value == 1);
            Assert.IsTrue(square.AssignType == AssignType.Hint);

            // Update it to initial
            sudoku.UpdateSquare(1, 1);

            // Assert: Number value + assign type
            Assert.IsTrue(square.SudokuNumber.Value == 1);
            Assert.IsTrue(square.AssignType == AssignType.Initial);

            // Update id 2 and id 3 to zero and back to their values to check that it's still okay
            sudoku.UpdateSquare(2, 0);
            sudoku.UpdateSquare(4, 0);
            sudoku.UpdateSquare(2, 2);
            sudoku.UpdateSquare(4, 3);

            // Assert: Number value + assign type
            Assert.IsTrue(square.SudokuNumber.Value == 1);
            Assert.IsTrue(square.AssignType == AssignType.Initial);

            // And set it back to the state where there will be 1 hint and then solve!
            sudoku.UpdateSquare(1, 0);

            // Assert: Number value + assign type
            Assert.IsTrue(square.SudokuNumber.Value == 1);
            Assert.IsTrue(square.AssignType == AssignType.Hint);

            // Solve
            sudoku.ToggleReady();
            sudoku.Solve();

            // Assert: Hint count
            Assert.IsTrue(!sudoku.SquareMethodHints.Any());

            // Assert: Number value + assign type
            Assert.IsTrue(square.SudokuNumber.Value == 1);
            Assert.IsTrue(square.AssignType == AssignType.Solver);

            // Assert: Sudoku squares left
            Assert.IsTrue(sudoku.SquaresLeft == 72);
        }

        [TestMethod]
        public void FourBirds()
        {
            // New sudoku
            var sudoku = SquareMethodCaseManager.FourBirds;

            // Assert: Hint count
            Assert.IsTrue(!sudoku.SquareMethodHints.Any());

            // Update Id 5 with Number 5 for the chain update
            sudoku.UpdateSquare(5, 5);

            // Assert: Hint count
            Assert.IsTrue(sudoku.SquareMethodHints.Count() == 4);

            // Assert: Control square values
            Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 1).SudokuNumber.Value == 1);
            Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 3).SudokuNumber.Value == 3);
            Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 7).SudokuNumber.Value == 7);
            Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 9).SudokuNumber.Value == 9);

            // Update Id 5 to remove the hints
            sudoku.UpdateSquare(5, 0);

            // Assert: Hint count
            Assert.IsTrue(!sudoku.SquareMethodHints.Any());
        }

        [TestMethod]
        public void FourBirdsReverse()
        {
            // New sudoku
            var sudoku = SquareMethodCaseManager.FourBirdsReverse;

            // Assert: Hint count
            Assert.IsTrue(sudoku.SquareMethodHints.Count() == 5);

            // Update Id 11 with 0 for the chain update
            sudoku.UpdateSquare(11, 0);

            // Assert: Hint count
            Assert.IsTrue(sudoku.SquareMethodHints.Count() == 2);

            // Assert: Control square values
            Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 1).SudokuNumber.Value == 0);
            Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 3).SudokuNumber.Value == 0);
            Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 5).SudokuNumber.Value == 0);
        }

        [TestMethod]
        public void FourBirdsExtended()
        {
            // New sudoku
            var sudoku = SquareMethodCaseManager.FourBirdsExtended;

            // Assert: Hint count
            Assert.IsTrue(sudoku.SquareMethodHints.Count() == 10);

            // Update Id 11 with 0 for the chain update
            sudoku.UpdateSquare(11, 0);

            // Assert: Hint count
            Assert.IsTrue(sudoku.SquareMethodHints.Count() == 2);

            // Assert: Control square values
            Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 1).SudokuNumber.Value == 0);
            Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 3).SudokuNumber.Value == 0);
            Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 5).SudokuNumber.Value == 0);
            Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 7).SudokuNumber.Value == 7);
            Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 9).SudokuNumber.Value == 9);

            Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 29).SudokuNumber.Value == 0);
            Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 32).SudokuNumber.Value == 0);
            Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 33).SudokuNumber.Value == 0);
            Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 35).SudokuNumber.Value == 0);

            Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 43).SudokuNumber.Value == 0);
        }

        [TestMethod]
        public void FourBirdsExtended2()
        {
            // New sudoku
            var sudoku = SquareMethodCaseManager.FourBirdsExtended2;

            // Assert: Hint count
            Assert.IsTrue(sudoku.SquareMethodHints.Count() == 9);

            // Update Id 32 with 0 for the chain update
            sudoku.UpdateSquare(32, 0);

            // Assert: Hint count
            Assert.IsTrue(sudoku.SquareMethodHints.Count() == 10);

            // Assert: Control square values
            Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 1).SudokuNumber.Value == 1);
            Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 3).SudokuNumber.Value == 3);
            Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 5).SudokuNumber.Value == 5);
            Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 7).SudokuNumber.Value == 7);
            Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 9).SudokuNumber.Value == 9);

            Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 29).SudokuNumber.Value == 7);
            Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 32).SudokuNumber.Value == 9);
            Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 33).SudokuNumber.Value == 5);
            Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 35).SudokuNumber.Value == 4);

            Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 43).SudokuNumber.Value == 5);
        }

        [TestMethod]
        public void FourBirdsExtended2_v2()
        {
            // New sudoku
            var sudoku = SquareMethodCaseManager.FourBirdsExtended2;

            // Assert: Hint count
            Assert.IsTrue(sudoku.SquareMethodHints.Count() == 9);

            // Update Id 32 with 0 for the chain update
            sudoku.UpdateSquare(26, 0);
            sudoku.UpdateSquare(32, 0);

            // Assert: Hint count
            Assert.IsTrue(sudoku.SquareMethodHints.Count() == 2);

            // Assert: Control square values
            //Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 1).SudokuNumber.Value == 1);
            //Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 3).SudokuNumber.Value == 3);
            //Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 5).SudokuNumber.Value == 5);
            //Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 7).SudokuNumber.Value == 7);
            //Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 9).SudokuNumber.Value == 9);

            //Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 29).SudokuNumber.Value == 7);
            //Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 32).SudokuNumber.Value == 9);
            //Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 33).SudokuNumber.Value == 5);
            //Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 35).SudokuNumber.Value == 4);

            //Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 43).SudokuNumber.Value == 5);
        }

        [TestMethod]
        public void TieBreak()
        {
            // New sudoku
            var sudoku = SquareMethodCaseManager.TieBreak;

            // Update Id 1 with Number 1 for the chain update
            sudoku.UpdateSquare(1, 1);

            // Assert: Hint count
            Assert.IsTrue(sudoku.SquareMethodHints.Count() == 3);
            
            // Update Id 1 with 0 to undo it
            sudoku.UpdateSquare(1, 0);

            // Assert: Control square values
            Assert.IsTrue(sudoku.SquareMethodHints.Count() == 0);

            // Assert: Id 1 should be zero
            Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 1).SudokuNumber.Value == 0);
        }
    }
}
