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

        public DateTime Version { get; set; }

        public Content() { }

        public Content(string internalId)
        {
            InternalId = internalId;
            IsExternal = false;

            Url = string.Format("/{0}", InternalId);

            var contentFileName = string.Format("{0}.html", internalId);

            // Read the content body from Views folder
            Body = ContentManager.ReadContentFile(contentFileName);

            Version = ContentManager.GetContentFileLastWriteTime(contentFileName);
        }

        public void CheckUpdates()
        {
            var contentFileName = string.Format("{0}.html", InternalId);

            var lastVersion = ContentManager.GetContentFileLastWriteTime(contentFileName);

            if (Version != lastVersion)
                Body = ContentManager.ReadContentFile(contentFileName);
        }
    }
}