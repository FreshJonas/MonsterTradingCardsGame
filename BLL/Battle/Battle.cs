using Models.BL_Models;
using Models.BL_Models.Cards;
using Models.DAL_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Battle
{
   public class Battle
   {
      private List<string> _log;
      private Player _playerA;
      private Player _playerB;
      private List<Card> _deckA;
      private List<Card> _deckB;
      Random _rnd;
      private int _rounds;
      private readonly int _maxRounds = 100;
      private Dictionary<CardElement, List<CardElement>> _effectiveness;

      public Battle( Player playerA, Player playerB )
      {
         _playerA = playerA;
         _playerB = playerB;
         _log = new();
         _rnd = new();
         InitEffectiveness();
         CreateDeckCopies();
      }

      public BattleResult Run()
      {
         while ( !EndCondition() )
         {
            _rounds++;

            // Get random card from deck
            Card cardA = GetRandom( _deckA );
            Card cardB = GetRandom( _deckB );

            // Determine looser
            Card loosCard = Fight( cardA, cardB );

            // Check for draw
            if ( loosCard == null ) continue;

            // Loosing Card changes to winners deck
            if ( loosCard == cardA )
            {
               _deckA.Remove( loosCard );
               _deckB.Add( loosCard );
            }
            else
            {
               _deckB.Remove( loosCard );
               _deckA.Add( loosCard );
            }
         }

         // End game
         if ( _rounds <= _maxRounds )
         {
            // Determine winner
            if ( !_deckA.Any() )
            {
               // Player B wins
               _log.Add( $"{_playerB.Name} has won over {_playerA.Name}" );
               return new BattleResult( _log, _playerB, _playerA );
            }
            else
            {
               // Player A wins
               _log.Add( $"{_playerA.Name} has won over {_playerB.Name}" );
               return new BattleResult( _log, _playerA, _playerB );
            }
         }

         _log.Add( "Battle finished in a draw" );
         return new BattleResult( _log, null, null );
      }

      // Returns the loosing card
      private Card Fight( Card cardA, Card cardB )
      {
         Card loosingCard = null;
         int dmgA = cardA.Damage;
         int dmgB = cardB.Damage;

         string newLog = $"{_playerA.Name}: {cardA.Name} ({dmgA} Damage) " +
            $"vs {_playerB.Name}: {cardB.Name} ({dmgB} Damage) => {dmgA} vs {dmgB} -> ";

         // Calculate element damage
         if ( ( cardA.Type == CardType.Spell || cardB.Type == CardType.Spell )
            && cardA.Element != cardB.Element )
         {
            if ( _effectiveness[cardA.Element].Contains( cardB.Element ) )
            {
               dmgA *= 2;
               dmgB /= 2;
            }
            else
            {
               dmgA /= 2;
               dmgB *= 2;
            }
         }

         // Special effects damage
         if ( cardA.MonsterType == MonsterType.Dragon && cardB.MonsterType == MonsterType.Goblin )
            dmgB = 0;
         if ( cardB.MonsterType == MonsterType.Dragon && cardA.MonsterType == MonsterType.Goblin )
            dmgA = 0;
         if ( cardA.MonsterType == MonsterType.Wizzard && cardB.MonsterType == MonsterType.Ork )
            dmgB = 0;
         if ( cardB.MonsterType == MonsterType.Wizzard && cardA.MonsterType == MonsterType.Ork )
            dmgA = 0;
         if ( cardA.MonsterType == MonsterType.Kraken && cardB.Type == CardType.Spell )
            dmgB = 0;
         if ( cardB.MonsterType == MonsterType.Kraken && cardA.Type == CardType.Spell )
            dmgA = 0;
         if ( cardA.MonsterType == MonsterType.Elve && cardA.Element == CardElement.Fire && 
            cardB.MonsterType == MonsterType.Dragon )
            dmgB = 0;
         if ( cardB.MonsterType == MonsterType.Elve && cardB.Element == CardElement.Fire &&
            cardA.MonsterType == MonsterType.Dragon )
            dmgA = 0;

         newLog += $"{dmgA} vs {dmgB} => ";

         // Compare final Damage
         if( dmgA < dmgB )
         {
            newLog += $"{cardB.Name} wins";
            loosingCard = cardA;
         }
         if (dmgA > dmgB )
         {
            newLog += $"{cardA.Name} wins";
            loosingCard = cardB;
         }
         if (dmgA == dmgB )
         {
            newLog += "Draw";
         }

         _log.Add( newLog );
         return loosingCard;
      }


      private void InitEffectiveness()
      {
         _effectiveness = new();
         _effectiveness.Add( CardElement.Normal, new List<CardElement>()
         {
            CardElement.Ice, CardElement.Wind
         } );
         _effectiveness.Add( CardElement.Fire, new List<CardElement>()
         {
            CardElement.Normal, CardElement.Wind
         } );
         _effectiveness.Add( CardElement.Water, new List<CardElement>()
         {
            CardElement.Normal, CardElement.Fire
         } );
         _effectiveness.Add( CardElement.Ice, new List<CardElement>()
         {
            CardElement.Fire, CardElement.Water
         } );
         _effectiveness.Add( CardElement.Wind, new List<CardElement>()
         {
            CardElement.Water, CardElement.Ice
         } );
      }

      private void CreateDeckCopies()
      {
         _deckA = new();
         _deckB = new();
         _playerA.Deck.Cards.ForEach( ( item ) =>
          {
             _deckA.Add( item );
          } );
         _playerB.Deck.Cards.ForEach( ( item ) =>
         {
            _deckB.Add( item );
         } );
      }

      private Card GetRandom( List<Card> deck )
      {
         int index = _rnd.Next( deck.Count );
         return deck[index];
      }

      private bool EndCondition()
      {
         return ( _rounds > _maxRounds || !_deckA.Any() || !_deckB.Any() );
      }

   }
}
