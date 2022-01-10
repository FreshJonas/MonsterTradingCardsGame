using DAL;
using PL;
using DAL.Repositories;
using Models.BL_Models.Cards;
using Models.DAL_Models;
using System;
using BLL;
using System.Net.Http;
using BLL.Controller;

namespace MonsterTradingCardsGame
{
   class Program
   {
      static void Main( string[] args )
      {
         // Create DB
         Database db = new Database();

         // Create Repos
         PlayerRepository playerRepository = new PlayerRepository( db );
         PackageRepository packageRepository = new PackageRepository( db );
         AuthRepository authRepository = new AuthRepository( db, playerRepository );

         // Create Controller
         UsersController usersController = new( playerRepository );
         SessionsController sessionsController = new( authRepository );
         PackagesController packagesController = new( packageRepository, authRepository );
         TransactionPackagesController transPackagesCtrlr = new( authRepository, packageRepository, playerRepository );
         StacksController stacksController = new( authRepository );
         DecksController decksController = new( authRepository, playerRepository );
         UsersProfileController usersProfileController = new( authRepository, playerRepository );
         BattlesController battlesController = new( authRepository, playerRepository );
         StatsController statsController = new( authRepository );
         ScoreController scoreController = new( playerRepository );

         // Add to MainController
         MainController mainController = new();
         mainController.Controller.Add( "/users", usersController );
         mainController.Controller.Add( "/sessions", sessionsController );
         mainController.Controller.Add( "/packages", packagesController );
         mainController.Controller.Add( "/transactions/packages", transPackagesCtrlr );
         mainController.Controller.Add( "/stacks", stacksController );
         mainController.Controller.Add( "/decks", decksController );
         mainController.Controller.Add( "/users/profile", usersProfileController );
         mainController.Controller.Add( "/battles", battlesController );
         mainController.Controller.Add( "/stats", statsController );
         mainController.Controller.Add( "/score", scoreController );

         // HTTP Server
         HttpServer httpServer = new HttpServer( 10001, mainController );
         httpServer.Run();
      }

   }
}

