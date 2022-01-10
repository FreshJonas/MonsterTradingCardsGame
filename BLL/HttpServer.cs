using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PL
{
   public class HttpServer
   {
      public MainController MainController { get; private set; }

      protected int port;
      TcpListener listener;

      public HttpServer( int port, MainController mainController )
      {
         this.port = port;
         this.MainController = mainController;
      }

      public void Run()
      {
         listener = new TcpListener( IPAddress.Loopback, port );
         listener.Start( 5 );
         while ( true )
         {
            TcpClient s = listener.AcceptTcpClient();
            HttpProcessor processor = new HttpProcessor( s, this );
            new Thread( processor.Process ).Start();

            // TODO
            // Clean up threads
         }
      }
   }
}
