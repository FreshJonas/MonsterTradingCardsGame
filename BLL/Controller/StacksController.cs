using BLL.Payloads.Incoming;
using DAL;
using DAL.Repositories;
using Models.BL_Models;
using Models.BL_Models.Cards;
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
   class StacksController : IController<HttpResponse>
   {
      private readonly AuthRepository _authRepository;

      public StacksController(AuthRepository authRepo)
      {
         _authRepository = authRepo;
      }

      public HttpResponse Delete( string token, StreamReader reader )
      {
         return new HttpResponse( 400 );
      }

      public HttpResponse Get( string token, StreamReader reader )
      {
         Player player;
         string jsonPayload;

         // Validate token
         if ( token == null || ( player = _authRepository.GetPlayer( token ) ) == null  )
         {
            return new HttpResponse( 401 );
         }

         // Send Stack to Client
         jsonPayload = JsonSerializer.Serialize<IList<CardPayload>>( ObjectConverter.ConvertToCardPl(player.Stack.Cards) );
         return new HttpResponse( 200, jsonPayload );
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
