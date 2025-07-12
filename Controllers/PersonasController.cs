using Microsoft.AspNetCore.Mvc;
using MiWebService.Models;
using MiWebService.Data;

namespace MiWebService.Controllers
{
    [ApiController]
    [Route("MiWebService")]
    public class PersonasController : ControllerBase
    {
        private readonly PersonaDatos _datos;
        private readonly MemoriaPersonas _memoriaPersonas;

        public PersonasController(PersonaDatos datos, MemoriaPersonas memoriaPersonas)
        {
            _datos = datos;
            _memoriaPersonas = memoriaPersonas;
        }

        [HttpPost]
        [Route("GetPersonas")]
        public List<Persona> GetPersonas([FromBody] FmPersona? fmG)
        {
            DateTime? fechaModificacion = null;

            if (fmG != null && !string.IsNullOrEmpty(fmG.FModificacion))
            {
                if (DateTime.TryParse(fmG.FModificacion, out DateTime fecha))
                {
                    fechaModificacion = fecha;
                }
            }
            _memoriaPersonas.ArregloPersonas(fechaModificacion);
            return _memoriaPersonas.ObtenerPersonas().Values.ToList();
        }

        [HttpPost]
        [Route("CreatePersona")]
        public int CreatePersona([FromBody] Persona persona)
        {
            int ID = 0;
            if (persona != null)
                ID = _datos.CreatePersona(persona);

            return ID;
        }

        [HttpPost]
        [Route("UpdatePersona")]
        public int UpdatePersona([FromBody] Persona persona)
        {
            int ID = 0;
            if (persona != null)
                ID = _datos.UpdatePersona(persona);
            return ID;
        }

        [HttpPost]
        [Route("DeletePersona")]
        public int DeletePersona([FromBody] int id)
        {

            int ID = 0;
            if (id != 0)
                ID = _datos.DeletePersona(id);
            return ID;
        }
    }
}