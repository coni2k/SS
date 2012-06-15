using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using OSP.SudokuSolver.Engine;

namespace OSP.SudokuSolver.WebApp.Models
{
    public class SudokuContainer
    {
        public int SudokuId { get; set; }
        private Sudoku Sudoku { get; set; }
        public int Size { get { return this.Sudoku.Size; } }
        public bool AutoSolve { get; set; }
        public bool Ready { get; set; }

        public SudokuContainer()
        {
            this.AutoSolve = false;
            this.Ready = false;
        }

        public void SetSudoku(Sudoku sudoku)
        {
            Sudoku = sudoku;
        }

        public List<SquareContainer> GetSquares()
        {
            var list = new List<SquareContainer>();
            foreach (var square in Sudoku.Squares)
                list.Add(new SquareContainer(square) { SquareId = square.Id, Number = square.Number.Value, AssignType = square.AssignType });

            return list;
        }

        public IEnumerable<GroupContainer> GetSquareGroups(GroupTypes type)
        {
            List<GroupContainer> containers = new List<GroupContainer>();
            switch (type)
            {
                case GroupTypes.Horizontal:
                    {
                        foreach (var group in Sudoku.HorizontalTypeGroups)
                        {
                            var container = new GroupContainer() { GroupId = group.Id, Type = (int)group.GroupType };

                            foreach (var s in group.Squares)
                                container.Squares.Add(new SquareContainer(s) { SquareId = s.Id, Number = s.Number.Value, AssignType = s.AssignType });

                            containers.Add(container);
                        }

                        break;
                    }
                case GroupTypes.Vertical:
                    {
                        foreach (var group in Sudoku.VerticalTypeGroups)
                        {
                            var container = new GroupContainer() { GroupId = group.Id, Type = (int)group.GroupType };

                            foreach (var s in group.Squares)
                                container.Squares.Add(new SquareContainer(s) { SquareId = s.Id, Number = s.Number.Value, AssignType = s.AssignType });

                            containers.Add(container);
                        }

                        break;
                    }
                case GroupTypes.Square:
                    {
                        foreach (var group in Sudoku.SquareTypeGroups)
                        {
                            var container = new GroupContainer() { GroupId = group.Id, Type = (int)group.GroupType };

                            foreach (var s in group.Squares)
                                container.Squares.Add(new SquareContainer(s) { SquareId = s.Id, Number = s.Number.Value, AssignType = s.AssignType });

                            containers.Add(container);
                        }

                        break;
                    }
            }

            return containers;
        }

        public IEnumerable<SquareContainer> GetUsedSquares()
        {
            var list = new List<SquareContainer>();

            foreach (var s in Sudoku.UsedSquares)
                list.Add(new SquareContainer(s) { SquareId = s.Id, Number = s.Number.Value, AssignType = s.AssignType });
            
            return list;
        }

        public IEnumerable<NumberContainer> GetNumbers()
        {
            var list = new List<NumberContainer>();

            foreach (var n in Sudoku.Numbers)
                list.Add(new NumberContainer() { Value = n.Value, Count = n.Count });

            return list;
        }

        public IEnumerable<PotentialContainer> GetPotentials()
        {
            var list = new List<PotentialContainer>();

            foreach (var p in Sudoku.PotentialSquares.OrderBy(p => p.Square.Id))
                list.Add(new PotentialContainer() { SquareId = p.Square.Id, PotentialValue = p.Number.Value, PotentialType = p.PotentialType } );

            return list;
        }

        public IEnumerable<AvailabilityContainer> GetAvailabilities()
        {
            var list = new List<AvailabilityContainer>();

            foreach (var s in Sudoku.Squares)
            {
                foreach (var n in Sudoku.NumbersExceptZero)
                {
                    //list.Add(new AvailabilityContainer() { SquareId = s.Id, Number = n.Value, IsAvailable = s.IsNumberAvailable(n) });
                    list.Add(new AvailabilityContainer() { SquareId = s.Id, Number = n.Value, IsAvailable = s.IsNumberAvailableNew(n) });
                }
            }

            return list;
        }

        public IEnumerable<Availability2Container> GetAvailabilities2()
        {
            var list = new List<Availability2Container>();

            //foreach (var s in Sudoku.Squares)
            //{
            //    foreach (var n in Sudoku.NumbersExceptZero)
            //        list.Add(new Availability2Container() { SquareId = s.Id, Number = n.Value, IsAvailable = s.IsNumberAvailable2(n) });
            //}

            return list;
        }

        //public IEnumerable<Availability2Container> GetAvailabilities3()
        //{
        //    var list = new List<Availability2Container>();

        //    foreach (var s in Sudoku.Squares)
        //    {
        //        foreach (var n in Sudoku.NumbersExceptZero)
        //            list.Add(new Availability2Container() { SquareId = s.Id, Number = n.Value, IsAvailable = false });
        //    }

        //    foreach (var s in Sudoku.Squares)
        //    {
        //        if (s.AvailableNumbers.Count() == 1)
        //        {
        //            list.First(a => a.SquareId == s.Id && a.Number == (s.AvailableNumbers.First().Value)).IsAvailable = true;
        //        }
        //        else
        //        {
        //            foreach (var n in s.AvailableNumbers)
        //            {
        //                foreach (var g in s.SquareGroups)
        //                {
        //                    var count = g.GetAvailableSquaresForNumber(n).Count();

        //                    if (count == 1)
        //                    {
        //                        list.First(a => a.SquareId == s.Id && a.Number == n.Value).IsAvailable = true;
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return list;
        //}

        public IEnumerable<GroupNumberAvailabilityContainer> GetGroupNumberAvailabilities()
        {
            var list = new List<GroupNumberAvailabilityContainer>();

            foreach (var g in Sudoku.SquareTypeGroups)
            {
                foreach (var n in Sudoku.NumbersExceptZero)
                {
                    //list.Add(new GroupNumberAvailabilityContainer() { GroupId = g.Id, Number = n.Value, Count = g.GetAvailableSquaresForNumber(n).Count() });
                    list.Add(new GroupNumberAvailabilityContainer() { GroupId = g.Id, Number = n.Value, Count = g.GetAvailableSquaresForNumberNew(n).Count() });
                }
            }

            return list;
        }

        public void UpdateSquare(int squareId, int number)
        {
            Sudoku.UpdateSquare(squareId, number);
        }

        public void ToggleReady()
        {
            Sudoku.Ready = !Sudoku.Ready;
            this.Ready = Sudoku.Ready;
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
