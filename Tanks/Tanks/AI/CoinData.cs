using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tanks.AI
{
    public class CoinData
    {
        public int x;
        public int y;
        public DateTime addedTime;
        public int span;
        public DateTime remTime;

        public CoinData(int x, int y, int span)
        {
            this.x = x;
            this.y = y;
            this.span = span;
            this.addedTime = DateTime.Now;
        }
    }
}