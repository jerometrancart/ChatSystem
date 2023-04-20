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

    //ON OUVRE UN THREAD EN LE DELCARANT LIE A LA FONCTION LIREMESSAGE
    Thread t = new Thread(LireMessage);
    //PLACE LE THREAD EN ARRIERE PLAN POUR EVITER LE BLOCAGE A LA FERMETURE DU SERVEUR
    t.IsBackground = true;
    //ON DEMARRE LE THREAD
    t.Start(socket);



    //BOUCLE INFINIE POUR FAIRE TOURNER LE SERVEUR
    while (true)
    {
        string? message = Console.ReadLine();
        //SI LE MESSAGE EST "Q" SORT DE LA BOUCLE ET DECONNECTE LE SERVEUR
        if (message == "q")
        {
            break;
        }
        //SECURISE DES VALEURS NULLABLES
        if (!string.IsNullOrEmpty(message))
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
    //SHUT UNIQUEMENT SI LE SOCKET ETAIT CONNECTE
    if (socket.Connected)
    {
        //FERME PROPREMENT LE SOCKET DANS LES DEUX SENS
        socket.Shutdown(SocketShutdown.Both);
    }
    //FERME LE SOCKET ET NETTOIE LES DONNEES
    socket.Close();
}

void LireMessage(object? obj)
{
    if (obj is Socket socket)
    {   
        try {

        
            while (true)
            {
                //CREE UN BUFFER POUR LA LECTURE DES MESSAGES MEME SI LE CLIENT N'EN A PAS ENVOYE
                byte[] buffer = new byte[4096];
                int read = socket.Receive(buffer);
                if (read > 0)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, read);
                    //ECRIT LE MESSAGE A RECEPTION
                    System.Console.WriteLine(message);
                }
            }

        }
        catch
        {
            System.Console.WriteLine("La connection a été interrompue.");
        }
    }
}