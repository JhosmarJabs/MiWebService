using Microsoft.AspNetCore.Mvc;
using MiWebService.Models;
using MiWebService.Data;
using MiWebService.Services;

namespace MiWebService.Controllers
{
    [ApiController]
    [Route("MiWebService")]
    public class PersonasController : ControllerBase
    {
        private PersonaDatos _datos;
        private MemoriaPersonas _memoriaPersonas;

        public PersonasController(PersonaDatos datos, MemoriaPersonas memoriaPersonas)
        {
            _datos = datos;
            _memoriaPersonas = new MemoriaPersonas(_datos);
        }


        [HttpPost]
        [Route("GetPersonas")]
        public List<IPersonas> GetPersonas([FromBody] FmPersona? fmG)
        {
            DateTime? fechaModificacion = null;

            if (DateTimeOffset.TryParse(fmG.FModificacion, out DateTimeOffset fechaOffset))
            {
                fechaModificacion = fechaOffset.DateTime;
            }
        
            return _memoriaPersonas.ObtenerPersonas(fechaModificacion);
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