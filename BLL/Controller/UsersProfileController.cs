using BLL.Payloads;
using DAL;
using DAL.Repositories;
using Models.DAL_Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BLL.Controller
{
   public class UsersProfileController : IController<HttpResponse>
   {
      private readonly AuthRepository _authRepository;
      private readonly PlayerRepository _playerRepository;

      public UsersProfileController( AuthRepository authRepo, PlayerRepository playerRepo )
      {
         _authRepository = authRepo;
         _playerRepository = playerRepo;
      }

      public HttpResponse Delete( string token, StreamReader reader )
      {
         throw new NotImplementedException();
      }

      public HttpResponse Get( string token, StreamReader reader )
      {
         UserProfilePayload profilePl;
         Player player;
         string jsonPayload;

         // Validate token
         if ( token == null || ( player = _authRepository.GetPlayer( token ) ) == null )
         {
            return new HttpResponse( 401 );
         }

         // Create payload
         profilePl = new UserProfilePayload( player.Bio );
         jsonPayload = JsonSerializer.Serialize<UserProfilePayload>( profilePl );

         return new HttpResponse( 200, jsonPayload );
      }

      public HttpResponse Post( string token, StreamReader reader )
      {
         throw new NotImplementedException();
      }

      public HttpResponse Put( string token, StreamReader reader )
      {
         UserProfilePayload profilePl;
         Player player;

         // Authentificate and Get Player
         if ( ( player = _authRepository.GetPlayer( token ) ) == null )
         {
            return new HttpResponse( 401 );
         }

         // Get incoming payload object
         profilePl = JsonSerializer.Deserialize<UserProfilePayload>( ReadAsString( reader ) );

         // Update Player
         if ( !player.ChangeBio( profilePl.Bio ) )
         {
            // Invalid Bio
            return new HttpResponse( 401 );
         }
         _playerRepository.Update( player );

         return new HttpResponse( 202 );
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
