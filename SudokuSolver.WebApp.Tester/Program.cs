﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OSP.SudokuSolver.Engine;
using OSP.SudokuSolver.WebApp.Models;

namespace OSP.SudokuSolver.WebApp.Tester
{
    //class Program
    //{
    //static void Main(string[] args)
    //{
    //    var endpoint = "http://localhost:56105/api/sudokuapi/";

    //    var webApiClient = new WebApiClient(endpoint);

    //    //Create a sudoku
    //    var postResult = webApiClient.Post<SudokuContainer>(null);

    //    //Get the list
    //    var getListResult = webApiClient.GetList<List<SudokuContainer>>();

    //    foreach (var s in getListResult)
    //    {
    //        Console.WriteLine("Sudoku - Id: {0}", s.Id.ToString());

    //        //foreach (var square in s.SquareList)
    //        //    Console.WriteLine("Square - Id: {0} - Number: {1}", square.SquareNo.ToString(), square.Number.ToString());

    //        Console.WriteLine();
    //    }

    //    //Update a square
    //    webApiClient.Put<SquareContainer>(10, new SquareContainer { SquareNo = 1, Number = 1 });
    //    webApiClient.Put<SquareContainer>(10, new SquareContainer { SquareNo = 2, Number = 2 });
    //    webApiClient.Put<SquareContainer>(10, new SquareContainer { SquareNo = 3, Number = 3 });
    //    webApiClient.Put<SquareContainer>(10, new SquareContainer { SquareNo = 4, Number = 4 });
    //    webApiClient.Put<SquareContainer>(10, new SquareContainer { SquareNo = 5, Number = 5 });
    //    webApiClient.Put<SquareContainer>(10, new SquareContainer { SquareNo = 6, Number = 6 });
    //    webApiClient.Put<SquareContainer>(10, new SquareContainer { SquareNo = 7, Number = 7 });
    //    webApiClient.Put<SquareContainer>(10, new SquareContainer { SquareNo = 8, Number = 8 });

    //    //Get the list again
    //    getListResult = webApiClient.GetList<List<SudokuContainer>>();

    //    foreach (var s in getListResult)
    //    {
    //        Console.WriteLine("Sudoku - Id: {0}", s.Id.ToString());

    //        //foreach (var square in s.SquareList)
    //        //    Console.WriteLine("Square - Id: {0} - Number: {1}", square.SquareNo.ToString(), square.Number.ToString());

    //        Console.WriteLine();
    //    }

    //    Console.ReadKey();

    //}
    //}

    class Program
    {
        static string endPointBase = "http://localhost:56105/api/sudokuapi/";
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

                //TODO Invalid parameter is only for fill + case?
                //Try to apple to the other commands?
                //Also fill [x,x] pattern is not checked, "fill 0" fails for instance
                if ((commandLine.StartsWith("fill") && !commandLine.StartsWith("filled")) || commandLine.StartsWith("load"))
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
                    case "fill": FillSquare(parameters); break;
                    case "solve": Solve(); break;
                    case "load": LoadCase(int.Parse(parameters)); break;
                    case "quit": return;

                    case "sudoku": ShowSudoku(); break;
                    case "filled": ShowFilled(); break;
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
                Console.WriteLine("Id: {0}", container.Id.ToString());
        }

        static void NewSudoku()
        {
            //Create a sudoku
            GetWebApiClient("create").Post<SudokuContainer>(null);
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

        static void FillSquare(string parameters)
        {
            //Get square id & number
            int id = int.Parse(parameters.Split(',')[0]);
            int number = int.Parse(parameters.Split(',')[1]);

            var squareContainer = new SquareContainer();
            squareContainer.SquareId = id;
            squareContainer.Number = number;

            //Fill
            GetWebApiClient("fillsquare").Post<SquareContainer>(currentSudokuId, squareContainer);
        }

        static void Solve()
        {
            //Toggle
            var result = GetWebApiClient("solve").Post<string>(currentSudokuId, "");

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
            var container = GetWebApiClient("item").GetItem<SudokuContainer>(id);

            //Set current sudoku id
            currentSudokuId = container.Id;

            //Message
            Console.WriteLine("Sudoku loaded; Id: {0}", container.Id.ToString());

            //foreach (var square in container.SquareList)
            //    Console.WriteLine("Square - Id: {0} - Number: {1}", square.Id.ToString(), square.Number.ToString());
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
                if ((group.Id % sqrtSize) == 0)
                    Console.WriteLine();
            }
        }

        static void ShowFilled()
        {
            //Get
            var squares = GetWebApiClient("filled").GetItem<IEnumerable<SquareContainer>>(currentSudokuId);

            foreach (var filled in squares)
                Console.WriteLine(string.Format("  Id {0}: {1} - {2}", filled.SquareId.ToString("D2"), filled.Number.ToString(), filled.FillType.ToString()));
        }

        /// <summary>
        /// Show the availibility per square, per number
        /// </summary>
        static void ShowAvailability()
        {
            //Get numbers first
            var numbers = GetWebApiClient("numbers").GetItem<IEnumerable<NumberContainer>>(currentSudokuId);

            //Remove zero
            var numbersExceptZero = numbers.Where(n => n.Value > 0);

            //Header; display the sudoku numbers
            Console.Write("        "); //Necessary to make the starting points equal - TODO This should check the length of the squareText?
            foreach (var number in numbersExceptZero)
                Console.Write(string.Format(" | {0}", number.ToString()));
            Console.WriteLine();

            //Details

            //Get the item
            var container = GetWebApiClient("item").GetItem<SudokuContainer>(currentSudokuId);

            foreach (var square in container.SquareList)
            {
                //Square text: Square id + value
                var output = string.Format("Id {0}: {1}", square.SquareId.ToString("D2"), square.Number.ToString());
                Console.Write(output);

                //Availability per number; "X" for available squares, "." for non-available ones
                foreach (var number in numbersExceptZero)
                {
                    var availableNumbersOfSquare = GetWebApiClient("squareavailability").GetItem<IEnumerable<NumberContainer>>(currentSudokuId, square.SquareId);

                    var availableText = string.Format(" | {0}", availableNumbersOfSquare.Any(availableNumber => availableNumber.Value.Equals(number.Value)) ? "X" : ".");
                    //var availableText = string.Format(" | {0}", true ? "X" : ".");
                    Console.Write(availableText);
                }

                Console.WriteLine();
            }
        }

        static void ShowNumbers()
        {
            //Get
            var numbers = GetWebApiClient("numbers").GetItem<IEnumerable<NumberContainer>>(currentSudokuId);

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
                Console.WriteLine(string.Format("P Id {0}: {1} - {2} - {3}", potential.SquareId.ToString("D2"), potential.SquareValue.ToString(), potential.PotentialValue.ToString(), potential.PotentialType.ToString()));
        }

        static void ClearScreen()
        {
            Console.Clear();
        }

        static void Help()
        {
            var sbOutput = new System.Text.StringBuilder();
            sbOutput.AppendLine("General Commands");
            sbOutput.AppendLine(". list");
            sbOutput.AppendLine(". new");
            sbOutput.AppendLine(". autosolve - Currently: " + (true ? "on" : "off"));
            sbOutput.AppendLine(". fill [square], [number]");
            sbOutput.AppendLine(". solve");
            sbOutput.AppendLine(". load [case no] - 1, 2, 3, 4, 5, 6, 7, 8, 9");
            sbOutput.AppendLine(". quit");

            sbOutput.AppendLine().AppendLine("Display Commands");
            sbOutput.AppendLine(". sudoku");
            sbOutput.AppendLine(". filled");
            sbOutput.AppendLine(". availability");
            sbOutput.AppendLine(". numbers");
            sbOutput.AppendLine(". potentials");
            sbOutput.AppendLine(". clear");

            Console.Write(sbOutput.ToString());
        }

        //static void Sudoku_SquareNumberChanged(Square square)
        //{
        //    Console.WriteLine(string.Format("  Id {0}: {1} - {2}", square.Id.ToString("D2"), square.Number.ToString(), square.FillType.ToString()));
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
