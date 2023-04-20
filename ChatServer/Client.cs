using System;
using System.Net.Sockets;

namespace ChatServer
{
    public class Client
    {
        /// SOCKET DU CLIENT
        public Socket Socket {get; set;}
        
        public string Nom {get; set;}
        /// RAJOUTE UN ID SOUS FORME DE GUID POUR EVITER LES DOUBLONS DE PSEUDO
        public Guid Id {get; set;}
    }
}
