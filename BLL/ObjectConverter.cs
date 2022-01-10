using BLL.Payloads.Incoming;
using Models.BL_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
   public static class ObjectConverter
   {
      public static List<CardPayload> ConvertToCardPl(List<Card> data )
      {
         List<CardPayload> newData = new();
         foreach(Card card in data )
         {
            newData.Add( new CardPayload( card.Guid.ToString(), card.Name, card.Damage,
               card.Type.ToString(), card.Element.ToString(), card.MonsterType.ToString() ) );
         }
         return newData;
      }
   }
}
