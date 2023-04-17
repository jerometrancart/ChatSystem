// IMPORT
using System.Net.Sockets;
using System.Net;

try
{

    //SOCKET(TypeIP, TypeSocket, TypeProtocole)
    Socket socket = new Socket(
        AddressFamily.InterNetwork,
        SocketType.Stream,
        ProtocolType.Tcp);

    //ENDPOINT = IP, PORT
    IPEndPoint endpoint = new IPEndPoint(
        IPAddress.Any, 
        2345
        );

    socket.Bind(endpoint);
    //OUVRE LE SOCKET
    socket.Listen();
    //BLOQUE LE SOCKET EN OUVERT
    socket.Accept();
    Console.WriteLine("Connexion réussie !");
}
catch
{
    Console.WriteLine("Erreur dans le serveur.");
}