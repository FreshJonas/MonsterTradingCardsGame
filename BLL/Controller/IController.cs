using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
   public interface IController<T>
   {
      // Returns Response
      public T Get( string token, StreamReader reader );
      public T Post( string token, StreamReader reader );
      public T Put( string token, StreamReader reader );
      public T Delete( string token, StreamReader reader );
   }
}
