using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAL_Models
{
   public class DAL_Player : IUniqueElement
   {
      public Guid Guid { get; private set; }
      public string Name { get; private set; }
      public string PwHash { get; private set; }
      public int Coins { get; private set; }
      public int Elo { get; private set; }
      public int Wins { get; private set; }
      public int Losses { get; private set; }
      public string Bio { get; private set; }
      public Guid StackId { get; private set; }
      public Guid DeckId { get; private set; }

      public DAL_Player( Guid guid, string name, string pwHash, int coins, int elo, int wins, int losses, string bio, Guid stackId, Guid deckId )
      {
         Name = name;
         PwHash = pwHash;
         Coins = coins;
         Guid = guid;
         Elo = elo;
         Wins = wins;
         Losses = losses;
         Bio = bio;
         StackId = stackId;
         DeckId = deckId;
      }

   }
}
