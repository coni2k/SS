using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace SudokuSolver.Engine
{
    public class CaseManager
    {
        /// <summary>
        /// 1. Type - Horizontal - 8
        /// </summary>
        /// <returns></returns>
        Sudoku Case1()
        {
            var sudoku = new Sudoku();
            sudoku.SudokuId = 1;
            sudoku.Title = "1. Type - Horizontal - 8";

            // Horizontal ID version
            //sudoku.UpdateSquare(1, 1);
            //sudoku.UpdateSquare(2, 2);
            //sudoku.UpdateSquare(3, 3);
            //sudoku.UpdateSquare(4, 4);
            //sudoku.UpdateSquare(5, 5);
            //sudoku.UpdateSquare(6, 6);
            //sudoku.UpdateSquare(7, 7);
            //sudoku.UpdateSquare(8, 8);

            // Square ID version
            sudoku.UpdateSquare(1, 1);
            sudoku.UpdateSquare(2, 2);
            sudoku.UpdateSquare(3, 3);
            sudoku.UpdateSquare(10, 4);
            sudoku.UpdateSquare(11, 5);
            sudoku.UpdateSquare(12, 6);
            sudoku.UpdateSquare(19, 7);
            sudoku.UpdateSquare(20, 8);

            sudoku.ToggleReady();

            return sudoku;
        }

        /// <summary>
        /// 1. Type - Vertical - 8
        /// </summary>
        /// <returns></returns>
        Sudoku Case2()
        {
            var sudoku = new Sudoku();

            sudoku.SudokuId = 2;
            sudoku.Title = "1. Type - Vertical - 8";

            // Horizontal ID version
            //sudoku.UpdateSquare(1, 1);
            //sudoku.UpdateSquare(10, 2);
            //sudoku.UpdateSquare(19, 3);
            //sudoku.UpdateSquare(28, 4);
            //sudoku.UpdateSquare(37, 5);
            //sudoku.UpdateSquare(46, 6);
            //sudoku.UpdateSquare(55, 7);
            //sudoku.UpdateSquare(64, 8);

            // Square ID version
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

        /// <summary>
        /// 1. Type - Square - 8
        /// </summary>
        /// <returns></returns>
        Sudoku Case3()
        {
            var sudoku = new Sudoku();

            sudoku.SudokuId = 3;
            sudoku.Title = "1. Type - Square - 8";

            // Horizontal ID version
            //sudoku.UpdateSquare(1, 1);
            //sudoku.UpdateSquare(2, 2);
            //sudoku.UpdateSquare(3, 3);
            //sudoku.UpdateSquare(10, 4);
            //sudoku.UpdateSquare(11, 5);
            //sudoku.UpdateSquare(12, 6);
            //sudoku.UpdateSquare(19, 7);
            //sudoku.UpdateSquare(20, 8);

            // Square ID version
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

        /// <summary>
        /// 1. Type - Mixed - 8
        /// </summary>
        /// <returns></returns>
        Sudoku Case4()
        {
            var sudoku = new Sudoku();

            sudoku.SudokuId = 4;
            sudoku.Title = "1. Type - Mixed - 8";

            // Horizontal ID version
            //sudoku.UpdateSquare(1, 1);
            //sudoku.UpdateSquare(2, 2);
            //sudoku.UpdateSquare(3, 3);
            //sudoku.UpdateSquare(8, 7);
            //sudoku.UpdateSquare(18, 8);
            //sudoku.UpdateSquare(36, 4);
            //sudoku.UpdateSquare(45, 5);
            //sudoku.UpdateSquare(54, 6);

            // Square ID version
            sudoku.UpdateSquare(1, 1);
            sudoku.UpdateSquare(2, 2);
            sudoku.UpdateSquare(3, 3);
            sudoku.UpdateSquare(20, 7);
            sudoku.UpdateSquare(24, 8);
            sudoku.UpdateSquare(48, 4);
            sudoku.UpdateSquare(51, 5);
            sudoku.UpdateSquare(54, 6);

            sudoku.ToggleReady();

            return sudoku;
        }

        /// <summary>
        /// 2. Type - Straight 1
        /// </summary>
        /// <returns></returns>
        Sudoku Case5()
        {
            var sudoku = new Sudoku();

            sudoku.SudokuId = 5;
            sudoku.Title = "2. Type - Straight 1";

            // Horizontal ID version
            //sudoku.UpdateSquare(13, 1);
            //sudoku.UpdateSquare(25, 1);
            //sudoku.UpdateSquare(29, 1);
            //sudoku.UpdateSquare(57, 1);

            // Square ID version
            sudoku.UpdateSquare(13, 1);
            sudoku.UpdateSquare(25, 1);
            sudoku.UpdateSquare(29, 1);
            sudoku.UpdateSquare(57, 1);

            sudoku.ToggleReady();

            return sudoku;
        }

        /// <summary>
        /// Mixed Types (triggers strange RelatedNumbers block)
        /// </summary>
        /// <returns></returns>
        Sudoku Case6()
        {
            var sudoku = new Sudoku();

            sudoku.SudokuId = 6;
            sudoku.Title = "Mixed Types (triggers strange RelatedNumbers block)";

            // Horizontal ID version
            //sudoku.UpdateSquare(25, 1);
            //sudoku.UpdateSquare(57, 1);
            //sudoku.UpdateSquare(69, 1);
            //sudoku.UpdateSquare(80, 2);

            // Square ID version
            sudoku.UpdateSquare(25, 1);
            sudoku.UpdateSquare(57, 1);
            sudoku.UpdateSquare(69, 1);
            sudoku.UpdateSquare(80, 2);

            sudoku.ToggleReady();

            return sudoku;
        }

        /// <summary>
        /// Domino
        /// </summary>
        /// <returns></returns>
        Sudoku Case7()
        {
            var sudoku = new Sudoku();

            sudoku.SudokuId = 7;
            sudoku.Title = "Domino";

            // Horizontal ID version
            //sudoku.UpdateSquare(10, 3);
            //sudoku.UpdateSquare(13, 1);
            //sudoku.UpdateSquare(17, 2);
            //sudoku.UpdateSquare(19, 4);
            //sudoku.UpdateSquare(25, 1);
            //sudoku.UpdateSquare(28, 5);
            //sudoku.UpdateSquare(29, 1);
            //sudoku.UpdateSquare(31, 2);
            //sudoku.UpdateSquare(46, 6);
            //sudoku.UpdateSquare(55, 7);
            //sudoku.UpdateSquare(57, 1);
            //sudoku.UpdateSquare(61, 2);
            //sudoku.UpdateSquare(64, 8);
            //sudoku.UpdateSquare(73, 9);

            // Square ID version
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

            sudoku.ToggleReady();

            return sudoku;
        }

        /// <summary>
        /// Beauty
        /// </summary>
        /// <returns></returns>
        Sudoku Case8()
        {
            var sudoku = new Sudoku();

            sudoku.SudokuId = 8;
            sudoku.Title = "Beauty";

            // Horizontal ID version
            //sudoku.UpdateSquare(2, 2);
            //sudoku.UpdateSquare(10, 4);
            //sudoku.UpdateSquare(11, 5);
            //sudoku.UpdateSquare(12, 6);
            //sudoku.UpdateSquare(20, 8);
            //sudoku.UpdateSquare(4, 9);
            //sudoku.UpdateSquare(6, 7);
            //sudoku.UpdateSquare(22, 3);
            //sudoku.UpdateSquare(24, 1);
            //sudoku.UpdateSquare(28, 3);
            //sudoku.UpdateSquare(30, 1);
            //sudoku.UpdateSquare(46, 9);
            //sudoku.UpdateSquare(48, 7);

            // Square ID version
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

            sudoku.ToggleReady();

            return sudoku;
        }

        /// <summary>
        /// Mini size
        /// </summary>
        /// <returns></returns>
        Sudoku Case9()
        {
            var sudoku = new Sudoku(4);

            sudoku.SudokuId = 9;
            sudoku.Title = "Mini size";

            sudoku.ToggleReady();

            return sudoku;
        }

        /// <summary>
        /// Mini size
        /// </summary>
        /// <returns></returns>
        Sudoku Case10()
        {
            var sudoku = new Sudoku(16);

            sudoku.SudokuId = 10;
            sudoku.Title = "Maxi size";

            sudoku.ToggleReady();

            return sudoku;
        }

        /// <summary>
        /// Real case
        /// </summary>
        /// <returns></returns>
        Sudoku Case11()
        {
            var sudoku = new Sudoku();

            sudoku.SudokuId = 11;
            sudoku.Title = "Real case";

            // Horizontal ID version
            //sudoku.UpdateSquare(3, 8);
            //sudoku.UpdateSquare(4, 3);
            //sudoku.UpdateSquare(5, 4);
            //sudoku.UpdateSquare(6, 2);
            //sudoku.UpdateSquare(7, 9);
            //sudoku.UpdateSquare(12, 9);
            //sudoku.UpdateSquare(16, 7);
            //sudoku.UpdateSquare(19, 4);
            //sudoku.UpdateSquare(27, 3);
            //sudoku.UpdateSquare(30, 6);
            //sudoku.UpdateSquare(31, 4);
            //sudoku.UpdateSquare(32, 7);
            //sudoku.UpdateSquare(33, 3);
            //sudoku.UpdateSquare(34, 2);
            //sudoku.UpdateSquare(38, 3);
            //sudoku.UpdateSquare(44, 1);
            //sudoku.UpdateSquare(48, 2);
            //sudoku.UpdateSquare(49, 8);
            //sudoku.UpdateSquare(50, 5);
            //sudoku.UpdateSquare(51, 1);
            //sudoku.UpdateSquare(52, 6);
            //sudoku.UpdateSquare(55, 7);
            //sudoku.UpdateSquare(63, 8);
            //sudoku.UpdateSquare(66, 4);
            //sudoku.UpdateSquare(70, 1);
            //sudoku.UpdateSquare(75, 3);
            //sudoku.UpdateSquare(76, 6);
            //sudoku.UpdateSquare(77, 9);
            //sudoku.UpdateSquare(78, 7);
            //sudoku.UpdateSquare(79, 5);

            // Square ID version
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

        /// <summary>
        /// Headache (1-2-3)
        /// </summary>
        /// <returns></returns>
        Sudoku Case12()
        {
            var sudoku = new Sudoku();

            sudoku.SudokuId = 12;
            sudoku.Title = "Headache (1-2-3)";

            // Horizontal ID version
            //sudoku.UpdateSquare(1, 1);
            //sudoku.UpdateSquare(2, 2);
            //sudoku.UpdateSquare(3, 3);
            //sudoku.UpdateSquare(13, 1);
            //sudoku.UpdateSquare(14, 2);
            //sudoku.UpdateSquare(15, 3);

            // Square ID version
            sudoku.UpdateSquare(1, 1);
            sudoku.UpdateSquare(2, 2);
            sudoku.UpdateSquare(3, 3);
            sudoku.UpdateSquare(13, 1);
            sudoku.UpdateSquare(14, 2);
            sudoku.UpdateSquare(15, 3);

            sudoku.ToggleReady();

            return sudoku;
        }

        /// <summary>
        /// Headache 2
        /// </summary>
        /// <returns></returns>
        Sudoku Case13()
        {
            var sudoku = new Sudoku();

            sudoku.SudokuId = 13;
            sudoku.Title = "Headache 2";

            // Horizontal ID version
            //sudoku.UpdateSquare(1, 1);
            //sudoku.UpdateSquare(13, 1);
            //sudoku.UpdateSquare(25, 2);
            //sudoku.UpdateSquare(26, 3);

            // Square ID version
            sudoku.UpdateSquare(1, 1);
            sudoku.UpdateSquare(13, 1);
            sudoku.UpdateSquare(25, 2);
            sudoku.UpdateSquare(26, 3);

            sudoku.ToggleReady();

            return sudoku;
        }

        /// <summary>
        /// Wrong
        /// </summary>
        /// <returns></returns>
        Sudoku Case14()
        {
            var sudoku = new Sudoku();

            sudoku.SudokuId = 14;
            sudoku.Title = "Wrong";

            // Horizontal ID version
            //sudoku.UpdateSquare(1, 9);
            //sudoku.UpdateSquare(2, 7);
            //sudoku.UpdateSquare(3, 3);
            //sudoku.UpdateSquare(4, 6);
            //sudoku.UpdateSquare(5, 2);
            //sudoku.UpdateSquare(6, 8);
            //sudoku.UpdateSquare(7, 4);
            //sudoku.UpdateSquare(8, 5);
            //sudoku.UpdateSquare(9, 1);
            //sudoku.UpdateSquare(10, 5);
            //sudoku.UpdateSquare(11, 6);
            //sudoku.UpdateSquare(12, 8);
            //sudoku.UpdateSquare(13, 4);
            //sudoku.UpdateSquare(14, 1);
            //sudoku.UpdateSquare(15, 3);
            //sudoku.UpdateSquare(16, 7);
            //sudoku.UpdateSquare(17, 9);
            //sudoku.UpdateSquare(18, 2);
            //sudoku.UpdateSquare(19, 1);
            //sudoku.UpdateSquare(20, 2);
            //sudoku.UpdateSquare(21, 4);
            //sudoku.UpdateSquare(22, 5);
            //sudoku.UpdateSquare(23, 9);
            //sudoku.UpdateSquare(24, 7);
            //sudoku.UpdateSquare(25, 8);
            //sudoku.UpdateSquare(26, 3);
            //sudoku.UpdateSquare(27, 6);
            //sudoku.UpdateSquare(28, 8);
            //sudoku.UpdateSquare(29, 1);
            //sudoku.UpdateSquare(30, 5);
            //sudoku.UpdateSquare(31, 7);
            //sudoku.UpdateSquare(33, 9);
            //sudoku.UpdateSquare(34, 6);
            //sudoku.UpdateSquare(35, 4);
            //sudoku.UpdateSquare(36, 3);
            //sudoku.UpdateSquare(37, 6);
            //sudoku.UpdateSquare(38, 4);
            //sudoku.UpdateSquare(39, 9);
            //sudoku.UpdateSquare(40, 3);
            //sudoku.UpdateSquare(41, 5);
            //sudoku.UpdateSquare(42, 2);
            //sudoku.UpdateSquare(43, 1);
            //sudoku.UpdateSquare(44, 7);
            //sudoku.UpdateSquare(45, 8);
            //sudoku.UpdateSquare(46, 2);
            //sudoku.UpdateSquare(47, 3);
            //sudoku.UpdateSquare(48, 7);
            //sudoku.UpdateSquare(49, 1);
            //sudoku.UpdateSquare(50, 6);
            //sudoku.UpdateSquare(51, 4);
            //sudoku.UpdateSquare(52, 9);
            //sudoku.UpdateSquare(54, 5);
            //sudoku.UpdateSquare(55, 4);
            //sudoku.UpdateSquare(56, 5);
            //sudoku.UpdateSquare(57, 2);
            //sudoku.UpdateSquare(58, 8);
            //sudoku.UpdateSquare(59, 7);
            //sudoku.UpdateSquare(61, 3);
            //sudoku.UpdateSquare(62, 6);
            //sudoku.UpdateSquare(63, 9);
            //sudoku.UpdateSquare(64, 7);
            //sudoku.UpdateSquare(65, 8);
            //sudoku.UpdateSquare(66, 6);
            //sudoku.UpdateSquare(67, 9);
            //sudoku.UpdateSquare(68, 3);
            //sudoku.UpdateSquare(69, 1);
            //sudoku.UpdateSquare(70, 5);
            //sudoku.UpdateSquare(71, 2);
            //sudoku.UpdateSquare(72, 4);
            //sudoku.UpdateSquare(73, 3);
            //sudoku.UpdateSquare(74, 9);
            //sudoku.UpdateSquare(75, 1);
            //sudoku.UpdateSquare(76, 2);
            //sudoku.UpdateSquare(77, 4);
            //sudoku.UpdateSquare(78, 6);
            //sudoku.UpdateSquare(80, 8);
            //sudoku.UpdateSquare(81, 7);

            // Square ID version
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
            sudoku.UpdateSquare(46, 6);
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
            sudoku.UpdateSquare(44, 6);
            sudoku.UpdateSquare(45, 4);
            sudoku.UpdateSquare(52, 9);
            sudoku.UpdateSquare(54, 5);
            sudoku.UpdateSquare(55, 4);
            sudoku.UpdateSquare(56, 5);
            sudoku.UpdateSquare(57, 2);
            sudoku.UpdateSquare(64, 8);
            sudoku.UpdateSquare(65, 7);
            sudoku.UpdateSquare(73, 3);
            sudoku.UpdateSquare(74, 6);
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
            sudoku.UpdateSquare(72, 6);
            sudoku.UpdateSquare(80, 8);
            sudoku.UpdateSquare(81, 7);

            sudoku.ToggleReady();

            return sudoku;
        }

        /// <summary>
        /// Wrong 2
        /// </summary>
        /// <returns></returns>
        Sudoku Case15()
        {
            var sudoku = new Sudoku();

            sudoku.SudokuId = 15;
            sudoku.Title = "Wrong 2"; // Try this after fixing CASE 1: ID 5

            // Horizontal ID version
            //sudoku.UpdateSquare(1, 1);
            //sudoku.UpdateSquare(2, 2);
            //sudoku.UpdateSquare(3, 3);
            //sudoku.UpdateSquare(4, 4);
            //sudoku.UpdateSquare(5, 5);
            //sudoku.UpdateSquare(6, 6);
            //sudoku.UpdateSquare(7, 7);
            //sudoku.UpdateSquare(8, 8);
            //sudoku.UpdateSquare(9, 9);
            //sudoku.UpdateSquare(10, 4);
            //sudoku.UpdateSquare(11, 5);
            //sudoku.UpdateSquare(12, 6);
            //sudoku.UpdateSquare(19, 7);
            //sudoku.UpdateSquare(20, 8);
            //sudoku.UpdateSquare(21, 9);
            //sudoku.UpdateSquare(13, 1);
            //sudoku.UpdateSquare(14, 2);

            // Square ID version
            sudoku.UpdateSquare(1, 1);
            sudoku.UpdateSquare(2, 2);
            sudoku.UpdateSquare(3, 3);
            sudoku.UpdateSquare(10, 4);
            sudoku.UpdateSquare(11, 5);
            sudoku.UpdateSquare(12, 6);
            sudoku.UpdateSquare(19, 7);
            sudoku.UpdateSquare(20, 8);
            sudoku.UpdateSquare(21, 9);
            sudoku.UpdateSquare(4, 4);
            sudoku.UpdateSquare(5, 5);
            sudoku.UpdateSquare(6, 6);
            sudoku.UpdateSquare(7, 7);
            sudoku.UpdateSquare(8, 8);
            sudoku.UpdateSquare(9, 9);
            sudoku.UpdateSquare(13, 1);
            sudoku.UpdateSquare(14, 2);

            sudoku.ToggleReady();

            return sudoku;
        }

        /// <summary>
        /// Availability bug
        /// </summary>
        /// <returns></returns>
        Sudoku Case16()
        {
            var sudoku = new Sudoku();

            sudoku.SudokuId = 16;
            sudoku.Title = "Availability bug";

            // Horizontal ID version
            //sudoku.UpdateSquare(22, 1);
            //sudoku.UpdateSquare(30, 1);
            //sudoku.UpdateSquare(22, 0);

            // Square ID version
            sudoku.UpdateSquare(16, 1);
            sudoku.UpdateSquare(30, 1);
            sudoku.UpdateSquare(16, 0);

            sudoku.ToggleReady();

            return sudoku;
        }

        /// <summary>
        /// Availability + hint bug
        /// SOLVED
        /// </summary>
        /// <returns></returns>
        Sudoku Case17()
        {
            var sudoku = new Sudoku();

            sudoku.SudokuId = 17;
            sudoku.Title = "Hint bug";

            // Horizontal ID version
            //sudoku.UpdateSquare(1, 1);
            //sudoku.UpdateSquare(2, 2);
            //sudoku.UpdateSquare(3, 3);
            //sudoku.UpdateSquare(4, 4);
            //sudoku.UpdateSquare(5, 5);
            //sudoku.UpdateSquare(6, 6);
            //sudoku.UpdateSquare(7, 7);
            //sudoku.UpdateSquare(8, 8);
            //sudoku.UpdateSquare(1, 9);

            // Square ID version
            sudoku.UpdateSquare(1, 1);
            sudoku.UpdateSquare(2, 2);
            sudoku.UpdateSquare(3, 3);
            sudoku.UpdateSquare(10, 4);
            sudoku.UpdateSquare(11, 5);
            sudoku.UpdateSquare(12, 6);
            sudoku.UpdateSquare(19, 7);
            sudoku.UpdateSquare(20, 8);
            sudoku.UpdateSquare(1, 9);

            return sudoku;
        }

        /// <summary>
        /// Free-style
        /// </summary>
        /// <returns></returns>
        Sudoku Case18()
        {
            var sudoku = new Sudoku();

            sudoku.SudokuId = 18;
            sudoku.Title = "Free-style";

            sudoku.ToggleReady();

            // Square ID version
            sudoku.UpdateSquare(1, 1);

            return sudoku;
        }

        public IEnumerable<Sudoku> GetCases()
        {
            var list = new Collection<Sudoku>();

            list.Add(Case1());
            list.Add(Case2());
            list.Add(Case3());
            list.Add(Case4());
            list.Add(Case5());
            list.Add(Case6());
            list.Add(Case7());
            list.Add(Case8());
            list.Add(Case9());
            list.Add(Case10());
            list.Add(Case11());
            list.Add(Case12());
            list.Add(Case13());
            list.Add(Case14());
            list.Add(Case15());
            list.Add(Case16());
            list.Add(Case17());
            list.Add(Case18());

            return list;
        }
    }
}
