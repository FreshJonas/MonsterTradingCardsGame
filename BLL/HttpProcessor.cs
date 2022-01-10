using BLL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PL
{
   class HttpProcessor
   {
      private TcpClient _socket;
      private StreamReader _reader;
      private StreamWriter _writer;
      private MainController _mainController;

      public string Method { get; private set; }
      public string Path { get; private set; }
      public string Version { get; private set; }
      public string Data { get; private set; }

      public Dictionary<string, string> Headers { get; }

      public HttpProcessor( TcpClient s, HttpServer httpServer )
      {
         this._socket = s;
         this._mainController = httpServer.MainController;

         Method = null;
         Headers = new();
      }

      public void Process()
      {
         _writer = new StreamWriter( _socket.GetStream() ) { AutoFlush = true };
         _reader = new StreamReader( _socket.GetStream() );

         GetMetaData();

         // Get Authorization token if provided
         Headers.TryGetValue( "Authorization", out string token );

         // Get Response from Controller
         SendResponse( _mainController.AssignController( Method, Path, token, _reader ) );

         _writer.Close();
         _reader.Close();
      }

      public void SendResponse( HttpResponse response )
      {
         // write the full HTTP-response
         WriteLine( $"HTTP/1.1 {response.Code} {HttpCodeToMessage( response.Code )}" );
         WriteLine( "Server: MTCG" );
         WriteLine( $"Current Time: {DateTime.Now}" );

         if(response.Data != "" )
         {
            WriteLine( $"Content-Length: {response.Data.Length}" );
            //WriteLine( "Content-Type: application/json" );
            WriteLine( "" );
            WriteLine( response.Data );
         }

         _writer.Flush();

         Console.WriteLine();
      }

      private void GetMetaData()
      {
         // read (and handle) the full HTTP-request
         string line = null;
         while ( ( line = _reader.ReadLine() ) != null )
         {
            Console.WriteLine( line );
            if ( line.Length == 0 )
               break;  // empty line means next comes the content (which is currently skipped)

            // handle first line of HTTP
            if ( Method == null )
            {
               var parts = line.Split( ' ' );
               Method = parts[0];
               Path = parts[1];
               Version = parts[2];
            }
            // handle HTTP headers
            else
            {
               var parts = line.Split( ": " );
               Headers.Add( parts[0], parts[1] );
            }
         }
      }

      private void WriteLine( string s )
      {
         Console.WriteLine( s );
         _writer.WriteLine( s );
      }

      private string HttpCodeToMessage( int Code )
      {
         if ( Code == 200 ) return "OK";
         if ( Code == 201 ) return "Accepted";
         if ( Code == 202 ) return "Created";
         if ( Code == 400 ) return "BadRequest";
         if ( Code == 401 ) return "Forbidden";
         if ( Code == 404 ) return "NotFound";
         return "Error";
      }
   }
}
