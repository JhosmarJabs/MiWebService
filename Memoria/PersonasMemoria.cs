using MiWebService.Models;

namespace MiWebService.Data
{
    public class MemoriaPersonas
    {
        private static Dictionary<int, Persona> _personas = new Dictionary<int, Persona>();
        private static DateTime? _fechaMaxima = null;

        private readonly PersonaDatos _personaDatos;

        public MemoriaPersonas(PersonaDatos personaDatos)
        {
            _personaDatos = personaDatos;
        }

        public void ArregloPersonas(DateTime? FModificacion)
        {
            var personasConsulta = _personaDatos.GetPersonas(FModificacion);


            if (_personas == null || _personas.Count == 0)
            {
                _personas = new Dictionary<int, Persona>();
                _fechaMaxima = FModificacion;
            }
            for (int i = 0; i < personasConsulta.Count; i++)
            {
                _personas[personasConsulta[i].Id] = personasConsulta[i];
            }

            _fechaMaxima = FModificacion;

        }

        public Dictionary<int, Persona> ObtenerPersonas()
        {
            return new Dictionary<int, Persona>(_personas);
        }
    }
}