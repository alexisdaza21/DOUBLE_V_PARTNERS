using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dtos.Deudas
{
    
    [Table("deudas")]
    public class DeudasDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int id { get; set; }

        [Required]
        [Column("id_usuario_presta")]
        public int idUsuarioPresta { get; set; }

        [Required]
        [Column("id_usuario_debe")]
        public int idUsuarioDebe { get; set; }

        [Required]
        [Column("descripcion", TypeName = "text")]
        public string descripcion { get; set; } = string.Empty;

        [Required]
        [Column("monto", TypeName = "numeric(10,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "El monto debe ser mayor o igual a 0")]
        public decimal monto { get; set; }

        [Column("pagada")]
        public bool pagada { get; set; } = false;

        [Column("estado")]
        public bool estado { get; set; } = true;
    }

    public class DeudaUsuarioDTO
    {
        public int idDeuda { get; set; }
        public string descripcion { get; set; } = string.Empty;
        public decimal monto { get; set; }
        public bool pagada { get; set; }
        public string usuarioPresta { get; set; } = string.Empty;
        public string usuarioDebe { get; set; } = string.Empty;

        public string fechaPago { get; set; }
    }

}