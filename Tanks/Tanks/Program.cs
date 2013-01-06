using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.Xna.Framework;
using Tanks.Background;
using System.Configuration;

namespace Tanks
{
#if WINDOWS || XBOX
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        public const int mapSize = 20;
        public const int iconSize = 32;


        public static TankGui game = new TankGui(640,640);
        public static State state = new State();
        public static Connection connection = new Connection();

        

        static void Main(string[] args)
        {


            Thread recieveThread;

            connection.server = ConfigurationSettings.AppSettings["serverIP"];
            connection.serverPort = 6000;
            connection.clientPort = 7000;
            connection.init();

            recieveThread = new Thread(connection.receiveData);
            recieveThread.Priority = ThreadPriority.Highest;
            recieveThread.Start();

            //Start Healthpack Update Thread
            HealthUpdater healthupdater = new HealthUpdater();
            healthupdater.startUpdateHealth();

            //Start Coin Update Thread
            CoinUpdater coinUpdater = new CoinUpdater();
            coinUpdater.startUpdateCoins();

            //initialize Map
            for (int i = 0; i < mapSize; i++) {
                for (int j = 0; j < mapSize; j++) { 
                    state.map[i,j] = 0;
                }
            }

                game.Run();
            
            

        }
    }
#endif
}

