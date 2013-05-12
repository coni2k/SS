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
        // TODO api/Sudoku/1 ?
        // TODO api/Sudoku/Sudoku/1 ?

        // GET api/Sudoku/Squares/1
        [HttpGet]
        public IEnumerable<Square> Squares(int id)
        {
            var sudoku = GetSudokuItem(id);

            return sudoku.GetSquares();
        }

        // GET api/Sudoku/Numbers/1
        [HttpGet]
        public IEnumerable<SudokuNumber> Numbers(int id)
        {
            var sudoku = GetSudokuItem(id);

            return sudoku.GetNumbers();
        }

        // GET api/Sudoku/Hints/1
        [HttpGet]
        public IEnumerable<Hint> Hints(int id)
        {
            var sudoku = GetSudokuItem(id);

            return sudoku.GetHints();
        }

        // GET api/Sudoku/Availabilities/1
        [HttpGet]
        public IEnumerable<Availability> Availabilities(int id)
        {
            var sudoku = GetSudokuItem(id);

            return sudoku.GetAvailabilities();
        }

        // GET api/Sudoku/GroupNumberAvailabilities/1
        [HttpGet]
        public IEnumerable<GroupNumberAvailabilityContainer> GroupNumberAvailabilities(int id)
        {
            var sudoku = GetSudokuItem(id);

            return sudoku.GetGroupNumberAvailabilities();
        }

        //// POST api/Sudoku/UpdateSquare/1
        //public void UpdateSquare(int id, Square square)
        //{
        //    var sudoku = GetSudokuItem(id);

        //    try
        //    {
        //        sudoku.UpdateSquare(square.SquareId, square.SudokuNumber.Value);
        //    }
        //    catch (Exception ex)
        //    {
        //        var response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
        //        throw new HttpResponseException(response);
        //    }
        //}

        // POST api/Sudoku/UpdateSquare/1
        public void UpdateSquare(int id, SquareContainer squareContainer)
        {
            var sudoku = GetSudokuItem(id);

            try
            {
                sudoku.UpdateSquare(squareContainer.SquareId, squareContainer.Value);
            }
            catch (Exception ex)
            {
                var response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
                throw new HttpResponseException(response);
            }
        }

        // POST api/Sudoku/ToggleReady/1
        public void ToggleReady(int id)
        {
            var sudoku = GetSudokuItem(id);

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
        public void ToggleAutoSolve(int id)
        {
            var sudoku = GetSudokuItem(id);

            sudoku.ToggleAutoSolve();
        }

        // POST api/Sudoku/Solve/1
        public void Solve(int id)
        {
            var sudoku = GetSudokuItem(id);

            sudoku.Solve();
        }

        // POST api/Sudoku/Reset/1
        public void Reset(int id)
        {
            var sudoku = GetSudokuItem(id);

            sudoku.Reset();
        }

        Sudoku GetSudokuItem(int id)
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
