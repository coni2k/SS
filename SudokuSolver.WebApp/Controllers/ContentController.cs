using SudokuSolver.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SudokuSolver.WebApp.Models;

namespace SudokuSolver.WebApp.Controllers
{
    public class ContentController : BaseController
    {
        // GET api/Content
        public IEnumerable<Content> GetContentList()
        {
            return Cache.Contents;
        }

        // GET api/Content/1
        public Content GetContent(string id)
        {
            // Search in Cache
            var content = Cache.Contents.SingleOrDefault(c => c.InternalId == id);

            // If there is none, throw an exception
            if (content == null)
            {
                var message = string.Format("Content not found - Id: {0}", id.ToString());
                var response = Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
                throw new HttpResponseException(response);
            }

            //Return
            return content;
        }
    }
}
