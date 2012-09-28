using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SudokuSolver.Engine;

namespace SudokuSolver.WebApp.Models
{
    public class CacheManager
    {
        public static List<Sudoku> SudokuList
        {
            get
            {
                if (HttpContext.Current.Cache["SudokuList"] == null)
                    LoadSamples();

                return (List<Sudoku>)HttpContext.Current.Cache["SudokuList"];
            }
        }

        public static void LoadSamples()
        {
            HttpContext.Current.Cache["SudokuList"] = new List<Sudoku>();

            var sample1 = new Sudoku(9);
            var sample2 = new Sudoku(9);
            var sample3 = new Sudoku(9);
            var sample4 = new Sudoku(9);
            var sample5 = new Sudoku(9);
            var sample6 = new Sudoku(9);
            var sample7 = new Sudoku(9);
            var sample8 = new Sudoku(9);
            var sample9 = new Sudoku(9);
            var sample10 = new Sudoku(9);
            var sample11 = new Sudoku(9);

            var sample12 = new Sudoku(4); //Size 4

            var sample13 = new Sudoku(9); // Beauty

            var sample14 = new Sudoku(9); // Availability bug

            var sample15 = new Sudoku(9); // Wrong 2

            sample1.UpdateSquare(1, 1);
            sample1.UpdateSquare(2, 2);
            sample1.UpdateSquare(3, 3);
            sample1.UpdateSquare(4, 4);
            sample1.UpdateSquare(5, 5);
            sample1.UpdateSquare(6, 6);
            sample1.UpdateSquare(7, 7);
            sample1.UpdateSquare(8, 8);

            sample2.UpdateSquare(1, 1);
            sample2.UpdateSquare(10, 2);
            sample2.UpdateSquare(19, 3);
            sample2.UpdateSquare(28, 4);
            sample2.UpdateSquare(37, 5);
            sample2.UpdateSquare(46, 6);
            sample2.UpdateSquare(55, 7);
            sample2.UpdateSquare(64, 8);

            sample3.UpdateSquare(1, 1);
            sample3.UpdateSquare(2, 2);
            sample3.UpdateSquare(3, 3);
            sample3.UpdateSquare(10, 4);
            sample3.UpdateSquare(11, 5);
            sample3.UpdateSquare(12, 6);
            sample3.UpdateSquare(19, 7);
            sample3.UpdateSquare(20, 8);

            sample4.UpdateSquare(1, 1);
            sample4.UpdateSquare(2, 2);
            sample4.UpdateSquare(3, 3);
            sample4.UpdateSquare(8, 7);
            sample4.UpdateSquare(18, 8);
            sample4.UpdateSquare(36, 4);
            sample4.UpdateSquare(45, 5);
            sample4.UpdateSquare(54, 6);

            sample5.UpdateSquare(13, 1);
            sample5.UpdateSquare(25, 1);
            sample5.UpdateSquare(29, 1);
            sample5.UpdateSquare(57, 1);

            sample6.UpdateSquare(25, 1);
            sample6.UpdateSquare(57, 1);
            sample6.UpdateSquare(69, 1);
            sample6.UpdateSquare(80, 2);

            sample7.UpdateSquare(10, 3);
            sample7.UpdateSquare(13, 1);
            sample7.UpdateSquare(17, 2);
            sample7.UpdateSquare(19, 4);
            sample7.UpdateSquare(25, 1);
            sample7.UpdateSquare(28, 5);
            sample7.UpdateSquare(29, 1);
            sample7.UpdateSquare(31, 2);
            sample7.UpdateSquare(46, 6);
            sample7.UpdateSquare(55, 7);
            sample7.UpdateSquare(57, 1);
            sample7.UpdateSquare(61, 2);
            sample7.UpdateSquare(64, 8);
            sample7.UpdateSquare(73, 9);

            sample8.UpdateSquare(1, 1);
            sample8.UpdateSquare(2, 2);
            sample8.UpdateSquare(3, 3);
            sample8.UpdateSquare(13, 1);
            sample8.UpdateSquare(14, 2);
            sample8.UpdateSquare(15, 3);

            sample9.UpdateSquare(1, 9);
            sample9.UpdateSquare(2, 7);
            sample9.UpdateSquare(3, 3);
            sample9.UpdateSquare(4, 6);
            sample9.UpdateSquare(5, 2);
            sample9.UpdateSquare(6, 8);
            sample9.UpdateSquare(7, 4);
            sample9.UpdateSquare(8, 5);
            sample9.UpdateSquare(9, 1);
            sample9.UpdateSquare(10, 5);
            sample9.UpdateSquare(11, 6);
            sample9.UpdateSquare(12, 8);
            sample9.UpdateSquare(13, 4);
            sample9.UpdateSquare(14, 1);
            sample9.UpdateSquare(15, 3);
            sample9.UpdateSquare(16, 7);
            sample9.UpdateSquare(17, 9);
            sample9.UpdateSquare(18, 2);
            sample9.UpdateSquare(19, 1);
            sample9.UpdateSquare(20, 2);
            sample9.UpdateSquare(21, 4);
            sample9.UpdateSquare(22, 5);
            sample9.UpdateSquare(23, 9);
            sample9.UpdateSquare(24, 7);
            sample9.UpdateSquare(25, 8);
            sample9.UpdateSquare(26, 3);
            sample9.UpdateSquare(27, 6);
            sample9.UpdateSquare(28, 8);
            sample9.UpdateSquare(29, 1);
            sample9.UpdateSquare(30, 5);
            sample9.UpdateSquare(31, 7);
            sample9.UpdateSquare(33, 9);
            sample9.UpdateSquare(34, 6);
            sample9.UpdateSquare(35, 4);
            sample9.UpdateSquare(36, 3);
            sample9.UpdateSquare(37, 6);
            sample9.UpdateSquare(38, 4);
            sample9.UpdateSquare(39, 9);
            sample9.UpdateSquare(40, 3);
            sample9.UpdateSquare(41, 5);
            sample9.UpdateSquare(42, 2);
            sample9.UpdateSquare(43, 1);
            sample9.UpdateSquare(44, 7);
            sample9.UpdateSquare(45, 8);
            sample9.UpdateSquare(46, 2);
            sample9.UpdateSquare(47, 3);
            sample9.UpdateSquare(48, 7);
            sample9.UpdateSquare(49, 1);
            sample9.UpdateSquare(50, 6);
            sample9.UpdateSquare(51, 4);
            sample9.UpdateSquare(52, 9);
            sample9.UpdateSquare(54, 5);
            sample9.UpdateSquare(55, 4);
            sample9.UpdateSquare(56, 5);
            sample9.UpdateSquare(57, 2);
            sample9.UpdateSquare(58, 8);
            sample9.UpdateSquare(59, 7);
            sample9.UpdateSquare(61, 3);
            sample9.UpdateSquare(62, 6);
            sample9.UpdateSquare(63, 9);
            sample9.UpdateSquare(64, 7);
            sample9.UpdateSquare(65, 8);
            sample9.UpdateSquare(66, 6);
            sample9.UpdateSquare(67, 9);
            sample9.UpdateSquare(68, 3);
            sample9.UpdateSquare(69, 1);
            sample9.UpdateSquare(70, 5);
            sample9.UpdateSquare(71, 2);
            sample9.UpdateSquare(72, 4);
            sample9.UpdateSquare(73, 3);
            sample9.UpdateSquare(74, 9);
            sample9.UpdateSquare(75, 1);
            sample9.UpdateSquare(76, 2);
            sample9.UpdateSquare(77, 4);
            sample9.UpdateSquare(78, 6);
            sample9.UpdateSquare(80, 8);
            sample9.UpdateSquare(81, 7);

            sample10.UpdateSquare(3, 8);
            sample10.UpdateSquare(4, 3);
            sample10.UpdateSquare(5, 4);
            sample10.UpdateSquare(6, 2);
            sample10.UpdateSquare(7, 9);
            sample10.UpdateSquare(12, 9);
            sample10.UpdateSquare(16, 7);
            sample10.UpdateSquare(19, 4);
            sample10.UpdateSquare(27, 3);
            sample10.UpdateSquare(30, 6);
            sample10.UpdateSquare(31, 4);
            sample10.UpdateSquare(32, 7);
            sample10.UpdateSquare(33, 3);
            sample10.UpdateSquare(34, 2);
            sample10.UpdateSquare(38, 3);
            sample10.UpdateSquare(44, 1);
            sample10.UpdateSquare(48, 2);
            sample10.UpdateSquare(49, 8);
            sample10.UpdateSquare(50, 5);
            sample10.UpdateSquare(51, 1);
            sample10.UpdateSquare(52, 6);
            sample10.UpdateSquare(55, 7);
            sample10.UpdateSquare(63, 8);
            sample10.UpdateSquare(66, 4);
            sample10.UpdateSquare(70, 1);
            sample10.UpdateSquare(75, 3);
            sample10.UpdateSquare(76, 6);
            sample10.UpdateSquare(77, 9);
            sample10.UpdateSquare(78, 7);
            sample10.UpdateSquare(79, 5);

            sample11.UpdateSquare(1, 1);
            sample11.UpdateSquare(13, 1);
            sample11.UpdateSquare(25, 2);
            sample11.UpdateSquare(26, 3);

            //Size 4 - doesnt have any assignment

            sample13.UpdateSquare(2, 2);
            sample13.UpdateSquare(10, 4);
            sample13.UpdateSquare(11, 5);
            sample13.UpdateSquare(12, 6);
            sample13.UpdateSquare(20, 8);
            sample13.UpdateSquare(4, 9);
            sample13.UpdateSquare(6, 7);
            sample13.UpdateSquare(22, 3);
            sample13.UpdateSquare(24, 1);
            sample13.UpdateSquare(28, 3);
            sample13.UpdateSquare(30, 1);
            sample13.UpdateSquare(46, 9);
            sample13.UpdateSquare(48, 7);

            sample14.UpdateSquare(22, 1);
            sample14.UpdateSquare(30, 1);
            sample14.UpdateSquare(22, 0);

            sample15.UpdateSquare(1, 1);
            sample15.UpdateSquare(2, 2);
            sample15.UpdateSquare(3, 3);
            sample15.UpdateSquare(4, 4);
            sample15.UpdateSquare(5, 5);
            sample15.UpdateSquare(6, 6);
            sample15.UpdateSquare(7, 7);
            sample15.UpdateSquare(8, 8);
            sample15.UpdateSquare(9, 9);
            sample15.UpdateSquare(10, 4);
            sample15.UpdateSquare(11, 5);
            sample15.UpdateSquare(12, 6);
            sample15.UpdateSquare(19, 7);
            sample15.UpdateSquare(20, 8);
            sample15.UpdateSquare(21, 9);
            sample15.UpdateSquare(13, 1);
            sample15.UpdateSquare(14, 2);

            //var cont1 = new SudokuContainer() { SudokuId = 1, Title = "1. Type - Horizontal - 8" };
            //var cont2 = new SudokuContainer() { SudokuId = 2, Title = "1. Type - Vertical - 8" };
            //var cont3 = new SudokuContainer() { SudokuId = 3, Title = "1. Type - Square - 8" };
            //var cont4 = new SudokuContainer() { SudokuId = 4, Title = "1. Type - Mixed - 8" };
            //var cont5 = new SudokuContainer() { SudokuId = 5, Title = "2. Type - Straight 1" };
            //var cont6 = new SudokuContainer() { SudokuId = 6, Title = "Mixed Types (triggers strange RelatedNumbers block)" };
            //var cont7 = new SudokuContainer() { SudokuId = 7, Title = "Domino" };
            //var cont8 = new SudokuContainer() { SudokuId = 8, Title = "Headache (1-2-3)" };
            //var cont9 = new SudokuContainer() { SudokuId = 9, Title = "Wrong" };
            //var cont10 = new SudokuContainer() { SudokuId = 10, Title = "Real case" };
            //var cont11 = new SudokuContainer() { SudokuId = 11, Title = "Headache 2" };
            //var cont12 = new SudokuContainer() { SudokuId = 12, Title = "Mini size" };
            //var cont13 = new SudokuContainer() { SudokuId = 13, Title = "Beauty" };
            //var cont14 = new SudokuContainer() { SudokuId = 14, Title = "Availability bug" };

            //var cont1 = new SudokuContainer();
            //var cont2 = new SudokuContainer();
            //var cont3 = new SudokuContainer();
            //var cont4 = new SudokuContainer();
            //var cont5 = new SudokuContainer();
            //var cont6 = new SudokuContainer();
            //var cont7 = new SudokuContainer();
            //var cont8 = new SudokuContainer();
            //var cont9 = new SudokuContainer();
            //var cont10 = new SudokuContainer();
            //var cont11 = new SudokuContainer();
            //var cont12 = new SudokuContainer();
            //var cont13 = new SudokuContainer();
            //var cont14 = new SudokuContainer();

            //cont1.SetSudoku(sample1);
            //cont2.SetSudoku(sample2);
            //cont3.SetSudoku(sample3);
            //cont4.SetSudoku(sample4);
            //cont5.SetSudoku(sample5);
            //cont6.SetSudoku(sample6);
            //cont7.SetSudoku(sample7);
            //cont8.SetSudoku(sample8);
            //cont9.SetSudoku(sample9);
            //cont10.SetSudoku(sample10);
            //cont11.SetSudoku(sample11);
            //cont12.SetSudoku(sample12);
            //cont13.SetSudoku(sample13);
            //cont14.SetSudoku(sample14);

            sample1.SudokuId = 1; //"1. Type - Horizontal - 8" };
            sample2.SudokuId = 2; //"1. Type - Vertical - 8" };
            sample3.SudokuId = 3; //"1. Type - Square - 8" };
            sample4.SudokuId = 4; //"1. Type - Mixed - 8" };
            sample5.SudokuId = 5; //"2. Type - Straight 1" };
            sample6.SudokuId = 6; //"Mixed Types (triggers strange RelatedNumbers block)" };
            sample7.SudokuId = 7; //"Domino" };
            sample8.SudokuId = 8; //"Headache (1-2-3)" };
            sample9.SudokuId = 9; //"Wrong" };
            sample10.SudokuId = 10; //"Real case" };
            sample11.SudokuId = 11; //"Headache 2" };
            sample12.SudokuId = 12; //"Mini size" };
            sample13.SudokuId = 13; //"Beauty" };
            sample14.SudokuId = 14; //"Availability bug" };
            sample15.SudokuId = 15;

            sample1.Title = "1. Type - Horizontal - 8";
            sample2.Title = "1. Type - Vertical - 8";
            sample3.Title = "1. Type - Square - 8";
            sample4.Title = "1. Type - Mixed - 8";
            sample5.Title = "2. Type - Straight 1";
            sample6.Title = "Mixed Types (triggers strange RelatedNumbers block)";
            sample7.Title = "Domino";
            sample8.Title = "Headache (1-2-3)";
            sample9.Title = "Wrong";
            sample10.Title = "Real case";
            sample11.Title = "Headache 2";
            sample12.Title = "Mini size";
            sample13.Title = "Beauty";
            sample14.Title = "Availability bug";
            sample15.Title = "Wrong 2"; // Try this after fixing CASE 1: ID 5

            sample1.ToggleReady();
            sample2.ToggleReady();
            sample3.ToggleReady();
            sample4.ToggleReady();
            sample5.ToggleReady();
            sample6.ToggleReady();
            sample7.ToggleReady();
            sample8.ToggleReady();
            sample9.ToggleReady();
            sample10.ToggleReady();
            sample11.ToggleReady();
            sample12.ToggleReady();
            sample13.ToggleReady();
            sample14.ToggleReady();
            sample15.ToggleReady();

            ((List<Sudoku>)HttpContext.Current.Cache["SudokuList"]).Add(sample1);
            ((List<Sudoku>)HttpContext.Current.Cache["SudokuList"]).Add(sample2);
            ((List<Sudoku>)HttpContext.Current.Cache["SudokuList"]).Add(sample3);
            ((List<Sudoku>)HttpContext.Current.Cache["SudokuList"]).Add(sample4);
            ((List<Sudoku>)HttpContext.Current.Cache["SudokuList"]).Add(sample5);
            ((List<Sudoku>)HttpContext.Current.Cache["SudokuList"]).Add(sample6);
            ((List<Sudoku>)HttpContext.Current.Cache["SudokuList"]).Add(sample7);
            ((List<Sudoku>)HttpContext.Current.Cache["SudokuList"]).Add(sample8);
            ((List<Sudoku>)HttpContext.Current.Cache["SudokuList"]).Add(sample9);
            ((List<Sudoku>)HttpContext.Current.Cache["SudokuList"]).Add(sample10);
            ((List<Sudoku>)HttpContext.Current.Cache["SudokuList"]).Add(sample11);
            ((List<Sudoku>)HttpContext.Current.Cache["SudokuList"]).Add(sample12);
            ((List<Sudoku>)HttpContext.Current.Cache["SudokuList"]).Add(sample13);
            ((List<Sudoku>)HttpContext.Current.Cache["SudokuList"]).Add(sample14);
            ((List<Sudoku>)HttpContext.Current.Cache["SudokuList"]).Add(sample15);
        }
    }
}