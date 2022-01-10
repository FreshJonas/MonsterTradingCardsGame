using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Models.BL_Models
{
   public enum CardType
   {
      Monster,
      Spell,
      Undefined
   }

   public enum CardElement
   {
      Fire,
      Water,
      Normal,
      Ice,
      Wind,
      Undefined
   }

   public enum MonsterType
   {
      Goblin,
      Dragon,
      Wizzard,
      Ork,
      Knight,
      Kraken,
      Elve,
      Undefined
   }

   public static class Enums
   {
      public static CardType StringToCardType( string str )
      {
         if ( ( String.Compare( "Monster", str, StringComparison.OrdinalIgnoreCase ) == 0 ) )
            return CardType.Monster;
         if ( ( String.Compare( "Spell", str, StringComparison.OrdinalIgnoreCase ) == 0 ) )
            return CardType.Spell;
         return CardType.Undefined;
      }

      public static CardElement StringToCardElement( string str )
      {
         if ( ( String.Compare( "Fire", str, StringComparison.OrdinalIgnoreCase ) == 0 ) )
            return CardElement.Fire;
         if ( ( String.Compare( "Water", str, StringComparison.OrdinalIgnoreCase ) == 0 ) )
            return CardElement.Water;
         if ( ( String.Compare( "Normal", str, StringComparison.OrdinalIgnoreCase ) == 0 ) )
            return CardElement.Normal;
         if ( ( String.Compare( "Ice", str, StringComparison.OrdinalIgnoreCase ) == 0 ) )
            return CardElement.Ice;
         if ( ( String.Compare( "Wind", str, StringComparison.OrdinalIgnoreCase ) == 0 ) )
            return CardElement.Wind;
         return CardElement.Undefined;
      }

      public static MonsterType StringToMonsterType( string str )
      {
         if ((String.Compare("Goblin", str, StringComparison.OrdinalIgnoreCase) == 0 ) )
            return MonsterType.Goblin;
         if ( ( String.Compare( "Dragon", str, StringComparison.OrdinalIgnoreCase ) == 0 ) )
            return MonsterType.Dragon;
         if ( ( String.Compare( "Wizzard", str, StringComparison.OrdinalIgnoreCase ) == 0 ) )
            return MonsterType.Wizzard;
         if ( ( String.Compare( "Ork", str, StringComparison.OrdinalIgnoreCase ) == 0 ) )
            return MonsterType.Ork;
         if ( ( String.Compare( "Knight", str, StringComparison.OrdinalIgnoreCase ) == 0 ) )
            return MonsterType.Knight;
         if ( ( String.Compare( "Kraken", str, StringComparison.OrdinalIgnoreCase ) == 0 ) )
            return MonsterType.Kraken;
         if ( ( String.Compare( "Elve", str, StringComparison.OrdinalIgnoreCase ) == 0 ) )
            return MonsterType.Elve;
         return MonsterType.Undefined;
      }

   }
}
