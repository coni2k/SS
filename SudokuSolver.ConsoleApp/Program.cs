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
        static int LineCounter { get; set; }
        static Sudoku CurrentSudoku { get; set; }
        static IEnumerable<Sudoku> Cases { get; set; }

        static void Main(string[] args)
        {
            LoadCases();

            NewSudoku();

            LoadCase(5);

            ShowHints();

            ProcessCommand(args);
        }

        static void LoadCases()
        {
            Cases = new CaseManager().GetCases();
        }

        /// <summary>
        /// Show available commands
        /// </summary>
        /// <param name="args"></param>
        static void ProcessCommand(string[] args)
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
                    case "numbers": ShowNumbers(); break;
                    case "avail1": ShowSquareAvailabilities(); break;
                    case "avail2": ShowGroupNumberAvailabilities(); break;
                    case "hints": ShowHints(); break;
                    case "clear": ClearScreen(); break;

                    default: ShowHelp(); break;
                }

            } while (true);
        }

        static void NewSudoku()
        {
            LoadSudoku(new Sudoku());
        }

        /// <summary>
        /// Loads the sample cases
        /// See Helper.xslx file for more info (The file is now outdated - 07 Oct. '12)
        /// </summary>
        /// <param name="caseNo"></param>
        static void LoadCase(int caseNo)
        {
            var sudoku = Cases.SingleOrDefault(s => s.SudokuId == caseNo);

            if (sudoku == null)
            {
                Console.WriteLine("Case {0} not found!", caseNo);
                return;
            }

            LoadSudoku(sudoku);
        }

        static void LoadSudoku(Sudoku sudoku)
        {
            // ClearScreen();

            CurrentSudoku = sudoku;
            CurrentSudoku.HintFound += new Hint.FoundEventHandler(Sudoku_HintFound);

            Console.WriteLine("New sudoku is loaded!");
        }

        static void ToggleAutoSolve()
        {
            CurrentSudoku.AutoSolve = !CurrentSudoku.AutoSolve;
            Console.WriteLine("autosolve - Currently: {0}", CurrentSudoku.AutoSolve ? "on" : "off");
        }

        static void UpdateSquare(string parameters)
        {
            try
            {
                // Get square id & number
                int id = int.Parse(parameters.Split(',')[0]);
                int number = int.Parse(parameters.Split(',')[1]);

                // Update
                CurrentSudoku.UpdateSquare(id, number);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }
        }

        static void Solve()
        {
            CurrentSudoku.Solve();
        }

        static void ShowSudoku()
        {
            // Loop through horizontal squares
            foreach (var group in CurrentSudoku.HorizontalTypeGroups)
            {
                // Divide the groups into square root sized groups
                var squareRootGroups = new Collection<IEnumerable<Square>>();

                for (var i = 0; i < CurrentSudoku.SquareRootOfSize; i++)
                    squareRootGroups.Add(group.Squares.Skip(i * CurrentSudoku.SquareRootOfSize).Take(CurrentSudoku.SquareRootOfSize));

                // Prepare & give the output
                var output = string.Join(" | ",
                    squareRootGroups.Select(squareRootGroup =>
                        string.Join(" ", squareRootGroup.Select(square => square.AssignType == AssignTypes.Hint ? "." : square.SudokuNumber.Value.ToString("#;;'.'")))));

                Console.WriteLine(output);

                // Extra line break after every 3. horizontal group
                if (group.Id % CurrentSudoku.SquareRootOfSize == 0)
                    Console.WriteLine();
            }
        }

        static void ShowNumbers()
        {
            foreach (var number in CurrentSudoku.Numbers)
                Console.WriteLine("Number: {0} - Counter: {1}", number.Value, number.Count);
        }

        static void ShowUsedSquares()
        {
            foreach (var usedSquare in CurrentSudoku.UsedSquares)
                Console.WriteLine("  Id {0:D2}: {1} - {2}", usedSquare.SquareId, usedSquare.SudokuNumber.Value, usedSquare.AssignType);
        }

        /// <summary>
        /// Show the availibility per square, per number
        /// </summary>
        static void ShowSquareAvailabilities()
        {
            // Header; display the sudoku numbers
            Console.Write("        "); // Necessary to make the starting points equal - TODO This should check the length of the squareText?
            foreach (var number in CurrentSudoku.NumbersExceptZero)
                Console.Write(" | {0}", number.Value);
            Console.WriteLine();

            // Details
            foreach (var square in CurrentSudoku.Squares)
            {
                // Square text: Square id + value
                Console.Write("Id {0:D2}: {1}", square.SquareId, square.SudokuNumber.Value);

                // Availability per number; "X" for available squares, "." for non-available ones
                foreach (var number in CurrentSudoku.NumbersExceptZero)
                    Console.Write(" | {0}", square.Availabilities.Single(availability => availability.Number.Equals(number)).IsAvailable ? "X" : ".");

                Console.WriteLine();
            }
        }

        /// <summary>
        /// Show the availibility per group, per number
        /// </summary>
        static void ShowGroupNumberAvailabilities()
        {
            foreach (var group in CurrentSudoku.Groups)
            {
                foreach (var groupNumber in group.GroupNumbers)
                {
                    Console.Write("Id {0:D2} - Type {1}: ", group.Id, group.GroupType.ToString()[0]);

                    var availabilityOutput = string.Join(" - ",
                        groupNumber.Availabilities.Select(squareAvailability =>
                            string.Format("{0:D2} - {1:D2} - {2}",
                            groupNumber.SudokuNumber.Value, // 0
                            squareAvailability.Square.SquareId, // 1
                            squareAvailability.IsAvailable ? "X" : "."))); // 2

                    Console.Write(availabilityOutput);

                    Console.WriteLine();
                }
            }
        }

        static void ShowHints()
        {
            if (CurrentSudoku.Hints.Count() == 0)
                Console.WriteLine("There are no hints");

            foreach (var hintSquare in CurrentSudoku.Hints.OrderBy(hintSquare => hintSquare.Square.SquareId))
                Console.WriteLine("P Id {0:D2}: {1}", hintSquare.Square.SquareId, hintSquare.SudokuNumber.Value);
        }

        static void ClearScreen()
        {
            Console.Clear();
        }

        static void ShowHelp()
        {
            var sbOutput = new StringBuilder();

            sbOutput.AppendLine("General Commands");
            sbOutput.AppendLine(". new");
            sbOutput.AppendFormat(". autosolve - Currently: {0}", CurrentSudoku.AutoSolve ? "on" : "off").AppendLine();
            sbOutput.AppendLine(". update [square], [number]");
            sbOutput.AppendLine(". solve");
            sbOutput.AppendFormat(". case [case no] - {0}", GetCaseIds()).AppendLine();
            sbOutput.AppendLine(". quit");

            sbOutput.AppendLine().AppendLine("Display Commands");
            sbOutput.AppendLine(". sudoku");
            sbOutput.AppendLine(". used");
            sbOutput.AppendLine(". numbers");
            sbOutput.AppendLine(". avail1");
            sbOutput.AppendLine(". avail2");
            sbOutput.AppendLine(". hints");
            sbOutput.AppendLine(". clear");

            Console.Write(sbOutput.ToString());
        }

        static string GetCaseIds()
        {
            return string.Join(", ", Cases.Select(s => s.SudokuId.ToString()));
        }

        static void Sudoku_HintFound(Hint hint)
        {
            Console.WriteLine("P Id {0:D2}: {1} - {2} - {3}", hint.Square.SquareId, hint.Square.SudokuNumber.Value, hint.SudokuNumber.Value, hint.Type);
        }
    }
}
