using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Payloads
{
   public class UserProfilePayload 
   {
      public string Bio { get; set; }
      public UserProfilePayload(string bio )
      {
         Bio = bio;
      }
   }
}
