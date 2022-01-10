using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Battle
{
   public class BattleSubscriber : ISubscriber<BattleResult>
   {
      public bool BattleFinished { get; private set; }
      public BattleResult BattleResult { get; private set; }

      public BattleSubscriber()
      {
         BattleFinished = false;
         BattleResult = null;
      }

      public void Update( BattleResult subject )
      {
         BattleFinished = true;
         BattleResult = subject;
      }
   }
}
