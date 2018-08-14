using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// List
using System.Collections.Generic;
//
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

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(texture, mask, Color.White);
		}

		public Rectangle Mask
		{
			get { return mask; }
		}
	}

	class Player : Solid
	{
		private const float SPEED = 0.25F;

		public Player(Rectangle mask) : base(mask){}
  
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

			foreach(Block block in blocks)
			{
				if (mask.Intersects(block.Mask))
				{
					intersected.Add(block);
				}
			}
   
			if(intersected.Count != 0)
			{
				if(previousMask.Left >= intersected[0].Mask.Right || previousMask.Right <= intersected[0].Mask.Left)
				{
					speedVector.X *= -1;
				}
				if(previousMask.Top >= intersected[0].Mask.Bottom || previousMask.Bottom <= intersected[0].Mask.Top)
				{
					speedVector.Y *= -1;
				}
			}

			foreach(Block block in intersected)
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
    
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
		Vector2 windowSize;
		Ball ball;
		Player player;
		List<Block> blocks;

		bool isWinScreen = false;
		bool isLoseScreen = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
  
        protected override void Initialize()
        {
			windowSize.X = graphics.PreferredBackBufferWidth = 800;
            windowSize.Y = graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();

			player = new Player(new Rectangle(200, 550, 100, 25));
			ball = new Ball(new Rectangle(200, 300, 25, 25), new Vector2(0.707F, 0.707F));

			blocks = new List<Block>();
			for (int i = 0; i < 10; i++)
			{
				blocks.Add(new Block(new Rectangle(150 + i * 50, 100, 50, 25), Color.Yellow));
				blocks.Add(new Block(new Rectangle(150 + i * 50, 125, 50, 25), Color.Blue));
				blocks.Add(new Block(new Rectangle(150 + i * 50, 150, 50, 25), Color.White));
				blocks.Add(new Block(new Rectangle(150 + i * 50, 175, 50, 25), Color.Purple));
			}

            base.Initialize();
        }
  
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            

			Texture2D playerTexture = Content.Load<Texture2D>("player");
			Texture2D ballTexture = Content.Load<Texture2D>("ball");
			Texture2D blockTexture = Content.Load<Texture2D>("block");

			player.LoadContent(playerTexture);
			ball.LoadContent(ballTexture);

			foreach(Block block in blocks)
			{
				block.LoadContent(blockTexture);
			}
        }
  
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
  
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

			KeyboardState keyboardState = Keyboard.GetState();
			// TODO: Add your update logic here
			if( !isLoseScreen && !isWinScreen)
			{
                player.Update(keyboardState, gameTime, windowSize);
				ball.Update(player, blocks, keyboardState, gameTime, windowSize);

				if(ball.isFallen(windowSize))
				{
					isLoseScreen = true;
				}

				if(blocks.Count == 0)
				{
					isWinScreen = true;
				}
			}
            
            base.Update(gameTime);
        }
  
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

			// TODO: Add your drawing code here
            spriteBatch.Begin();
			if(isWinScreen)
			{
				// TODO: Write Win screen there
			}
			if(isLoseScreen)
			{
				// TODO: Write lose screen there
			}
			else
			{
                player.Draw(spriteBatch);
                ball.Draw(spriteBatch);

                foreach (Block block in blocks)
                {
                    block.Draw(spriteBatch);
                }
			}
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
