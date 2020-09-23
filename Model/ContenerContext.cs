using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApi02.Model
{
    public class ContenerContext : DbContext
    {
        public ContenerContext(DbContextOptions<ContenerContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}