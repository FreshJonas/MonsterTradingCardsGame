using Models.DAL_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Battle
{
   public class BattleResult
   {
      public List<string> Log { get; private set; }
      public Player Winner { get; private set; }
      public Player Looser { get; private set; }

      public BattleResult(List<string> log, Player winner, Player looser )
      {
         Log = log;
         Winner = winner;
         Looser = looser;
      }
   }
}
