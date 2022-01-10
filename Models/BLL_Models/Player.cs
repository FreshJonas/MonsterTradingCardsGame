using Models.BL_Models.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAL_Models
{
   public class Player
   {
      public Guid Guid { get; init; }
      public string Name { get; private set; }
      public string PwHash { get; private set; }
      public int Coins { get; private set; }
      public int Elo { get; private set; }
      public int Wins { get; private set; }
      public int Losses { get; private set; }
      public string Bio { get; private set; }
      public CardStack Stack { get; init; }
      public Deck Deck { get; init; }

      public Player( Guid guid, string name, string pwHash, CardStack stack, Deck deck )
      {
         Guid = guid;
         Name = name;
         PwHash = pwHash;
         Coins = 20; ;
         Elo = 100;
         Wins = 0;
         Losses = 0;
         Bio = "";
         Stack = stack;
         Deck = deck;
      }

      public Player( Guid guid, string name, string pwHash, int coins, int elo, int wins, int losses, string bio, CardStack stack, Deck deck )
      {
         Guid = guid;
         Name = name;
         PwHash = pwHash;
         Coins = coins;
         Elo = elo;
         Stack = stack;
         Deck = deck;
         Wins = wins;
         Losses = losses;
         Bio = bio;
      }

      public bool Pay(int amount )
      {
         if ( ( Coins - amount ) < 0 ) return false;
         Coins -= amount;
         return true;
      }

      public bool ChangeBio(string newBio )
      {
         if ( newBio.Length > 255 ) return false;
         Bio = newBio;
         return true;
      }

      public void AddWin()
      {
         Wins++;
         Elo += 3;
      }
      public void AddLoss()
      {
         Losses++;
         Elo -= 5;
      }
   }
}
