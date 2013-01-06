using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Tanks.Background;
using System.Configuration;
using Tanks.AI;
using System.Diagnostics;


namespace Tanks.Background
{
    class CoinUpdater
    {

        int updateperiod = int.Parse(ConfigurationSettings.AppSettings["CoinUpdatePeriod"]);

        public void startUpdateCoins()
        {
            Thread updateThread = new Thread(update);
            updateThread.Start();
        }


        private void update()
        {
            while (true)
            {
                Thread.Sleep(updateperiod);
                if (Program.state.coins == null || Program.state.coins.Count == 0) continue;

                CoinData tmp = null;

                foreach (CoinData coin in Program.state.coins)
                {
                    tmp = null;
                    TimeSpan elapsedTime = DateTime.Now.Subtract(coin.addedTime);
                    int elapedMillis = (int)elapsedTime.Ticks / 10000;
                    //Trace.WriteLine("Elapsed: " + elapedMillis + " span: " + coin.span);

                    if (elapedMillis > coin.span)
                    {
                        tmp = coin;
                        break;
                    }

                    

                }
                if (tmp != null)
                {
                    Program.state.coins.Remove(tmp);
                    Program.state.map[tmp.y, tmp.x] = 0;
                }
            }
        }




    }
}
