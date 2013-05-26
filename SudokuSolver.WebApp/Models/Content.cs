using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SudokuSolver.WebApp.Managers;

namespace SudokuSolver.WebApp.Models
{
    public class Content
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public bool IsExternal { get; set; }

        public string InternalId { get; private set; }

        public string Body { get; private set; }

        public Content() { }

        public Content(string internalId)
        {
            InternalId = internalId;
            IsExternal = false;

            Url = string.Format("/{0}", internalId);

            // Read the content body from Views folder
            var contentFileName = string.Format("{0}.html", internalId);
            Body = ContentManager.ReadContentFile(contentFileName);
        }
    }
}