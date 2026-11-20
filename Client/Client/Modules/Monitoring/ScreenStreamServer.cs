using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks;

namespace Client.Modules.Monitoring
{
    public class ScreenStreamServer
    {
        private readonly string adminUrl = "http://localhost:5003/api/screen/";
        private readonly string machineName = Environment.MachineName;

        private static readonly HttpClient _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(5)
        };

        [SupportedOSPlatform("windows")]
        public async Task SendingScreensToAdmin(CancellationToken cancellationToken)
        {
            while(!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    byte[] jpegBytes = CaptureScreenBytes();

                    using var request = new HttpRequestMessage(HttpMethod.Post, adminUrl);
                    request.Headers.Add("MachineName", machineName);

                    request.Content = new ByteArrayContent(jpegBytes);
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

                    using var response = await _httpClient.SendAsync(request, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }

                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        [SupportedOSPlatform("windows")]
        private static byte[] CaptureScreenBytes()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);

            using var bitmap = new Bitmap(bounds.Width, bounds.Height);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
            }

            using var ms = new MemoryStream();

            bitmap.Save(ms, ImageFormat.Jpeg);
            return ms.ToArray();
        }
    }
}
