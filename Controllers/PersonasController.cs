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
            string f = fmG?.FModificacion ?? "";
            DateTime? fecha = null;

            if (f != "" && DateTime.TryParse(f, out DateTime fechaParsed))
            {
                fecha = fechaParsed;
            }

            _memoriaPersonas.ArregloPersonas(fecha);
            return _memoriaPersonas.ObtenerPersonas().Values.ToList();
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