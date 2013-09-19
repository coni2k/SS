using SudokuSolver.Engine;
using SudokuSolver.Engine.Dtos;
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
        public IEnumerable<SudokuDto> GetSudokuList()
        {
            return Cache.SudokuCases.Select(sudoku => new SudokuDto(sudoku));
        }

        // GET api/Sudoku/1
        public SudokuDto GetSudoku(int sudokuId)
        {
            return new SudokuDto(GetSudokuItem(sudokuId));
        }

        // POST api/Sudoku/PostSudoku
        public HttpResponseMessage PostSudoku(SudokuDto sudokuDto)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid sudoku"));

            // Id of the container
            sudokuDto.SudokuId = 1;

            // TODO Thread safety?
            lock (this)
            {
                if (Cache.SudokuCases.Count > 0)
                    sudokuDto.SudokuId = Cache.SudokuCases.Max(s => s.SudokuId) + 1;

                // Create and add a new sudoku
                var sudoku = new Sudoku(sudokuDto.Size)
                {
                    SudokuId = sudokuDto.SudokuId,
                    Title = sudokuDto.Title,
                    Description = sudokuDto.Description
                };
                Cache.SudokuCases.Add(sudoku);
            }

            // Response
            var response = Request.CreateResponse<SudokuDto>(HttpStatusCode.Created, sudokuDto);
            var uri = Url.Link(WebApiRouteConfig.SudokuRouteName, new { action = "GetSudoku", sudokuId = sudokuDto.SudokuId });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        // POST api/Sudoku/ResetList
        public HttpResponseMessage ResetList()
        {
            Cache.InitSudokuCases();

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // GET api/Sudoku/GetNumbers/1
        public IEnumerable<SudokuNumberDto> GetNumbers(int sudokuId)
        {
            var sudoku = GetSudokuItem(sudokuId);

            return sudoku.Numbers.Select(sudokuNumber => new SudokuNumberDto(sudokuNumber));
        }

        // GET api/Sudoku/GetUpdatedNumbers/1
        public IEnumerable<SudokuNumberDto> GetUpdatedNumbers(int sudokuId)
        {
            var sudoku = GetSudokuItem(sudokuId);

            return sudoku.UpdatedNumbers.Select(sudokuNumber => new SudokuNumberDto(sudokuNumber));
        }

        // GET api/Sudoku/GetSquares/1
        public IEnumerable<SquareDto> GetSquares(int sudokuId)
        {
            var sudoku = GetSudokuItem(sudokuId);

            return sudoku.Squares.Select(square => new SquareDto(square));
        }

        // GET api/Sudoku/GetUpdatedSquares/1
        public IEnumerable<SquareDto> GetUpdatedSquares(int sudokuId)
        {
            var sudoku = GetSudokuItem(sudokuId);

            return sudoku.UpdatedSquares.Select(square => new SquareDto(square));
        }

        // GET api/Sudoku/GetSquareAvailabilities/1
        public IEnumerable<SquareAvailabilityDto> GetSquareAvailabilities(int sudokuId)
        {
            var sudoku = GetSudokuItem(sudokuId);

            return sudoku.SquareAvailabilities.Select(availability => new SquareAvailabilityDto(availability));
        }

        // GET api/Sudoku/GetUpdatedSquareAvailabilities/1
        public IEnumerable<SquareAvailabilityDto> GetUpdatedSquareAvailabilities(int sudokuId)
        {
            var sudoku = GetSudokuItem(sudokuId);

            return sudoku.UpdatedSquareAvailabilities.Select(availability => new SquareAvailabilityDto(availability));
        }

        // GET api/Sudoku/GetGroupNumberAvailabilities/1
        public IEnumerable<GroupNumberAvailabilityDto> GetGroupNumberAvailabilities(int sudokuId)
        {
            var sudoku = GetSudokuItem(sudokuId);

            return sudoku.GroupNumberAvailabilities
                .Where(a => a.GroupNumber.Group.GroupType == GroupType.Square)
                .Select(availability => new GroupNumberAvailabilityDto(availability));
        }

        // GET api/Sudoku/GetUpdatedGroupNumberAvailabilities/1
        public IEnumerable<GroupNumberAvailabilityDto> GetUpdatedGroupNumberAvailabilities(int sudokuId)
        {
            var sudoku = GetSudokuItem(sudokuId);

            return sudoku.UpdatedGroupNumberAvailabilities
                .Where(a => a.GroupNumber.Group.GroupType == GroupType.Square)
                .Select(availability => new GroupNumberAvailabilityDto(availability));
        }

        // GET api/Sudoku/GetHints/1
        public IEnumerable<HintDto> GetHints(int sudokuId)
        {
            //var sudoku = GetSudokuItem(sudokuId);

            //var hints = sudoku.HintSquares.SelectMany(square => square.Hints);

            //return hints.Select(h => new HintDto(h));

            return null;
        }

        // PUT api/Sudoku/UpdateSquare/1/1
        public void PutSquare(int sudokuId, int squareId, SquareDto squareDto)
        {
            if (!ModelState.IsValid)
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
                var message = string.Format("Sudoku not found - Id: {0}", sudokuId);
                var response = Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
                throw new HttpResponseException(response);
            }

            //Return
            return sudoku;
        }
    }
}
