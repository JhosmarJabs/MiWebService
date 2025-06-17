namespace MiWebService.Models
{
    public class Persona
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string APaterno { get; set; } = string.Empty;
        public string AMaterno { get; set; } = string.Empty;
        public long Telefono { get; set; }
        public string Correo { get; set; } = string.Empty;
        public string NameTag { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public DateTime FechaModificacion { get; set; } = DateTime.Now;
        public DateTime FechaNacimiento { get; set; }
        public int EmpresaId { get; set; }
        public bool EnUso { get; set; } = true;
    }
}