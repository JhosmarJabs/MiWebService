using Microsoft.AspNetCore.Mvc;
using MiWebService.Models;
using MiWebService.Data;

namespace MiWebService.Controllers
{
    [ApiController]
    [Route("MiWebService")]
    public class PersonasController : ControllerBase
    {
        private readonly Datos _datos;

        [HttpPost]
        [Route("GetPersonas")]
        public List<Persona> GetPersonas(DateTime? dtFechaModificacion)
        {
            return _datos.GetPersonas(dtFechaModificacion);
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