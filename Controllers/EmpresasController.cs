using Microsoft.AspNetCore.Mvc;
using MiWebService.Models;
using MiWebService.Data;

namespace MiWebService.Controllers
{
    [ApiController]
    [Route("MiWebService")]
    public class EmpresasController : ControllerBase
    {
        private readonly EmpresasDatos _empresasDatos;

        public EmpresasController(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("ConexionServidor");
            _empresasDatos = new EmpresasDatos(connectionString);
        }

        [HttpPost]
        [Route("GetEmpresas")]
        public List<Empresa> GetEmpresas([FromBody] GetEmpresasRequest? fechaModificacion)
        {
            string fecha = fechaModificacion?.DtFechaModificacion ?? string.Empty;
            return _empresasDatos.GetEmpresas(fecha);
        }

        [HttpPost]
        [Route("CreateEmpresa")]
        public int CreateEmpresa([FromBody] Empresa empresa)
        {
            return _empresasDatos.CreateEmpresa(empresa);
        }

        [HttpPost]
        [Route("UpdateEmpresa")]
        public int UpdateEmpresa([FromBody] Empresa empresa)
        {
            return _empresasDatos.UpdateEmpresa(empresa);
        }

        [HttpPost]
        [Route("DeleteEmpresa")]
        public int DeleteEmpresa([FromBody] int id)
        {
            return _empresasDatos.DeleteEmpresa(id);
        }
    }
}