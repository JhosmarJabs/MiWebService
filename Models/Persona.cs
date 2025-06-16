using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiWebService.Models
{
    [Table("usuarios")]
    public class Persona
    {
        [Key]
        [Column("int_id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("var_nombre")]
        public string Nombre { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        [Column("var_apaterno")]
        public string APaterno { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        [Column("var_amaterno")]
        public string AMaterno { get; set; } = string.Empty;
        
        [Required]
        [Column("var_telefono")]
        public long Telefono { get; set; }
        
        [MaxLength(150)]
        [Column("var_correo")]
        public string Correo { get; set; } = string.Empty;
        
        [MaxLength(100)]
        [Column("var_nametag")]
        public string NameTag { get; set; } = string.Empty;
        
        [Column("dt_fecha_registro")]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        
        [Column("dt_fecha_modificacion")]
        public DateTime FechaModificacion { get; set; } = DateTime.Now;
        
        [Required]
        [Column("dt_fecha_nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        [Column("int_empresa_id")]
        public int EmpresaId { get; set; }

        [Column("bool_enuso")]
        public bool EnUso { get; set; } = true;
    }
}
