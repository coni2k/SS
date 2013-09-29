﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace SudokuSolver.Engine.Tests
{
    [TestClass]
    public class SudokuTests
    {
        [TestMethod]
        public void NumberMethod()
        {
            // New sudoku
            var sudoku = SampleCaseManager.NumberMethod;

            // a. Hint count
            Assert.IsTrue(sudoku.NumberMethodHints.Count() == 1);

            // Get the square
            var square = sudoku.NumberMethodHints.Single();

            // b. Square id
            Assert.IsTrue(square.SquareId == 1);

            // c. Hint number value
            Assert.IsTrue(square.SudokuNumber.Value == 1);

            // Solve
            sudoku.ToggleReady();
            sudoku.Solve();

            // d. Hint count
            Assert.IsTrue(!sudoku.NumberMethodHints.Any());

            // e. Square assign type
            Assert.IsTrue(square.AssignType == AssignType.Solver);

            // f. Sudoku squares left
            Assert.IsTrue(sudoku.SquaresLeft == 76);
        }

        [TestMethod]
        public void GroupNumberMethodWithHelp()
        {
            // New sudoku
            var sudoku = SampleCaseManager.GroupNumberMethodWithHelp;

            // a. Hint count
            Assert.IsTrue(sudoku.NumberMethodHints.Count() == 1);

            // Get the square
            var square = sudoku.NumberMethodHints.Single();

            // b. Square id
            Assert.IsTrue(square.SquareId == 81);

            // c. Hint number value
            Assert.IsTrue(square.SudokuNumber.Value == 1);

            // Solve
            sudoku.ToggleReady();
            sudoku.Solve();

            // d. Hint count
            Assert.IsTrue(!sudoku.NumberMethodHints.Any());

            // e. Square assign type
            Assert.IsTrue(square.AssignType == AssignType.Solver);

            // f. Sudoku squares left
            Assert.IsTrue(sudoku.SquaresLeft == 76);
        }

        [TestMethod]
        public void DominoMixed()
        {
            // TODO UPDATE
            Assert.Fail();
            return;

            // New sudoku
            var sudoku = SampleCaseManager.DominoMixed;

            // a. Hint count
            Assert.IsTrue(sudoku.Hints.Count() == 3);

            // b. Square id
            Assert.IsTrue(sudoku.Hints.Any(square => square.SquareId == 1));
            Assert.IsTrue(sudoku.Hints.Any(square => square.SquareId == 31));

            // c. Number value
            Assert.IsTrue(sudoku.Hints.First(square => square.SquareId == 1).SudokuNumber.Value == 1);
            Assert.IsTrue(sudoku.Hints.First(square => square.SquareId == 31).SudokuNumber.Value == 2);

            // Solve
            sudoku.ToggleReady();
            sudoku.Solve();

            // d. Hint count
            Assert.IsTrue(!sudoku.Hints.Any());

            // e. Square assign type
            Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 1).AssignType == AssignType.Solver);
            Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 31).AssignType == AssignType.Solver);
            Assert.IsTrue(sudoku.Squares.Single(square => square.SquareId == 54).AssignType == AssignType.Solver);

            // b. Squares left
            Assert.IsTrue(sudoku.SquaresLeft == 66);
        }

        [TestMethod]
        public void BeautyReverse()
        {
            // TODO UPDATE
            Assert.Fail();
            return;

            // New sudoku
            var sudoku = SquareMethodCaseManager.FourBirds;

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

            // Try to return to an earlier state; hints should to be removed
            // TODO THE LINE BELOW GETS STACKOVERFLOW EXCEPTION SO COMMENTED OUT BUT THE TEST SHOULD WORK WITH THIS LINE ACTUALLY
            // FIX IT AND TEST IT AGAIN
            //sudoku.UpdateSquare(34, 0);

            // b. Squares left
            // Assert.IsTrue(sudoku.SquaresLeft == 64);
        }

        [TestMethod]
        public void HintUpdate()
        {
            // TODO UPDATE
            Assert.Fail();
            return;

            // New sudoku
            var sudoku = SampleCaseManager.HintUpdate;

            // a. Hint count
            Assert.IsTrue(sudoku.SquareMethodHints.Count() == 1);

            // Get the square
            var square = sudoku.SquareMethodHints.First();

            // b. Hint square value
            Assert.IsTrue(square.SquareId == 21);

            // c. Hint number value; check the number of the hint gets updated
            Assert.IsTrue(square.SudokuNumber.Value == 1);
        }

        [TestMethod]
        public void HintUpdate2()
        {
            // New sudoku
            var sudoku = SampleCaseManager.HintUpdate2;

            // a. Hint count
            //Assert.IsTrue(sudoku.GroupNumberMethodHintSquares.Count() == 2);

            // Get the square
            //var square = sudoku.SquareMethodHintSquares.First();

            // b. Hint square value
            //Assert.IsTrue(square.SquareId == 21);

            // c. Hint number value; check the number of the hint gets updated
            //Assert.IsTrue(square.SudokuNumber.Value == 1);
        }

        [TestMethod]
        public void RealCase()
        {
            // TODO UPDATE
            Assert.Fail();
            return;

            // New sudoku
            var sudoku = SampleCaseManager.RealSudoku;

            // Solve
            sudoku.Solve();

            // Test
            // a. Squares left
            Assert.IsTrue(sudoku.SquaresLeft == 0);
        }

        [TestMethod]
        public void InvalidSudoku()
        {
            // TODO UPDATE
            Assert.Fail();
            return;

            // Test
            // a. Invalid sudoku
            try
            {
                SampleCaseManager.InvalidSudoku.Solve();
                Assert.Fail();
            }
            catch (InvalidOperationException) { }
        }

        [TestMethod]
        public void HintsAvailabilityBug()
        {
            // TODO UPDATE
            Assert.Fail();
            return;

            // Test
            // a. Invalid sudoku; should not be initialized

            try
            {
                var sudoku = SampleCaseManager.HintAvailabilityBug;
                sudoku.UpdateSquare(27, 9);
                Assert.Fail();
            }
            catch (InvalidOperationException) { }
        }

        [TestMethod]
        public void ValueRemoveSquareAvailabilityBug()
        {
            // TODO UPDATE
            Assert.Fail();
            return;

            // Test
            // a. Invalid sudoku; should not be initialized

            try
            {
                SampleCaseManager.ValueRemoveSquareAvailabilityBug.Solve();
                Assert.Fail();
            }
            catch (InvalidOperationException) { }
        }

        [TestMethod]
        public void HintValueUpdate()
        {
            // TODO UPDATE
            Assert.Fail();
            return;

            // Test
            // a. Invalid sudoku; should not be initialized

            try
            {
                SampleCaseManager.HintValueUpdate.Solve();
                Assert.Fail();
            }
            catch (InvalidOperationException) { }
        }

        //[TestMethod]
        //public void Headache()
        //{
        //    // Test
        //    // a. Invalid sudoku
        //    try
        //    {
        //        SampleCaseManager.Headache.Solve();
        //        Assert.Fail();
        //    }
        //    catch (InvalidOperationException) { }
        //}

        //[TestMethod]
        //public void Headache2()
        //{
        //    // Test
        //    // a. Invalid sudoku
        //    try
        //    {
        //        SampleCaseManager.Headache2.Solve();
        //        Assert.Fail();
        //    }
        //    catch (InvalidOperationException) { }
        //}

        //[TestMethod]
        //public void Headache3()
        //{
        //    // Test
        //    // a. Invalid sudoku
        //    try
        //    {
        //        SampleCaseManager.Headache3.Solve();
        //        Assert.Fail();
        //    }
        //    catch (InvalidOperationException) { }
        //}

        //[TestMethod]
        //public void Headache4()
        //{
        //    // Test
        //    // a. Invalid sudoku; 
        //    try
        //    {
        //        SampleCaseManager.Headache4.Solve();
        //        Assert.Fail();
        //    }
        //    catch (InvalidOperationException) { }
        //}

        [TestMethod]
        public void GroupNumberRemoveHintBug()
        {
            // TODO ?!

            // Test
            // a. Bug hunt!
            //try
            //{
            var sudoku = SampleCaseManager.GroupNumberRemoveHintBug;
            //    Assert.Fail();
            //}
            //catch (Exception ex)
            //{
            //    Assert.IsInstanceOfType(ex, typeof(InvalidOperationException));
            //}
        }

        [TestMethod]
        public void GroupNumberHintUpdate()
        {
            // New sudoku
            var sudoku = SampleCaseManager.NumberMethod;

            // a. Hint count
            Assert.IsTrue(sudoku.NumberMethodHints.Count() == 1);

            // Get the square
            var square = sudoku.NumberMethodHints.Single();

            // b. Square id
            Assert.IsTrue(square.SquareId == 1);

            // c. Hint number value
            Assert.IsTrue(square.SudokuNumber.Value == 1);

            sudoku.UpdateSquare(13, 0);

            // Solve
            //sudoku.Solve();

            //// d. Hint count
            //Assert.IsTrue(!sudoku.GroupNumberMethodHintSquares.Any());

            //// e. Square assign type
            //Assert.IsTrue(square.AssignType == AssignTypes.Solver);

            //// f. Sudoku squares left
            //Assert.IsTrue(sudoku.SquaresLeft == 76);

        }
    }
}
