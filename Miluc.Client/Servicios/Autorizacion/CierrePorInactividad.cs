namespace Miluc.Client.Servicios.Autorizacion
{
    using Microsoft.JSInterop;
    using System.Timers;

    public class CierrePorInactividad(IJSRuntime jsRuntime) : IDisposable
    {
        private readonly IJSRuntime _jsRuntime = jsRuntime;
        private Timer? _timer;
        public event Action? OnTimeout;

        public void StartMonitoringInactividad(int minutes)
        {
            _timer = new Timer(minutes * 60 * 1000);
            _timer.Elapsed += (s, e) => OnTimeout?.Invoke();
            _timer.AutoReset = false;
            _timer.Start();

            _jsRuntime.InvokeVoidAsync("idleHelper.registerActivity", DotNetObjectReference.Create(this));
        }

        [JSInvokable]
        public void ResetTimer()
        {
            _timer?.Stop();
            _timer?.Start();
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
