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
   class StatsController : IController<HttpResponse>
   {
      private readonly AuthRepository _authRepository;

      public StatsController( AuthRepository authRepo)
      {
         _authRepository = authRepo;
      }

      public HttpResponse Get( string token, StreamReader reader )
      {
         StatsPayload statsPl;
         Player player;
         string jsonPayload;

         // Validate token
         if ( token == null || ( player = _authRepository.GetPlayer( token ) ) == null )
         {
            return new HttpResponse( 401 );
         }

         // Create payload
         statsPl = new StatsPayload( player.Elo, player.Wins, player.Losses );
         jsonPayload = JsonSerializer.Serialize<StatsPayload>( statsPl );

         return new HttpResponse( 200, jsonPayload );
      }

      public HttpResponse Delete( string token, StreamReader reader )
      {
         return new HttpResponse( 400 );
      }

      public HttpResponse Post( string token, StreamReader reader )
      {
         return new HttpResponse( 400 );
      }

      public HttpResponse Put( string token, StreamReader reader )
      {
         return new HttpResponse( 400 );
      }
   }
}
