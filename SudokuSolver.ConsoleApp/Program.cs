using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SudokuSolver.Engine;
using System.Collections.ObjectModel;

namespace SudokuSolver.ConsoleApp
{
    /// <summary>
    /// For testing purposes
    /// </summary>
    class Program
    {
        static Sudoku Sudoku { get; set; }
        static IEnumerable<Sudoku> Cases { get; set; }

        static void Main(string[] args)
        {
            LoadCases();

            NewSudoku();

            ShowCommands(args);
        }

        static void LoadCases()
        {
            Cases = new CaseManager().GetCases();
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

                if (args.Length == 0)
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

                // TODO Invalid parameter is only for update + case?
                // Try to apply to the other commands?
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
                    case "hints": ShowHints(); break;
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
            Sudoku.HintFound += new Hint.FoundEventHandler(Sudoku_HintFound);

            Console.WriteLine("New sudoku is ready!");
        }

        /// <summary>
        /// Loads the sample cases
        /// See Helper.xslx file for more info (The file is now outdated - 07 Oct. '12)
        /// </summary>
        /// <param name="caseNo"></param>
        static void LoadCase(int caseNo)
        {
            ClearScreen();

            var sudoku = Cases.SingleOrDefault(s => s.SudokuId == caseNo);

            if (sudoku == null)
            {
                Console.WriteLine("Case {0} not found!", caseNo);
                return;
            }

            Sudoku = sudoku;
            Sudoku.SquareNumberChanged += new Square.SquareEventHandler(Sudoku_SquareNumberChanged);
            Sudoku.HintFound += new Hint.FoundEventHandler(Sudoku_HintFound);

            Console.WriteLine("Case {0} is ready!", caseNo);
        }

        static void ToggleAutoSolve()
        {
            Sudoku.AutoSolve = !Sudoku.AutoSolve;
            Console.WriteLine("autosolve - Currently: {0}", Sudoku.AutoSolve ? "on" : "off");
        }

        static void UpdateSquare(string parameters)
        {
            try
            {
                // Get square id & number
                int id = int.Parse(parameters.Split(',')[0]);
                int number = int.Parse(parameters.Split(',')[1]);

                // Update
                Sudoku.UpdateSquare(id, number);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }
        }

        static void Solve()
        {
            Sudoku.Solve();
        }

        static void ShowSudoku()
        {
            // Loop through horizontal squares
            foreach (var group in Sudoku.GetHorizontalTypeGroups())
            {
                // Divide the groups into square root sized groups
                var squareRootGroups = new Collection<IEnumerable<Square>>();

                for (var i = 0; i < Sudoku.SquareRootOfSize; i++)
                    squareRootGroups.Add(group.Squares.Skip(i * Sudoku.SquareRootOfSize).Take(Sudoku.SquareRootOfSize));

                // Prepare & give the output
                var output = string.Join(" | ",
                    squareRootGroups.Select(squareRootGroup =>
                        string.Join(" ", squareRootGroup.Select(square => square.SudokuNumber.Value.ToString("#;;'.'")))));

                Console.WriteLine(output);

                // Extra line break after every 3. horizontal group
                if (group.Id % Sudoku.SquareRootOfSize == 0)
                    Console.WriteLine();
            }
        }

        static void ShowUsedSquares()
        {
            foreach (var usedSquare in Sudoku.GetUsedSquares())
                Console.WriteLine("  Id {0:D2}: {1} - {2}", usedSquare.SquareId, usedSquare.SudokuNumber.Value, usedSquare.AssignType);
        }

        /// <summary>
        /// Show the availibility per square, per number
        /// </summary>
        static void ShowAvailability()
        {
            // Header; display the sudoku numbers
            Console.Write("        "); // Necessary to make the starting points equal - TODO This should check the length of the squareText?
            foreach (var number in Sudoku.GetNumbersExceptZero())
                Console.Write(" | {0}", number.Value);
            Console.WriteLine();

            // Details
            foreach (var square in Sudoku.GetSquares())
            {
                // Square text: Square id + value
                Console.Write("Id {0:D2}: {1}", square.SquareId, square.SudokuNumber.Value);

                // Availability per number; "X" for available squares, "." for non-available ones
                foreach (var number in Sudoku.GetNumbersExceptZero())
                    Console.Write(" | {0}", square.IsNumberAvailable(number) ? "X" : ".");

                Console.WriteLine();
            }
        }

        static void ShowNumbers()
        {
            foreach (var number in Sudoku.GetNumbers())
                Console.WriteLine("Number: {0} - Counter: {1}", number.Value, number.Count);
        }

        static void ShowHints()
        {
            if (Sudoku.GetHints().Count() == 0)
                Console.WriteLine("There are no hints");

            foreach (var hint in Sudoku.GetHints().OrderBy(s => s.Square.SquareId))
                Console.WriteLine("P Id {0:D2}: {1} - {2} - {3}", hint.Square.SquareId, hint.Square.SudokuNumber.Value, hint.Number.Value, hint.Type);
        }

        static void ClearScreen()
        {
            Console.Clear();
        }

        static void Help()
        {
            var sbOutput = new StringBuilder();

            sbOutput.AppendLine("General Commands");
            sbOutput.AppendLine(". new");
            sbOutput.AppendFormat(". autosolve - Currently: {0}", Sudoku.AutoSolve ? "on" : "off").AppendLine();
            sbOutput.AppendLine(". update [square], [number]");
            sbOutput.AppendLine(". solve");
            sbOutput.AppendFormat(". case [case no] - {0}", GetCaseIds()).AppendLine();
            sbOutput.AppendLine(". quit");

            sbOutput.AppendLine().AppendLine("Display Commands");
            sbOutput.AppendLine(". sudoku");
            sbOutput.AppendLine(". used");
            sbOutput.AppendLine(". availability");
            sbOutput.AppendLine(". numbers");
            sbOutput.AppendLine(". hints");
            sbOutput.AppendLine(". clear");

            Console.Write(sbOutput.ToString());
        }

        static string GetCaseIds()
        {
            return string.Join(", ", Cases.Select(s => s.SudokuId.ToString()));
        }

        static void Sudoku_SquareNumberChanged(Square square)
        {
            Console.WriteLine("  Id {0:D2}: {1} - {2}", square.SquareId, square.SudokuNumber.Value, square.AssignType);
        }

        static void Sudoku_HintFound(Hint hint)
        {
            Console.WriteLine("P Id {0:D2}: {1} - {2} - {3}", hint.Square.SquareId, hint.Square.SudokuNumber.Value, hint.Number.Value, hint.Type);
        }
    }
}
