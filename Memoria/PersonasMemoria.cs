using MiWebService.Models;
using System.Collections.Concurrent;

namespace MiWebService.Data
{
    public class MemoriaPersonas
    {
        private static ConcurrentDictionary<int, Persona> _personas = new ConcurrentDictionary<int, Persona>();
        private static DateTime? _ultimaFechaModificacion = null;
        private PersonaDatos _personaDatos;

        public MemoriaPersonas(PersonaDatos personaDatos)
        {
            _personaDatos = personaDatos;
        }

        public void ArregloPersonas()
        {
            var personasConsulta = _personaDatos.GetPersonas(_ultimaFechaModificacion, out DateTime? nuevaFechaMaxima);

            for (int i = 0; i < personasConsulta.Count; i++)
            {
                var persona = personasConsulta[i];
                var estaEnUso = persona.EnUso;

                if (estaEnUso)
                {
                    _personas.AddOrUpdate(persona.Id, persona, (key, oldValue) => persona);
                }
                else
                {
                    _personas.TryRemove(persona.Id, out _);
                }
            }
        }

        public List<IPersonas> ObtenerPersonas(DateTime? fechaModificacion = null)
        {
            var listaPersonas = new List<IPersonas>();
            var personasEnMemoria = _personas.Values.ToList();

            for (int i = 0; i < personasEnMemoria.Count; i++)
            {
                var persona = personasEnMemoria[i];
                var fechaPersona = DateTime.Parse(persona.FModificacion);

                if (fechaModificacion == null || fechaPersona >= fechaModificacion.Value)
                {
                    var iPersona = new IPersonas
                    {
                        Id = persona.Id,
                        Nombre = persona.Nombre,
                        APaterno = persona.APaterno,
                        AMaterno = persona.AMaterno,
                        Telefono = persona.Telefono,
                        Correo = persona.Correo,
                        NameTag = persona.NameTag,
                        FModificacion = fechaPersona,
                        FechaNacimiento = persona.FechaNacimiento,
                        EmpresaId = persona.EmpresaId,
                        EnUso = persona.EnUso
                    };

                    listaPersonas.Add(iPersona);
                }
                else
                {
                    _personas.TryRemove(persona.Id, out _);
                    Console.WriteLine($"Persona {persona.Id} - {persona.Nombre} removida de memoria por fecha inferior");
                }
            }

            return listaPersonas;
        }
    }
}