using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OSP.SudokuSolver.Engine;

namespace OSP.SudokuSolver.WebApp.Models
{
    public class CacheManager
    {
        public static List<SudokuContainer> SudokuList
        {
            get
            {
                if (HttpContext.Current.Cache["SudokuList"] == null)
                    LoadSamples();

                return (List<SudokuContainer>) HttpContext.Current.Cache["SudokuList"];
            }
        }

        public static void LoadSamples()
        {
            HttpContext.Current.Cache["SudokuList"] = new List<SudokuContainer>();

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

            var cont1 = new SudokuContainer() { SudokuId = 1 };
            var cont2 = new SudokuContainer() { SudokuId = 2 };
            var cont3 = new SudokuContainer() { SudokuId = 3 };
            var cont4 = new SudokuContainer() { SudokuId = 4 };
            var cont5 = new SudokuContainer() { SudokuId = 5 };
            var cont6 = new SudokuContainer() { SudokuId = 6 };
            var cont7 = new SudokuContainer() { SudokuId = 7 };
            var cont8 = new SudokuContainer() { SudokuId = 8 };
            var cont9 = new SudokuContainer() { SudokuId = 9 };
            var cont10 = new SudokuContainer() { SudokuId = 10 };

            cont1.SetSudoku(sample1);
            cont2.SetSudoku(sample2);
            cont3.SetSudoku(sample3);
            cont4.SetSudoku(sample4);
            cont5.SetSudoku(sample5);
            cont6.SetSudoku(sample6);
            cont7.SetSudoku(sample7);
            cont8.SetSudoku(sample8);
            cont9.SetSudoku(sample9);
            cont10.SetSudoku(sample10);

            cont1.ToggleReady();
            cont2.ToggleReady();
            cont3.ToggleReady();
            cont4.ToggleReady();
            cont5.ToggleReady();
            cont6.ToggleReady();
            cont7.ToggleReady();
            cont8.ToggleReady();
            cont9.ToggleReady();
            cont10.ToggleReady();

            ((List<SudokuContainer>)HttpContext.Current.Cache["SudokuList"]).Add(cont1);
            ((List<SudokuContainer>)HttpContext.Current.Cache["SudokuList"]).Add(cont2);
            ((List<SudokuContainer>)HttpContext.Current.Cache["SudokuList"]).Add(cont3);
            ((List<SudokuContainer>)HttpContext.Current.Cache["SudokuList"]).Add(cont4);
            ((List<SudokuContainer>)HttpContext.Current.Cache["SudokuList"]).Add(cont5);
            ((List<SudokuContainer>)HttpContext.Current.Cache["SudokuList"]).Add(cont6);
            ((List<SudokuContainer>)HttpContext.Current.Cache["SudokuList"]).Add(cont7);
            ((List<SudokuContainer>)HttpContext.Current.Cache["SudokuList"]).Add(cont8);
            ((List<SudokuContainer>)HttpContext.Current.Cache["SudokuList"]).Add(cont9);
            ((List<SudokuContainer>)HttpContext.Current.Cache["SudokuList"]).Add(cont10);
        }
    }
}