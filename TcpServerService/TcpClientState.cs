using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpServerService;

public class TcpClientState(TcpClient tcpClient, byte[] buffer)
{
    public TcpClient TcpClient { get; private set; } = tcpClient;

    public byte[] Buffer { get; private set; } = buffer;

    public NetworkStream Stream => TcpClient.GetStream();

    public void Close()
    {
        TcpClient.Close();
        Buffer = [];
    }

    public void BeginRead(AsyncCallback callback)
    {
        Stream.BeginRead(Buffer, 0, Buffer.Length, callback, this);
    }

    public int EndRead(IAsyncResult result)
    {
        try
        {
            return Stream.EndRead(result);
        }
        catch
        {
            return 0;
        }
    }
}
