using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OSP.SudokuSolver.Engine;
using System.ComponentModel.Design.Serialization;

namespace OSP.SudokuSolver.WebApp.Models
{
    public class SquareContainer
    {
        private Square Square { get; set; }
        public int SquareId { get; set; }
        public int Number { get; set; }
        public AssignTypes AssignType { get; set; }

        public SquareContainer() { }

        public SquareContainer(Square square)
        {
            this.Square = square;
        }

        //public IEnumerable<NumberContainer> GetAvailableNumbers()
        //{
        //    var availableNumbers = new List<NumberContainer>();

        //    foreach (var n in this.Square.AvailableNumbers)
        //        availableNumbers.Add(new NumberContainer() { Value = n.Value, Count = n.Count });

        //    return availableNumbers;
        //}
    }
}
