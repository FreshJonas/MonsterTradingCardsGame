using BLL.Models;
using DAL;
using Models.BL_Models.Cards;
using Models.DAL_Models;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;

namespace BLL
{
   public class UsersController : IController<HttpResponse>
   {
      private readonly PlayerRepository _playerRepository;

      public UsersController( PlayerRepository playerRepository )
      {
         _playerRepository = playerRepository;
      }

      public HttpResponse Post( string token, StreamReader reader )
      {
         Player newPlayer;
         string pwHash;

         UserPayload user = JsonSerializer.Deserialize<UserPayload>( ReadAsString( reader ) );

         // Hash PW
         // FOR DEMONSTRATION PURPOSES ONLY
         pwHash = user.Password + "Hashed";

         // Create new Player
         newPlayer = new( Guid.NewGuid(), user.Username, pwHash,
            new CardStack( Guid.NewGuid() ), new Deck( Guid.NewGuid() ) );

         // Add to DB
         if ( !_playerRepository.Create( newPlayer ) )
         {
            return new HttpResponse( 401 );
         }

         return new HttpResponse( 202 );
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
