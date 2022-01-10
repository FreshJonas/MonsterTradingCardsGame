using BLL.Payloads.Incoming;
using DAL.Repositories;
using Models.BL_Models;
using Models.BLL_Models.Cards;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BLL.Controller
{
   class PackagesController : IController<HttpResponse>
   {
      private readonly PackageRepository _packageRepository;
      private readonly AuthRepository _authRepository;

      public PackagesController( PackageRepository packageRepository, AuthRepository authRepository )
      {
         _packageRepository = packageRepository;
         _authRepository = authRepository;
      }

      public HttpResponse Post( string token, StreamReader reader )
      {
         Package newPackage;

         // Validate token
         if ( token == null || _authRepository.GetPlayer( token ) == null )
         {
            return new HttpResponse( 401 );
         }

         // Deserialize Payload
         PackagePayload packagePl = JsonSerializer.Deserialize<PackagePayload>( ReadAsString( reader ) );

         // Create Package
         newPackage = new Package( Guid.NewGuid(), packagePl.Name);
         foreach ( CardPayload cardPl in packagePl.Cards )
         {
            newPackage.Cards.Add( new Card( Guid.NewGuid(), cardPl.Name, cardPl.Damage,
               Enums.StringToCardType( cardPl.Type ), Enums.StringToCardElement( cardPl.Element ),
               Enums.StringToMonsterType( cardPl.MonsterType ) ) );
         }

         // Add to DB
         if(!_packageRepository.Create( newPackage ) )
         {
            return new HttpResponse( 401 );
         }

         return new HttpResponse( 202 );
      }

      public HttpResponse Delete( string token, StreamReader reader )
      {
         return new HttpResponse( 400 );
      }

      public HttpResponse Get( string token, StreamReader reader )
      {
         return new HttpResponse( 400 );
      }

      public HttpResponse Put( string token, StreamReader reader )
      {
         return new HttpResponse( 400 );
      }
      private string ReadAsString( StreamReader reader )
      {
         string str = "";
         while ( reader.Peek() >= 0 )
         {
            str += (char)reader.Read();
         }
         return str;
      }
   }
}
