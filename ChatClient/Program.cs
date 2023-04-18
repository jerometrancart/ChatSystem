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
try 
{
    //SE CONNECTE SUR LE SERVEUR
    socket.Connect(endpoint);

    //BOUCLE INFINIE POUR FAIRE TOURNER LE SERVEUR
    while(true)
    {
        string? message = Console.ReadLine();
        //SI LE MESSAGE EST "Q" SORT DE LA BOUCLE ET DECONNECTE LE SERVEUR
        if(message == "q")
        {
            break;
        }
        //SECURISE DES VALEURS NULLABLES
        if(!string.IsNullOrEmpty(message))
        {
            //INVERSE DU COTE SERVEUR
            var buffer = Encoding.UTF8.GetBytes(message);
            socket.Send(buffer);
        }

    }
}
catch
{
    Console.WriteLine("Le serveur est injoignable.");
}

finally
{   
    if(socket.Connected)
    {
        //FERME PROPREMENT LE SOCKET DANS LES DEUX SENS
        socket.Shutdown(SocketShutdown.Both);
    }
    //FERME LE SOCKET ET NETTOIE LES DONNEES
    socket.Close();
}   
