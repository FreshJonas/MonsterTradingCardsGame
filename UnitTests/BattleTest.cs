using Models.BL_Models.Cards;
using Models.DAL_Models;
using NUnit.Framework;
using System;

namespace UnitTests
{
   [TestFixture]

   public class PlayerTest
   {
      private CardStack stackA;
      private Deck deckA;
      private Player playerA;
      private CardStack stackB;
      private Deck deckB;
      private Player playerB;

      [SetUp]
      public void Setup()
      {
         stackA = new(Guid.NewGuid());
         deckA = new( Guid.NewGuid() );
         playerA = new Player( Guid.NewGuid(), "TestPlayerA", "pwHash", stackA, deckA );
         stackB = new( Guid.NewGuid() );
         deckB = new( Guid.NewGuid() );
         playerB = new Player( Guid.NewGuid(), "TestPlayerB", "pwHash", stackB, deckB );
      }

      [Test]
      public void PlayerCoinsInitValue()
      {
         var actualCoins = playerA.Coins;
         Assert.AreEqual( 20, actualCoins );
      }
   }
}