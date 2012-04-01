using System;

namespace OSP.SudokuSolver.Engine
{
    /// <summary>
    /// Group of squares (for horizontal, vertical and square groups), inherited from Squares class.
    /// </summary>
    public class SquaresGroupOld : Squares
    {
        private SquaresGroupNumber[] _Numbers;

        internal delegate void OnSquareSpottedEventHandler(Square square);
        internal event OnSquareSpottedEventHandler OnSquareSpotted;

        //Constructor
        internal SquaresGroupOld(int size)
        {
            _Numbers = new SquaresGroupNumber[size];

            for (int i = 0; i < size; i++)
                _Numbers[i] = new SquaresGroupNumber(i + 1, size);
        }

        internal new int Add(Square value)
        {
            for (int i = 0; i < _Numbers.Length; i++)
            {
                //this.Numbers[i].AvailableSquares.Add(value);
                this.Numbers[i].AvailableSquares.Add(value);
            }

            return List.Add(value);
        }

        internal SquaresGroupNumber[] Numbers
        {
            get { return _Numbers; }
        }

        internal void SetNumberAvailableSquares(Square square, int iNumber, bool isAvailable)
        {
            if (isAvailable)
            {
                if (square.IsNumberUsedInRelatedGroups(iNumber + 1)) //If the number is used in another group, don't make it available...
                    return;

                //if (!this.Numbers[iNumber].AvailableSquares.Contains(square))
                //    this.Numbers[iNumber].AvailableSquares.Add(square);

                if (!this.Numbers[iNumber].AvailableSquares.Contains(square))
                    this.Numbers[iNumber].AvailableSquares.Add(square);
            }
            else
            {
                //if (this.Numbers[iNumber].AvailableSquares.Contains(square))
                //    this.Numbers[iNumber].AvailableSquares.Remove(square);

                if (this.Numbers[iNumber].AvailableSquares.Contains(square))
                    this.Numbers[iNumber].AvailableSquares.Remove(square);
            }
        }

        //Checks the available squares of the number. If only one square left for the number, event raises...
        internal void Check(int iNumber)
        {
            if (this.Numbers[iNumber].AvailableSquares.Count == 1)
            {
                foreach (Square currentSquare in this.Numbers[iNumber].AvailableSquares)
                {
                    if (currentSquare.IsAvailable)
                    {
                        if (OnSquareSpotted != null)
                        {
                            //TODO This cannot work, because the class name has changed..
                            //currentSquare.SetSpottedbyGroup(iNumber + 1, this);
                            OnSquareSpotted(currentSquare);
                        }

                        break;                    
                    }
                }
            }
        }
    }
}