using Dtos;
using Data;
using Dtos.Deudas;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class ContextPostgres : DbContext
    {
        public ContextPostgres(DbContextOptions<ContextPostgres> options) : base(options)
        {

        }        
        public virtual DbSet<UsuarioDTO> Usuarios { get; set; }

    }
}
