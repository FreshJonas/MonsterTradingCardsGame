using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.BL_Models
{
   public interface ICardCollection<T>
   {
      public bool AddCard( T card );
      public bool RemoveCard( T card );
   }
}
