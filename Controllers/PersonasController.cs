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

        public PersonasController()
        {
            _connectionString = "Host=localhost;Database=bd_jabs_eleven;Username=postgres;Password=admin;Port=5432";
        }

        [HttpGet]
        public IActionResult GetPersonas(DateTime? dtFechaModificacion)
        {
            var personas = new List<Persona>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                string sql = @"SELECT int_id, var_nombre, var_apaterno, var_amaterno, var_telefono, var_correo, 
                                      var_nametag, dt_fecha_registro, dt_fecha_modificacion, dt_fecha_nacimiento, int_empresa_id, bol_enuso 
                               FROM usuarios
                               WHERE ";

                if (dtFechaModificacion == null)
                    sql += "bol_enuso = true";
                else
                    sql += "dt_fecha_modificacion >= @dtFechaModificacion AND bol_enuso = true";
 
                sql += " ORDER BY dt_fecha_modificacion DESC, var_nombre";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    if (dtFechaModificacion != null && dtFechaModificacion != DateTime.MinValue)
                        command.Parameters.AddWithValue("@dtFechaModificacion", dtFechaModificacion.Value);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var persona = new Persona
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("int_id")),
                                Nombre = reader.GetString(reader.GetOrdinal("var_nombre")),
                                APaterno = reader.GetString(reader.GetOrdinal("var_apaterno")),
                                AMaterno = reader.GetString(reader.GetOrdinal("var_amaterno")),
                                Telefono = reader.GetInt64(reader.GetOrdinal("var_telefono")),
                                Correo = reader.GetString(reader.GetOrdinal("var_correo")),
                                NameTag = reader.GetString(reader.GetOrdinal("var_nametag")),
                                FechaRegistro = reader.GetDateTime(reader.GetOrdinal("dt_fecha_registro")),
                                FechaModificacion = reader.GetDateTime(reader.GetOrdinal("dt_fecha_modificacion")),
                                FechaNacimiento = reader.GetDateTime(reader.GetOrdinal("dt_fecha_nacimiento")),
                                EmpresaId = reader.GetInt32(reader.GetOrdinal("int_empresa_id")),
                                EnUso = reader.GetBoolean(reader.GetOrdinal("bol_enuso"))
                            };
                            personas.Add(persona);
                        }
                    }
                }
                connection.Close();
            }
            return Ok(personas);
        }

        [HttpPost]
        public IActionResult CreatePersona([FromBody] Persona persona)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                string sql = @"INSERT INTO usuarios 
                               (var_nombre, var_apaterno, var_amaterno, var_telefono, var_correo, var_nametag, dt_fecha_nacimiento, int_empresa_id, bol_enuso)
                               VALUES (@nombre, @apaterno, @amaterno, @telefono, @correo, @nametag, @fechaNacimiento, @empresaId, true)
                               RETURNING int_id;";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("nombre", persona.Nombre);
                    command.Parameters.AddWithValue("apaterno", persona.APaterno);
                    command.Parameters.AddWithValue("amaterno", persona.AMaterno);
                    command.Parameters.AddWithValue("telefono", persona.Telefono);
                    command.Parameters.AddWithValue("correo", persona.Correo);
                    command.Parameters.AddWithValue("nametag", persona.NameTag);
                    command.Parameters.AddWithValue("empresaId", persona.EmpresaId);
                    command.Parameters.AddWithValue("fechaNacimiento", persona.FechaNacimiento.Date);

                    var result = command.ExecuteScalar();
                    if (result == null)
                        throw new InvalidOperationException("No se pudo crear la persona");
                    
                    persona.Id = (int)result;
                }
                connection.Close();
            }
            return Created($"api/personas/{persona.Id}", persona);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePersona(int id, [FromBody] Persona persona)
        {
            DateTime fechaModificacion = DateTime.Now;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                string sql = @"UPDATE usuarios SET 
                               var_nombre = @nombre, 
                               var_apaterno = @apaterno,
                               var_amaterno = @amaterno,
                               var_telefono = @telefono,
                               var_correo = @correo,
                               var_nametag = @nametag,
                               dt_fecha_nacimiento = @fechaNacimiento,
                               int_empresa_id = @empresaId,
                               dt_fecha_modificacion = @fechaModificacion
                               WHERE int_id = @id AND bol_enuso = true
                               RETURNING dt_fecha_modificacion;";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("id", id);
                    command.Parameters.AddWithValue("nombre", persona.Nombre);
                    command.Parameters.AddWithValue("apaterno", persona.APaterno);
                    command.Parameters.AddWithValue("amaterno", persona.AMaterno);
                    command.Parameters.AddWithValue("telefono", persona.Telefono);
                    command.Parameters.AddWithValue("correo", persona.Correo);
                    command.Parameters.AddWithValue("nametag", persona.NameTag);
                    command.Parameters.AddWithValue("fechaNacimiento", persona.FechaNacimiento.Date);
                    command.Parameters.AddWithValue("empresaId", persona.EmpresaId);
                    command.Parameters.AddWithValue("fechaModificacion", fechaModificacion);

                    var result = command.ExecuteScalar();

                    if (result == null)
                        return NotFound(new { message = "Persona desactivada" });
                    fechaModificacion = (DateTime)result;
                }

                connection.Close();
            }

            return Ok();
        }

        [HttpPut("delete/{id}")]
        public IActionResult DeletePersona(int id)
        {
            DateTime fechaModificacion = DateTime.Now;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                string sql = @"UPDATE usuarios SET 
                               bol_enuso = false,
                               dt_fecha_modificacion = @fechaModificacion
                               WHERE int_id = @id AND bol_enuso = true;";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("id", id);
                    command.Parameters.AddWithValue("fechaModificacion", fechaModificacion);

                    int affected = command.ExecuteNonQuery();
                    if (affected == 0)
                        return NotFound(new { message = "Persona desactivada" });
                }
                connection.Close();
            }
            return Ok(new
            {
                message = "Persona desactivada"
            });
        }
    }
}