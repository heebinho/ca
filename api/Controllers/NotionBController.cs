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

namespace API.Controllers
{
    /// <summary>
    /// Projects
    /// </summary>
    [ApiVersion("3.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class NotionBController : ControllerBase
    {
        private readonly TnRContext context;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="ctx"></param>
        public NotionBController(TnRContext ctx)
        {
            context = ctx;
        }

        /// <summary>
        /// Interprocedural approach
        /// </summary>
        /// <param name="username">Admin or special value</param>
        /// <param name="password">123456</param>
        /// <returns>list of projects</returns>
        /// <remarks>
        /// # Interprocedural approach
        /// 
        /// Simply pass the tainted sql string to another method and return a new, still tainted string.
        /// 
        /// The analyzer is still able to recognize the vulnerability
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
            string sql = "select * from Users where Name='" + username + "' and Pwd='" + password + "' ";

            string taintedSql = PassThrough(sql);
            var query = context.Users.FromSql(taintedSql);

            User user = query.SingleOrDefault();
            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(context.Projects);
        }


        string PassThrough(string sql) {

            var taintedSQL = sql += "";
            return taintedSQL;
        }


    }
}