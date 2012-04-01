using System;
using System.Collections.Generic;
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

        [ActionName("horizontaltypegroups")]
        public IEnumerable<GroupContainer> GetHorizontalTypeGroups(int id)
        {
            return GetSquareGroup(id, GroupTypes.Horizontal);
        }

        [ActionName("verticaltypegroups")]
        public IEnumerable<GroupContainer> GetVerticalTypeGroups(int id)
        {
            return GetSquareGroup(id, GroupTypes.Vertical);
        }

        [ActionName("squaretypegroups")]
        public IEnumerable<GroupContainer> GetSquareTypeGroups(int id)
        {
            return GetSquareGroup(id, GroupTypes.Square);
        }

        private IEnumerable<GroupContainer> GetSquareGroup(int id, GroupTypes type)
        {
            var container = ValidateAndGetSudokuContainer(id);

            return container.GetSquareGroup(type);
        }

        [ActionName("filledsquares")]
        public IEnumerable<SquareContainer> GetFilledSquares(int id)
        {
            var container = ValidateAndGetSudokuContainer(id);

            return container.GetFilledSquares();
        }

        [ActionName("squareavailability")]
        public IEnumerable<NumberContainer> GetSquareAvailability(int id, int squareid)
        {
            var container = ValidateAndGetSudokuContainer(id);

            return container.SquareList.Find(s => s.SquareId.Equals(squareid)).GetAvailableNumbers();
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

        private SudokuContainer ValidateAndGetSudokuContainer(int id)
        {
            //Search the container in CacheManager
            var container = CacheManager.SudokuList.Find(s => s.Id.Equals(id));

            //If there is no, throw an exception
            if (container == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            //Return
            return container;
        }

        [HttpPost]
        [ActionName("create")]
        public HttpResponseMessage<SudokuContainer> Post()
        {
            //Id of the container
            //TODO Thread safety?
            int nextId = 1;
            lock (this)
            {
                if (CacheManager.SudokuList.Count > 0)
                    nextId = (CacheManager.SudokuList.Max(s => s.Id) + 1);
            }

            //Sudoku
            var sudoku = new Sudoku(9);
            sudoku.AutoSolve = true;

            //New container
            var container = new SudokuContainer();
            container.Id = nextId;
            container.SetSudoku(sudoku);

            container.Prepare();

            CacheManager.SudokuList.Add(container);

            var response = new HttpResponseMessage<SudokuContainer>(container, HttpStatusCode.Created);
            string uri = Url.Route(null, new { id = container.Id });
            response.Headers.Location = new Uri(Request.RequestUri, uri);

            return response;
        }

        [HttpPost]
        [ActionName("fillsquare")]
        public void FillSquare(int id, SquareContainer square)
        {
            var container = ValidateAndGetSudokuContainer(id);

            try
            {
                container.FillSquare(square.SquareId, square.Number);
            }
            catch (Exception)
            {
                //TODO Correct code?
                //Return a message !!!!!
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        //We have to put action; fill square and toggleautosolve
        //In this case, a GET request for “api/products/details/1” would map to the Details method. This style of routing is similar to ASP.NET MVC, and may be appropriate for an RPC-style API. For a RESTful API, you should avoid using verbs in the URIs, because a URI should identify a resource, not an action.

        [HttpPost]
        [ActionName("toggleautosolve")]
        public HttpResponseMessage<SudokuContainer> ToggleAutoSolve(int id)
        {
            var container = ValidateAndGetSudokuContainer(id);

            container.ToggleAutoSolve();

            var response = new HttpResponseMessage<SudokuContainer>(container, HttpStatusCode.OK);
            //TODO Necessary to return the location?
            //string uri = Url.Route(null, new { id = container.Id });
            //response.Headers.Location = new Uri(Request.RequestUri, uri);

            return response;
        }

        [HttpPost]
        [ActionName("solve")]
        public HttpResponseMessage<SudokuContainer> Solve(int id)
        {
            var container = ValidateAndGetSudokuContainer(id);

            container.Solve();

            var response = new HttpResponseMessage<SudokuContainer>(container, HttpStatusCode.OK);
            //TODO Necessary to return the location?
            //string uri = Url.Route(null, new { id = container.Id });
            //response.Headers.Location = new Uri(Request.RequestUri, uri);

            return response;
        }
    }
}
