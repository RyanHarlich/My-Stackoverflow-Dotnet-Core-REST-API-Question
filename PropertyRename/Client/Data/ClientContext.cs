using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Client.Models;
using Library.Data;
using Library.Models;

namespace Client.Data
{
    public class ClientContext : DbContext, LibraryContext
    {
        public ClientContext (DbContextOptions<ClientContext> options)
            : base(options)
        {
        }

        public DbSet<Client.Models.Level1> Level1 { get; set; }
        public DbSet<Info> Info { get; set; }
    }
}
