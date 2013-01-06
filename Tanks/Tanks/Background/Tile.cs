using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Tanks.Background
{
    public abstract class Tile
    {
        public Vector2 position;
        private int size;
        protected Texture2D texture;
        private Rectangle destination;

        public Vector2 getPosition() {
            return position;
        }

        public Tile(Vector2 position) {
            size = Program.iconSize;
            this.position = position;
            destination = new Rectangle();
            destination.Width = size;
            destination.Height = size;
        }

        public virtual void draw(SpriteBatch spriteBatch) {

            //Vector2 tmp = Vector2.Multiply(position, (float)size);
            destination.X = (int)position.X * size;
            destination.Y = (int)position.Y * size; ;

            spriteBatch.Draw(texture, destination, null, Color.White, 0f, Vector2.Zero , SpriteEffects.None, 0);
        }

    }

    public class Brick : Tile {

        public int damage = 0;

        public Brick(Vector2 position, ContentManager contentManager): base(position) {
            this.texture = contentManager.Load<Texture2D>("brick_wall");
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            //Have to change this later to draw according to the health
            if(damage!=4) base.draw(spriteBatch);
        }

        public void setDamage(int damage) {
            this.damage = damage;
        }

    }

    public class Stone : Tile {
        
         public Stone(Vector2 position, ContentManager contentManager): base(position) {
            this.texture = contentManager.Load<Texture2D>("stone");
        }

    }

    public class Water : Tile {
        
         public Water(Vector2 position, ContentManager contentManager): base(position) {
            this.texture = contentManager.Load<Texture2D>("water");
        }

    }

    public class Coin : Tile { 
        public Coin(Vector2 position, ContentManager contentManager): base(position) {
            this.texture = contentManager.Load<Texture2D>("Coin");
        }

    }

    public class Health : Tile
    {
        public Health(Vector2 position, ContentManager contentManager)
            : base(position)
        {
            this.texture = contentManager.Load<Texture2D>("health");
        }

    }

}
