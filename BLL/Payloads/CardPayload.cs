using Models.BL_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Payloads.Incoming
{
   public class CardPayload
   {
      public string Guid { get; set; }
      public string Name { get; set; }
      public int Damage { get; set; }
      public string Type { get; set; }
      public string Element { get; set; }
      public string MonsterType { get; set; }

      public CardPayload( string guid, string name, int damage, string type, string element, string monstertype )
      {
         Guid = guid;
         Name = name;
         Damage = damage;
         Type = type;
         Element = element;
         MonsterType = monstertype;
      }

   }
}
