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

        public PersonasController(PersonaDatos datos)
        {
            _datos = datos;
        }

        [HttpPost]
        [Route("GetPersonas")]
        public List<Persona> GetPersonas([FromBody] FmPersona? fmG)
        {
            string f = fmG?.FModificacion ?? string.Empty;
            DateTime? fecha = null;

            if (f != null && DateTime.TryParse(f, out DateTime fechaParsed))
            {
                fecha = fechaParsed;
            }

            return _datos.GetPersonas(fecha);
        }

        [HttpPost]
        [Route("CreatePersona")]
        public int CreatePersona([FromBody] Persona persona)
        {
            return _datos.CreatePersona(persona);
        }

        [HttpPost]
        [Route("UpdatePersona")]
        public int UpdatePersona([FromBody] Persona persona)
        {
            return _datos.UpdatePersona(persona);
        }

        [HttpPost]
        [Route("DeletePersona")]
        public int DeletePersona([FromBody] int id)
        {

            return _datos.DeletePersona(id);
        }
    }
}