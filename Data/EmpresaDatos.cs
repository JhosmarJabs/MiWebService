using Npgsql;
using MiWebService.Models;

namespace MiWebService.Data
{
    public class EmpresasDatos
    {
        private readonly string _connectionString;

        public EmpresasDatos(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Empresa> GetEmpresas(string dtFechaModificacion)
        {
            NpgsqlConnection connection = null;
            var empresas = new List<Empresa>();

            try
            {
                connection = new NpgsqlConnection(_connectionString);
                connection.Open();

                string sql = @"SELECT int_id, var_nombre, var_ubicacion, dt_fecha_registro, 
                              dt_fecha_modificacion, bol_enuso 
                       FROM empresas
                       WHERE ";

                if (string.IsNullOrEmpty(dtFechaModificacion))
                    sql += "bol_enuso = true";
                else
                    sql += "dt_fecha_modificacion >= @dtFechaModificacion AND bol_enuso = true";

                sql += " ORDER BY dt_fecha_modificacion DESC, var_nombre";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    if (!string.IsNullOrEmpty(dtFechaModificacion))
                    {
                        if (DateTime.TryParse(dtFechaModificacion, out DateTime fechaParsed))
                        {
                            command.Parameters.AddWithValue("@dtFechaModificacion", fechaParsed);
                        }
                        else
                        {
                            return empresas;
                        }
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var empresa = new Empresa
                            {
                                Id = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                Nombre = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                Ubicacion = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                FechaRegistro = reader.IsDBNull(3) ? string.Empty : reader.GetDateTime(3).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                                FechaModificacion = reader.IsDBNull(4) ? string.Empty : reader.GetDateTime(4).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                                EnUso = reader.IsDBNull(5) ? false : reader.GetBoolean(5)
                            };
                            empresas.Add(empresa);
                        }
                    }
                }
            }
            catch (Exception)
            {
                empresas = new List<Empresa>();
            }
            finally
            {
                if (connection?.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }

            return empresas;
        }

        public int CreateEmpresa(Empresa empresa)
        {
            if (empresa == null)
                return -1;

            NpgsqlConnection connection = null;

            try
            {
                connection = new NpgsqlConnection(_connectionString);
                connection.Open();

                using (var setTimezoneCommand = new NpgsqlCommand("SET TIME ZONE 'UTC'", connection))
                {
                    setTimezoneCommand.ExecuteNonQuery();
                }

                string sql = @"INSERT INTO empresas 
                               (var_nombre, var_ubicacion, dt_fecha_registro, dt_fecha_modificacion, bol_enuso)
                               VALUES (@nombre, @ubicacion, date_trunc('milliseconds', @fechaRegistro), 
                                       date_trunc('milliseconds', @fechaModificacion), true)
                               RETURNING int_id;";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    DateTime fechaActual = DateTime.UtcNow;
                    
                    command.Parameters.AddWithValue("@nombre", empresa.Nombre ?? string.Empty);
                    command.Parameters.AddWithValue("@ubicacion", empresa.Ubicacion ?? string.Empty);
                    command.Parameters.AddWithValue("@fechaRegistro", fechaActual);
                    command.Parameters.AddWithValue("@fechaModificacion", fechaActual);

                    var result = command.ExecuteScalar();
                    return result == null ? 0 : (int)result;
                }
            }
            catch (Exception)
            {
                return -1;
            }
            finally
            {
                connection?.Close();
            }
        }

        public int UpdateEmpresa(Empresa empresa)
        {
            if (empresa == null || empresa.Id <= 0)
                return -1;

            NpgsqlConnection connection = null;

            try
            {
                connection = new NpgsqlConnection(_connectionString);
                connection.Open();

                using (var setTimezoneCommand = new NpgsqlCommand("SET TIME ZONE 'UTC'", connection))
                {
                    setTimezoneCommand.ExecuteNonQuery();
                }

                string sql = @"UPDATE empresas SET 
                               var_nombre = @nombre, 
                               var_ubicacion = @ubicacion,
                               dt_fecha_modificacion = date_trunc('milliseconds', @fechaModificacion)
                               WHERE int_id = @id AND bol_enuso = true
                               RETURNING int_id;";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", empresa.Id);
                    command.Parameters.AddWithValue("@nombre", empresa.Nombre ?? string.Empty);
                    command.Parameters.AddWithValue("@ubicacion", empresa.Ubicacion ?? string.Empty);
                    command.Parameters.AddWithValue("@fechaModificacion", DateTime.UtcNow);

                    var result = command.ExecuteScalar();
                    return result == null ? 0 : (int)result;
                }
            }
            catch (Exception)
            {
                return -1;
            }
            finally
            {
                connection?.Close();
            }
        }

        public int DeleteEmpresa(int id)
        {
            if (id <= 0)
                return -1;

            NpgsqlConnection connection = null;

            try
            {
                connection = new NpgsqlConnection(_connectionString);
                connection.Open();

                using (var setTimezoneCommand = new NpgsqlCommand("SET TIME ZONE 'UTC'", connection))
                {
                    setTimezoneCommand.ExecuteNonQuery();
                }

                string sql = @"UPDATE empresas SET 
                               bol_enuso = false,
                               dt_fecha_modificacion = date_trunc('milliseconds', @fechaModificacion)
                               WHERE int_id = @id AND bol_enuso = true
                               RETURNING int_id;";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@fechaModificacion", DateTime.UtcNow);

                    var result = command.ExecuteScalar();
                    return result == null ? 0 : id;
                }
            }
            catch (Exception)
            {
                return -1;
            }
            finally
            {
                connection?.Close();
            }
        }
    }
}