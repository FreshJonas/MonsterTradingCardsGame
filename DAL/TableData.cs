using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   public static class TableData
   {
      public static List<OrderedDictionary> GetDummyData()
      {
         List<OrderedDictionary> data = new();
         for ( int i = 0; i < 5; i++ )
         {
            OrderedDictionary row = new();
            row.Add( "user_guid", $"someLongGuid{i}" );
            row.Add( "user_username", $"someUsername{i}" );
            row.Add( "user_pw", $"somePw{i}" );
            row.Add( "user_coins", $"someCoins{i}" );
            data.Add( row );
         }
         return data;
      }


      public static void PrintTableData( List<OrderedDictionary> data )
      {
         int rowCnt = 0;
         foreach ( OrderedDictionary row in data )
         {
            Console.Write( $"{rowCnt} : " );
            foreach ( DictionaryEntry entry in row )
            {
               Console.Write( $" : {entry.Key} : {entry.Value} : " );
            }
            Console.WriteLine();
            rowCnt++;
         }
      }
   }
}
