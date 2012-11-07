using SudokuSolver.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SudokuSolver.WebApp.Controllers
{
    public class SudokuController : ApiController
    {
        #region + Application level commands

        [ActionName("sudokulist")]
        public ICollection<Sudoku> GetSudokuList()
        {
            return CacheManager.SudokuList;
        }

        [HttpPost]
        [ActionName("newsudoku")]
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
            string uri = Url.Link("DefaultApi", new { id = sudoku.SudokuId });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        [HttpPost]
        [ActionName("resetlist")]
        public void ResetList()
        {
            CacheManager.LoadSamples();
        }

        #endregion

        #region + Sudoku level commands

        [ActionName("squares")]
        public IEnumerable<Square> GetSquares(int id)
        {
            var sudoku = GetSudoku(id);

            return sudoku.GetSquares();
        }

        [ActionName("numbers")]
        public IEnumerable<SudokuNumber> GetNumbers(int id)
        {
            var sudoku = GetSudoku(id);

            return sudoku.GetNumbers();
        }

        [ActionName("hints")]
        public IEnumerable<Hint> GetHints(int id)
        {
            var sudoku = GetSudoku(id);

            return sudoku.GetHints();
        }

        [ActionName("availabilities")]
        public IEnumerable<Availability> GetAvailabilities(int id)
        {
            var sudoku = GetSudoku(id);

            return sudoku.GetAvailabilities();
        }

        [ActionName("groupnumberavailabilities")]
        public IEnumerable<GroupNumberAvailabilityContainer> GetGroupNumberAvailabilities(int id)
        {
            var sudoku = GetSudoku(id);

            return sudoku.GetGroupNumberAvailabilities();
        }

        [HttpPost]
        [ActionName("updatesquare")]
        //public void UpdateSquare(int id, SquareContainer square)
        public void UpdateSquare(int id, Square square)
        {
            var sudoku = GetSudoku(id);

            try
            {
                sudoku.UpdateSquare(square.SquareId, square.SudokuNumber.Value);
            }
            catch (Exception ex)
            {
                var response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                throw new HttpResponseException(response);
            }
        }


        [HttpPost]
        [ActionName("toggleready")]
        public void ToggleReady(int id)
        {
            var sudoku = GetSudoku(id);

            try
            {
                sudoku.ToggleReady();
            }
            catch (Exception ex)
            {
                var response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                throw new HttpResponseException(response);
            }
        }

        //We have to put action; update square and toggleautosolve
        //In this case, a GET request for “api/products/details/1” would map to the Details method. This style of routing is similar to ASP.NET MVC, and may be appropriate for an RPC-style API. For a RESTful API, you should avoid using verbs in the URIs, because a URI should identify a resource, not an action.

        [HttpPost]
        [ActionName("toggleautosolve")]
        public void ToggleAutoSolve(int id)
        {
            var sudoku = GetSudoku(id);

            sudoku.ToggleAutoSolve();
        }

        [HttpPost]
        [ActionName("solve")]
        public void Solve(int id)
        {
            var sudoku = GetSudoku(id);

            sudoku.Solve();
        }

        [HttpPost]
        [ActionName("reset")]
        public void Reset(int id)
        {
            var sudoku = GetSudoku(id);

            sudoku.Reset();
        }

        #endregion

        Sudoku GetSudoku(int id)
        {
            //Search the container in CacheManager
            var sudoku = CacheManager.SudokuList.SingleOrDefault(s => s.SudokuId == id);

            //If there is no, throw an exception
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
