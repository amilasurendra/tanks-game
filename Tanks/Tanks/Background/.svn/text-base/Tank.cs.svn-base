using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Tanks.Background
{
    public class Tank
    {
        public const int NORTH = 0;
        public const int EAST = 1;
        public const int SOUTH = 2;
        public const int WEST = 3;

        private Texture2D texture;
        private Rectangle destination;

        private Vector2 position;
        private Vector2 oldPos;
        private Vector2 newPos;

        private int oldDir;
        public int direction;
        private int newDir;
        private int rotation = 0;

        private float degrees = 0;

        private int up = 0;
        private int down = 0;
        private int left = 0;
        private int right = 0;

        private float speed = 2f;
        private const int size = Program.iconSize;

        //  1 - Clockwise  2- Anti-Clockwise
        int[,] dirMat = {{0,1,1,2},{2,0,1,1},{1,2,0,1},{1,1,2,0}};

        public Tank(int x, int y, int dir)
        {
            this.direction = oldDir = newDir = dir*90;
            degrees = direction;
           
            Vector2 tmp = new Vector2(x * size + size / 2, y * size + size / 2);
            position = oldPos = newPos = tmp;
            destination.Height = destination.Width = size;
        }

        
        public void setPosition(int x, int y, int dir)
        {
            direction = oldDir;
            position = oldPos;

            newDir = dir * 90;
            newPos = new Vector2(x * size + size / 2, y * size + size / 2);

            if (newPos != oldPos)
            {
                position = oldPos;

                Vector2 result = newPos - oldPos;

                right = result.X > 0 ? 1 : 0;
                left = result.X < 0 ? 1 : 0;
                up = result.Y < 0 ? 1 : 0;
                down = result.Y > 0 ? 1 : 0;

                oldPos = newPos;

            }
            else if (newDir != oldDir)
            {
                direction = oldDir;
                rotation = dirMat[oldDir/90,newDir/90];

                oldDir = newDir;
            }

        }

        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            texture = theContentManager.Load<Texture2D>(theAssetName);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            destination.X = (int)position.X;
            destination.Y = (int)position.Y;

            spriteBatch.Draw(texture, destination, null, Color.White, (float)Math.PI*direction/180, new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, 0);
        }

        public void Update(GameTime gameTime)
        {
            //Read variables and act accordingly

            if (newPos == position) right = left = up = down = 0; ;
            if (newDir == direction) rotation = 0;

            if ((newPos == position) && (direction==newDir))
            {
                return;
            }


            if (right == 1)
            {
                position.X += speed;
            }
            else if (left == 1)
            {
                position.X -= speed;
            }
            else if (up == 1)
            {
                position.Y -= speed;
            }
            else if (down == 1)
            {
                position.Y += speed;
            }


            if (rotation == 1) {
                int tmp = direction;
                tmp += 9;
                tmp = tmp % 360;
                direction = tmp;
            }
            if (rotation == 2) {
                int tmp = direction;
                tmp -= 9;
                if (tmp < 0) tmp += 360;
                tmp = tmp % 360;
                direction = tmp;
            }

        }
    }
}
