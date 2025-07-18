namespace MiWebService.Models
{
    public class Persona : FMRecibe // Lo recibe del front
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? APaterno { get; set; }
        public string? AMaterno { get; set; }
        public long Telefono { get; set; }
        public string? Correo { get; set; }
        public string? NameTag { get; set; }
        public string? FechaRegistro { get; set; }
        public string? FechaNacimiento { get; set; }
        public int EmpresaId { get; set; }
        public bool EnUso { get; set; }
    }

    public class PersonaResponde // Lo envia al front
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? APaterno { get; set; }
        public string? AMaterno { get; set; }
        public long Telefono { get; set; }
        public string? Correo { get; set; }
        public string? NameTag { get; set; }
        public DateTime FModificacion { get; set; }
        public string? FechaNacimiento { get; set; }
        public int EmpresaId { get; set; }
        public bool EnUso { get; set; }
    }

    public class FMRecibe
    {
        public string? FModificacion { get; set; }// Lo resibe desde el font
    } 
}