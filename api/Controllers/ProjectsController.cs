using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Model;

namespace API.Controllers
{
    /// <summary>
    /// Projects
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly TnRContext context;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="ctx"></param>
        public ProjectsController(TnRContext ctx)
        {
            context = ctx;
        }

        /// <summary>
        /// REST Method to get a list of projects
        /// </summary>
        /// <param name="username">Admin or special value:' or 1=1 or ' </param>
        /// <param name="password">123456</param>
        /// <returns>list of projects</returns>
        /// <remarks>
        /// # Static Code Analysis
        /// 
        /// A method with two input parameters to test and illustrate a sql injection vulnerability.
        /// To get the list of projects use either a valid username and a valid password or try the injection value:
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
            var query = context.Users.FromSql(taintedSql);

            User user = query.SingleOrDefault();
            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(context.Projects);
        }


        /// <summary>
        /// Projects
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Project> GetProjects()
        {
            return context.Projects;
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = await context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        /// <summary>
        /// Put
        /// </summary>
        /// <param name="id"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject([FromRoute] int id, [FromBody] Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != project.Id)
            {
                return BadRequest();
            }

            context.Entry(project).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Post
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostProject([FromBody] Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            context.Projects.Add(project);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetProject", new { id = project.Id }, project);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = await context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            context.Projects.Remove(project);
            await context.SaveChangesAsync();

            return Ok(project);
        }

        private bool ProjectExists(int id)
        {
            return context.Projects.Any(e => e.Id == id);
        }
    }
}