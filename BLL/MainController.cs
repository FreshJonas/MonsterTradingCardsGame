using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
   public class MainController
   {
      public Dictionary<string, IController<HttpResponse>> Controller { get; private set; }

      public MainController()
      {
         Controller = new();
      }

      public HttpResponse AssignController( string method, string path, string token, StreamReader reader )
      {
         HttpResponse response;
         IController<HttpResponse> controller;
         Controller.TryGetValue( path, out controller );

         if ( controller == null )
         {
            return new HttpResponse(404, "Path not found");
         }

         switch ( method.ToLower() )
         {
            case "get":
               {
                  response = controller.Get( token, reader );
                  break;
               }
            case "post":
               {
                  response = controller.Post( token, reader );
                  break;
               }
            case "put":
               {
                  response = controller.Put( token, reader );
                  break;
               }
            case "delete":
               {
                  response = controller.Delete( token, reader );
                  break;
               }
            default:
               {
                  response = new HttpResponse( 400, "Bad request" );
                  break;
               }
         }
         return response;
      }


   }
}
