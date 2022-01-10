using BLL.Payloads;
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
   public class DecksController : IController<HttpResponse>
   {
      private readonly AuthRepository _authRepository;
      private readonly PlayerRepository _playerRepository;

      public DecksController( AuthRepository authRepo, PlayerRepository playerRepo )
      {
         _authRepository = authRepo;
         _playerRepository = playerRepo;
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
         if ( token == null || ( player = _authRepository.GetPlayer( token ) ) == null )
         {
            return new HttpResponse( 401 );
         }

         // Send Deck to Client
         jsonPayload = JsonSerializer.Serialize<IList<Card>>( player.Deck.Cards );
         return new HttpResponse( 200, jsonPayload );
      }

      public HttpResponse Post( string token, StreamReader reader )
      {
         return new HttpResponse( 400 );
      }

      public HttpResponse Put( string token, StreamReader reader )
      {
         Player player;
         IList<CardPayload> newDeckPl;
         List<Card> newDeck = new();

         // Authentificate and Get Player
         if ( ( player = _authRepository.GetPlayer( token ) ) == null )
         {
            return new HttpResponse( 401 );
         }

         // Get new Deck
         newDeckPl = JsonSerializer.Deserialize<IList<CardPayload>>( ReadAsString( reader ) );

         // Transform to Card Obj
         foreach(CardPayload cardPl in newDeckPl )
         {
            newDeck.Add( new Card( new Guid(cardPl.Guid), cardPl.Name, cardPl.Damage,
               Enums.StringToCardType( cardPl.Type ), Enums.StringToCardElement( cardPl.Element ),
               Enums.StringToMonsterType( cardPl.MonsterType ) ) );
         }

         // Validate ownership of cards
         foreach ( Card card in newDeck )
         {
            if ( !player.Stack.Cards.Contains( card ) )
            {
               return new HttpResponse( 401 );
            }
         }

         // Validate length of new deck
         if ( newDeck.Count != 4 ) return new HttpResponse( 401 );

         // Update player deck
         player.Deck.Cards.Clear();
         player.Deck.Cards.AddRange( newDeck );
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
