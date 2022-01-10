using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.DAL_Models;
using Npgsql;

namespace DAL.Repositories
{
   public class AuthRepository
   {
      private readonly Database _db;
      private readonly PlayerRepository _playerRepository;

      public AuthRepository( Database db, PlayerRepository playerRepository )
      {
         _db = db;
         _playerRepository = playerRepository;
      }

      public Player GetPlayer( string token )
      {
         object res;
         var cmd = new NpgsqlCommand( "SELECT player_guid FROM Token WHERE token_value=@p;" );
         cmd.Parameters.AddWithValue( "p", token );
         if ( ( res = _db.ExecuteScalar( cmd ) ) == null )
         {
            return null;
         }
         return _playerRepository.Read( new Guid( res.ToString() ) );
      }

      public string Login( string username, string pwHash )
      {
         string playerId;
         string token;
         object res;
         NpgsqlCommand cmd;

         // Validate credentials
         cmd = new NpgsqlCommand( "SELECT player_guid FROM Player " +
            "WHERE player_name=@p1 AND player_pw=@p2;" );
         cmd.Parameters.AddWithValue( "p1", username );
         cmd.Parameters.AddWithValue( "p2", pwHash );
         if ( ( res = _db.ExecuteScalar( cmd ) ) == null )
         {
            return "";
         }

         playerId = res.ToString();

         // Check for existing token
         cmd = new NpgsqlCommand( "SELECT token_value FROM Token WHERE player_guid=@p;" );
         cmd.Parameters.AddWithValue( "p", playerId );
         if((res = _db.ExecuteScalar(cmd)) != null )
         {
            token = res.ToString();
            return token;
         }

         // Create and insert new token
         token = "Basic " + username + "-mtcgToken";
         cmd = new NpgsqlCommand( "INSERT INTO Token (token_value, player_guid)" +
            "VALUES (@p1, @p2);" );
         cmd.Parameters.AddWithValue( "p1", token );
         cmd.Parameters.AddWithValue( "p2", playerId );
         _db.ExecuteNonQuery( cmd );

         return token;
      }
   }
}
