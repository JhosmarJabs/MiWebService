namespace MiWebService.Models
{
    public class Persona
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string APaterno { get; set; }
        public string AMaterno { get; set; }
        public long Telefono { get; set; }
        public string Correo { get; set; }
        public string NameTag { get; set; }
        public string? FechaRegistro { get; set; }
        public string? FechaModificacion { get; set; }
        public string FechaNacimiento { get; set; }
        public int EmpresaId { get; set; }
        public bool EnUso { get; set; }
    }

    public class GetPersonasRequest
    {
        public string? DtFechaModificacion { get; set; }
    } 
}