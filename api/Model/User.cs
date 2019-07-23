using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Model
{
    /// <summary>
    /// User Model
    /// </summary>
    public class User
    {
        /// <summary>
        /// Identifier of the User
        /// </summary>
        //[JsonIgnoreAttribute]
        public int Id { get; set; }

        /// <summary>
        /// Name of the User
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Name of the User
        /// </summary>
        public string Pwd { get; set; }

    }
}
