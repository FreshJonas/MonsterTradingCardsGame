using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.BL_Models.Cards
{
   public class Deck : ICardCollection<Card>, IUniqueElement
   {
      public Guid Guid { get; private set; }
      public List<Card> Cards { get; private set; }

      public Deck(Guid guid)
      {
         Guid = guid;
         Cards = new();
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
