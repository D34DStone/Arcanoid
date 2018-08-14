using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Arcanoid.Desktop
{
    abstract class Solid
    {
        protected Texture2D texture;
        protected Rectangle mask;

        public Solid(Rectangle mask)
        {
            this.mask = mask;
        }

        public void LoadContent(Texture2D texture)
        {
            this.texture = texture;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, mask, Color.White);
        }

        public Rectangle Mask
        {
            get { return mask; }
        }
    }
}
