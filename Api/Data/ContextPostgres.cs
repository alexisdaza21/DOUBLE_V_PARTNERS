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
        //public virtual DbSet<ConfigDTO> config { get; set; }
        //public virtual DbSet<DataSP> dataSP { get; set; }
        //public virtual DbSet<DataSPResponse> dataSPResponse { get; set; }
        //public virtual DbSet<LogExcelDTO> logExcelDTO { get; set; }

    }
}
