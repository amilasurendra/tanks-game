using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tanks.AI
{
    public class TankData
    {
        public int x { get; set; }
        public int y { get; set; }
        public int direction { get; set; }
        public int health { get; set; }
        public string name { get; set; }


        public TankData(int x, int y, int dir)
        {
            this.x = x;
            this.y = y;
            this.direction = dir;
        }

    }
}
