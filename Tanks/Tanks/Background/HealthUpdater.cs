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
    class HealthUpdater
    {
        int updateperiod = int.Parse( ConfigurationSettings.AppSettings["HealthUpdatePeriod"]);

        public void startUpdateHealth(){
            Thread updateThread = new Thread(update);
            updateThread.Start();
        }


        private void update() {
            while (true) {
                Thread.Sleep(updateperiod);
                if (Program.state.healthpacks == null || Program.state.healthpacks.Count == 0) continue;

                HealthData tmp = null;
                //Trace.WriteLine("--------------------");
                foreach (HealthData healthPack in Program.state.healthpacks) {
                    tmp = null;
                    TimeSpan elapsedTime = DateTime.Now.Subtract(healthPack.addedTime);
                    
                    int elapedMillis = (int)elapsedTime.Ticks / 10000;
                    

                    if (elapedMillis > healthPack.span) {
                        tmp = healthPack;
                        //Trace.WriteLine("Condition true");
                        break;
                    }

                    //Trace.WriteLine(healthPack.x + " " + healthPack.y);
                    //Trace.WriteLine("Elaped: " + elapedMillis + " span: " + healthPack.span);

                }

                //Trace.WriteLine("--------------------");

                if (tmp != null)
                {
                    Program.state.healthpacks.Remove(tmp);
                    Program.state.map[tmp.y, tmp.x] = 0;
                    Trace.WriteLine("Method called");
                }
            }
        }

    }
}
