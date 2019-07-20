using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace API.Model
{
    /// <summary>
    /// DB Model
    /// </summary>
    public class TnRContext : DbContext
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="options"></param>
        public TnRContext(DbContextOptions<TnRContext> options)
            : base(options)
        { }

        /// <summary>
        /// Project set
        /// </summary>
        public DbSet<Project> Projects { get; set; }

        /// <summary>
        /// User set
        /// </summary>
        public DbSet<User> Users { get; set; }

    }
    
}
