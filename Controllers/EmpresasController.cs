using Microsoft.AspNetCore.Mvc;
using Npgsql;
using MiWebService.Models;

namespace MiWebService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpresasController : ControllerBase
    {
        private readonly string _connectionString;
        
        public EmpresasController()
        {
            _connectionString = "Host=localhost;Database=bd_jabs_eleven;Username=postgres;Password=admin;Port=5432";
        }

        [HttpGet]
        public IActionResult GetEmpresas()
        {
            var empresas = new List<Empresa>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                string sql = @"SELECT int_id, var_nombre
                               FROM empresas";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var empresa = new Empresa
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("int_id")),
                                Nombre = reader.GetString(reader.GetOrdinal("var_nombre")),
                            };
                            empresas.Add(empresa);
                        }
                    }
                }
                connection.Close();
            }
            
            return Ok(empresas);
        }
    }
}   