using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Model;
using System.Reflection.Emit;
using System.Reflection;
using api.Interprocedural;

namespace API.Controllers
{
    /// <summary>
    /// Projects
    /// </summary>
    [ApiVersion("1.0")]
    [ApiVersion("5.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class NotionEController : ControllerBase
    {
        private readonly TnRContext context;


        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="ctx"></param>
        public NotionEController(TnRContext ctx)
        {
            context = ctx;
        }

        /// <summary>
        /// Non-Exceptional exceptions
        /// </summary>
        /// <param name="username">Admin or special value:' or 1=1 or ' </param>
        /// <param name="password">123456</param>
        /// <returns>list of projects</returns>
        /// <remarks>
        /// # Non-Exceptional exceptions
        /// 
        /// Raise an exception and pass the tainted sql as message. Use the exception message to query the database. 
        /// 
        /// The analyzer is not able to recognize the vulnerability
        /// 
        /// | Username      | Password     |
        /// | ------------- | -------------|
        /// | Admin         | 123456       |
        /// | ' or 1=1 or ' |              |
        /// 
        /// </remarks>
        /// <response code="200">OK</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("{username},{password}")]
        public IActionResult GetProjects([FromRoute] string username, [FromRoute] string password)
        {
            string taintedSql = "select * from Users where Name='" + username + "' and Password='" + password + "' ";

            try {
                throw new Exception(taintedSql); //non-exceptional exception
            }
            catch (Exception any)
            {
                var query = context.Users.FromSql(any.Message);
                User user = query.SingleOrDefault();
                if (user == null)
                {
                    return Unauthorized();
                }

                return Ok(context.Projects);
            }
        }
    }
}