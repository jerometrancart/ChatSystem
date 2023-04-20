/// IMPORT
using System.Net.Sockets;
using System.Net;
using System.Text;
using ChatServer;

///SOCKET(TypeIP, TypeSocket, TypeProtocole)
Socket socket = new Socket(
    AddressFamily.InterNetwork,
    SocketType.Stream,
    ProtocolType.Tcp);

///ENDPOINT = IP, PORT
IPEndPoint endpoint = new IPEndPoint(
    IPAddress.Any,
    2345
    );

///ETABLIT UNE LISTE DES CLIENTS
List<Client> clients = new List<Client>();

try
{
    socket.Bind(endpoint);
    ///OUVRE LE SOCKET
    socket.Listen();
}
catch
{
    Console.WriteLine("Impossible de démarrer le serveur.");
    ///CHANGE LE CODE DE RETOUR POUR -1 POUR DIRE QU'IL Y A EU UN PB
    Environment.Exit(-1);
}


/// SEPARE LES TRY CATCH POUR DIFFERENCIER L'ERREUR DE DEMARRAGE DU SERVEUR DE LA FERMETURE DU CLIENT

try
{   
    ///A FAIRE UN NOMBRE INDEFINI DE FOIS POUR FAIRE TOURNER LE SERVEUR
    while(true)
    {
        ///CREE UNE NOUVELLE INSTANCE DE SOCKET QUI ATTEND D'ACCEPTER LA CONNEXION
        var clientSocket = socket.Accept();
        ///OUVRIR UN THREAD POUR GERER CHAQUE NOUVEAU CLIENT
        Thread threadClient = new Thread(EcouterClient);
        threadClient.Start(clientSocket);



        Console.WriteLine("Connexion réussie !");
        if (clientSocket.RemoteEndPoint is not null)
        {
            Console.WriteLine("Client connecté depuis l'adresse : " 
            + clientSocket.RemoteEndPoint.ToString());
            ///CREE UN CLIENT PUIS AJOUTE LE A LA LISTE
            var client = new Client
            {
                Socket = clientSocket,
                Id = Guid.NewGuid()
            };
            clients.Add(client);
            //OUVRE UN THREAD POUR ECOUTER CE CLIENT
            Thread t = new Thread(EcouterClient);
            t.IsBackground = true;
            t.Start(client);
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
        ///FERME PROPREMENT LE SOCKET DANS LES DEUX SENS
        socket.Shutdown(SocketShutdown.Both);
    }
    ///FERME LE SOCKET ET NETTOIE LES DONNEES
    socket.Close();
}
    ///LA METHODE UTILISEE PLUS HAUT POUR ECOUTER LES CLIENTS
    void EcouterClient(object? obj)
    {
        if(obj is Client client)
        {
            try{

            
                ///OBLIGE LE CLIENT A SAISIR UN NOM
                while(string.IsNullOrWhiteSpace(client.Nom))
                {
                    ///LE SERVEUR ENVOIE UN MESSAGE AU CLIENT SUR SON SOCKET POUR LUI DEMANDER SON NOM
                    var message = "Veuillez saisir votre nom";
                    byte[] buff = Encoding.UTF8.GetBytes(message);
                    client.Socket.Send(buff);
                    byte[] nomBuff = new byte[128];
                    int read = client.Socket.Receive(nomBuff);
                    client.Nom = Encoding.UTF8.GetString(nomBuff, 0, read);
                    var message2 = $"Vous êtes connecté au chat, {client.Nom}, vous pouvez discuter";
                    byte[] buff2 = Encoding.UTF8.GetBytes(message2);
                    client.Socket.Send(buff2);
                }
                ///BOUCLE WHILE INFINIE POUR FAIRE TOURNER LE SERVEUR
                while (true)
                {   
                    ///CREER UN TABLEAU D'OCTETS QUI RECEVRA LES DONNEES AU MEME FORMAT QUE CE QUI EST ENVOYE
                    ///CAD byte[]
                    ///TOUJOURS INITIALISER UN TABLEAU EN PUISSANCE DE 2
                    byte[] buffer = new byte[4096];
                    ///STOCKER LE RESULTAT DE CE QUI A REELLEMENT ETE LU
                    int nb = client.Socket.Receive(buffer);
                    if(nb == 0 ) break;
                    ///AFFICHE LE MESSAGE RECU + SYSTEM.TEXT.DECODE(LE BUFFER, DEPUIS QUEL INDEX, LE NOMBRE D'OCTETS)
                    var message = Encoding.UTF8.GetString(buffer, 0, nb);
                    Console.WriteLine($"Message reçu de {client.Nom} :  {message}");


                    ///CREE UN BUFFER QUI CONTIENT LE NOM DU CLIENT + SON MESSAGE
                    ///BOUCLE ENSUITE POUR CHAQUE CLIENT DE LA LISTE POUR REPRODUIRE CE PHENOMENE
                    byte[] sendBuffer = new byte[8192];
                    sendBuffer = Encoding.UTF8.GetBytes($"{client.Nom} : {message}");

                    foreach (var c in clients)
                    {
                        ///NE S APPLIQUE QUE SI LE CLIENT EST DIFFERENT DU CLIENT ACTUEL
                        ///(POUR AFFICHER UNIQUEMENT LES NOMS-MESSAGES DES AUTRES UTILISATEURS)
                        try
                        {
                            if(c.Id != client.Id)
                            {
                                c.Socket.Send(sendBuffer);
                            }
                        }
                        catch 
                        {
                            System.Console.WriteLine($"Impossible d'envoyer un message à {c.Nom}");
                        }
                    }
                }
            }
            catch
            {
                ///SI ERREUR AFFICHE LE NOM DU CLIENT INJOIGNABLE
                System.Console.WriteLine($"Le client {client.Nom} s'est déconnecté");
            }
            finally
            {
                if(client.Socket.Connected)
                {
                    client.Socket.Shutdown(SocketShutdown.Both);
                }
                client.Socket.Close();
                ///EMPECHE LES AUTRES CLIENTS DE LUI ENVOYER DES MESSAGES EN LE RETIRANT DE LA LISTE
                clients.Remove(client);
            }
        }
    }
