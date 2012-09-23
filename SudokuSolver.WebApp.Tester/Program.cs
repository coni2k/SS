using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SudokuSolver.Engine;
using SudokuSolver.WebApp.Models;

namespace SudokuSolver.WebApp.Tester
{
    class Program
    {
        static string endPointBase = "http://localhost:56105/api/sudoku/";
        static int currentSudokuId = 1;

        static void Main(string[] args)
        {
            ShowCommands(args);
        }

        /// <summary>
        /// Show available commands
        /// </summary>
        /// <param name="args"></param>
        static void ShowCommands(string[] args)
        {
            string commandLine = "", parameters = "";

            do
            {
                Console.WriteLine();

                if (args.Length.Equals(0))
                {
                    Console.WriteLine("Please enter your command: ");
                    commandLine = Console.ReadLine().ToLower().Trim();
                    Console.WriteLine();
                }
                else
                {
                    commandLine = string.Format("{0} {1}", args[0], args[1]);
                    args = new string[0] { };
                }

                //TODO Invalid parameter is only for update + case?
                //Try to apple to the other commands?
                //Also update [x,x] pattern is not checked, "update 0" fails for instance
                if (commandLine.StartsWith("update") || commandLine.StartsWith("load"))
                {
                    if (commandLine.IndexOf(" ") > 0)
                    {
                        parameters = commandLine.Substring(commandLine.IndexOf(" ") + 1, commandLine.Length - (commandLine.IndexOf(" ") + 1));
                        commandLine = commandLine.Substring(0, commandLine.IndexOf(" "));
                    }
                    else
                    {
                        Console.WriteLine("Invalid parameter");
                        continue;
                    }
                }

                switch (commandLine)
                {
                    case "list": ShowList(); break;
                    case "new": NewSudoku(); break;
                    case "autosolve": ToggleAutoSolve(); break;
                    case "update": UpdateSquare(parameters); break;
                    case "solve": Solve(); break;
                    case "load": LoadCase(int.Parse(parameters)); break;
                    case "reset": Reset(); break;
                    case "quit": return;

                    case "sudoku": ShowSudoku(); break;
                    case "used": ShowUsed(); break;
                    case "availability": ShowAvailability(); break;
                    case "numbers": ShowNumbers(); break;
                    case "potentials": ShowPotentialSquares(); break;
                    case "clear": ClearScreen(); break;

                    default: Help(); break;
                }

            } while (true);
        }

        static void ShowList()
        {
            //Get the list
            var list = GetWebApiClient("list").GetList<List<SudokuContainer>>();

            Console.WriteLine("Sudoku list;");
            foreach (var container in list)
                Console.WriteLine("Id: {0}", container.SudokuId.ToString());
        }

        static void NewSudoku()
        {
            //Create a sudoku
            GetWebApiClient("newsudoku").Post<SudokuContainer>(null);
        }

        static void ToggleAutoSolve()
        {
            //Toggle
            var result = GetWebApiClient("toggleautosolve").Post<string>(currentSudokuId, "");

            //Get the item
            var container = GetWebApiClient("item").GetItem<SudokuContainer>(currentSudokuId);

            //Message
            Console.WriteLine("autosolve - Currently: " + (container.AutoSolve ? "on" : "off"));
        }

        static void UpdateSquare(string parameters)
        {
            //Get square id & number
            int id = int.Parse(parameters.Split(',')[0]);
            int number = int.Parse(parameters.Split(',')[1]);

            var squareContainer = new SquareContainer();
            squareContainer.SquareId = id;
            squareContainer.Number = number;

            //Update
            GetWebApiClient("updatesquare").Post<SquareContainer>(currentSudokuId, squareContainer);
        }

        static void Solve()
        {
            //Toggle
            var result = GetWebApiClient("solve").Post(currentSudokuId);

            Console.WriteLine("OK");
        }

        /// <summary>
        /// Loads the sample cases
        /// See Helper.xslx file for more info
        /// </summary>
        /// <param name="caseNo"></param>
        static void LoadCase(int id)
        {
            //Get the item
            SudokuContainer container = null;
            try
            {
                container = GetWebApiClient("item").GetItem<SudokuContainer>(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return;
            }

            //Set current sudoku id
            currentSudokuId = container.SudokuId;

            //Message
            Console.WriteLine("Sudoku loaded; Id: {0}", container.SudokuId.ToString());

            //foreach (var square in container.SquareList)
            //    Console.WriteLine("Square - Id: {0} - Number: {1}", square.Id.ToString(), square.Number.ToString());
        }

        static void Reset()
        {
            GetWebApiClient("reset").Post(currentSudokuId);

            Console.WriteLine("OK");
        }

        static void ShowSudoku()
        {
            //Get
            var groups = GetWebApiClient("horizontaltypegroups").GetItem<IEnumerable<GroupContainer>>(currentSudokuId);

            var size = groups.Count();
            var sqrtSize = (int)Math.Sqrt(size);

            foreach (var group in groups)
            {
                foreach (var square in group.Squares)
                {
                    //Square value
                    string output = string.Format(" {0}", square.Number.Equals(0) ? "." : square.Number.ToString());

                    //Seperator for square groups; (3rd, 6th, 12th squares)
                    if (square.SquareId % sqrtSize == 0 && square.SquareId % size != 0)
                        output += " |";

                    Console.Write(output);
                }

                //Line break after every row (9th, 18th, 27th etc. squares)
                Console.WriteLine();

                //Extra line break after every 3. horizontal group (27th, 54th squares)
                if ((group.GroupId % sqrtSize) == 0)
                    Console.WriteLine();
            }
        }

        static void ShowUsed()
        {
            //Get
            var squares = GetWebApiClient("used").GetItem<IEnumerable<SquareContainer>>(currentSudokuId);

            foreach (var usedSquare in squares)
                Console.WriteLine(string.Format("  Id {0}: {1} - {2}", usedSquare.SquareId.ToString("D2"), usedSquare.Number.ToString(), usedSquare.AssignType.ToString()));
        }

        /// <summary>
        /// Show the availibility per square, per number
        /// </summary>
        static void ShowAvailability()
        {
            //Get numbers first
            var numbers = GetWebApiClient("numbers").GetItem<IEnumerable<Number>>(currentSudokuId);

            //Remove zero
            var numbersExceptZero = numbers.Where(n => n.Value > 0);

            //Header; display the sudoku numbers
            Console.Write("        "); //Necessary to make the starting points equal - TODO This should check the length of the squareText?
            foreach (var number in numbersExceptZero)
                Console.Write(string.Format(" | {0}", number.ToString()));
            Console.WriteLine();

            //Details

            //Get the item
            var squares = GetWebApiClient("squares").GetItem<IEnumerable<SquareContainer>>(currentSudokuId);

            foreach (var square in squares)
            {
                //Square text: Square id + value
                var output = string.Format("Id {0}: {1}", square.SquareId.ToString("D2"), square.Number.ToString());
                Console.Write(output);

                //Availability per number; "X" for available squares, "." for non-available ones
                foreach (var number in numbersExceptZero)
                {
                    //TODO THIS METHOD DOESNT EXIST ANYMORE - UPDATE THIS BLOCK!
                    var availableNumbersOfSquare = GetWebApiClient("squareavailability").GetItem<IEnumerable<Number>>(currentSudokuId, square.SquareId);

                    var availableText = string.Format(" | {0}", availableNumbersOfSquare.Any(availableNumber => availableNumber.Value.Equals(number.Value)) ? "X" : ".");
                    Console.Write(availableText);
                }

                Console.WriteLine();
            }
        }

        static void ShowNumbers()
        {
            //Get
            var numbers = GetWebApiClient("numbers").GetItem<IEnumerable<Number>>(currentSudokuId);

            foreach (var number in numbers)
                Console.WriteLine(string.Format("Number: {0} - Counter: {1}", number.ToString(), number.Count.ToString()));
        }

        static void ShowPotentialSquares()
        {
            //Get
            var potentials = GetWebApiClient("potentials").GetItem<IEnumerable<PotentialContainer>>(currentSudokuId);

            if (potentials.Count().Equals(0))
                Console.WriteLine("There are no potential squares");

            foreach (var potential in potentials)
                Console.WriteLine(string.Format("P Id {0}: {1} - {2} - {3}", potential.SquareId.ToString("D2"), potential.SquareGroupId.ToString(), potential.PotentialValue.ToString(), potential.PotentialType.ToString()));
        }

        static void ClearScreen()
        {
            Console.Clear();
        }

        static void Help()
        {
            //Get current item

            var container = GetWebApiClient("item").GetItem<SudokuContainer>(currentSudokuId);

            var sbOutput = new System.Text.StringBuilder();
            sbOutput.AppendLine("General Commands");
            sbOutput.AppendLine(". list");
            sbOutput.AppendLine(". new");
            sbOutput.AppendLine(". autosolve - Currently: " + (container.AutoSolve ? "on" : "off"));
            sbOutput.AppendLine(". update [square], [number]");
            sbOutput.AppendLine(". solve");
            sbOutput.AppendLine(". load [case no] - 1, 2, 3, 4, 5, 6, 7, 8, 9");
            sbOutput.AppendLine(". quit");

            sbOutput.AppendLine().AppendLine("Display Commands");
            sbOutput.AppendLine(". sudoku");
            sbOutput.AppendLine(". used");
            sbOutput.AppendLine(". availability");
            sbOutput.AppendLine(". numbers");
            sbOutput.AppendLine(". potentials");
            sbOutput.AppendLine(". clear");

            Console.Write(sbOutput.ToString());
        }

        //static void Sudoku_SquareNumberChanged(Square square)
        //{
        //    Console.WriteLine(string.Format("  Id {0}: {1} - {2}", square.Id.ToString("D2"), square.Number.ToString(), square.AssignType.ToString()));
        //}

        //static void Sudoku_PotentialSquareFound(Potential potential)
        //{
        //    Console.WriteLine(string.Format("P Id {0}: {1} - {2} - {3}", potential.Square.Id.ToString("D2"), potential.Square.Number.ToString(), potential.Number.ToString(), potential.PotentialType.ToString()));
        //}

        /// <summary>
        /// Creates a web api client
        /// </summary>
        /// <param name="action"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        static WebApiClient GetWebApiClient(string action)
        {
            //endPoint = endPointBase + action
            var endPoint = string.Format("{0}{1}/", endPointBase, action);

            //Return
            return new WebApiClient(endPoint);
        }
    }
}
