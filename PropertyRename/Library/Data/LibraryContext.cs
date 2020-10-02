using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Library.Models;

namespace Library.Data
{

    public interface LibraryContext
    {
        public DbSet<Info> Info { get; set; }

    }
}
