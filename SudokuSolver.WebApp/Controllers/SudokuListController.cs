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
        // GET api/SudokuList
        public IEnumerable<Sudoku> GetSudokuList()
        {
            return CacheManager.SudokuList;
        }

        // GET api/SudokuList/1
        public Sudoku GetSudoku(int id)
        {
            return GetSudokuItem(id);
        }

        // POST api/SudokuList/PostSudoku
        public HttpResponseMessage PostSudoku(SudokuDto sudokuDto)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid model state"));

            // Id of the container
            // TODO Thread safety?
            int nextId = 1;
            lock (this)
            {
                if (CacheManager.SudokuList.Count > 0)
                    nextId = (CacheManager.SudokuList.Max(s => s.SudokuId) + 1);
            }

            // Create and add a new sudoku
            var sudoku = new Sudoku(sudokuDto.Size)
            {
                SudokuId = nextId,
                Title = sudokuDto.Title,
                Description = sudokuDto.Description
            };
            CacheManager.SudokuList.Add(sudoku);

            // Response
            var response = Request.CreateResponse<Sudoku>(HttpStatusCode.Created, sudoku);
            string uri = Url.Link(WebApiConfig.RouteNameControllerActionId, new { action = "GetSudoku", id = sudoku.SudokuId });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        // POST api/SudokuList/Reset
        public HttpResponseMessage Reset()
        {
            CacheManager.LoadSamples();

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        Sudoku GetSudokuItem(int id)
        {
            // Search in CacheManager
            var sudoku = CacheManager.SudokuList.SingleOrDefault(s => s.SudokuId == id);

            // If there is no, throw an exception
            if (sudoku == null)
            {
                var message = string.Format("Sudoku with Id = {0} not found", id.ToString());
                var response = Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
                throw new HttpResponseException(response);
            }

            //Return
            return sudoku;
        }
    }
}
