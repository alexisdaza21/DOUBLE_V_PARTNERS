
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dtos.Deudas
{
    [Table("usuarios")]
    public class UsuarioDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        [Column("email")]
        [MaxLength(100)]
        public string email { get; set; }

        [Required]
        [Column("password")]
        [MaxLength(500)]
        public string password { get; set; }
    }
}
