using BLL.Models;
using BLL.Payloads.Outgoing;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BLL.Controller
{
   class SessionsController : IController<HttpResponse>
   {
      private readonly AuthRepository _authRepository;

      public SessionsController(AuthRepository authRepository )
      {
         _authRepository = authRepository;
      }

      public HttpResponse Post( string token, StreamReader reader )
      {
         TokenPayload newToken;
         string pwHash;
         string result;

         // Get user object
         UserPayload user = JsonSerializer.Deserialize<UserPayload>( ReadAsString( reader ) );

         // Hash password
         pwHash = user.Password + "Hashed";

         // Validate login
         if( (result = _authRepository.Login( user.Username, pwHash )) == "" )
         {
            return new HttpResponse( 401 );
         }

         // Create token object
         newToken = new TokenPayload( result );

         // Create response
         return new HttpResponse( 202, JsonSerializer.Serialize<TokenPayload>( newToken ) );
      }

      public HttpResponse Delete( string token, StreamReader reader )
      {
         return new HttpResponse( 400 );
      }

      public HttpResponse Get( string token, StreamReader reader )
      {
         return new HttpResponse( 400 );
      }

      public HttpResponse Put( string token, StreamReader reader )
      {
         return new HttpResponse( 400 );
      }

      private string ReadAsString( StreamReader reader )
      {
         string str = "";
         while ( reader.Peek() >= 0 )
         {
            str += (char)reader.Read();
         }
         return str;
      }
   }
}
