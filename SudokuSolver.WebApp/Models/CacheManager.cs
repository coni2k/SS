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

                    sample1.FillSquare(1, 1);
                    sample1.FillSquare(2, 2);
                    sample1.FillSquare(3, 3);
                    sample1.FillSquare(4, 4);
                    sample1.FillSquare(5, 5);
                    sample1.FillSquare(6, 6);
                    sample1.FillSquare(7, 7);
                    sample1.FillSquare(8, 8);
                    sample2.FillSquare(1, 1);
                    sample2.FillSquare(10, 2);
                    sample2.FillSquare(19, 3);
                    sample2.FillSquare(28, 4);
                    sample2.FillSquare(37, 5);
                    sample2.FillSquare(46, 6);
                    sample2.FillSquare(55, 7);
                    sample2.FillSquare(64, 8);
                    sample3.FillSquare(1, 1);
                    sample3.FillSquare(2, 2);
                    sample3.FillSquare(3, 3);
                    sample3.FillSquare(10, 4);
                    sample3.FillSquare(11, 5);
                    sample3.FillSquare(12, 6);
                    sample3.FillSquare(19, 7);
                    sample3.FillSquare(20, 8);
                    sample4.FillSquare(1, 1);
                    sample4.FillSquare(2, 2);
                    sample4.FillSquare(3, 3);
                    sample4.FillSquare(8, 7);
                    sample4.FillSquare(18, 8);
                    sample4.FillSquare(36, 4);
                    sample4.FillSquare(45, 5);
                    sample4.FillSquare(54, 6);
                    sample5.FillSquare(13, 1);
                    sample5.FillSquare(25, 1);
                    sample5.FillSquare(29, 1);
                    sample5.FillSquare(57, 1);
                    sample6.FillSquare(25, 1);
                    sample6.FillSquare(57, 1);
                    sample6.FillSquare(69, 1);
                    sample6.FillSquare(80, 2);
                    sample7.FillSquare(10, 3);
                    sample7.FillSquare(13, 1);
                    sample7.FillSquare(17, 2);
                    sample7.FillSquare(19, 4);
                    sample7.FillSquare(25, 1);
                    sample7.FillSquare(28, 5);
                    sample7.FillSquare(29, 1);
                    sample7.FillSquare(31, 2);
                    sample7.FillSquare(46, 6);
                    sample7.FillSquare(55, 7);
                    sample7.FillSquare(57, 1);
                    sample7.FillSquare(61, 2);
                    sample7.FillSquare(64, 8);
                    sample7.FillSquare(73, 9);
                    sample8.FillSquare(1, 9);
                    sample8.FillSquare(2, 7);
                    sample8.FillSquare(3, 3);
                    sample8.FillSquare(4, 6);
                    sample8.FillSquare(5, 2);
                    sample8.FillSquare(6, 8);
                    sample8.FillSquare(7, 4);
                    sample8.FillSquare(8, 5);
                    sample8.FillSquare(9, 1);
                    sample8.FillSquare(10, 5);
                    sample8.FillSquare(11, 6);
                    sample8.FillSquare(12, 8);
                    sample8.FillSquare(13, 4);
                    sample8.FillSquare(14, 1);
                    sample8.FillSquare(15, 3);
                    sample8.FillSquare(16, 7);
                    sample8.FillSquare(17, 9);
                    sample8.FillSquare(18, 2);
                    sample8.FillSquare(19, 1);
                    sample8.FillSquare(20, 2);
                    sample8.FillSquare(21, 4);
                    sample8.FillSquare(22, 5);
                    sample8.FillSquare(23, 9);
                    sample8.FillSquare(24, 7);
                    sample8.FillSquare(25, 8);
                    sample8.FillSquare(26, 3);
                    sample8.FillSquare(27, 6);
                    sample8.FillSquare(28, 8);
                    sample8.FillSquare(29, 1);
                    sample8.FillSquare(30, 5);
                    sample8.FillSquare(31, 7);
                    sample8.FillSquare(33, 9);
                    sample8.FillSquare(34, 6);
                    sample8.FillSquare(35, 4);
                    sample8.FillSquare(36, 3);
                    sample8.FillSquare(37, 6);
                    sample8.FillSquare(38, 4);
                    sample8.FillSquare(39, 9);
                    sample8.FillSquare(40, 3);
                    sample8.FillSquare(41, 5);
                    sample8.FillSquare(42, 2);
                    sample8.FillSquare(43, 1);
                    sample8.FillSquare(44, 7);
                    sample8.FillSquare(45, 8);
                    sample8.FillSquare(46, 2);
                    sample8.FillSquare(47, 3);
                    sample8.FillSquare(48, 7);
                    sample8.FillSquare(49, 1);
                    sample8.FillSquare(50, 6);
                    sample8.FillSquare(51, 4);
                    sample8.FillSquare(52, 9);
                    sample8.FillSquare(54, 5);
                    sample8.FillSquare(55, 4);
                    sample8.FillSquare(56, 5);
                    sample8.FillSquare(57, 2);
                    sample8.FillSquare(58, 8);
                    sample8.FillSquare(59, 7);
                    sample8.FillSquare(61, 3);
                    sample8.FillSquare(62, 6);
                    sample8.FillSquare(63, 9);
                    sample8.FillSquare(64, 7);
                    sample8.FillSquare(65, 8);
                    sample8.FillSquare(66, 6);
                    sample8.FillSquare(67, 9);
                    sample8.FillSquare(68, 3);
                    sample8.FillSquare(69, 1);
                    sample8.FillSquare(70, 5);
                    sample8.FillSquare(71, 2);
                    sample8.FillSquare(72, 4);
                    sample8.FillSquare(73, 3);
                    sample8.FillSquare(74, 9);
                    sample8.FillSquare(75, 1);
                    sample8.FillSquare(76, 2);
                    sample8.FillSquare(77, 4);
                    sample8.FillSquare(78, 6);
                    sample8.FillSquare(80, 8);
                    sample8.FillSquare(81, 7);
                    sample9.FillSquare(3, 8);
                    sample9.FillSquare(4, 3);
                    sample9.FillSquare(5, 4);
                    sample9.FillSquare(6, 2);
                    sample9.FillSquare(7, 9);
                    sample9.FillSquare(12, 9);
                    sample9.FillSquare(16, 7);
                    sample9.FillSquare(19, 4);
                    sample9.FillSquare(27, 3);
                    sample9.FillSquare(30, 6);
                    sample9.FillSquare(31, 4);
                    sample9.FillSquare(32, 7);
                    sample9.FillSquare(33, 3);
                    sample9.FillSquare(34, 2);
                    sample9.FillSquare(38, 3);
                    sample9.FillSquare(44, 1);
                    sample9.FillSquare(48, 2);
                    sample9.FillSquare(49, 8);
                    sample9.FillSquare(50, 5);
                    sample9.FillSquare(51, 1);
                    sample9.FillSquare(52, 6);
                    sample9.FillSquare(55, 7);
                    sample9.FillSquare(63, 8);
                    sample9.FillSquare(66, 4);
                    sample9.FillSquare(70, 1);
                    sample9.FillSquare(75, 3);
                    sample9.FillSquare(76, 6);
                    sample9.FillSquare(77, 9);
                    sample9.FillSquare(78, 7);
                    sample9.FillSquare(79, 5);

                    var cont1 = new SudokuContainer() { Id = 1 };
                    var cont2 = new SudokuContainer() { Id = 2 };
                    var cont3 = new SudokuContainer() { Id = 3 };
                    var cont4 = new SudokuContainer() { Id = 4 };
                    var cont5 = new SudokuContainer() { Id = 5 };
                    var cont6 = new SudokuContainer() { Id = 6 };
                    var cont7 = new SudokuContainer() { Id = 7 };
                    var cont8 = new SudokuContainer() { Id = 8 };
                    var cont9 = new SudokuContainer() { Id = 9 };

                    cont1.SetSudoku(sample1);
                    cont2.SetSudoku(sample2);
                    cont3.SetSudoku(sample3);
                    cont4.SetSudoku(sample4);
                    cont5.SetSudoku(sample5);
                    cont6.SetSudoku(sample6);
                    cont7.SetSudoku(sample7);
                    cont8.SetSudoku(sample8);
                    cont9.SetSudoku(sample9);

                    cont1.Prepare();
                    cont2.Prepare();
                    cont3.Prepare();
                    cont4.Prepare();
                    cont5.Prepare();
                    cont6.Prepare();
                    cont7.Prepare();
                    cont8.Prepare();
                    cont9.Prepare();

                    ((List<SudokuContainer>)HttpContext.Current.Cache["SudokuList"]).Add(cont1);
                    ((List<SudokuContainer>)HttpContext.Current.Cache["SudokuList"]).Add(cont2);
                    ((List<SudokuContainer>)HttpContext.Current.Cache["SudokuList"]).Add(cont3);
                    ((List<SudokuContainer>)HttpContext.Current.Cache["SudokuList"]).Add(cont4);
                    ((List<SudokuContainer>)HttpContext.Current.Cache["SudokuList"]).Add(cont5);
                    ((List<SudokuContainer>)HttpContext.Current.Cache["SudokuList"]).Add(cont6);
                    ((List<SudokuContainer>)HttpContext.Current.Cache["SudokuList"]).Add(cont7);
                    ((List<SudokuContainer>)HttpContext.Current.Cache["SudokuList"]).Add(cont8);
                    ((List<SudokuContainer>)HttpContext.Current.Cache["SudokuList"]).Add(cont9);
                }

                return (List<SudokuContainer>) HttpContext.Current.Cache["SudokuList"];
            }
        }
    }
}