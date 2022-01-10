using DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BLL.Controller
{
   class ScoreController : IController<HttpResponse>
   {
      private readonly PlayerRepository _playerRepository;

      public ScoreController(PlayerRepository playerRepo )
      {
         _playerRepository = playerRepo;
      }

      public HttpResponse Get( string token, StreamReader reader )
      {
         string jsonPayload = JsonSerializer.Serialize( _playerRepository.GetAllScores() );
         return new HttpResponse( 200, jsonPayload );
      }

      public HttpResponse Delete( string token, StreamReader reader )
      {
         return new HttpResponse( 401 );
      }
      public HttpResponse Post( string token, StreamReader reader )
      {
         return new HttpResponse( 401 );
      }

      public HttpResponse Put( string token, StreamReader reader )
      {
         return new HttpResponse( 401 );
      }
   }
}
