using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dtos.Deudas
{
    [Table("abonos")]
    public class AbonosDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int id { get; set; }

        [Required]
        [Column("id_deuda")]
        public int idDeuda { get; set; }

        [Required]
        [Column("monto", TypeName = "numeric(10,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
        public decimal monto { get; set; }

        [Column("fecha", TypeName = "timestamp")]
        public DateTime fecha { get; set; } = DateTime.Now;

    }
}
