using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OSP.SudokuSolver.Engine;

namespace OSP.SudokuSolver.ConsoleApp
{
    /// <summary>
    /// For testing purposes
    /// </summary>
    class Program
    {
        static Sudoku Sudoku { get; set; }

        static void Main(string[] args)
        {
            NewSudoku();

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
                if (commandLine.StartsWith("fill ") || commandLine.StartsWith("case"))
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
                    case "new": NewSudoku(); break;
                    case "autosolve": ToggleAutoSolve(); break;
                    case "fill": FillSquare(parameters); break;
                    case "solve": Solve(); break;
                    case "case": LoadCase(int.Parse(parameters)); break;
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

        static void NewSudoku()
        {
            ClearScreen();

            Sudoku = new Sudoku();
            Sudoku.SquareNumberChanged += new Square.SquareEventHandler(Sudoku_SquareNumberChanged);
            Sudoku.PotentialSquareFound += new Potential.FoundEventHandler(Sudoku_PotentialSquareFound);

            Console.WriteLine("New sudoku is ready!");
        }

        static void ToggleAutoSolve()
        {
            Sudoku.AutoSolve = !Sudoku.AutoSolve;
            Console.WriteLine("autosolve - Currently: " + (Sudoku.AutoSolve ? "on" : "off"));
        }

        static void FillSquare(string parameters)
        {
            try
            {
                //Get square id & number
                int id = int.Parse(parameters.Split(',')[0]);
                int number = int.Parse(parameters.Split(',')[1]);

                //Fill
                Sudoku.FillSquare(id, number);
            }
            catch (Exception ex) //TODO Can be removed later?
            {
                //Console.WriteLine("Exception");
                Console.WriteLine("Message: " + ex.Message);
                //Console.WriteLine("Stacktrace: " + ex.StackTrace);
            }
        }

        static void Solve()
        {
            Sudoku.Solve();
        }

        static void ShowSudoku()
        {
            foreach (var group in Sudoku.AllHorizontalTypeGroups)
            {
                foreach (var square in group.Squares)
                {
                    //Square value
                    string output = string.Format(" {0}", square.Number.IsZero ? "." : square.Number.ToString());

                    //Seperator for square groups; (3rd, 6th, 12th squares)
                    if (square.Id % Sudoku.SquareRootOfSize == 0 && square.Id % Sudoku.Size != 0)
                        output += " |";

                    Console.Write(output);
                }

                //Line break after every row (9th, 18th, 27th etc. squares)
                Console.WriteLine();

                //Extra line break after every 3. horizontal group (27th, 54th squares)
                if ((group.Id % Sudoku.SquareRootOfSize) == 0)
                    Console.WriteLine();
            }
        }

        static void ShowFilled()
        {
            foreach (var filled in Sudoku.AllFilledSquares)
                Console.WriteLine(string.Format("  Id {0}: {1} - {2}", filled.Id.ToString("D2"), filled.Number.ToString(), filled.FillType.ToString()));
        }

        /// <summary>
        /// Show the availibility per square, per number
        /// </summary>
        static void ShowAvailability()
        {
            //Header; display the sudoku numbers
            Console.Write("        "); //Necessary to make the starting points equal - TODO This should check the length of the squareText?
            foreach (var number in Sudoku.AllNumbersExceptZero)
                Console.Write(string.Format(" | {0}", number.ToString()));
            Console.WriteLine();

            //Details
            foreach (var square in Sudoku.AllSquares)
            {
                //Square text: Square id + value
                var output = string.Format("Id {0}: {1}", square.Id.ToString("D2"), square.Number.ToString());
                Console.Write(output);

                //Availability per number; "X" for available squares, "." for non-available ones
                foreach (var number in Sudoku.AllNumbersExceptZero)
                {
                    var availableText = string.Format(" | {0}", square.IsNumberAvailable(number) ? "X" : ".");
                    Console.Write(availableText);
                }

                Console.WriteLine();
            }
        }

        static void ShowNumbers()
        {
            foreach (var number in Sudoku.AllNumbers)
                Console.WriteLine(string.Format("Number: {0} - Counter: {1}", number.ToString(), number.Count.ToString()));
        }

        static void ShowPotentialSquares()
        {
            if (Sudoku.PotentialSquares.Count().Equals(0))
                Console.WriteLine("There are no potential squares");

            foreach (var potential in Sudoku.PotentialSquares.OrderBy(s => s.Square.Id))
            {
                Console.WriteLine(string.Format("P Id {0}: {1} - {2} - {3}", potential.Square.Id.ToString("D2"), potential.Square.Number.ToString(), potential.Number.ToString(), potential.PotentialType.ToString()));
            }
        }

        static void ClearScreen()
        {
            Console.Clear();
        }

        /// <summary>
        /// Loads the sample cases
        /// See Helper.xslx file for more info
        /// </summary>
        /// <param name="caseNo"></param>
        static void LoadCase(int caseNo)
        {
            //Reset first
            NewSudoku();

            //Load the case
            switch (caseNo)
            {
                case 1:
                    Sudoku.FillSquare(1, 1);
                    Sudoku.FillSquare(2, 2);
                    Sudoku.FillSquare(3, 3);
                    Sudoku.FillSquare(4, 4);
                    Sudoku.FillSquare(5, 5);
                    Sudoku.FillSquare(6, 6);
                    Sudoku.FillSquare(7, 7);
                    Sudoku.FillSquare(8, 8);

                    break;

                case 2:

                    Sudoku.FillSquare(1, 1);
                    Sudoku.FillSquare(10, 2);
                    Sudoku.FillSquare(19, 3);
                    Sudoku.FillSquare(28, 4);
                    Sudoku.FillSquare(37, 5);
                    Sudoku.FillSquare(46, 6);
                    Sudoku.FillSquare(55, 7);
                    Sudoku.FillSquare(64, 8);

                    break;

                case 3:

                    Sudoku.FillSquare(1, 1);
                    Sudoku.FillSquare(2, 2);
                    Sudoku.FillSquare(3, 3);
                    Sudoku.FillSquare(10, 4);
                    Sudoku.FillSquare(11, 5);
                    Sudoku.FillSquare(12, 6);
                    Sudoku.FillSquare(19, 7);
                    Sudoku.FillSquare(20, 8);

                    break;

                case 4:

                    Sudoku.FillSquare(1, 1);
                    Sudoku.FillSquare(2, 2);
                    Sudoku.FillSquare(3, 3);
                    Sudoku.FillSquare(8, 7);
                    Sudoku.FillSquare(18, 8);
                    Sudoku.FillSquare(36, 4);
                    Sudoku.FillSquare(45, 5);
                    Sudoku.FillSquare(54, 6);

                    break;

                case 5:

                    Sudoku.FillSquare(13, 1);
                    Sudoku.FillSquare(25, 1);
                    Sudoku.FillSquare(29, 1);
                    Sudoku.FillSquare(57, 1);

                    break;

                case 6:

                    Sudoku.FillSquare(25, 1);
                    Sudoku.FillSquare(57, 1);
                    Sudoku.FillSquare(69, 1);
                    Sudoku.FillSquare(80, 2);

                    break;

                case 7:

                    Sudoku.FillSquare(10, 3);
                    Sudoku.FillSquare(13, 1);
                    Sudoku.FillSquare(17, 2);
                    Sudoku.FillSquare(19, 4);
                    Sudoku.FillSquare(25, 1);
                    Sudoku.FillSquare(28, 5);
                    Sudoku.FillSquare(29, 1);
                    Sudoku.FillSquare(31, 2);
                    Sudoku.FillSquare(46, 6);
                    Sudoku.FillSquare(55, 7);
                    Sudoku.FillSquare(57, 1);
                    Sudoku.FillSquare(61, 2);
                    Sudoku.FillSquare(64, 8);
                    Sudoku.FillSquare(73, 9);

                    break;

                case 101: //Real sudoku samples

                    Sudoku.FillSquare(1, 9);
                    Sudoku.FillSquare(2, 7);
                    Sudoku.FillSquare(3, 3);
                    Sudoku.FillSquare(4, 6);
                    Sudoku.FillSquare(5, 2);
                    Sudoku.FillSquare(6, 8);
                    Sudoku.FillSquare(7, 4);
                    Sudoku.FillSquare(8, 5);
                    Sudoku.FillSquare(9, 1);
                    Sudoku.FillSquare(10, 5);
                    Sudoku.FillSquare(11, 6);
                    Sudoku.FillSquare(12, 8);
                    Sudoku.FillSquare(13, 4);
                    Sudoku.FillSquare(14, 1);
                    Sudoku.FillSquare(15, 3);
                    Sudoku.FillSquare(16, 7);
                    Sudoku.FillSquare(17, 9);
                    Sudoku.FillSquare(18, 2);
                    Sudoku.FillSquare(19, 1);
                    Sudoku.FillSquare(20, 2);
                    Sudoku.FillSquare(21, 4);
                    Sudoku.FillSquare(22, 5);
                    Sudoku.FillSquare(23, 9);
                    Sudoku.FillSquare(24, 7);
                    Sudoku.FillSquare(25, 8);
                    Sudoku.FillSquare(26, 3);
                    Sudoku.FillSquare(27, 6);
                    Sudoku.FillSquare(28, 8);
                    Sudoku.FillSquare(29, 1);
                    Sudoku.FillSquare(30, 5);
                    Sudoku.FillSquare(31, 7);
                    Sudoku.FillSquare(33, 9);
                    Sudoku.FillSquare(34, 6);
                    Sudoku.FillSquare(35, 4);
                    Sudoku.FillSquare(36, 3);
                    Sudoku.FillSquare(37, 6);
                    Sudoku.FillSquare(38, 4);
                    Sudoku.FillSquare(39, 9);
                    Sudoku.FillSquare(40, 3);
                    Sudoku.FillSquare(41, 5);
                    Sudoku.FillSquare(42, 2);
                    Sudoku.FillSquare(43, 1);
                    Sudoku.FillSquare(44, 7);
                    Sudoku.FillSquare(45, 8);
                    Sudoku.FillSquare(46, 2);
                    Sudoku.FillSquare(47, 3);
                    Sudoku.FillSquare(48, 7);
                    Sudoku.FillSquare(49, 1);
                    Sudoku.FillSquare(50, 6);
                    Sudoku.FillSquare(51, 4);
                    Sudoku.FillSquare(52, 9);
                    Sudoku.FillSquare(54, 5);
                    Sudoku.FillSquare(55, 4);
                    Sudoku.FillSquare(56, 5);
                    Sudoku.FillSquare(57, 2);
                    Sudoku.FillSquare(58, 8);
                    Sudoku.FillSquare(59, 7);
                    Sudoku.FillSquare(61, 3);
                    Sudoku.FillSquare(62, 6);
                    Sudoku.FillSquare(63, 9);
                    Sudoku.FillSquare(64, 7);
                    Sudoku.FillSquare(65, 8);
                    Sudoku.FillSquare(66, 6);
                    Sudoku.FillSquare(67, 9);
                    Sudoku.FillSquare(68, 3);
                    Sudoku.FillSquare(69, 1);
                    Sudoku.FillSquare(70, 5);
                    Sudoku.FillSquare(71, 2);
                    Sudoku.FillSquare(72, 4);
                    Sudoku.FillSquare(73, 3);
                    Sudoku.FillSquare(74, 9);
                    Sudoku.FillSquare(75, 1);
                    Sudoku.FillSquare(76, 2);
                    Sudoku.FillSquare(77, 4);
                    Sudoku.FillSquare(78, 6);
                    Sudoku.FillSquare(80, 8);
                    Sudoku.FillSquare(81, 7);

                    break;

                case 102:

                    Sudoku.FillSquare(3, 8);
                    Sudoku.FillSquare(4, 3);
                    Sudoku.FillSquare(5, 4);
                    Sudoku.FillSquare(6, 2);
                    Sudoku.FillSquare(7, 9);
                    Sudoku.FillSquare(12, 9);
                    Sudoku.FillSquare(16, 7);
                    Sudoku.FillSquare(19, 4);
                    Sudoku.FillSquare(27, 3);
                    Sudoku.FillSquare(30, 6);
                    Sudoku.FillSquare(31, 4);
                    Sudoku.FillSquare(32, 7);
                    Sudoku.FillSquare(33, 3);
                    Sudoku.FillSquare(34, 2);
                    Sudoku.FillSquare(38, 3);
                    Sudoku.FillSquare(44, 1);
                    Sudoku.FillSquare(48, 2);
                    Sudoku.FillSquare(49, 8);
                    Sudoku.FillSquare(50, 5);
                    Sudoku.FillSquare(51, 1);
                    Sudoku.FillSquare(52, 6);
                    Sudoku.FillSquare(55, 7);
                    Sudoku.FillSquare(63, 8);
                    Sudoku.FillSquare(66, 4);
                    Sudoku.FillSquare(70, 1);
                    Sudoku.FillSquare(75, 3);
                    Sudoku.FillSquare(76, 6);
                    Sudoku.FillSquare(77, 9);
                    Sudoku.FillSquare(78, 7);
                    Sudoku.FillSquare(79, 5);

                    break;

                default:
                    break;
            }
        }

        static void Help()
        {
            var sbOutput = new System.Text.StringBuilder();
            sbOutput.AppendLine("General Commands");
            sbOutput.AppendLine(". new");
            sbOutput.AppendLine(". autosolve - Currently: " + (Sudoku.AutoSolve ? "on" : "off"));
            sbOutput.AppendLine(". fill [square], [number]");
            sbOutput.AppendLine(". solve");
            sbOutput.AppendLine(". case [case no] - 1, 2, 3, 4, 5, 6, 7, 101, 102");
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

        static void Sudoku_SquareNumberChanged(Square square)
        {
            Console.WriteLine(string.Format("  Id {0}: {1} - {2}", square.Id.ToString("D2"), square.Number.ToString(), square.FillType.ToString()));
        }

        static void Sudoku_PotentialSquareFound(Potential potential)
        {
            Console.WriteLine(string.Format("P Id {0}: {1} - {2} - {3}", potential.Square.Id.ToString("D2"), potential.Square.Number.ToString(), potential.Number.ToString(), potential.PotentialType.ToString()));
        }
    }
}
