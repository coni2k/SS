using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SudokuSolver.Engine
{
    public class GroupNumberAvailability
    {
        public GroupNumber GroupNumber { get; private set; }
        public Square Square { get; private set; }
        internal Square SquareTypeSource { get; private set; }
        internal Square HorizontalTypeSource { get; private set; }
        internal Square VerticalTypeSource { get; private set; }
        //public bool IsAvailable
        //{
        //    get
        //    {
        //        return SquareTypeSource == null
        //            && HorizontalTypeSource == null
        //            && VerticalTypeSource == null;
        //        // && Square.IsAvailable;
        //    }
        //}
        public bool GetAvailability()
        {
            return GetAvailability(null);
        }
        public bool GetAvailability(Square source)
        {
            return (SquareTypeSource == null
                || SquareTypeSource.Equals(source))
                && (HorizontalTypeSource == null
                || HorizontalTypeSource.Equals(source))
                && (VerticalTypeSource == null
                || VerticalTypeSource.Equals(source));
        }
        internal bool Updated { get; set; }

        internal GroupNumberAvailability(GroupNumber groupNumber, Square square)
        {
            GroupNumber = groupNumber;
            Square = square;
        }

        internal void UpdateAvailability(GroupType type, Square source)
        {
            switch (type)
            {
                case GroupType.Square: SquareTypeSource = source; break;
                case GroupType.Horizontal: HorizontalTypeSource = source; break;
                case GroupType.Vertical: VerticalTypeSource = source; break;
            }

            // TODO Remove
            //Console.WriteLine("{0} - {1} - {2}", this, type.ToString()[0], source);

            Updated = true;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "GroupNumber: {0} - Square: {1} - IsAvailable: {2}", GroupNumber, Square, GetAvailability());
        }
    }
}
