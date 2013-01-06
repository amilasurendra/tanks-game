using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace Tanks.AI
{
    class Play
    {
        public Move move = new Move();
        PathFind finder = new PathFind();

        Random random = new Random();
        int randomNumber;

        public void startGame()
        {
            while (true)
            {

                //Skip if not initialized
                if (Program.state.my_tank_data == null || Program.state.coins == null)
                {
                    Thread.Sleep(1000);
                    continue;
                }
                
                randomNumber = random.Next(0, 8);

                if (Program.state.tankOnSight)
                {
                    fireOnSight();
                }
                else
                {
                    switch (randomNumber)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:

                            if (Program.state.healthpacks.Count != 0 && Program.state.AIState != State.COIN_FOLLOWING)
                            {
                                followHealth();
                            }
                            break;
                            
                        default:

                            if (Program.state.coins.Count != 0 && Program.state.AIState != State.HEALTH_FOLLOWING)
                            {
                                collectCoins();
                            }
                            break;
                    }
                }
                
                Thread.Sleep(1);
            }
        }

        private void fireOnSight()
        {
            if (Program.state.currentMovementThread != null) Program.state.currentMovementThread.Abort();
            while (Program.state.tankOnSight)
            {
                if (Program.state.tankOnSightDir != Program.state.my_tank_data.direction)
                {
                    move.turnToDir(Program.state.tankOnSightDir);
                }
                move.sendData("SHOOT#");
            }
            Program.state.AIState = State.SHOOTING;
        }


        private void followHealth()
        {
            Trace.WriteLine("Following Health");
            //if (Program.state.followingHealth) return;
            if (Program.state.newHealth)
            {
                finder.startCell = new Cell(Program.state.my_tank_data.x, Program.state.my_tank_data.y);
                finder.pathFind();

                HealthData tmpHealth = Program.state.healthpacks.First.Value;

                int min = 1000;
                int tmp;

                foreach (HealthData item in Program.state.healthpacks)
                {
                    tmp = finder.getStepCount(item.x, item.y);
                    if (tmp < min)
                    {
                        min = tmp;
                        tmpHealth = item;
                    }
                }


                move.x = tmpHealth.x;
                move.y = tmpHealth.y;
                move.goToSquare();
                Program.state.newHealth = false;
                Program.state.AIState = State.HEALTH_FOLLOWING;


            }
        }

        private void collectCoins()
        {
            if (!Program.state.newCoin) return;

            List<int> distance = new List<int>();

            finder.startCell = new Cell(Program.state.my_tank_data.x, Program.state.my_tank_data.y);
            finder.pathFind();

            foreach (CoinData coin in Program.state.coins)
            {
                int tmp = finder.getStepCount(coin.x, coin.y);
                distance.Add(tmp);
            }

            if (distance.Count == 0) return;

            int min = distance.Min();

            foreach (CoinData coin in Program.state.coins)
            {
                if (min == finder.getStepCount(coin.x, coin.y))
                {
                    move.x = coin.x;
                    move.y = coin.y;
                    break;
                }
            }

            move.goToSquare();
            Program.state.newCoin = false;
            Program.state.AIState = State.COIN_FOLLOWING;
        }
    }
}
