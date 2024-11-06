using System.Net;
using System.Net.Sockets;
using System.Text;
using EMS.Core.Interfaces;
using EMS.Core.Models;
using Newtonsoft.Json;

namespace EMS.QueueDisplay.Services;

public class NetworkService : INetworkService, IDisposable
{
    private readonly int _listenPort;
    private CancellationTokenSource _cts;
    private TcpListener _listener;

    public NetworkService(int listenPort = 5000)
    {
        _listenPort = listenPort;
    }

    public void Dispose()
    {
        _cts?.Cancel();
        _listener?.Stop();
    }

    public Task SendCommandAsync(NetworkCommand command)
    {
        throw new NotImplementedException("SendCommand не реализован в QueueDisplay.");
    }

    public Task<NetworkCommand> ReceiveCommandAsync()
    {
        throw new NotImplementedException("ReceiveCommand не реализован в QueueDisplay.");
    }

    public event Action<NetworkCommand> OnCommandReceived;

    public async Task StartListeningAsync()
    {
        _listener = new TcpListener(IPAddress.Any, _listenPort);
        _listener.Start();
        _cts = new CancellationTokenSource();

        while (!_cts.Token.IsCancellationRequested)
            try
            {
                var client = await _listener.AcceptTcpClientAsync();
                _ = HandleClientAsync(client);
            }
            catch (ObjectDisposedException)
            {
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при принятии клиента: {ex.Message}");
            }
    }

    private async Task HandleClientAsync(TcpClient client)
    {
        using (client)
        {
            var stream = client.GetStream();
            var buffer = new byte[1024];
            var sb = new StringBuilder();
            int bytesRead;

            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
            {
                sb.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
                var content = sb.ToString();
                int delimiterIndex;
                while ((delimiterIndex = content.IndexOf('\n')) >= 0)
                {
                    var json = content.Substring(0, delimiterIndex);
                    content = content.Substring(delimiterIndex + 1);
                    sb = new StringBuilder(content);
                    try
                    {
                        var command = JsonConvert.DeserializeObject<NetworkCommand>(json);
                        OnCommandReceived?.Invoke(command);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка десериализации команды: {ex.Message}");
                    }
                }
            }
        }
    }
}