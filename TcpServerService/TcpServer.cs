using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpServerService;

public class TcpServer
{
    private readonly TcpListener _tcpListener;
    private List<TcpClientState> _clientStates = [];

    public TcpServer()
    {
        var ipEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 13000);
        _tcpListener = new TcpListener(ipEndPoint);
        StartServer();
    }

    private void StartServer()
    {
        _tcpListener.Start();
        AcceptConnection();
    }

    private void AcceptConnection() => _tcpListener.BeginAcceptTcpClient(HandleConnection, _tcpListener);

    private void HandleConnection(IAsyncResult result)
    {
        var client = _tcpListener.EndAcceptTcpClient(result);
        if (client == null)
            return;


        Console.WriteLine($"New Connection: {client.Client.RemoteEndPoint}");
        var buffer = new byte[client.ReceiveBufferSize];
        var state = new TcpClientState(client, buffer);

        lock (_clientStates)
        {
            _clientStates.Add(state);
        }

        state.BeginRead(HandleDataReceived);
        AcceptConnection();
    }

    private void HandleDataReceived(IAsyncResult result)
    {
        if (result.AsyncState == null)
            return; // TODO?

        var state = (TcpClientState)result.AsyncState;
        var recv = state.EndRead(result);

        if (recv == 0)
        {
            lock (_clientStates)
            {
                _clientStates.Remove(state);
                return;
            }
        }

        var buffer = new byte[recv];
        Buffer.BlockCopy(state.Buffer, 0, buffer, 0, recv);

        var message = Encoding.ASCII.GetString(buffer);
        Console.WriteLine(message);

        state.BeginRead(HandleDataReceived);
    }
}
