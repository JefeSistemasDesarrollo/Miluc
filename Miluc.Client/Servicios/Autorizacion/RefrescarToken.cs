namespace Miluc.Client.Servicios.Autorizacion
{
    using System.Timers;
    public class RefrescarToken : IDisposable
    {
        //private readonly IJSRuntime _jsRuntime;
        private Timer? _timer;
        public event Action? OnTimeout;

        public void StartMonitorRefreshToken(int minutes)
        {
            _timer = new Timer(minutes * 60 * 1000);
            _timer.Elapsed += (s, e) => OnTimeout?.Invoke();
            _timer.AutoReset = true;
            _timer.Start();
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
