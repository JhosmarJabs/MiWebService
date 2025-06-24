namespace MiWebService.Models
{
    public class Empresa
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Ubicacion { get; set; }
        public string FechaRegistro { get; set; }
        public string FechaModificacion { get; set; }
        public bool EnUso { get; set; } = true;
    }
}