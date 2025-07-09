using MiWebService.Models;
using System.Collections.Concurrent;

namespace MiWebService.Data
{
    public class MemoriaPersonas
    {
        private static readonly ConcurrentDictionary<int, Persona> _personas = new ConcurrentDictionary<int, Persona>();
        private static DateTime? _ultimaFechaModificacion = null;
        private readonly PersonaDatos _personaDatos;

        public MemoriaPersonas(PersonaDatos personaDatos)
        {
            _personaDatos = personaDatos;
        }

        public void ArregloPersonas(DateTime? fechaModificacion)
        {
            var personasConsulta = _personaDatos.GetPersonas(fechaModificacion);

            foreach (var persona in personasConsulta)
            {
                if (persona.EnUso)
                {
                    _personas.AddOrUpdate(persona.Id, persona, (key, oldValue) => persona);
                }
                else
                {
                    _personas.TryRemove(persona.Id, out _);
                }
            }

            if (personasConsulta.Any())
            {
                var nuevaFechaMaxima = personasConsulta
                    .Select(p => DateTime.Parse(p.FModificacion))
                    .Max();

                if (_ultimaFechaModificacion == null || nuevaFechaMaxima > _ultimaFechaModificacion)
                {
                    _ultimaFechaModificacion = nuevaFechaMaxima;
                }
            }
            Console.WriteLine($"Personas en memoria: {_personas.Count}");
        }

        public DateTime? ObtenerUltimaFechaModificacion()
        {
            Console.WriteLine($"Última fecha de modificación: {_ultimaFechaModificacion}");
            return _ultimaFechaModificacion;
        }

        public Dictionary<int, Persona> ObtenerPersonas()
        {
            return new Dictionary<int, Persona>(_personas);
        }
    }
} 