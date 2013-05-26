using SudokuSolver.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SudokuSolver.WebApp.Controllers
{
    public class SudokuController : BaseController
    {
        // GET api/Sudoku
        public IEnumerable<Sudoku> GetSudokuList()
        {
            return Cache.SudokuCases;
        }

        // GET api/Sudoku/1
        public Sudoku GetSudoku(int sudokuId)
        {
            return GetSudokuItem(sudokuId);
        }

        // POST api/Sudoku/PostSudoku
        public HttpResponseMessage PostSudoku(SudokuDto sudokuDto)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid sudoku"));

            // Id of the container
            // TODO Thread safety?
            int nextId = 1;
            lock (this)
            {
                if (Cache.SudokuCases.Count > 0)
                    nextId = (Cache.SudokuCases.Max(s => s.SudokuId) + 1);
            }

            // Create and add a new sudoku
            var sudoku = new Sudoku(sudokuDto.Size)
            {
                SudokuId = nextId,
                Title = sudokuDto.Title,
                Description = sudokuDto.Description
            };
            Cache.SudokuCases.Add(sudoku);

            // Response
            var response = Request.CreateResponse<Sudoku>(HttpStatusCode.Created, sudoku);
            string uri = Url.Link(WebApiRouteConfig.SudokuRouteName, new { action = "GetSudoku", id = sudoku.SudokuId });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        // POST api/Sudoku/ResetList
        public HttpResponseMessage ResetList()
        {
            Cache.InitSudokuCases();

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // GET api/Sudoku/Squares/1
        public IEnumerable<Square> GetSquares(int sudokuId)
        {
            var sudoku = GetSudokuItem(sudokuId);

            return sudoku.GetSquares();
        }

        // GET api/Sudoku/Numbers/1
        public IEnumerable<SudokuNumber> GetNumbers(int sudokuId)
        {
            var sudoku = GetSudokuItem(sudokuId);

            return sudoku.GetNumbers();
        }

        // GET api/Sudoku/Hints/1
        public IEnumerable<Hint> GetHints(int sudokuId)
        {
            var sudoku = GetSudokuItem(sudokuId);

            return sudoku.GetHints();
        }

        // GET api/Sudoku/Availabilities/1
        public IEnumerable<Availability> GetAvailabilities(int sudokuId)
        {
            var sudoku = GetSudokuItem(sudokuId);

            return sudoku.GetAvailabilities();
        }

        // GET api/Sudoku/GroupNumberAvailabilities/1
        public IEnumerable<GroupNumberAvailabilityContainer> GetGroupNumberAvailabilities(int sudokuId)
        {
            var sudoku = GetSudokuItem(sudokuId);

            return sudoku.GetGroupNumberAvailabilities();
        }

        // PUT api/Sudoku/UpdateSquare/1/1
        public void PutSquare(int sudokuId, int squareId, SquareDto squareDto)
        {
            if (!ModelState.IsValid || sudokuId != squareDto.SudokuId)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid square"));

            var sudoku = GetSudokuItem(sudokuId);

            try
            {
                sudoku.UpdateSquare(squareDto.SquareId, squareDto.Value);
            }
            catch (Exception ex)
            {
                var response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                throw new HttpResponseException(response);
            }
        }

        // POST api/Sudoku/ToggleReady/1
        public void ToggleReady(int sudokuId)
        {
            var sudoku = GetSudokuItem(sudokuId);

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

        // POST api/Sudoku/ToggleAutoSolve/1
        public void ToggleAutoSolve(int sudokuId)
        {
            var sudoku = GetSudokuItem(sudokuId);

            sudoku.ToggleAutoSolve();
        }

        // POST api/Sudoku/Solve/1
        public void Solve(int sudokuId)
        {
            var sudoku = GetSudokuItem(sudokuId);

            sudoku.Solve();
        }

        // POST api/Sudoku/Reset/1
        public void Reset(int sudokuId)
        {
            var sudoku = GetSudokuItem(sudokuId);

            sudoku.Reset();
        }

        Sudoku GetSudokuItem(int sudokuId)
        {
            // Search in CacheManager
            var sudoku = Cache.SudokuCases.SingleOrDefault(s => s.SudokuId == sudokuId);

            // If there is none, throw an exception
            if (sudoku == null)
            {
                var message = string.Format("Sudoku not found - Id: {0}", sudokuId.ToString());
                var response = Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
                throw new HttpResponseException(response);
            }

            //Return
            return sudoku;
        }
    }
}
