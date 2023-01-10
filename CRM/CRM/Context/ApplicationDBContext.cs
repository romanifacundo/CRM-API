using CRM.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM.Context
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Agente> Agentes { get; set; }
        public DbSet<Prospecto> Prospectos { get; set; }
        public DbSet<Contacto> Contactos { get; set; }
    }
}
