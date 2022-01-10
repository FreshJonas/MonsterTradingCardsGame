using Models.BL_Models;
using Models.BLL_Models.Cards;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
   public class PackageRepository : IRepository<Package>
   {
      private Database _db;

      public PackageRepository( Database db )
      {
         _db = db;
      }

      public bool Create( Package package )
      {
         NpgsqlCommand cmd;

         // Create package
         cmd = new NpgsqlCommand( "INSERT INTO Package " +
            "(package_guid, package_name)" +
            "VALUES (@p1, @p2);" );
         cmd.Parameters.AddWithValue( "p1", package.Guid );
         cmd.Parameters.AddWithValue( "p2", package.Name );
         _db.ExecuteNonQuery( cmd );

         // Add cards to card table
         foreach ( Card card in package.Cards )
         {
            cmd = new NpgsqlCommand( "INSERT INTO Card " +
               "(card_guid, card_name, card_damage, card_type, card_element, card_monsterType)" +
               "VALUES(@p1, @p2, @p3, @p4, @p5, @p6); " );
            cmd.Parameters.AddWithValue( "p1", card.Guid );
            cmd.Parameters.AddWithValue( "p2", card.Name );
            cmd.Parameters.AddWithValue( "p3", card.Damage );
            cmd.Parameters.AddWithValue( "p4", card.Type );
            cmd.Parameters.AddWithValue( "p5", card.Element );
            cmd.Parameters.AddWithValue( "p6", card.MonsterType );
            _db.ExecuteNonQuery( cmd );
         }

         // Add cards to package table
         cmd = new();
         foreach ( Card card in package.Cards )
         {
            cmd.CommandText += $"INSERT INTO CardPackage (package_guid, card_guid) " +
               $"VALUES (\'{package.Guid}\', \'{card.Guid}\');\n";
         }
         if ( cmd.CommandText != "" )
         {
            _db.ExecuteNonQuery( cmd );
         }
         return true;
      }

      public bool Delete( Guid guid )
      {
         NpgsqlCommand cmd;

         // Delete cards from package
         cmd = new();
         cmd.CommandText = $"DELETE FROM CardPackage WHERE package_guid=\'{guid}\';\n";
         cmd.CommandText += $"DELETE FROM Package WHERE package_guid=\'{guid}\';";
         _db.ExecuteNonQuery( cmd );

         return true;
      }

      public Package Read( Guid guid )
      {
         OrderedDictionary row;
         Package package;
         NpgsqlCommand cmd;

         // Get Package
         cmd = new NpgsqlCommand( $"SELECT * FROM Package WHERE package_guid=\'{guid}\';" );
         row = _db.GetRow( cmd );
         package = DataRowToPackage( row );

         return GetCards( package );
      }

      public Package ReadByName( string name )
      {
         OrderedDictionary row;
         Package package;
         NpgsqlCommand cmd;

         // Get Package
         cmd = new NpgsqlCommand( $"SELECT * FROM Package WHERE package_name=@p;" );
         cmd.Parameters.AddWithValue( "p", name );
         row = _db.GetRow( cmd );
         package = DataRowToPackage( row );

         return GetCards( package );
      }

      public List<Guid> ReadAllGuids()
      {
         List<Guid> list = new();
         List<OrderedDictionary> data;
         var cmd = new NpgsqlCommand( "SELECT package_guid FROM Package;" );
         data = _db.ExecuteQuery( cmd );
         foreach ( OrderedDictionary row in data )
         {
            list.Add( new Guid( (string)row["package_guid"] ) );
         }

         return list;
      }

      public bool Update( Package data )
      {
         throw new NotImplementedException();
      }

      private Package GetCards( Package package )
      {
         List<OrderedDictionary> data;
         NpgsqlCommand cmd;
         Package newPackage = package;

         // Get Cards
         cmd = new NpgsqlCommand( $"SELECT " +
            $"Card.card_guid, Card.card_name, Card.card_damage, Card.card_type, Card.card_element, Card.card_monsterType " +
            $"FROM Card " +
            $"INNER JOIN CardPackage ON Card.card_guid = CardPackage.card_guid " +
            $"WHERE CardPackage.package_guid = \'{newPackage.Guid}\';" );
         data = _db.ExecuteQuery( cmd );
         foreach ( OrderedDictionary cardRow in data )
         {
            newPackage.AddCard( DataRowToCard( cardRow ) );
         }

         return newPackage;
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

      private Package DataRowToPackage( OrderedDictionary row )
      {
         Guid guid = new( (string)row["package_guid"] );
         string name = (string)row["package_name"];

         return new Package( guid, name );
      }
   }
}
