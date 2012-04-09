using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OSP.SudokuSolver.Engine;

namespace OSP.SudokuSolver.WebApp.Models
{
    public class SudokuContainer
    {
        public int SudokuId { get; set; }
        private Sudoku Sudoku { get; set; }
        public bool AutoSolve { get; set; }
        public List<SquareContainer> SquareList { get; set; }
        public IEnumerable<GroupContainer> HorizontalTypeGroups { get; set; }
        public IEnumerable<GroupContainer> VerticalTypeGroups { get; set; }
        public IEnumerable<GroupContainer> SquareTypeGroups { get; set; }
        public IEnumerable<PotentialContainer> Potentials { get; set; }

        public void SetSudoku(Sudoku sudoku)
        {
            Sudoku = sudoku;
        }

        public void Prepare()
        {
            SquareList = new List<SquareContainer>();
            foreach (var square in Sudoku.AllSquares)
                SquareList.Add(new SquareContainer(square) { SquareId = square.Id, Number = square.Number.Value });

            AutoSolve = this.Sudoku.AutoSolve;
        }

        public IEnumerable<GroupContainer> GetSquareGroup(GroupTypes type)
        {
            List<GroupContainer> containers = new List<GroupContainer>();
            switch (type)
            {
                case GroupTypes.Horizontal:
                    {
                        foreach (var group in Sudoku.AllHorizontalTypeGroups)
                        {
                            var container = new GroupContainer() { GroupId = group.Id, Type = (int)group.GroupType };

                            foreach (var s in group.Squares)
                                container.Squares.Add(new SquareContainer(s) { SquareId = s.Id, Number = s.Number.Value });

                            containers.Add(container);
                        }

                        break;
                    }
                case GroupTypes.Vertical:
                    {
                        foreach (var group in Sudoku.AllVerticalTypeGroups)
                        {
                            var container = new GroupContainer() { GroupId = group.Id, Type = (int)group.GroupType };

                            foreach (var s in group.Squares)
                                container.Squares.Add(new SquareContainer(s) { SquareId = s.Id, Number = s.Number.Value });

                            containers.Add(container);
                        }

                        break;
                    }
                case GroupTypes.Square:
                    {
                        foreach (var group in Sudoku.AllSquareTypeGroups)
                        {
                            var container = new GroupContainer() { GroupId = group.Id, Type = (int)group.GroupType };

                            foreach (var s in group.Squares)
                                container.Squares.Add(new SquareContainer(s) { SquareId = s.Id, Number = s.Number.Value });

                            containers.Add(container);
                        }

                        break;
                    }
            }

            return containers;
        }

        public IEnumerable<SquareContainer> GetFilledSquares()
        {
            var list = new List<SquareContainer>();

            foreach (var s in Sudoku.AllFilledSquares)
                list.Add(new SquareContainer(s) { SquareId = s.Id, Number = s.Number.Value, FillType = (int)s.FillType });
            
            return list;
        }

        public IEnumerable<NumberContainer> GetNumbers()
        {
            var list = new List<NumberContainer>();

            foreach (var n in Sudoku.AllNumbers)
                list.Add(new NumberContainer() { Value = n.Value, Count = n.Count });

            return list;
        }

        public IEnumerable<PotentialContainer> GetPotentials()
        {
            var list = new List<PotentialContainer>();

            foreach (var p in Sudoku.PotentialSquares.OrderBy(p => p.Square.Id))
                list.Add(new PotentialContainer() { SquareId = p.Square.Id, SquareValue = p.Number.Value, PotentialValue = p.Number.Value, PotentialType = p.PotentialType } );

            return list;
        }

        public void FillSquare(int squareId, int number)
        {
            Sudoku.FillSquare(squareId, number);

            //TODO ?
            Prepare();
        }

        public void ToggleAutoSolve()
        {
            Sudoku.AutoSolve = !Sudoku.AutoSolve;
            this.AutoSolve = Sudoku.AutoSolve;
        }

        public void Solve()
        {
            Sudoku.Solve();
        }
    }
}
