using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;



namespace Tanks.AI
{
    enum type
    {
        empty,
        blocked,
        target,
        start
    }

    class PathFind
    {


        const int mapSize = Program.mapSize;
        List<Cell> list;
        public Cell startCell;
        public Cell endCell;



        // Movements is an array of various directions.
        Cell[] movements = {
	        new Cell(0, -1),
	        new Cell(1, 0),
	        new Cell(0, 1),
	        new Cell(-1, 0)
	    };
        // Squares is an array of square objects.
        GridCell[,] grid = new GridCell[mapSize, mapSize];


        public List<Cell> getpath()
        {
            list.Reverse();
            list.RemoveAt(0);
            return list;
        }

        private void createGrid()
        {
            int i, j = 0;

            for (i = 0; i < mapSize; i++)
            {
                for (j = 0; j < mapSize; j++)
                {
                    grid[j, i] = new GridCell();
                    grid[j, i].setType(Program.state.map[i, j]);
                }
            }
        }

        static private bool ValidCoordinates(int x, int y)
        {
            // Our coordinates are constrained between 0 and 14.
            if (x < 0)
            {
                return false;
            }
            if (y < 0)
            {
                return false;
            }
            if (x > mapSize - 1)
            {
                return false;
            }
            if (y > mapSize - 1)
            {
                return false;
            }
            return true;
        }

        public void pathFind()
        {
            createGrid();
            list = new List<Cell>();
            list.Insert(0,endCell);

            // Find path from source to target. First, get coordinates of source.
            Cell startingPoint = startCell;
            int source_x = startingPoint.x;
            int source_y = startingPoint.y;
            if (source_x == -1 || source_y == -1)
            {
                return;
            }
            // Source starts at distance of 0.
            grid[source_x, source_y].DistanceSteps = 0;

            while (true)
            {
                bool madeProgress = false;

                // Look at each square on the board.
                foreach (Cell mainPoint in AllCells())
                {
                    int x = mainPoint.x;
                    int y = mainPoint.y;

                    // If the square is open, look through valid moves given
                    // the coordinates of that square.
                    if (SquareOpen(x, y))
                    {
                        int passHere = grid[x, y].DistanceSteps;

                        foreach (Cell movePoint in ValidMoves(x, y))
                        {
                            int newX = movePoint.x;
                            int newY = movePoint.y;
                            int newPass = passHere + 1;

                            if (grid[newX, newY].DistanceSteps > newPass)
                            {
                                grid[newX, newY].DistanceSteps = newPass;
                                madeProgress = true;
                            }
                        }
                    }
                }
                if (!madeProgress)
                {
                    break;
                }
            }



            //HighlightPath();

        }

        public void HighlightPath()
        {
            // Mark the path from target to source.
            Cell startingPoint = endCell;
            int pointX = startingPoint.x;
            int pointY = startingPoint.y;
            if (pointX == -1 && pointY == -1)
            {
                return;
            }

            while (true)
            {
                // Look through each direction and find the square
                // with the lowest number of steps marked.
                Cell lowestPoint = new Cell(0, 0);
                int lowest = 1000;

                foreach (Cell movePoint in ValidMoves(pointX, pointY))
                {
                    int count = grid[movePoint.x, movePoint.y].DistanceSteps;
                    if (count < lowest)
                    {
                        lowest = count;
                        lowestPoint.x = movePoint.x;
                        lowestPoint.y = movePoint.y;
                    }
                }
                if (lowest != 1000)
                {
                    // Mark the square as part of the path if it is the lowest
                    // number. Set the current position as the square with
                    // that number of steps.
                    list.Add(new Cell(lowestPoint.x, lowestPoint.y));
                    pointX = lowestPoint.x;
                    pointY = lowestPoint.y;
                }
                else
                {
                    break;
                }

                if (pointX == startCell.x && pointY == startCell.y)
                {
                    // We went from monster to hero, so we're finished.
                    break;
                }
            }
        }

        private static IEnumerable<Cell> AllCells()
        {
            /*
             * 
             * Return every point on the board in order.
             * 
             * */
            for (int x = 0; x < mapSize; x++)
            {
                for (int y = 0; y < mapSize; y++)
                {
                    yield return new Cell(x, y);
                }
            }
        }

        private bool SquareOpen(int x, int y)
        {
            switch (grid[x, y].ContentCode)
            {
                case type.empty: return true;
                case type.target: return true;
                case type.start: return true;
                case type.blocked: return false;
                default: return false;
            }
        }

        private IEnumerable<Cell> ValidMoves(int x, int y)
        {
            /*
             * 
             * Return each valid square we can move to.
             * 
             * */
            foreach (Cell movePoint in movements)
            {
                int newX = x + movePoint.x;
                int newY = y + movePoint.y;

                if (ValidCoordinates(newX, newY) &&
                    SquareOpen(newX, newY))
                {
                    yield return new Cell(newX, newY);
                }
            }
        }


        public int getStepCount(int x, int y) {
            return grid[x, y].DistanceSteps;
        }


    }


    class Cell
    {
        public int x;
        public int y;

        public Cell(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }


    class GridCell
    {
        type _contentCode = type.empty;
        public type ContentCode
        {
            get { return _contentCode; }
            set { _contentCode = value; }
        }

        int _distanceSteps = 1000;
        public int DistanceSteps
        {
            get { return _distanceSteps; }
            set { _distanceSteps = value; }
        }

        bool _isPath = false;
        public bool IsPath
        {
            get { return _isPath; }
            set { _isPath = value; }
        }

        public void setType(int x)
        {
            switch (x)
            {
                case 0: _contentCode = type.empty; break;
                case 1: _contentCode = type.blocked; break;
                case 2: _contentCode = type.blocked; break;
                case 3: _contentCode = type.blocked; break;
                default: _contentCode= type.empty; break;
            }
        }
    }
}
