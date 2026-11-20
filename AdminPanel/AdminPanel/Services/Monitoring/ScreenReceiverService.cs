using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace AdminPanel.Services.Monitoring
{
    public class ScreenReceiverService : IDisposable
    {
        private readonly HttpListener _listener = new HttpListener();
        private readonly Action<BitmapImage> _onFrameReceived;
        private CancellationTokenSource? _cts;

        private volatile bool _isMonitoringEnabled;
        private string? _targetMachineName;

        public ScreenReceiverService(int port, Action<BitmapImage> onFrameReceived)
        {
            _onFrameReceived = onFrameReceived;
            _listener.Prefixes.Add($"http://+:{port}/api/screen/");
        }

        public void StartListener()
        {
            if (_listener.IsListening) return;

            _cts = new CancellationTokenSource();
            _listener.Start();
            _ = Task.Run(() => ListenLoopAsync(_cts.Token));
        }

        public void StartShowing(string machineName)
        {
            _targetMachineName = machineName;
            _isMonitoringEnabled = true;
        }

        public void StopShowing()
        {
            _isMonitoringEnabled = false;
        }

        private async Task ListenLoopAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested && _listener.IsListening)
            {
                HttpListenerContext? context = null;
                try
                {
                    context = await _listener.GetContextAsync();
                    _ = Task.Run(() => HandleRequestAsync(context), token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (HttpListenerException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[ScreenReceiver] Помилка слухача: {ex.Message}");
                }
            }
        }

        private async Task HandleRequestAsync(HttpListenerContext context)
        {
            try
            {
                if (!string.Equals(context.Request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase))
                {
                    context.Response.StatusCode = 405;
                    context.Response.Close();
                    return;
                }

                string? senderMachine = context.Request.Headers["MachineName"];

                if (!_isMonitoringEnabled ||
                    string.IsNullOrWhiteSpace(senderMachine) ||
                    string.IsNullOrWhiteSpace(_targetMachineName) ||
                    !string.Equals(senderMachine.Trim(), _targetMachineName.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    context.Response.StatusCode = 204; 
                    context.Response.Close();
                    return;
                }

                using var ms = new MemoryStream();
                await context.Request.InputStream.CopyToAsync(ms);
                byte[] imageBytes = ms.ToArray();

                context.Response.StatusCode = 200;
                context.Response.Close();

                if (imageBytes.Length == 0) return;

                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = new MemoryStream(imageBytes);
                bitmap.EndInit();
                bitmap.Freeze();

                _onFrameReceived(bitmap);
            }
            catch
            {
                try { context.Response.StatusCode = 500; context.Response.Close(); } catch { }
            }
        }

        public void Dispose()
        {
            _isMonitoringEnabled = false;
            _cts?.Cancel();
            _listener.Stop();
            _listener.Close();
        }
    }
}
