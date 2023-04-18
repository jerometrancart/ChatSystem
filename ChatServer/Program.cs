// IMPORT
using System.Net.Sockets;
using System.Net;
using System.Text;

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
try
{
    socket.Bind(endpoint);
    //OUVRE LE SOCKET
    socket.Listen();
}
catch
{
    Console.WriteLine("Impossible de démarrer le serveur.");
    //CHANGE LE CODE DE RETOUR POUR -1 POUR DIRE QU'IL Y A EU UN PB
    Environment.Exit(-1);
}


// SEPARE LES TRY CATCH POUR DIFFERENCIER L'ERREUR DE DEMARRAGE DU SERVEUR DE LA FERMETURE DU CLIENT

try
{

    //CREE UNE NOUVELLE INSTANCE DE SOCKET QUI ATTEND D'ACCEPTER LA CONNEXION
    var clientSocket = socket.Accept();
    Console.WriteLine("Connexion réussie !");
    if (clientSocket.RemoteEndPoint is not null)
    {
        Console.WriteLine("Client connecté depuis l'adresse : " 
        + clientSocket.RemoteEndPoint.ToString());

        

        //BOUCLE WHILE INFINIE POUR FAIRE TOURNER LE SERVEUR
        while (true)
        {   
            //CREER UN TABLEAU D'OCTETS QUI RECEVRA LES DONNEES AU MEME FORMAT QUE CE QUI EST ENVOYE
            //CAD byte[]
            //TOUJOURS INITIALISER UN TABLEAU EN PUISSANCE DE 2
            byte[] buffer = new byte[128];
            //STOCKER LE RESULTAT DE CE QUI A REELLEMENT ETE LU
            int nb = clientSocket.Receive(buffer);
            //AFFICHE LE MESSAGE RECU + SYSTEM.TEXT.DECODE(LE BUFFER, DEPUIS QUEL INDEX, LE NOMBRE D'OCTETS)
            Console.WriteLine("Message reçu : " + Encoding.UTF8.GetString(buffer, 0, nb));
        }
    }
}

catch
{
    Console.WriteLine("La communication avec le client n'est pas possible.");
}

finally
{
    if (socket.Connected)
    {
        //FERME PROPREMENT LE SOCKET DANS LES DEUX SENS
        socket.Shutdown(SocketShutdown.Both);
    }
    //FERME LE SOCKET ET NETTOIE LES DONNEES
    socket.Close();
}
