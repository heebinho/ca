using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Model
{
    /// <summary>
    /// Project Model
    /// </summary>
    public class Project
    {
        /// <summary>
        /// Identifier of the project
        /// </summary>
        //[JsonIgnoreAttribute]
        public int Id { get; set; }

        /// <summary>
        /// Name of the project
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description of the project
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Creation date
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Modified date
        /// </summary>
        public DateTime Modified{ get; set; }


    }
}
