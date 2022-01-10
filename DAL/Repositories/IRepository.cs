using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   public interface IRepository<T>
   {
      public bool Create( T data);
      public T Read( Guid guid );
      //public List<T> ReadAll();
      public bool Update( T data );
      public bool Delete( Guid guid );
   }
}
