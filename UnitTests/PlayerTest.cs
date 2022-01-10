using Models.BL_Models.Cards;
using Models.DAL_Models;
using NUnit.Framework;
using System;

namespace UnitTests
{
   [TestFixture]

   public class BattleTest
   {
      private CardStack stackA;
      private Deck deckA;
      private Player playerA;

      [SetUp]
      public void Setup()
      {
         stackA = new(Guid.NewGuid());
         deckA = new( Guid.NewGuid() );
         playerA = new Player( Guid.NewGuid(), "TestPlayerA", "pwHash", stackA, deckA );
      }

      [Test]
      public void PlayerCoinsInitValue()
      {
         var actualCoins = playerA.Coins;
         Assert.AreEqual( 20, actualCoins );
      }

      [Test]
      public void PlayerEloInitValue()
      {
         var actualElo = playerA.Elo;
         Assert.AreEqual(100, actualElo);
      }

      [Test]
      public void PlayerBioInitValue()
      {
         var actualBio = playerA.Bio;
         Assert.AreEqual( "", actualBio );
      }

      [Test]
      public void PlayerAddWinElo()
      {
         Player player = new( Guid.NewGuid(), "", "", stackA, deckA );
         player.AddWin();
         var actualElo = player.Elo;
         Assert.AreEqual( 100+3, actualElo );
      }

      [Test]
      public void PlayerAddWinWinsCount()
      {
         Player player = new( Guid.NewGuid(), "", "", stackA, deckA );
         player.AddWin();
         var actualWins = player.Wins;
         Assert.AreEqual( 1, actualWins );
      }

      [Test]
      public void PlayerAddLossElo()
      {
         Player player = new( Guid.NewGuid(), "", "", stackA, deckA );
         player.AddLoss();
         var actualElo = player.Elo;
         Assert.AreEqual( 100 - 5, actualElo );
      }

      [Test]
      public void PlayerAddLossLossesCount()
      {
         Player player = new( Guid.NewGuid(), "", "", stackA, deckA );
         player.AddLoss();
         var actualLosses = player.Losses;
         Assert.AreEqual( 1, actualLosses );
      }
   }
}