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

        //public bool GetAvailability()
        //{
        //    return GetAvailability(null);
        //}

        // This is for remove hints
        public bool GetAvailability(Square source)
        {
            return (SquareTypeSource == null
                || SquareTypeSource.Equals(source))
                && (HorizontalTypeSource == null
                || HorizontalTypeSource.Equals(source))
                && (VerticalTypeSource == null
                || VerticalTypeSource.Equals(source));
        }
        
        // Currently this is just for SearchHints not for Remove..
        public bool GetAvailability()
        {
            if (!Square.IsAvailable)
                return false;

            var squareTypeSourceCondition = SquareTypeSource == null
                || (SquareTypeSource.IsGroupNumberMethodHint
                && ((!Square.IsAvailable
                && SquareTypeSource.Availabilities.Single(a => a.SudokuNumber == Square.SudokuNumber).GetAvailabilityForSource(Square))
                || SquareTypeSource.Equals(Square)));

            var horizontalTypeSourceCondition = HorizontalTypeSource == null
                || (HorizontalTypeSource.IsGroupNumberMethodHint
                && ((!Square.IsAvailable
                && HorizontalTypeSource.Availabilities.Single(a => a.SudokuNumber == Square.SudokuNumber).GetAvailabilityForSource(Square))
                || HorizontalTypeSource.Equals(Square)));

            var verticalTypeSourceCondition = VerticalTypeSource == null
                || (VerticalTypeSource.IsGroupNumberMethodHint
                && ((!Square.IsAvailable
                && VerticalTypeSource.Availabilities.Single(a => a.SudokuNumber == Square.SudokuNumber).GetAvailabilityForSource(Square))
                || VerticalTypeSource.Equals(Square)));

            return squareTypeSourceCondition && horizontalTypeSourceCondition && verticalTypeSourceCondition;
        }

        public bool GetAvailabilityForSource(Square source)
        {
            return (SquareTypeSource == null || SquareTypeSource == source)
                && (HorizontalTypeSource == null || HorizontalTypeSource == source)
                && (VerticalTypeSource == null || VerticalTypeSource == source);
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
            //return string.Format(CultureInfo.InvariantCulture, "GN: {0} - S: {1} - IsAvailable: {2}", GroupNumber, Square, GetAvailability());
            return string.Format(CultureInfo.InvariantCulture, "GN: {0} - S: {1} - S: {2,21} - H: {3,21} - V: {4,21}", GroupNumber, Square, SquareTypeSource, HorizontalTypeSource, VerticalTypeSource);
        }
    }
}
