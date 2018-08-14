using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// List
using System.Collections.Generic;
using System;

namespace Arcanoid.Desktop
{
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
