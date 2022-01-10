using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Payloads.Outgoing
{
   public class TokenPayload
   {
      public string AuthorizationToken { get; private set; }

      public TokenPayload(string token)
      {
         AuthorizationToken = token;
      }
   }
}
