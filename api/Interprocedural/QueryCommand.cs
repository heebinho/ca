using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Interprocedural
{
    /// <summary>
    /// Command encapsulation
    /// </summary>
    public class QueryCommand
    {
        readonly string query;
        readonly object[] parameters;

        /// <summary>
        /// ctor
        /// </summary>
        public QueryCommand(string query, params object[] parameters)
        {
            this.query = query;
            this.parameters = parameters;
        }

        /// <summary>
        /// Return the query
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(query, parameters);
        }

        /// <summary>
        /// Holds a sql query
        /// </summary>
        public string TaintedSQL { get { return ToString(); } }
    }
}
