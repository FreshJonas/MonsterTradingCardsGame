using Models.BL_Models;
using Models.BL_Models.Cards;
using Models.DAL_Models;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   public class PlayerRepository : IRepository<Player>
   {
      private readonly Database _db;

      public PlayerRepository( Database db )
      {
         _db = db;
      }

      public bool Create( Player player )
      {
         NpgsqlCommand cmd = new();

         // Check if user exists
         cmd.CommandText = "SELECT * FROM Player WHERE player_name=@p";
         cmd.Parameters.AddWithValue( "p", player.Name );
         if ( _db.ExecuteScalar( cmd ) != null )
            return false;

         // Stack and Deck Table
         cmd.CommandText = "";
         cmd.CommandText += $"INSERT INTO Stack (stack_guid) " +
            $"VALUES (\'{player.Stack.Guid}\');\n";
         cmd.CommandText += $"INSERT INTO Deck (deck_guid) " +
            $"VALUES (\'{player.Deck.Guid}\');\n";
         _db.ExecuteNonQuery( cmd );

         // Player Table
         cmd = new NpgsqlCommand( "INSERT INTO Player " +
            "(player_guid, player_name, player_pw, player_coins, player_elo, " +
            "player_wins, player_losses, player_bio, stack_guid, deck_guid) " +
            "VALUES (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10);" );
         cmd.Parameters.AddWithValue( "p1", player.Guid.ToString() );
         cmd.Parameters.AddWithValue( "p2", player.Name );
         cmd.Parameters.AddWithValue( "p3", player.PwHash );
         cmd.Parameters.AddWithValue( "p4", player.Coins );
         cmd.Parameters.AddWithValue( "p5", player.Elo );
         cmd.Parameters.AddWithValue( "p6", player.Wins );
         cmd.Parameters.AddWithValue( "p7", player.Losses );
         cmd.Parameters.AddWithValue( "p8", player.Bio );
         cmd.Parameters.AddWithValue( "p9", player.Stack.Guid.ToString() );
         cmd.Parameters.AddWithValue( "p10", player.Deck.Guid.ToString() );
         _db.ExecuteNonQuery( cmd );

         // CardStack and CardDeck Table
         cmd = new();
         foreach ( Card card in player.Stack.Cards )
         {
            cmd.CommandText += $"INSERT INTO CardStack (stack_guid, card_guid) " +
               $"VALUES (\'{player.Stack.Guid}\', \'{card.Guid}\');\n";
         }
         foreach ( Card card in player.Deck.Cards )
         {
            cmd.CommandText += $"INSERT INTO CardDeck (deck_guid, card_guid) " +
               $"VALUES (\'{player.Deck.Guid}, \'{card.Guid}\');\n";
         }
         if ( cmd.CommandText != "" )
         {
            _db.ExecuteNonQuery( cmd );
         }

         return true;
      }

      public Player Read( Guid guid )
      {
         DAL_Player dPlayer;
         CardStack stack;
         Deck deck;
         List<OrderedDictionary> data;
         OrderedDictionary row;
         NpgsqlCommand cmd;

         // Player Table
         cmd = new NpgsqlCommand( $"SELECT * FROM Player WHERE player_guid=\'{guid}\'" );
         if ( ( row = _db.GetRow( cmd ) ) == null )
            return null;
         dPlayer = DataRowToPlayer( row );

         // Creating Stack and Deck
         stack = new( dPlayer.StackId );
         deck = new( dPlayer.DeckId );

         // Stack Table
         cmd = new NpgsqlCommand( $"SELECT " +
            $"Card.card_guid, Card.card_name, Card.card_damage, Card.card_type, " +
            $"Card.card_element, Card.card_monsterType " +
            $"FROM Card " +
            $"INNER JOIN CardStack ON Card.card_guid = CardStack.card_guid " +
            $"WHERE CardStack.stack_guid = \'{dPlayer.StackId}\';" );
         data = _db.ExecuteQuery( cmd );
         foreach ( OrderedDictionary cardRow in data )
         {
            stack.AddCard( DataRowToCard( cardRow ) );
         }

         // Deck Table
         cmd = new NpgsqlCommand( $"SELECT " +
            $"Card.card_guid, Card.card_name, Card.card_damage, Card.card_type, " +
            $"Card.card_element, Card.card_monsterType " +
            $"FROM Card " +
            $"INNER JOIN CardDeck ON Card.card_guid = CardDeck.card_guid " +
            $"WHERE CardDeck.deck_guid = \'{dPlayer.DeckId}\';" );
         data = _db.ExecuteQuery( cmd );
         foreach ( OrderedDictionary cardRow in data )
         {
            deck.AddCard( DataRowToCard( cardRow ) );
         }

         return new Player( 
            dPlayer.Guid, dPlayer.Name, dPlayer.PwHash, dPlayer.Coins, dPlayer.Elo, 
            dPlayer.Wins, dPlayer.Losses, dPlayer.Bio, stack, deck );
      }

      public bool Update( Player player )
      {
         bool res = true;
         NpgsqlCommand cmd;

         // Player Table
         cmd = new NpgsqlCommand( "UPDATE Player " +
            "SET player_name=@p2, player_pw=@p3, player_coins=@p4, player_elo=@p5," +
            "player_wins=@p6, player_losses=@p7, player_bio=@p8 " +
            "WHERE player_guid=@p1" );
         cmd.Parameters.AddWithValue( "p1", player.Guid.ToString() );
         cmd.Parameters.AddWithValue( "p2", player.Name );
         cmd.Parameters.AddWithValue( "p3", player.PwHash );
         cmd.Parameters.AddWithValue( "p4", player.Coins );
         cmd.Parameters.AddWithValue( "p5", player.Elo );
         cmd.Parameters.AddWithValue( "p6", player.Wins );
         cmd.Parameters.AddWithValue( "p7", player.Losses );
         cmd.Parameters.AddWithValue( "p8", player.Bio );
         if ( !_db.ExecuteNonQuery( cmd ) )
            res = false;

         // CardStack and CardDeck Table
         cmd = new();
         cmd.CommandText = "";
         cmd.CommandText += ( $"DELETE FROM CardStack " +
            $"WHERE stack_guid=\'{player.Stack.Guid}\';\n" );
         cmd.CommandText += ( $"DELETE FROM CardDeck " +
            $"WHERE deck_guid=\'{player.Deck.Guid}\';\n" );
         foreach ( Card card in player.Stack.Cards )
         {
            cmd.CommandText += $"INSERT INTO CardStack (stack_guid, card_guid) " +
               $"VALUES (\'{player.Stack.Guid}\', \'{card.Guid}\');\n";
         }
         foreach ( Card card in player.Deck.Cards )
         {
            cmd.CommandText += $"INSERT INTO CardDeck (deck_guid, card_guid) " +
               $"VALUES (\'{player.Deck.Guid}\', \'{card.Guid}\');\n";
         }
         if ( !_db.ExecuteNonQuery( cmd ) )
            res = false;

         return res;
      }

      public Dictionary<string, int> GetAllScores()
      {
         Dictionary<string, int> scores = new();
         List<OrderedDictionary> data;

         var cmd = new NpgsqlCommand( "SELECT player_name, player_elo FROM Player" );
         data = _db.ExecuteQuery( cmd );
         foreach(OrderedDictionary row in data )
         {
            scores.Add( (string)row["player_name"], (int)row["player_elo"] );  
         }
         return scores;
      }

      public bool Delete( Guid guid )
      {
         throw new NotImplementedException();
      }

      private DAL_Player DataRowToPlayer( OrderedDictionary row )
      {
         Guid guid = new Guid( (string)row["player_guid"] );
         string name = (string)row["player_name"];
         string pwHash = (string)row["player_pw"];
         int coins = (int)row["player_coins"];
         int elo = (int)row["player_elo"];
         int wins = (int)row["player_wins"];
         int losses = (int)row["player_losses"];
         string bio = (string)row["player_bio"];
         Guid stackId = new( (string)row["stack_guid"] );
         Guid deckId = new( (string)row["deck_guid"] );

         return new DAL_Player( 
            guid, name, pwHash, coins, elo, wins, losses, bio, stackId, deckId );
      }

      private Card DataRowToCard( OrderedDictionary row )
      {
         Guid guid = new( (string)row["card_guid"] );
         string name = (string)row["card_name"];
         int damage = (int)row["card_damage"];
         CardType type = (CardType)row["card_type"];
         CardElement element = (CardElement)row["card_element"];
         MonsterType monsterType = (MonsterType)row["card_monstertype"];

         return new Card( guid, name, damage, type, element, monsterType );
      }

   }
}
