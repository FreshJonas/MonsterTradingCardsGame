using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Payloads
{
   class StatsPayload
   {
      public int Elo { get; private set; }
      public int Wins { get; private set; }
      public int Losses { get; private set; }

      public StatsPayload(int elo, int wins, int losses )
      {
         Elo = elo;
         Wins = wins;
         Losses = losses;
      }
   }
}
