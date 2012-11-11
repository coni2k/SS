using SudokuSolver.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SudokuSolver.WebApp.Controllers
{
    public class SudokuListController : ApiController
    {
        // GET api/SudokuList/List
        [HttpGet]
        public IEnumerable<Sudoku> List()
        {
            return CacheManager.SudokuList;
        }

        // POST api/SudokuList/NewSudoku
        public HttpResponseMessage NewSudoku(SudokuContainer container)
        {
            // Validate
            if (container == null)
                throw new ArgumentNullException("container");

            // Size?

            // Title?
            if (string.IsNullOrWhiteSpace(container.Title))
                throw new ArgumentNullException("title");

            //Id of the container
            //TODO Thread safety?
            int nextId = 1;
            lock (this)
            {
                if (CacheManager.SudokuList.Count > 0)
                    nextId = (CacheManager.SudokuList.Max(s => s.SudokuId) + 1);
            }

            // Create and add a new sudoku
            var sudoku = new Sudoku(container.Size);
            sudoku.SudokuId = nextId;
            sudoku.Title = container.Title;
            sudoku.Description = container.Description;
            CacheManager.SudokuList.Add(sudoku);

            //Response
            var response = Request.CreateResponse<Sudoku>(HttpStatusCode.Created, sudoku);
            string uri = Url.Link(WebApiConfig.RouteNameControllerAction, new { id = sudoku.SudokuId });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        // POST api/SudokuList/ResetList
        public HttpResponseMessage ResetList()
        {
            CacheManager.LoadSamples();

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
