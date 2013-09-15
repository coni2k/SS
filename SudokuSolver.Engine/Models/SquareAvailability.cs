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
        public SudokuNumber SudokuNumber { get; private set; }
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

        //public bool GetAvailability(Square source)
        //{
        //    return (SquareTypeSource == null
        //        || SquareTypeSource.Equals(source))
        //        && (HorizontalTypeSource == null
        //        || HorizontalTypeSource.Equals(source))
        //        && (VerticalTypeSource == null
        //        || VerticalTypeSource.Equals(source));
        //}

        //public bool GetAvailability2()
        //{
        //    return GetAvailability2(null);
        //}

        //public bool GetAvailability2(Square source)
        //{
        //    return (SquareTypeSource == null
        //        )
        //        && (HorizontalTypeSource == null
        //        )
        //        && (VerticalTypeSource == null
        //        );
        //}

        //public bool GetAvailabilityOld()
        //{
        //    return SquareTypeSource == null
        //        && HorizontalTypeSource == null
        //        && VerticalTypeSource == null;
        //}

        //public bool GetAvailabilityForRemoveHints2()
        //{
        //    return (SquareTypeSource == null
        //        || SquareTypeSource.AssignType == AssignType.Hint)
        //        && (HorizontalTypeSource == null
        //        || HorizontalTypeSource.AssignType == AssignType.Hint)
        //        && (VerticalTypeSource == null
        //        || VerticalTypeSource.AssignType == AssignType.Hint);
        //}

        //public bool GetAvailabilityForRemoveHints3(Square source)
        //{
        //    return (SquareTypeSource == null
        //        || (SquareTypeSource.AssignType == AssignType.Hint
        //        && (SquareTypeSource != source
        //        && (source.IsAvailable
        //        || SquareTypeSource.Availabilities.Single(a => a.Number == source.SudokuNumber).GetAvailabilityForSource(source)))))
        //        && (HorizontalTypeSource == null
        //        || (HorizontalTypeSource.AssignType == AssignType.Hint
        //        && (HorizontalTypeSource != source
        //        && (source.IsAvailable
        //        || HorizontalTypeSource.Availabilities.Single(a => a.Number == source.SudokuNumber).GetAvailabilityForSource(source)))))
        //        && (VerticalTypeSource == null
        //        || (VerticalTypeSource.AssignType == AssignType.Hint
        //        && (VerticalTypeSource != source
        //        && (source.IsAvailable
        //        || VerticalTypeSource.Availabilities.Single(a => a.Number == source.SudokuNumber).GetAvailabilityForSource(source)))));
        //}

        // public bool GetAvailabilityForRemoveHints(Square source)

        // This is for both search and remove hints! be careful
        public bool GetAvailability()
        {
            //if (!Square.IsAvailable)
            //    return false;

            var squareTypeSourceCondition = SquareTypeSource == null
                || (SquareTypeSource.IsSquareMethodHint
                && ((!Square.IsAvailable
                && SquareTypeSource.Availabilities.Single(a => a.SudokuNumber == Square.SudokuNumber).GetAvailabilityForSource(Square))
                || SquareTypeSource.Equals(Square)));

            var horizontalTypeSourceCondition = HorizontalTypeSource == null
                || (HorizontalTypeSource.IsSquareMethodHint
                && ((!Square.IsAvailable
                && HorizontalTypeSource.Availabilities.Single(a => a.SudokuNumber == Square.SudokuNumber).GetAvailabilityForSource(Square))
                || HorizontalTypeSource.Equals(Square)));

            var verticalTypeSourceCondition = VerticalTypeSource == null
                || (VerticalTypeSource.IsSquareMethodHint
                && ((!Square.IsAvailable
                && VerticalTypeSource.Availabilities.Single(a => a.SudokuNumber == Square.SudokuNumber).GetAvailabilityForSource(Square))
                || VerticalTypeSource.Equals(Square)));

            //return squareTypeSourceCondition && horizontalTypeSourceCondition && verticalTypeSourceCondition && Square.IsAvailable;
            return squareTypeSourceCondition && horizontalTypeSourceCondition && verticalTypeSourceCondition;
        }

        public bool GetAvailabilityForSource(Square source)
        {
            return (SquareTypeSource == null || SquareTypeSource == source)
                && (HorizontalTypeSource == null || HorizontalTypeSource == source)
                && (VerticalTypeSource == null || VerticalTypeSource == source);
        }

        internal bool Updated { get; set; }

        internal SquareAvailability(Square square, SudokuNumber number)
        {
            Square = square;
            SudokuNumber = number;
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
            //return string.Format(CultureInfo.InvariantCulture, "Square: {0} - Number: {1} - IsAvailable: {2}", Square, Number, GetAvailability());
            return string.Format(CultureInfo.InvariantCulture, "S: {0} - N: {1} - S: {2,21} - H: {3,21} - V: {4,21}", Square, SudokuNumber, SquareTypeSource, HorizontalTypeSource, VerticalTypeSource);
        }
    }
}
