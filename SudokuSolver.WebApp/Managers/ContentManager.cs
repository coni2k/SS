using SudokuSolver.WebApp.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Web;

namespace SudokuSolver.WebApp.Managers
{
    public class ContentManager
    {
        public IEnumerable<Content> GetContents()
        {
            var contactContent = new Content("contact") { Title = "Contact" };
            var faqContent = new Content("faq") { Title = "FAQ" };
            var licenseContent = new Content("license") { Title = "License" };
            var sourceContent = new Content("source") { Title = "Source" };
            
            var apiHelpContent = new Content()
            {
                Title = "API Help",
                Url = "/help",
                IsExternal = true
            };

            var sudokuContent = new Content("sudoku") { Title = "Sudoku" };

            var contentList = new Collection<Content>();
            contentList.Add(contactContent);
            contentList.Add(faqContent);
            contentList.Add(licenseContent);
            contentList.Add(sourceContent);
            contentList.Add(apiHelpContent);
            contentList.Add(sudokuContent);

            return contentList;
        }

        public static string ReadContentFile(string fileName)
        {
            var filePath = HttpContext.Current.Server.MapPath(string.Format(@"Views\{0}", fileName));
            return File.ReadAllText(filePath);
        }
    }
}