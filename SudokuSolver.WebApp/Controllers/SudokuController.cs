using SudokuSolver.Engine;
using SudokuSolver.WebApp.Models;
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
        [ActionName("list")]
        public List<Sudoku> GetList()
        {
            return CacheManager.SudokuList;
        }

        [ActionName("usedsquares")]
        public IEnumerable<Square> GetUsedSquares(int id)
        {
            var container = ValidateAndGetSudoku(id);

            return container.GetUsedSquares();
        }

        [ActionName("numbers")]
        public IEnumerable<Number> GetNumbers(int id)
        {
            var container = ValidateAndGetSudoku(id);

            return container.GetNumbers();
        }

        [ActionName("potentials")]
        public IEnumerable<Potential> GetPotentialSquares(int id)
        {
            var container = ValidateAndGetSudoku(id);

            return container.GetPotentialSquares();
        }

        [ActionName("usedavailabilities")]
        public IEnumerable<Availability> GetAvailabilities(int id)
        {
            var container = ValidateAndGetSudoku(id);

            return container.GetUsedAvailabilities();
        }

        [ActionName("availabilities2")]
        public IEnumerable<Availability> GetAvailabilities2(int id)
        {
            var container = ValidateAndGetSudoku(id);

            return container.GetAvailabilities2();
        }

        [ActionName("groupnumberavailabilities")]
        public IEnumerable<GroupNumberAvailabilityContainer> GetGroupNumberAvailabilities(int id)
        {
            var container = ValidateAndGetSudoku(id);

            return container.GetGroupNumberAvailabilities();
        }

        [HttpPost]
        [ActionName("item")]
        public HttpResponseMessage Post()
        {
            //Id of the container
            //TODO Thread safety?
            int nextId = 1;
            lock (this)
            {
                if (CacheManager.SudokuList.Count > 0)
                    nextId = (CacheManager.SudokuList.Max(s => s.SudokuId) + 1);
            }

            // Create and add a new sudoku
            var sudoku = new Sudoku(9);
            sudoku.SudokuId = nextId;
            sudoku.Title = string.Format("New sudoku {0}", nextId.ToString());
            CacheManager.SudokuList.Add(sudoku);

            //Response
            var response = Request.CreateResponse<Sudoku>(HttpStatusCode.Created, sudoku);
            string uri = Url.Link("DefaultApi", new { id = sudoku.SudokuId });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        [HttpPost]
        [ActionName("updatesquare")]
        public void UpdateSquare(int id, SquareContainer square)
        {
            var container = ValidateAndGetSudoku(id);

            try
            {
                container.UpdateSquare(square.SquareId, square.Value);
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
            var container = ValidateAndGetSudoku(id);

            try
            {
                container.ToggleReady();
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
            var container = ValidateAndGetSudoku(id);
            container.ToggleAutoSolve();
        }

        [HttpPost]
        [ActionName("solve")]
        public void Solve(int id)
        {
            var container = ValidateAndGetSudoku(id);
            container.Solve();
        }

        [HttpPost]
        [ActionName("reset")]
        public void Reset()
        {
            CacheManager.LoadSamples();
        }

        Sudoku ValidateAndGetSudoku(int id)
        {
            //Search the container in CacheManager
            var container = CacheManager.SudokuList.Find(s => s.SudokuId.Equals(id));

            //If there is no, throw an exception
            if (container == null)
            {
                var message = string.Format("Sudoku with Id = {0} not found", id.ToString());
                var response = Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
                //responseMessage.ReasonPhrase = "Sudoku ID Not Found";                
                throw new HttpResponseException(response);
            }

            //Return
            return container;
        }
    }
}
