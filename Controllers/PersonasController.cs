using Microsoft.AspNetCore.Mvc;
using Npgsql;
using MiWebService.Models;

namespace MiWebService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonasController : ControllerBase

    {
        private readonly string _connectionString;

        public PersonasController(IConfiguration configuration)
        {
            _connectionString = "Host=localhost;Database=bdJosmarEleven;Username=postgres;Password=admin;Port=5432";
        }

        [HttpGet]
        public async Task<IActionResult> GetPersonas()
        {
            var personas = new List<Persona>();
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string sql = @"SELECT int_id, var_nombre, var_apaterno, var_amaterno, var_telefono, var_correo, 
                                      var_nametag, dt_fecha_registro, dt_fecha_modificacion, dt_fecha, bol_enuso 
                               FROM usuarios 
                               WHERE bol_enuso = true
                               ORDER BY var_nombre;";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var persona = new Persona
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("int_id")),
                                Nombre = reader.GetString(reader.GetOrdinal("var_nombre")),
                                APaterno = reader.GetString(reader.GetOrdinal("var_apaterno")),
                                AMaterno = reader.GetString(reader.GetOrdinal("var_amaterno")),
                                Telefono = reader.GetInt64(reader.GetOrdinal("var_telefono")),
                                Correo = reader.IsDBNull(reader.GetOrdinal("var_correo"))
                                                ? null : reader.GetString(reader.GetOrdinal("var_correo")),
                                NameTag = reader.IsDBNull(reader.GetOrdinal("var_nametag"))
                                                ? null : reader.GetString(reader.GetOrdinal("var_nametag")),
                                FechaRegistro = reader.GetDateTime(reader.GetOrdinal("dt_fecha_registro")),
                                FechaModificacion = reader.GetDateTime(reader.GetOrdinal("dt_fecha_modificacion")),
                                Fecha = reader.GetDateTime(reader.GetOrdinal("dt_fecha")),
                                EnUso = reader.GetBoolean(reader.GetOrdinal("bol_enuso"))
                            };
                            personas.Add(persona);
                        }
                    }
                }
                await connection.CloseAsync();
            }
            return Ok(personas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersona(int id)
        {
            Persona? persona = null;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string sql = @"SELECT int_id, var_nombre, var_apaterno, var_amaterno, var_telefono, var_correo, 
                                      var_nametag, dt_fecha_registro, dt_fecha_modificacion, dt_fecha, bol_enuso 
                               FROM usuarios 
                               WHERE int_id = @id AND bol_enuso = true;";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            persona = new Persona
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("int_id")),
                                Nombre = reader.GetString(reader.GetOrdinal("var_nombre")),
                                APaterno = reader.GetString(reader.GetOrdinal("var_apaterno")),
                                AMaterno = reader.GetString(reader.GetOrdinal("var_amaterno")),
                                Telefono = reader.GetInt64(reader.GetOrdinal("var_telefono")),
                                Correo = reader.IsDBNull(reader.GetOrdinal("var_correo"))
                                                ? null : reader.GetString(reader.GetOrdinal("var_correo")),
                                NameTag = reader.IsDBNull(reader.GetOrdinal("var_nametag"))
                                                ? null : reader.GetString(reader.GetOrdinal("var_nametag")),
                                FechaRegistro = reader.GetDateTime(reader.GetOrdinal("dt_fecha_registro")),
                                FechaModificacion = reader.GetDateTime(reader.GetOrdinal("dt_fecha_modificacion")),
                                Fecha = reader.GetDateTime(reader.GetOrdinal("dt_fecha")),
                                EnUso = reader.GetBoolean(reader.GetOrdinal("bol_enuso"))
                            };
                        }
                    }
                }
                await connection.CloseAsync();
            }
            if (persona == null)
                return NotFound(new { message = "Persona no encontrada" });

            return Ok(persona);
        }
    }
}
