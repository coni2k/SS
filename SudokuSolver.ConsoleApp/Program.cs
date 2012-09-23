using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SudokuSolver.Engine;

namespace SudokuSolver.ConsoleApp
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

                //TODO Invalid parameter is only for update + case?
                //Try to apply to the other commands?
                if (commandLine.StartsWith("update") || commandLine.StartsWith("case"))
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
                    case "update": UpdateSquare(parameters); break;
                    case "solve": Solve(); break;
                    case "case": LoadCase(int.Parse(parameters)); break;
                    case "quit": return;

                    case "sudoku": ShowSudoku(); break;
                    case "used": ShowUsedSquares(); break;
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

        static void UpdateSquare(string parameters)
        {
            try
            {
                //Get square id & number
                int id = int.Parse(parameters.Split(',')[0]);
                int number = int.Parse(parameters.Split(',')[1]);

                //Update
                Sudoku.UpdateSquare(id, number);
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
            foreach (var group in Sudoku.HorizontalTypeGroups)
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

        static void ShowUsedSquares()
        {
            foreach (var usedSquare in Sudoku.UsedSquares)
                Console.WriteLine(string.Format("  Id {0}: {1} - {2}", usedSquare.Id.ToString("D2"), usedSquare.Number.ToString(), usedSquare.AssignType.ToString()));
        }

        /// <summary>
        /// Show the availibility per square, per number
        /// </summary>
        static void ShowAvailability()
        {
            //Header; display the sudoku numbers
            Console.Write("        "); //Necessary to make the starting points equal - TODO This should check the length of the squareText?
            foreach (var number in Sudoku.NumbersExceptZero)
                Console.Write(string.Format(" | {0}", number.ToString()));
            Console.WriteLine();

            //Details
            foreach (var square in Sudoku.Squares)
            {
                //Square text: Square id + value
                var output = string.Format("Id {0}: {1}", square.Id.ToString("D2"), square.Number.ToString());
                Console.Write(output);

                //Availability per number; "X" for available squares, "." for non-available ones
                foreach (var number in Sudoku.NumbersExceptZero)
                {
                    //var availableText = string.Format(" | {0}", square.IsNumberAvailable(number) ? "X" : ".");
                    var availableText = string.Format(" | {0}", square.IsNumberAvailable(number) ? "X" : ".");
                    Console.Write(availableText);
                }

                Console.WriteLine();
            }
        }

        static void ShowNumbers()
        {
            //foreach (var number in Sudoku.Numbers)
            //    Console.WriteLine(string.Format("Number: {0} - Counter: {1}", number.ToString(), number.GetCount().ToString()));

            foreach (var number in Sudoku.Numbers)
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
                    Sudoku.UpdateSquare(1, 1);
                    Sudoku.UpdateSquare(2, 2);
                    Sudoku.UpdateSquare(3, 3);
                    Sudoku.UpdateSquare(4, 4);
                    Sudoku.UpdateSquare(5, 5);
                    Sudoku.UpdateSquare(6, 6);
                    Sudoku.UpdateSquare(7, 7);
                    Sudoku.UpdateSquare(8, 8);

                    break;

                case 2:

                    Sudoku.UpdateSquare(1, 1);
                    Sudoku.UpdateSquare(10, 2);
                    Sudoku.UpdateSquare(19, 3);
                    Sudoku.UpdateSquare(28, 4);
                    Sudoku.UpdateSquare(37, 5);
                    Sudoku.UpdateSquare(46, 6);
                    Sudoku.UpdateSquare(55, 7);
                    Sudoku.UpdateSquare(64, 8);

                    break;

                case 3:

                    Sudoku.UpdateSquare(1, 1);
                    Sudoku.UpdateSquare(2, 2);
                    Sudoku.UpdateSquare(3, 3);
                    Sudoku.UpdateSquare(10, 4);
                    Sudoku.UpdateSquare(11, 5);
                    Sudoku.UpdateSquare(12, 6);
                    Sudoku.UpdateSquare(19, 7);
                    Sudoku.UpdateSquare(20, 8);

                    break;

                case 4:

                    Sudoku.UpdateSquare(1, 1);
                    Sudoku.UpdateSquare(2, 2);
                    Sudoku.UpdateSquare(3, 3);
                    Sudoku.UpdateSquare(8, 7);
                    Sudoku.UpdateSquare(18, 8);
                    Sudoku.UpdateSquare(36, 4);
                    Sudoku.UpdateSquare(45, 5);
                    Sudoku.UpdateSquare(54, 6);

                    break;

                case 5:

                    Sudoku.UpdateSquare(13, 1);
                    Sudoku.UpdateSquare(25, 1);
                    Sudoku.UpdateSquare(29, 1);
                    Sudoku.UpdateSquare(57, 1);

                    break;

                case 6:

                    Sudoku.UpdateSquare(25, 1);
                    Sudoku.UpdateSquare(57, 1);
                    Sudoku.UpdateSquare(69, 1);
                    Sudoku.UpdateSquare(80, 2);

                    break;

                case 7:

                    Sudoku.UpdateSquare(10, 3);
                    Sudoku.UpdateSquare(13, 1);
                    Sudoku.UpdateSquare(17, 2);
                    Sudoku.UpdateSquare(19, 4);
                    Sudoku.UpdateSquare(25, 1);
                    Sudoku.UpdateSquare(28, 5);
                    Sudoku.UpdateSquare(29, 1);
                    Sudoku.UpdateSquare(31, 2);
                    Sudoku.UpdateSquare(46, 6);
                    Sudoku.UpdateSquare(55, 7);
                    Sudoku.UpdateSquare(57, 1);
                    Sudoku.UpdateSquare(61, 2);
                    Sudoku.UpdateSquare(64, 8);
                    Sudoku.UpdateSquare(73, 9);

                    break;

                case 101: //Real sudoku samples

                    Sudoku.UpdateSquare(1, 9);
                    Sudoku.UpdateSquare(2, 7);
                    Sudoku.UpdateSquare(3, 3);
                    Sudoku.UpdateSquare(4, 6);
                    Sudoku.UpdateSquare(5, 2);
                    Sudoku.UpdateSquare(6, 8);
                    Sudoku.UpdateSquare(7, 4);
                    Sudoku.UpdateSquare(8, 5);
                    Sudoku.UpdateSquare(9, 1);
                    Sudoku.UpdateSquare(10, 5);
                    Sudoku.UpdateSquare(11, 6);
                    Sudoku.UpdateSquare(12, 8);
                    Sudoku.UpdateSquare(13, 4);
                    Sudoku.UpdateSquare(14, 1);
                    Sudoku.UpdateSquare(15, 3);
                    Sudoku.UpdateSquare(16, 7);
                    Sudoku.UpdateSquare(17, 9);
                    Sudoku.UpdateSquare(18, 2);
                    Sudoku.UpdateSquare(19, 1);
                    Sudoku.UpdateSquare(20, 2);
                    Sudoku.UpdateSquare(21, 4);
                    Sudoku.UpdateSquare(22, 5);
                    Sudoku.UpdateSquare(23, 9);
                    Sudoku.UpdateSquare(24, 7);
                    Sudoku.UpdateSquare(25, 8);
                    Sudoku.UpdateSquare(26, 3);
                    Sudoku.UpdateSquare(27, 6);
                    Sudoku.UpdateSquare(28, 8);
                    Sudoku.UpdateSquare(29, 1);
                    Sudoku.UpdateSquare(30, 5);
                    Sudoku.UpdateSquare(31, 7);
                    Sudoku.UpdateSquare(33, 9);
                    Sudoku.UpdateSquare(34, 6);
                    Sudoku.UpdateSquare(35, 4);
                    Sudoku.UpdateSquare(36, 3);
                    Sudoku.UpdateSquare(37, 6);
                    Sudoku.UpdateSquare(38, 4);
                    Sudoku.UpdateSquare(39, 9);
                    Sudoku.UpdateSquare(40, 3);
                    Sudoku.UpdateSquare(41, 5);
                    Sudoku.UpdateSquare(42, 2);
                    Sudoku.UpdateSquare(43, 1);
                    Sudoku.UpdateSquare(44, 7);
                    Sudoku.UpdateSquare(45, 8);
                    Sudoku.UpdateSquare(46, 2);
                    Sudoku.UpdateSquare(47, 3);
                    Sudoku.UpdateSquare(48, 7);
                    Sudoku.UpdateSquare(49, 1);
                    Sudoku.UpdateSquare(50, 6);
                    Sudoku.UpdateSquare(51, 4);
                    Sudoku.UpdateSquare(52, 9);
                    Sudoku.UpdateSquare(54, 5);
                    Sudoku.UpdateSquare(55, 4);
                    Sudoku.UpdateSquare(56, 5);
                    Sudoku.UpdateSquare(57, 2);
                    Sudoku.UpdateSquare(58, 8);
                    Sudoku.UpdateSquare(59, 7);
                    Sudoku.UpdateSquare(61, 3);
                    Sudoku.UpdateSquare(62, 6);
                    Sudoku.UpdateSquare(63, 9);
                    Sudoku.UpdateSquare(64, 7);
                    Sudoku.UpdateSquare(65, 8);
                    Sudoku.UpdateSquare(66, 6);
                    Sudoku.UpdateSquare(67, 9);
                    Sudoku.UpdateSquare(68, 3);
                    Sudoku.UpdateSquare(69, 1);
                    Sudoku.UpdateSquare(70, 5);
                    Sudoku.UpdateSquare(71, 2);
                    Sudoku.UpdateSquare(72, 4);
                    Sudoku.UpdateSquare(73, 3);
                    Sudoku.UpdateSquare(74, 9);
                    Sudoku.UpdateSquare(75, 1);
                    Sudoku.UpdateSquare(76, 2);
                    Sudoku.UpdateSquare(77, 4);
                    Sudoku.UpdateSquare(78, 6);
                    Sudoku.UpdateSquare(80, 8);
                    Sudoku.UpdateSquare(81, 7);

                    break;

                case 102:

                    Sudoku.UpdateSquare(3, 8);
                    Sudoku.UpdateSquare(4, 3);
                    Sudoku.UpdateSquare(5, 4);
                    Sudoku.UpdateSquare(6, 2);
                    Sudoku.UpdateSquare(7, 9);
                    Sudoku.UpdateSquare(12, 9);
                    Sudoku.UpdateSquare(16, 7);
                    Sudoku.UpdateSquare(19, 4);
                    Sudoku.UpdateSquare(27, 3);
                    Sudoku.UpdateSquare(30, 6);
                    Sudoku.UpdateSquare(31, 4);
                    Sudoku.UpdateSquare(32, 7);
                    Sudoku.UpdateSquare(33, 3);
                    Sudoku.UpdateSquare(34, 2);
                    Sudoku.UpdateSquare(38, 3);
                    Sudoku.UpdateSquare(44, 1);
                    Sudoku.UpdateSquare(48, 2);
                    Sudoku.UpdateSquare(49, 8);
                    Sudoku.UpdateSquare(50, 5);
                    Sudoku.UpdateSquare(51, 1);
                    Sudoku.UpdateSquare(52, 6);
                    Sudoku.UpdateSquare(55, 7);
                    Sudoku.UpdateSquare(63, 8);
                    Sudoku.UpdateSquare(66, 4);
                    Sudoku.UpdateSquare(70, 1);
                    Sudoku.UpdateSquare(75, 3);
                    Sudoku.UpdateSquare(76, 6);
                    Sudoku.UpdateSquare(77, 9);
                    Sudoku.UpdateSquare(78, 7);
                    Sudoku.UpdateSquare(79, 5);

                    break;

                default:
                    break;
            }

            Sudoku.Ready = true;
        }

        static void Help()
        {
            var sbOutput = new System.Text.StringBuilder();
            sbOutput.AppendLine("General Commands");
            sbOutput.AppendLine(". new");
            sbOutput.AppendLine(". autosolve - Currently: " + (Sudoku.AutoSolve ? "on" : "off"));
            sbOutput.AppendLine(". update [square], [number]");
            sbOutput.AppendLine(". solve");
            sbOutput.AppendLine(". case [case no] - 1, 2, 3, 4, 5, 6, 7, 101, 102");
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

        static void Sudoku_SquareNumberChanged(Square square)
        {
            Console.WriteLine(string.Format("  Id {0}: {1} - {2}", square.Id.ToString("D2"), square.Number.ToString(), square.AssignType.ToString()));
        }

        static void Sudoku_PotentialSquareFound(Potential potential)
        {
            Console.WriteLine(string.Format("P Id {0}: {1} - {2} - {3}", potential.Square.Id.ToString("D2"), potential.Square.Number.ToString(), potential.Number.ToString(), potential.PotentialType.ToString()));
        }
    }
}
