using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Arcanoid.Desktop
{
	class Player : Solid
	{
		private const float SPEED = 0.25F;

		public Player(Rectangle mask) : base(mask) { }

		public void Update(KeyboardState keyboardState, GameTime gameTime, Vector2 windowSize)
		{
			float msElapsed = gameTime.ElapsedGameTime.Milliseconds;

			if (keyboardState.IsKeyDown(Keys.A) && mask.Left >= 0)
			{
				mask.Offset(new Vector2(-SPEED * msElapsed, 0));
			}
			if (keyboardState.IsKeyDown(Keys.D) && mask.Right < windowSize.X)
			{
				mask.Offset(new Vector2(SPEED * msElapsed, 0));
			}
		}
	}
}

