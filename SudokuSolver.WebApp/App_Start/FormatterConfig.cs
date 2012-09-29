using System.Net.Http.Formatting;

namespace SudokuSolver.WebApp
{
    public class FormatterConfig
    {
        public static void RegisterFormatters(MediaTypeFormatterCollection formatters)
        {
            // Remove xml formatter (at least for the moment)
            formatters.Remove(formatters.XmlFormatter);
        }
    }
}