// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Net.Sockets;
using System.Text;

var port = Console.ReadLine();
using var client = new TcpClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), int.Parse(port!)));
client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 13000));

while(true)
{
    var message = Console.ReadLine();
    if (message == null)
        continue;

    var data = Encoding.ASCII.GetBytes(message);

    client.GetStream().Write(data, 0, data.Length);
}
