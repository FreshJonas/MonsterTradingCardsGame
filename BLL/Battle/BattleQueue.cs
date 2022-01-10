using Models;
using Models.DAL_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Battle
{
   public class BattleQueue
   {
      private Dictionary<Player, ISubscriber<BattleResult>> _subscribers;
      private Queue<Player> _queue;
      private readonly Object _queueLock;

      public BattleQueue()
      {
         _subscribers = new();
         _queue = new();
         _queueLock = new();
      }

      public void Run()
      {
         Player playerA;
         Player playerB;

         while ( true )
         {
            playerA = null;
            playerB = null;

            lock ( _queueLock )
            {
               if ( _queue.Count >= 2 )
               {
                  playerA = _queue.Dequeue();
                  playerB = _queue.Dequeue();
               }
            }

            if ( playerA != null && playerB != null )
            {
               // Possible optimization
               // Extra thread
               InitiateBattle( playerA, playerB );
            }

            Thread.Sleep( 1000 );
         }
      }

      public bool EnqueueAndSubscribe( Player player, ISubscriber<BattleResult> subscriber )
      {
         lock ( _queueLock )
         {
            if ( _queue.Contains( player ) )
            {
               // Player already in queue
               return false;
            }
            _queue.Enqueue( player );
         }
         _subscribers.Add( player, subscriber );
         return true;
      }

      private void InitiateBattle( Player playerA, Player playerB )
      {
         Battle battle = new Battle( playerA, playerB );
         BattleResult battleResult = battle.Run();

         // Notify Subscribers
         _subscribers[playerA].Update( battleResult );
         _subscribers[playerB].Update( battleResult );

         // Remove subscribers
         _subscribers.Remove( battleResult.Winner );
         _subscribers.Remove( battleResult.Looser );
      }

   }
}
