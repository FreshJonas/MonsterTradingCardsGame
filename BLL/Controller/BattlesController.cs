using BLL.Battle;
using DAL;
using DAL.Repositories;
using Models.DAL_Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Controller
{
   public class BattlesController : IController<HttpResponse>
   {
      private readonly AuthRepository _authRepository;
      private readonly PlayerRepository _playerRepository;
      private readonly BattleQueue _battleQueue;

      public BattlesController(AuthRepository authRepo, PlayerRepository playerRepo )
      {
         _authRepository = authRepo;
         _playerRepository = playerRepo;
         _battleQueue = new BattleQueue();

         // Start queue
         Thread t = new Thread( new ThreadStart( _battleQueue.Run ) );
         t.Start();
      }
      public HttpResponse Post( string token, StreamReader reader )
      {
         BattleSubscriber battleSubscriber;
         Player player;

         // Validate token
         if ( token == null || ( player = _authRepository.GetPlayer( token ) ) == null )
         {
            return new HttpResponse( 401 );
         }

         // Validate deck
         if (player.Deck.Cards.Count != 4 )
         {
            return new HttpResponse( 401 );
         }

         // Add to battle queue
         battleSubscriber = new();
         if (! _battleQueue.EnqueueAndSubscribe( player, battleSubscriber )){
            // Player already in queue
            return new HttpResponse( 401 );
         }

         // Wait for result
         while ( true )
         {
            if ( battleSubscriber.BattleFinished )
            {
               // Update stats
               if (battleSubscriber.BattleResult.Winner == player )
               {
                  player.AddWin();
               }
               if ( battleSubscriber.BattleResult.Looser == player )
               {
                  player.AddLoss();
               }
               _playerRepository.Update( player );

               // Write response
               HttpResponse response = new( 200 );
               foreach(string line in battleSubscriber.BattleResult.Log )
               {
                  response.Data += line + "\n";
               }
               return response;
            }
            Thread.Sleep( 1000 );
         }
         
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
   }
}
