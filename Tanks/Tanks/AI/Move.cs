using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Timers;
using Tanks;

namespace Tanks.AI
{
    public class Move
    {

        public int x;
        public int y;

        public const int UP = 0;
        public const int RIGHT = 1;
        public const int DOWN = 2;
        public const int LEFT = 3;

        private static int milliSec = 0;
        private static System.Timers.Timer timer;

        PathFind test = new PathFind();

        public Move()
        {
            timer = new System.Timers.Timer(200);
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            milliSec += 200;
            if (milliSec > 10000) milliSec = 10000;
        }

        

        //Go to a adjacent cell and adjust state variables (in case of updates missed)
        public void gotoAdjacent(int x, int y) {
            int curr_x = Program.state.my_tank_data.x;
            int curr_y = Program.state.my_tank_data.y;
            int curr_dir = Program.state.my_tank_data.direction;

            if (x > curr_x) 
            {

                sendData("RIGHT#");
                if (curr_dir != 1) Program.state.my_tank_data.direction = 1;
                else Program.state.my_tank_data.x++;

            }else if(x < curr_x) 
            {
                sendData("LEFT#");
                if (curr_dir != 3) Program.state.my_tank_data.direction = 3;
                else Program.state.my_tank_data.x--;
            }else if (y > curr_y)       
            {
                sendData("DOWN#");
                if (curr_dir != 2) Program.state.my_tank_data.direction = 2;
                else Program.state.my_tank_data.y++;
            } else if (y < curr_y)   
            {
                sendData("UP#");
                if (curr_dir != 0) Program.state.my_tank_data.direction = 0;
                else Program.state.my_tank_data.y--;
            }

        }
        
        public void goToSquare(){
            if (Program.state.currentMovementThread != null) {
                Program.state.currentMovementThread.Abort(); 
            }
            Program.state.currentMovementThread = new Thread(go);
            Program.state.currentMovementThread.Start();
        }

        private void go() {
            if (Program.state.my_tank_data == null) return;
            
            test.startCell = new Cell(Program.state.my_tank_data.x, Program.state.my_tank_data.y);
            test.endCell = new Cell(x, y);
            test.pathFind();
            test.HighlightPath();
            Trace.WriteLine("Path Found");

            List<Cell> path = test.getpath();
            if (path.Count == 0) {
                Trace.WriteLine("Path Length is Zero");

                return; 
            }

            foreach (Cell node in path)
            {
                gotoAdjacent(node.x, node.y);
                if (Program.state.my_tank_data.x != node.x || Program.state.my_tank_data.y != node.y)
                {
                    gotoAdjacent(node.x, node.y);
                    //Trace.WriteLine(node.x + ","+ node.y);
                }
            }

            if (Program.state.AIState == State.COIN_FOLLOWING) {
                Program.state.newHealth = true;
                Program.state.AIState = State.HEALTH_FOLLOWING;
            }
            if (Program.state.AIState == State.HEALTH_FOLLOWING) {
                Program.state.newCoin = true;
                Program.state.AIState = State.COIN_FOLLOWING;
            }
        }


        public void turnToDir(int dir) {
            string data = "";

            switch (dir) { 
                case UP: data="UP#"; break;
                case RIGHT: data = "RIGHT#"; break;
                case DOWN: data = "DOWN#"; break;
                case LEFT: data = "LEFT#"; break;
            }
            sendData(data);
        }


        public void sendData(string data) {
            while (milliSec <= 1000) {
                if(data.Equals("SHOOT#"))Thread.Sleep(200);
                else Thread.Sleep(300);
            }

            Program.connection.sendData(data);
            milliSec = 0;
        }
    }
}
    