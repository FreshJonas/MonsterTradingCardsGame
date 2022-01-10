using BLL.Payloads.Incoming;
using DAL;
using DAL.Repositories;
using Models.BL_Models;
using Models.BLL_Models.Cards;
using Models.DAL_Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BLL.Controller
{
   public class TransactionPackagesController : IController<HttpResponse>
   {
      private readonly AuthRepository _authRepository;
      private readonly PackageRepository _packageRepository;
      private readonly PlayerRepository _playerRepository;

      public TransactionPackagesController( 
         AuthRepository authRepo, PackageRepository packageRepo, PlayerRepository playerRepo )
      {
         _authRepository = authRepo;
         _packageRepository = packageRepo;
         _playerRepository = playerRepo;
      }

      public HttpResponse Post( string token, StreamReader reader )
      {
         string jsonPayload;
         Player player;
         Package package;
         Random rnd = new Random();
         int index;

         // Validate token
         if ( token == null || ( player = _authRepository.GetPlayer( token ) ) == null )
         {
            return new HttpResponse( 401 );
         }

         // Check funding
         if ( !player.Pay(5) )
         {
            return new HttpResponse( 401 );
         }

         // Get package list
         List<Guid> packageIds = _packageRepository.ReadAllGuids();
         if(packageIds.Count <= 0 )
         {
            return new HttpResponse( 401 );
         }

         // Aquire random package
         index = rnd.Next( packageIds.Count );
         package = _packageRepository.Read( packageIds[index] );
         _packageRepository.Delete( package.Guid );

         // Add to Player stack
         foreach(Card card in package.Cards )
         {
            player.Stack.AddCard( card );
         }
         _playerRepository.Update( player );

         // Send to Client
         jsonPayload = JsonSerializer.Serialize<Package>( package );
         return new HttpResponse( 202, jsonPayload );
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
   }
}
