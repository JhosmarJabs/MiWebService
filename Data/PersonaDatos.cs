using Npgsql;
using MiWebService.Models;
using Microsoft.AspNetCore.Mvc;

namespace MiWebService.Data
{
    public class PersonaDatos
    {
        private readonly string _connectionString;

        public PersonaDatos(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Persona> GetPersonas(DateTime? DtModificacion)
        {
            NpgsqlConnection connection = null;
            var personas = new List<Persona>();

            try
            {
                connection = new NpgsqlConnection(_connectionString);
                connection.Open();

                string sql = @"SELECT int_id, var_nombre, var_apaterno, var_amaterno, var_telefono, var_correo, var_nametag, dt_fecha_registro, dt_fecha_modificacion, dt_fecha_nacimiento, int_empresa_id, bol_enuso 
                                FROM usuarios
                                WHERE ";

                if (DtModificacion == null)
                    sql += "bol_enuso = true";
                else
                    sql += "dt_fecha_modificacion >= @DtModificacion";

                sql += " ORDER BY dt_fecha_modificacion DESC, var_nombre";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    if (DtModificacion.HasValue)
                    {
                        command.Parameters.AddWithValue("@DtModificacion", DtModificacion.Value);
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var persona = new Persona
                            {
                                Id = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                APaterno = reader.GetString(2),
                                AMaterno = reader.GetString(3),
                                Telefono = reader.GetInt64(4),
                                Correo = reader.GetString(5),
                                NameTag = reader.GetString(6),
                                FModificacion = reader.GetDateTime(8).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                                FechaNacimiento = reader.GetDateTime(9).ToString("yyyy-MM-dd"),
                                EmpresaId = reader.IsDBNull(10) ? 0 : reader.GetInt32(10),
                                EnUso = reader.GetBoolean(11)
                            };
                            personas.Add(persona);
                        }
                    }
                }
            }
            catch (Exception)
            {
                personas = new List<Persona>();
            }
            finally
            {
                if (connection?.State != System.Data.ConnectionState.Closed)
                    connection.Close();
            }

            return personas;
        }

        public int CreatePersona(Persona persona)
        {
            int ID = 0;
            if (persona == null)
                ID = -1;

            NpgsqlConnection connection = null;

            try
            {
                connection = new NpgsqlConnection(_connectionString);
                connection.Open();

                string sql = @"INSERT INTO usuarios 
                       (var_nombre, var_apaterno, var_amaterno, var_telefono, var_correo, var_nametag, 
                        dt_fecha_nacimiento, dt_fecha_registro, dt_fecha_modificacion, int_empresa_id, bol_enuso)
                       VALUES (@nombre, @apaterno, @amaterno, @telefono, @correo, @nametag, 
                               @fechaNacimiento, CURRENT_TIMESTAMP(3) AT TIME ZONE 'UTC', CURRENT_TIMESTAMP(3) AT TIME ZONE 'UTC', @empresaId, true)
                       RETURNING int_id;";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@nombre", persona.Nombre);
                    command.Parameters.AddWithValue("@apaterno", persona.APaterno);
                    command.Parameters.AddWithValue("@amaterno", persona.AMaterno);
                    command.Parameters.AddWithValue("@telefono", persona.Telefono);
                    command.Parameters.AddWithValue("@correo", persona.Correo);
                    command.Parameters.AddWithValue("@nametag", persona.NameTag);
                    command.Parameters.AddWithValue("@empresaId", persona.EmpresaId);

                    DateTime fechaNacimiento = DateTime.Parse(persona.FechaNacimiento);
                    command.Parameters.AddWithValue("@fechaNacimiento", fechaNacimiento.Date);

                    var result = command.ExecuteScalar();
                    ID = result == null ? 0 : (int)result;
                }
            }
            catch (Exception)
            {
                ID = -1;
            }
            finally
            {
                if (connection?.State != System.Data.ConnectionState.Closed)
                    connection.Close();
            }
            return ID;
        }

        public int UpdatePersona(Persona persona)
        {
            if (persona == null || persona.Id <= 0)
                return -1;

            NpgsqlConnection connection = null;
            int ID = 0;

            if (persona == null)
                ID = -1;
            try
            {
                connection = new NpgsqlConnection(_connectionString);
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
                       dt_fecha_modificacion = CURRENT_TIMESTAMP(3) AT TIME ZONE 'UTC'
                       WHERE int_id = @id AND bol_enuso = true;";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", persona.Id);
                    command.Parameters.AddWithValue("@nombre", persona.Nombre);
                    command.Parameters.AddWithValue("@apaterno", persona.APaterno);
                    command.Parameters.AddWithValue("@amaterno", persona.AMaterno);
                    command.Parameters.AddWithValue("@telefono", persona.Telefono);
                    command.Parameters.AddWithValue("@correo", persona.Correo);
                    command.Parameters.AddWithValue("@nametag", persona.NameTag);
                    command.Parameters.AddWithValue("@empresaId", persona.EmpresaId);

                    DateTime fechaNacimiento = DateTime.Parse(persona.FechaNacimiento);
                    command.Parameters.AddWithValue("@fechaNacimiento", fechaNacimiento.Date);

                    int filasAfectadas = command.ExecuteNonQuery();
                    ID = filasAfectadas > 0 ? persona.Id : 0;
                }
            }
            catch (Exception)
            {
                ID = -1;
            }
            finally
            {
                if (connection?.State != System.Data.ConnectionState.Closed)
                    connection.Close();
            }

            return ID;
        }

        public int DeletePersona(int id)
        {
            NpgsqlConnection connection = null;
            int ID = 0;

            if (id <= 0)
                ID = -1;
            try
            {
                connection = new NpgsqlConnection(_connectionString);
                connection.Open();

                string sql = @"UPDATE usuarios SET 
                               bol_enuso = false,
                               dt_fecha_modificacion = CURRENT_TIMESTAMP(3) AT TIME ZONE 'UTC'
                               WHERE int_id = @id AND bol_enuso = true
                               RETURNING int_id;";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    var result = command.ExecuteScalar();
                    ID = result == null ? 0 : id;
                }
            }
            catch (Exception)
            {
                ID = -1;
            }
            finally
            {
                if (connection?.State != System.Data.ConnectionState.Closed)
                    connection.Close();
            }
            return ID;
        }
    }
}