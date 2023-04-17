using System.Net.Sockets;
using System.Net;
using System.Text;


//SOCKET(TypeIP, TypeSocket, TypeProtocole)
var socket = new Socket(
    AddressFamily.InterNetwork,
    SocketType.Stream,
    ProtocolType.Tcp);

//ON MET LOOPBACK POUR L'IP DU LOCALHOST
var endpoint = new IPEndPoint(
    IPAddress.Loopback,
    2345
);

//SE CONNECTE SUR LE SERVEUR
socket.Connect(endpoint);

//BOUCLE INFINIE POUR FAIRE TOURNER LE SERVEUR
while(true)
{
    string message = Console.ReadLine();
    //SI LE MESSAGE EST "Q" SORT DE LA BOUCLE ET DECONNECTE LE SERVEUR
    if(message == "q")
    {
        break;
    }
    //INVERSE DU COTE SERVEUR
    var buffer = Encoding.UTF8.GetBytes(message);
    socket.Send(buffer);

}


    
