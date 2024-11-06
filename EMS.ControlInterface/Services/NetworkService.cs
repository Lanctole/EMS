using System.IO;
using System.Net.Sockets;
using System.Text;
using EMS.Core.Interfaces;
using EMS.Core.Models;
using Newtonsoft.Json;

namespace EMS.ControlInterface.Services;

public class NetworkService : INetworkService, IDisposable
{
    private readonly string _serverIp;
    private readonly int _serverPort;
    private TcpClient _client;
    private bool _disposed;
    private NetworkStream _stream;

    public NetworkService(string serverIp = "127.0.0.1", int serverPort = 5000)
    {
        _serverIp = serverIp;
        _serverPort = serverPort;
    }

    public void Dispose()
    {
        if (_disposed) return;

        _stream?.Close();
        _client?.Close();
        _disposed = true;
    }

    public async Task SendCommandAsync(NetworkCommand command)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(NetworkService));

        if (_client == null || !_client.Connected) await ConnectAsync();

        try
        {
            var json = JsonConvert.SerializeObject(command);
            var data = Encoding.UTF8.GetBytes(json + "\n");

            if (_stream is null)
                throw new InvalidOperationException("Поток не инициализирован. Попробуйте переподключиться.");
            if (_stream.CanWrite)
            {
                await _stream.WriteAsync(data, 0, data.Length);
                await _stream.FlushAsync();
            }
            else
            {
                Console.WriteLine("Ошибка: Невозможно записать в поток.");
                await ConnectAsync();
                await SendCommandAsync(command);
            }
        }
        catch (IOException ioEx)
        {
            OnConnectionError?.Invoke("Сервер недоступен. Проверьте, запущено ли второе приложение.");
        }
        catch (Exception ex)
        {
            OnConnectionError?.Invoke($"Ошибка подключения: {ex.Message}");
        }
    }

    public Task<NetworkCommand> ReceiveCommandAsync()
    {
        throw new NotImplementedException("ReceiveCommand не реализован в ControlInterface.");
    }

    public event Action<string> OnConnectionError;

    public async Task ConnectAsync()
    {
        if (_client != null && _client.Connected) return;

        try
        {
            _client = new TcpClient();
            await _client.ConnectAsync(_serverIp, _serverPort);
            _stream = _client.GetStream();
        }
        catch (Exception ex)
        {
            OnConnectionError?.Invoke($"Ошибка подключения: {ex.Message}");
        }
    }
}