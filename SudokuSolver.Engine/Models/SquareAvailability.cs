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

        // This is for both search and remove hints! be careful
        public bool GetAvailability()
        {
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
            return string.Format(CultureInfo.InvariantCulture, "S: {0} - N: {1} - S: {2,21} - H: {3,21} - V: {4,21}", Square, SudokuNumber, SquareTypeSource, HorizontalTypeSource, VerticalTypeSource);
        }
    }
}
