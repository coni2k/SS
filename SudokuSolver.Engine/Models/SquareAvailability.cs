using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SudokuSolver.Engine
{
    public class SquareAvailability
    {
        internal Square Square { get; private set; }
        // TODO Internal ?
        public SudokuNumber Number { get; private set; }
        internal Square SquareTypeSource { get; private set; }
        internal Square HorizontalTypeSource { get; private set; }
        internal Square VerticalTypeSource { get; private set; }
        //public bool IsAvailableToAddHint
        //{
        //    get
        //    {
        //        return SquareTypeSource == null
        //            && HorizontalTypeSource == null
        //            && VerticalTypeSource == null;
        //        // && Square.IsAvailable;
        //    }
        //}
        //public bool IsAvailableToRemoveHint
        //{
        //    get
        //    {
        //        return SquareTypeSource == null || SquareTypeSource
        //            && HorizontalTypeSource == null
        //            && VerticalTypeSource == null;
        //        // && Square.IsAvailable;
        //    }
        //}
        public bool GetAvailability()
        {
            return GetAvailability(null);
        }
        //public bool GetAvailability(Square source)
        //{
        //    return (SquareTypeSource == null
        //        || SquareTypeSource.Equals(source))
        //        && (HorizontalTypeSource == null
        //        || HorizontalTypeSource.Equals(source))
        //        && (VerticalTypeSource == null
        //        || VerticalTypeSource.Equals(source));
        //}
        public bool GetAvailability(Square source)
        {
            return (SquareTypeSource == null
                )
                && (HorizontalTypeSource == null
                )
                && (VerticalTypeSource == null
                );
        }
        internal bool Updated { get; set; }

        internal SquareAvailability(Square square, SudokuNumber number)
        {
            Square = square;
            Number = number;
        }

        internal void UpdateAvailability(GroupType type, Square source)
        {
            switch (type)
            {
                case GroupType.Square: SquareTypeSource = source; break;
                case GroupType.Horizontal: HorizontalTypeSource = source; break;
                case GroupType.Vertical: VerticalTypeSource = source; break;
            }

            Updated = true;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Square: {0} - Number: {1} - IsAvailable: {2}", Square, Number, GetAvailability());
        }
    }
}
