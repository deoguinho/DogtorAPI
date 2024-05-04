using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DogtorAPI.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace DogtorAPI.Data
{
    public class DogtorAPIContext : IdentityDbContext<IdentityUser>
    {
        public DogtorAPIContext (DbContextOptions<DogtorAPIContext> options)
            : base(options)
        {
        }

        public DbSet<DogtorAPI.Model.Pet> Pet { get; set; } = default!;

        public DbSet<DogtorAPI.Model.Tutor>? Tutor { get; set; }

        public DbSet<DogtorAPI.Model.Veterinario>? Veterinario { get; set; }

        public DbSet<DogtorAPI.Model.Especialidade>? Especialidade { get; set; }

        public DbSet<DogtorAPI.Model.VeterinarioFotos>? VeterinarioFotos { get; set; }

        public DbSet<DogtorAPI.Model.Consulta>? Consulta { get; set; }



    }
}
