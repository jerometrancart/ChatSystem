// IMPORT
using System.Net.Sockets;
using System.Net;
using System.Text;

try
{

    //SOCKET(TypeIP, TypeSocket, TypeProtocole)
    var socket = new Socket(
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
    //CREE UNE NOUVELLE INSTANCE DE SOCKET QUI ATTEND D'ACCEPTER LA CONNEXION
    var clientSocket = socket.Accept();
    Console.WriteLine("Connexion réussie !");
    if(clientSocket.RemoteEndPoint is not null)
    {
    Console.WriteLine("Client connecté depuis l'adresse : " + clientSocket.RemoteEndPoint.ToString());
    }
    //CREER UN TABLEAU D'OCTETS QUI RECEVRA LES DONNEES AU MEME FORMAT QUE CE QUI EST ENVOYE
    //CAD byte[]
    //TOUJOURS INITIALISER UN TABLEAU EN PUISSANCE DE 2

    //BOUCLE WHILE INFINIE POUR FAIRE TOURNER LE SERVEUR
    while(true)
    {
        byte[] buffer = new byte[128];
        //STOCKER LE RESULTAT DE CE QUI A REELLEMENT ETE LU
        int nb = clientSocket.Receive(buffer);
        //AFFICHE LE MESSAGE RECU + SYSTEM.TEXT.DECODE(LE BUFFER, DEPUIS QUEL INDEX, LE NOMBRE D'OCTETS)
        Console.WriteLine("Message reçu : "+ Encoding.UTF8.GetString(buffer, 0, nb));
    }
}
catch
{
    Console.WriteLine("Erreur dans le serveur.");
}
