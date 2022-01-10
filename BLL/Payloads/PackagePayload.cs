using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Payloads.Incoming
{
   class PackagePayload
   {
      public string Name { get; set; }
      public IList<CardPayload> Cards { get; set; }
   }
}
