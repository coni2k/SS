using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OSP.SudokuSolver.Engine;
using OSP.SudokuSolver.WebApp.Models;

namespace OSP.SudokuSolver.WebApp.Controllers
{
    public class SudokuApiController : ApiController
    {
        [ActionName("list")]
        public List<SudokuContainer> GetList()
        {
            return CacheManager.SudokuList;
        }

        [ActionName("item")]
        public SudokuContainer GetItem(int id)
        {
            var container = ValidateAndGetSudokuContainer(id);

            return container;
        }

        [ActionName("squares")]
        public IEnumerable<SquareContainer> GetSquares(int id)
        {
            var container = ValidateAndGetSudokuContainer(id);

            return container.GetSquares();
        }

        [ActionName("horizontaltypegroups")]
        public IEnumerable<GroupContainer> GetHorizontalTypeGroups(int id)
        {
            return GetSquareGroups(id, GroupTypes.Horizontal);
        }

        [ActionName("verticaltypegroups")]
        public IEnumerable<GroupContainer> GetVerticalTypeGroups(int id)
        {
            return GetSquareGroups(id, GroupTypes.Vertical);
        }

        [ActionName("squaretypegroups")]
        public IEnumerable<GroupContainer> GetSquareTypeGroups(int id)
        {
            return GetSquareGroups(id, GroupTypes.Square);
        }

        private IEnumerable<GroupContainer> GetSquareGroups(int id, GroupTypes type)
        {
            var container = ValidateAndGetSudokuContainer(id);

            return container.GetSquareGroups(type);
        }

        [ActionName("usedsquares")]
        public IEnumerable<SquareContainer> GetUsedSquares(int id)
        {
            var container = ValidateAndGetSudokuContainer(id);

            return container.GetUsedSquares();
        }

        [ActionName("squareavailability")]
        public IEnumerable<NumberContainer> GetSquareAvailability(int id, int squareid)
        {
            var container = ValidateAndGetSudokuContainer(id);

            //TODO Container classes are expensive in general, but this method is much more worse than the rest!
            return container.GetSquares().Find(s => s.SquareId.Equals(squareid)).GetAvailableNumbers();
        }

        [ActionName("numbers")]
        public IEnumerable<NumberContainer> GetNumbers(int id)
        {
            var container = ValidateAndGetSudokuContainer(id);

            return container.GetNumbers();
        }

        [ActionName("potentials")]
        public IEnumerable<PotentialContainer> GetPotentials(int id)
        {
            var container = ValidateAndGetSudokuContainer(id);

            return container.GetPotentials();
        }

        [ActionName("availabilities")]
        public IEnumerable<AvailabilityContainer> GetAvailabilities(int id)
        {
            var container = ValidateAndGetSudokuContainer(id);

            return container.GetAvailabilities();
        }

        private SudokuContainer ValidateAndGetSudokuContainer(int id)
        {
            //Search the container in CacheManager
            var container = CacheManager.SudokuList.Find(s => s.SudokuId.Equals(id));

            //If there is no, throw an exception
            if (container == null)
            {
                var response = new HttpResponseMessage();
                response.StatusCode = HttpStatusCode.NotFound;
                response.Content = new StringContent("Sudoku not found");
                throw new HttpResponseException(response);
            }

            //Return
            return container;
        }

        [HttpPost]
        [ActionName("newsudoku")]
        public HttpResponseMessage<SudokuContainer> Post()
        {
            //Id of the container
            //TODO Thread safety?
            int nextId = 1;
            lock (this)
            {
                if (CacheManager.SudokuList.Count > 0)
                    nextId = (CacheManager.SudokuList.Max(s => s.SudokuId) + 1);
            }

            //Sudoku
            var sudoku = new Sudoku(9);

            //New container
            var container = new SudokuContainer();
            container.SudokuId = nextId;
            container.SetSudoku(sudoku);

            CacheManager.SudokuList.Add(container);

            var response = new HttpResponseMessage<SudokuContainer>(container, HttpStatusCode.Created);
            string uri = Url.Route(null, new { id = container.SudokuId });
            response.Headers.Location = new Uri(Request.RequestUri, uri);

            return response;
        }

        [HttpPost]
        [ActionName("updatesquare")]
        public void UpdateSquare(int id, SquareContainer square)
        {
            var container = ValidateAndGetSudokuContainer(id);

            try
            {
                container.UpdateSquare(square.SquareId, square.Number);
            }
            catch (Exception ex) 
            {
                var response = new HttpResponseMessage<JsonValue>(ex.Message, HttpStatusCode.BadRequest);
                throw new HttpResponseException(response);
            }
        }

        [HttpPost]
        [ActionName("toggleready")]
        public void ToggleReady(int id)
        {
            var container = ValidateAndGetSudokuContainer(id);

            try
            {
                container.ToggleReady();
            }
            catch (Exception ex)
            {
                var response = new HttpResponseMessage<JsonValue>(ex.Message, HttpStatusCode.BadRequest);
                throw new HttpResponseException(response);            
            }
        }

        //We have to put action; update square and toggleautosolve
        //In this case, a GET request for “api/products/details/1” would map to the Details method. This style of routing is similar to ASP.NET MVC, and may be appropriate for an RPC-style API. For a RESTful API, you should avoid using verbs in the URIs, because a URI should identify a resource, not an action.

        [HttpPost]
        [ActionName("toggleautosolve")]
        public void ToggleAutoSolve(int id)
        {
            var container = ValidateAndGetSudokuContainer(id);
            container.ToggleAutoSolve();
        }

        [HttpPost]
        [ActionName("solve")]
        public void Solve(int id)
        {
            var container = ValidateAndGetSudokuContainer(id);
            container.Solve();
        }

        [HttpPost]
        [ActionName("reset")]
        public void Reset()
        {
            CacheManager.LoadSamples();
        }
    }
}
