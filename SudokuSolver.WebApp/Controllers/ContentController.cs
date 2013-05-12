using SudokuSolver.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SudokuSolver.WebApp.Controllers
{
    public class ContentController : ApiController
    {
        /// <summary>
        /// Currently all the content pages are already stored on the client side and routing can be done over there.
        /// In case if it can't find the content in it's list, then it calls the server to get a Not Found error.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public string ResourceNotFound(string id)
        {
            var exceptionMessage = string.Format("Content / sudoku not found: {0}", id);
            var response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, exceptionMessage);
            throw new HttpResponseException(response);
        }

        //[HttpGet]
        //public string GetContent()
        //{
        //    // return 
        //}
    }
}
