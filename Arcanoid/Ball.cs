using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Arcanoid.Desktop
{
	class Ball : Solid
	{
		private const float SPEED = 0.25F;
		private const float PLAYER_INTERSECT_INTERVAL_MS = 1000.0F;

		protected Vector2 speedVector;

		protected float playerIntersectCooldown = 0;

		public Ball(Rectangle mask, Vector2 speedVector) : base(mask)
		{
			this.speedVector = speedVector;
		}

		public void Update(Player player, List<Block> blocks, KeyboardState keyboardState, GameTime gameTime, Vector2 windowSize)
		{
			float msElapsed = gameTime.ElapsedGameTime.Milliseconds;

			Rectangle previousMask = mask;
			mask.Offset(new Vector2(speedVector.X * SPEED * msElapsed, speedVector.Y * SPEED * msElapsed));

			// handle collision with blocks
			List<Block> intersected = new List<Block>();

			foreach (Block block in blocks)
			{
				if (mask.Intersects(block.Mask))
				{
					intersected.Add(block);
				}
			}

			if (intersected.Count != 0)
			{
				if (previousMask.Left >= intersected[0].Mask.Right || previousMask.Right <= intersected[0].Mask.Left)
				{
					speedVector.X *= -1;
				}
				if (previousMask.Top >= intersected[0].Mask.Bottom || previousMask.Bottom <= intersected[0].Mask.Top)
				{
					speedVector.Y *= -1;
				}
			}

			foreach (Block block in intersected)
			{
				blocks.Remove(block);
			}

			// handle collision with player
			if (mask.Intersects(player.Mask) && playerIntersectCooldown <= 0)
			{
				if (previousMask.Left >= player.Mask.Right || previousMask.Right <= player.Mask.Left)
				{
					speedVector.X *= -1;
				}
				if (previousMask.Top >= player.Mask.Bottom || previousMask.Bottom <= player.Mask.Top)
				{
					speedVector.Y *= -1;
				}

				playerIntersectCooldown = PLAYER_INTERSECT_INTERVAL_MS;
			}
			playerIntersectCooldown -= gameTime.ElapsedGameTime.Milliseconds;

			// handle collisions with borders of window
			if (mask.Left < 0 || mask.Right >= windowSize.X)
			{
				speedVector.X *= -1;
			}

			if (mask.Top < 0)
			{
				speedVector.Y *= -1;
			}
		}

		public bool isFallen(Vector2 windowSize)
		{
			return (mask.Top >= windowSize.Y);
		}
	}
}
