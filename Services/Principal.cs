using System.Timers;
using MiWebService.Data;
using MiWebService.Models;

namespace MiWebService.Services
{
    public class Principal : IHostedService, IDisposable
    {
        private System.Timers.Timer? _timer;
        public MemoriaPersonas _memoriaPersonas;
        private string _connectionString;

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
            _timer.Elapsed += timerControl!;
            _timer.AutoReset = true;
            _timer.Start();

            return Task.CompletedTask;
        }
        private void timerControl(object sender, ElapsedEventArgs e)
        {
            _timer?.Stop();
            _memoriaPersonas.ArregloPersonas();
            _timer?.Start();
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
