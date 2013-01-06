using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Tanks.Background;
using Tanks.AI;
using System.Threading;

namespace Tanks
{
    public class State
    {

        public string my_tank;
        public int playerCount = 0;
        int mapSize = Program.mapSize;
        public int score = 0;
        public string recieved { get; set; }


        public int gameState = 0; // 0-Not_Started  1-Started
        public bool initialized = false; //" S: camed"
        public bool mapInitialided = false;


        //Variables for holding players
        public TankData my_tank_data;
        public TankData[] other_tanks;
        public LinkedList<CoinData> coins = new LinkedList<CoinData>();
        public LinkedList<HealthData> healthpacks = new LinkedList<HealthData>();

        //0-none 1-brick 2-stone 3-water 4-Coin 5-Health
        public int[,] map = new int[Program.mapSize, Program.mapSize]; //map[y,x] - note the convension reversed :(|)


        //AI Constants

        public const int HEALTH_FOLLOWING = 2;
        public const int COIN_FOLLOWING =1;
        public const int SHOOTING = 3;


        //AI states
        public bool newCoin = false;
        public bool newHealth = false;
        public bool tankOnSight = false;
        public int tankOnSightDir;
        public bool tooQuickReturned = false;
        
        
        public int AIState = 0; //1-following coin 2-following health 3-shooting

        public Thread currentMovementThread;
        

        public void update(Object stateInfo)
        {

            recieved = recieved.Replace('#', '\n');

            if (recieved.StartsWith("I:"))
            { // OUR_TANK|Brick|STONE|WATER
                initialization(recieved.Substring(2));
                Program.game.gameCanvas.initializeMap();

            } if (recieved.StartsWith("S:"))
            { //Tank1|Tank2|TankN
                initPlayers(recieved.Substring(2));
                initialized = true;

            } if (recieved.StartsWith("G:"))
            { //Tanks|Bricks
                updateGame(recieved.Substring(2));
                calcTanksOnSight();
                Program.game.gameCanvas.updateTanks();
                Program.game.gameCanvas.updateChangingTiles();

            } if (recieved.StartsWith("C:"))
            {
                addCoin(recieved.Substring(2));

            } if (recieved.StartsWith("L:"))
            {
                addHealth(recieved.Substring(2));

            } if (recieved.StartsWith("TOO_QUICK"))
            {
                tooQuickReturned = true;
            }

        }

        private void initialization(string message)
        {
            string[] tmp_colon = message.Split(':');
            my_tank = tmp_colon[0];
            addTiles(tmp_colon[1], 1);
            addTiles(tmp_colon[2], 2);
            addTiles(tmp_colon[3], 3);

        }

        private void initPlayers(string message)
        {
            string[] tmp_colon = message.Split(':');
            int count = 0;

            Trace.WriteLine("InitPlayers called");

            foreach (string command in tmp_colon)
            {
                if (command.StartsWith("P")) count++;
            }
            playerCount = count - 1;

            count = 0;

            foreach (string command in tmp_colon)
            {

                string[] tmp_semi = command.Split(';');

                int x = getInt(tmp_semi[1].Split(',')[0]);
                int y = getInt(tmp_semi[1].Split(',')[1]);
                int dir = getInt(tmp_semi[2]);


                if (command.StartsWith(my_tank))
                {
                    my_tank_data = new TankData(x, y, dir);
                }
                else
                {

                    if (other_tanks == null) other_tanks = new TankData[playerCount];


                    Trace.WriteLine("Player " + tmp_semi[0] + " X:" + x + " Y:" + y + " Dir:" + dir);

                    other_tanks[count] = new TankData(x, y, dir);
                    other_tanks[count].name = tmp_semi[0];
                    count++;
                }
            }

        }

        private void updateGame(string message)
        {

            string[] tmp_colon = message.Split(':');
            int count = 0;
            if (initialized == false) return;

            foreach (string command in tmp_colon)
            {

                //For players
                if (command.StartsWith("P"))
                {

                    string[] tmp_semi = command.Split(';');

                    int x = getInt(tmp_semi[1].Split(',')[0]);
                    int y = getInt(tmp_semi[1].Split(',')[1]);
                    int dir = getInt(tmp_semi[2]);
                    int health = getInt(tmp_semi[4]);
                    int points = getInt(tmp_semi[6]);

                    if (command.StartsWith(my_tank))
                    {
                        my_tank_data.x = x;
                        my_tank_data.y = y;
                        my_tank_data.direction = dir;
                        my_tank_data.health = health;
                        score = points;
                    }
                    else
                    {

                        other_tanks[count].x = x;
                        other_tanks[count].y = y;
                        other_tanks[count].direction = dir;
                        other_tanks[count].health = health;

                        count++;
                    }

                    Program.state.map[y, x] = 0;
                    updateCoinHealth(x, y);
                }
            }


            //For Bricks (Update Damage)
            int cmdCount = tmp_colon.Length - 1;
            string[] tmp_semi_colon = tmp_colon[cmdCount].Split(';');

            foreach (string dataItem in tmp_semi_colon)
            {
                string[] tmp = dataItem.Split(',');
                int x = getInt(tmp[0]);
                int y = getInt(tmp[1]);
                int damage = getInt(tmp[2]);
                if (damage == 4)
                {
                    map[y, x] = 0;
                }
            }
        }

        private void updateCoinHealth(int x, int y) {

            HealthData tmp = null;

            foreach (HealthData health in Program.state.healthpacks) {
                if (health.x == x && health.y == y) {
                    tmp = health;
                    break;
                }
            }

            Program.state.healthpacks.Remove(tmp);

            CoinData tmpCoin = null;
            foreach (CoinData coin in Program.state.coins)
            {
                if (coin.x == x && coin.y == y)
                {
                    tmpCoin = coin;
                    break;
                }
            }
            Program.state.coins.Remove(tmpCoin);
            
         }

        private void addTiles(string message, int type)
        {
            string[] tmp_semi = message.Split(';');

            foreach (string coord in tmp_semi)
            {
                string[] tmp = coord.Split(',');
                int x = getInt(tmp[0]);
                int y = getInt(tmp[1]);

                map[y, x] = type;
            }

        }

        private void addCoin(string message)
        {
            string[] tmp_colon = message.Split(':');

            string[] coordinate = tmp_colon[0].Split(',');
            int x = getInt(coordinate[0]);
            int y = getInt(coordinate[1]);
            int span = getInt(tmp_colon[1]);

            coins.AddFirst(new CoinData(x, y, span));
            map[y, x] = 4;
            newCoin = true;
        }

        private void addHealth(string message)
        {
            string[] tmp_colon = message.Split(':');

            string[] coordinate = tmp_colon[0].Split(',');
            int x = getInt(coordinate[0]);
            int y = getInt(coordinate[1]);
            int span = getInt(tmp_colon[1]);

            healthpacks.AddFirst(new HealthData(x, y, span));
            map[y, x] = 5;
            newHealth = true;

        }

        private int getInt(string x)
        {
            int number = 0;
            try
            {
                number = int.Parse(Regex.Replace(x, "[a-zA-Z]*", ""));
            }
            catch (Exception e) { e.Source = x; Trace.WriteLine("Error: " + x); }

            return number;
        }

        public void printArray()
        {
            int i, j = 0;

            for (i = 0; i < Program.mapSize; i++)
            {

                for (j = 0; j < Program.mapSize; j++)
                {
                    Trace.Write(map[i, j] + "   ");
                }
                Trace.WriteLine("");
            }

        }

        private void calcTanksOnSight()
        {
            if (playerCount == 0) return;
            tankOnSight = false;

            foreach (TankData tank in other_tanks)
            {

                bool tmp = true;

                if (tank.x != my_tank_data.x && tank.y != my_tank_data.y)
                {
                    tmp = false;
                    continue;
                }

                if (tank.health == 0)
                {
                    tmp = false;
                    continue;
                }

                if (tank.x == my_tank_data.x)
                {

                    if (my_tank_data.y < tank.y)
                    {
                        tankOnSightDir = Move.DOWN;
                        for (int i = my_tank_data.y; i <= tank.y; i++)
                        {
                            if (map[i, tank.x] == 1 || map[i, tank.x] == 2)
                            {
                                tmp = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        tankOnSightDir = Move.UP;
                        for (int i = tank.y; i <= my_tank_data.y; i++)
                        {
                            if (map[i, tank.x] == 1 || map[i, tank.x] == 2)
                            {
                                tmp = false;
                                break;
                            }
                        }
                    }
                }
                else if (tank.y == my_tank_data.y)
                {
                    if (my_tank_data.x < tank.x)
                    {
                        tankOnSightDir = Move.RIGHT;
                        for (int i = my_tank_data.x; i <= tank.x; i++)
                        {
                            if (map[tank.y, i] == 1 || map[tank.y, i] == 2)
                            {
                                tmp = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        tankOnSightDir = Move.LEFT;
                        for (int i = tank.y; i <= my_tank_data.y; i++)
                        {
                            if (map[tank.y, i] == 1 || map[tank.y, i] == 2)
                            {
                                tmp = false;
                                break;
                            }

                        }
                    }
                }
                if (tmp)
                {
                    tankOnSight = true;
                    return;
                }
            }
        }

    }


}
