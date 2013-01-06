using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Text.RegularExpressions;

namespace Tanks.Background
{
    public class BackGround
    {
        public const int BRICK = 1;
        public const int STONE = 2;
        public const int WATER = 3;

        public int mapSize;
        Map gameMap;

        ContentManager contentManager;


        public Boolean completed = false;

        public BackGround()
        {
            mapSize = Program.mapSize;
            gameMap = new Map(mapSize);
        }



        public void initializeMap() {
            

            for (int i = 0; i < Program.mapSize; i++) { 
                for(int j =0; j< Program.mapSize; j++){

                    switch (Program.state.map[i, j]) {
                        case 1:
                            gameMap.addTile(new Brick(new Vector2(j, i), this.contentManager));
                            break;
                        case 2:
                            gameMap.addTile(new Stone(new Vector2(j, i), this.contentManager));
                            break;
                        case 3:
                            gameMap.addTile(new Water(new Vector2(j, i), this.contentManager));
                            break;
                        default: break;
                    }

                }
            }

        }

        public void updateChangingTiles() {
            for (int i = 0; i < Program.mapSize; i++)
            {
                for (int j = 0; j < Program.mapSize; j++)
                {


                    switch (Program.state.map[i, j])
                    {
                        case 0:
                            gameMap.tiles[j, i] = null;
                            break;

                        case 4:
                            gameMap.addTile(new Coin(new Vector2(j, i), this.contentManager));
                            break;


                        case 5:
                            gameMap.addTile(new Health(new Vector2(j, i), this.contentManager));
                            break;

                        default: break;
                    }

                }
            }
        }


        public void updateTanks()
        {
            if (Program.state.initialized == false) return;

            if (Program.game.my_tank == null || Program.game.other_tanks == null)
            {
                Program.game.my_tank = new Tank(Program.state.my_tank_data.x, Program.state.my_tank_data.y, Program.state.my_tank_data.direction);
                Program.game.my_tank.LoadContent(Program.game.Content, "MY_Tank");


                Program.game.other_tanks = new Tank[Program.state.playerCount];
                int count = 0;

                for (count = 0; count < Program.state.playerCount; count++)
                {
                    Program.game.other_tanks[count] = new Tank(Program.state.other_tanks[count].x, Program.state.other_tanks[count].y, Program.state.other_tanks[count].direction);
                    Program.game.other_tanks[count].LoadContent(Program.game.Content, "Other_Tank");
                }

                completed = true;

            }

            Program.game.my_tank.setPosition(Program.state.my_tank_data.x, Program.state.my_tank_data.y, Program.state.my_tank_data.direction);


            for (int count = 0; count < Program.state.playerCount; count++)
            {
                Program.game.other_tanks[count].setPosition(Program.state.other_tanks[count].x, Program.state.other_tanks[count].y, Program.state.other_tanks[count].direction);

            }


        }


        public void updateCoins() { 
        
        
        }
 


        

        

        #region XNA Methods
        public void setContentManager(ContentManager manager)
        {
            this.contentManager = manager;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            gameMap.draw(spriteBatch);
        }
        #endregion

    }


    //Just holds tiles array and assigns tiles according to tile position
    class Map
    {

        public Tile[,] tiles;
        int size;

        public Map(int size)
        {
            tiles = new Tile[size, size];
            this.size = size;
        }


        public void addTile(Tile tile)
        {
            tiles[(int)tile.getPosition().X, (int)tile.getPosition().Y] = tile;
        }

        public Tile getTile(int x, int y)
        {
            return tiles[x, y];
        }

        public void draw(SpriteBatch spriteBatch)
        {
            int i, j;

            for (i = 0; i < size; i++)
            {
                for (j = 0; j < size; j++)
                {
                    if (tiles[i, j] != null)
                    {
                        tiles[i, j].draw(spriteBatch);
                    }
                }
            }
        }

    }
}
