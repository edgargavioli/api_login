using Microsoft.EntityFrameworkCore;
using DispesasEmpresa.Model.User;

namespace DispesasEmpresa.Context
{
    public class DispesasEmpresaContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=Empresa;Username=postgres;Password=root");
        }
    }
}
