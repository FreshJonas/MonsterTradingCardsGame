using Models.BL_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.BLL_Models.Cards
{
   public class Package : ICardCollection<Card>
   {
      public Guid Guid { get; private set; }
      public List<Card> Cards { get; private set; }
      public string Name { get; private set; }

      public Package( Guid guid, string name )
      {
         Guid = guid;
         Cards = new();
         Name = name;
      }

      public bool AddCard( Card card )
      {
         Cards.Add( card );
         return true;
      }

      public bool RemoveCard( Card card )
      {
         return Cards.Remove( card );
      }

   }
}
