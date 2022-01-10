using Models.BL_Models;
using Npgsql;
using Npgsql.NameTranslation;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   public class Database
   {
      public NpgsqlConnection Conn { get; private set; }
      private readonly Object connectionLock = new();

      // TODO
      // Write in settings file
      public string _connString = "Host=localhost;Username=postgres;Password=test123;" +
         "Database=mctg;Include Error Detail=true;";

      public Database()
      {
         Conn = new NpgsqlConnection( _connString );
         Conn.Open();

         // Map Enums
         Conn.TypeMapper.MapEnum<CardType>( "cardtype", new NpgsqlNullNameTranslator() );
         Conn.TypeMapper.MapEnum<CardElement>( "element", new NpgsqlNullNameTranslator() );
         Conn.TypeMapper.MapEnum<MonsterType>( "monstertype", new NpgsqlNullNameTranslator() );
      }

      public bool ExecuteNonQuery( NpgsqlCommand cmd )
      {
         int res = 0;

         lock ( connectionLock )
         {
            cmd.Connection = Conn;
            res = cmd.ExecuteNonQuery();
         }

         if ( res == -1 )
         {
            // Error
            return false;
         }
         return true;
      }

      public Object ExecuteScalar( NpgsqlCommand cmd )
      {
         lock ( connectionLock )
         {
            cmd.Connection = Conn;
            return cmd.ExecuteScalar();
         }
      }

      public List<OrderedDictionary> ExecuteQuery( NpgsqlCommand cmd )
      {
         List<OrderedDictionary> data = new();
         NpgsqlDataReader reader;

         lock ( connectionLock )
         {
            cmd.Connection = Conn;

            using ( reader = cmd.ExecuteReader() )
            {
               while ( reader.Read() )
               {
                  OrderedDictionary row = new();
                  for ( int i = 0; i < reader.FieldCount; i++ )
                  {
                     row.Add( reader.GetName( i ), reader[i] );
                  }
                  data.Add( row );
               }
            }
         }
         return data;
      }

      public OrderedDictionary GetRow(NpgsqlCommand cmd )
      {
         OrderedDictionary row = new();
         NpgsqlDataReader reader;

         lock ( connectionLock )
         {
            cmd.Connection = Conn;
            using ( reader = cmd.ExecuteReader() )
            {
               reader.Read();
               for ( int i = 0; i < reader.FieldCount; i++ )
               {
                  row.Add( reader.GetName( i ), reader[i] );
               }
            }
         }
         return row;
      }
   }
}
