using System.Timers;
using MiWebService.Data;
using MiWebService.Models;

namespace MiWebService.Services
{
    public class Principal : IHostedService, IDisposable
    {
        private System.Timers.Timer _timer;
        private readonly MemoriaPersonas _memoriaPersonas;
        private readonly string _connectionString;

        public Principal(string connectionString)
        {
            _connectionString = connectionString;
            var personaDatos = new PersonaDatos(_connectionString);
            _memoriaPersonas = new MemoriaPersonas(personaDatos);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Ejecutando ArregloPersonas...");
            _timer = new System.Timers.Timer(10000);
            _timer.Elapsed += (sender, e) =>
            {
                _memoriaPersonas.ArregloPersonas(_memoriaPersonas.ObtenerUltimaFechaModificacion());
            };
            _timer.AutoReset = true;
            _timer.Start();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Stop();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
