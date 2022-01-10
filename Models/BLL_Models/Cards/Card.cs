using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.BL_Models
{
   // TODO
   // ICard Interface
   public class Card : IUniqueElement, ICard, IEquatable<Card>
   {
      public Guid Guid { get; protected set; }
      public string Name { get; protected set; }
      public int Damage { get; protected set; }
      public CardType Type { get; protected set; }
      public CardElement Element { get; protected set; }
      public MonsterType MonsterType { get; protected set; }

      // Creates a spell card
      public Card(Guid guid, string name, int damage, CardElement element )
      {
         Guid = guid;
         Name = name;
         Damage = damage;
         Element = element;
         Type = CardType.Spell;
         MonsterType = MonsterType.Undefined;
      }

      // Creates a monster card
      public Card( Guid guid, string name, int damage, CardElement element, MonsterType monsterType )
      {
         Guid = guid;
         Name = name;
         Damage = damage;
         Element = element;
         Type = CardType.Monster;
         MonsterType = monsterType;
      }

      // Creates any card
      public Card( Guid guid, string name, int damage, CardType type, CardElement element, MonsterType monsterType )
      {
         Guid = guid;
         Name = name;
         Damage = damage;
         Element = element;
         Type = type;
         MonsterType = monsterType;
      }

      public void SomeCardFunction()
      {
         throw new NotImplementedException();
      }

      public bool Equals( Card other )
      {
         if ( Guid == other.Guid && Name == other.Name && Damage == other.Damage
            && Type == other.Type && Element == other.Element && MonsterType == other.MonsterType )
         {
            return true;
         }
         else
         {
            return false;
         }
      }
   }
}
