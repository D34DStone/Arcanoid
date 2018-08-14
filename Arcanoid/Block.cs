using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Arcanoid.Desktop
{
	class Block : Solid
	{
		private Color color;

		public Block(Rectangle mask, Color color) : base(mask)
		{
			this.color = color;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(texture, mask, color);
		}
	}
}
