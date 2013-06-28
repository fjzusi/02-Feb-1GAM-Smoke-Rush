using System;
using System.Collections.Generic;
using Constriction.Constrict;
using Constriction.GameState;
using Constriction.Particle;
using Constriction.Walls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Constriction
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        public const int SCREEN_WIDTH = 800;
        public const int SCREEN_HEIGHT = 600;
        public const int WALL_SIZE = 16;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Texture2D emptyTex;
        public static SpriteFont hudFont;
        public static SpriteFont scoreFont;
        public static Random rand;

        Dictionary<GameStateEnum, IGameState> gameStates;
        GameStateEnum curState;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
        }

        protected override void Initialize()
        {
            rand = new Random();

            gameStates = new Dictionary<GameStateEnum, IGameState>();
            gameStates.Add(GameStateEnum.Title, new TitleState());
            gameStates.Add(GameStateEnum.Help, new HelpState());
            gameStates.Add(GameStateEnum.MainGame, new MainGameState());
            gameStates.Add(GameStateEnum.GameOver, new GameOverState());

            gameStates[GameStateEnum.Title].Initialize(this);
            gameStates[GameStateEnum.Help].Initialize(this);
            gameStates[GameStateEnum.MainGame].Initialize(this);
            gameStates[GameStateEnum.GameOver].Initialize(this);

            curState = GameStateEnum.Title;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            emptyTex = Content.Load<Texture2D>("Graphic/Empty");
            hudFont = Content.Load<SpriteFont>("Font/HUDFont");
            scoreFont = Content.Load<SpriteFont>("Font/ScoreEffectFont");

            gameStates[GameStateEnum.Title].LoadContent(Content);
            gameStates[GameStateEnum.Help].LoadContent(Content);
            gameStates[GameStateEnum.MainGame].LoadContent(Content);
            gameStates[GameStateEnum.GameOver].LoadContent(Content);

            ((MainGameState)gameStates[GameStateEnum.MainGame]).LoadParticleContent(this, spriteBatch);
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) || GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                this.Exit();
            }

            gameStates[curState].HandleInput(Keyboard.GetState());
            gameStates[curState].Update(gameTime);
            
            var nextState = gameStates[curState].GetNextState();

            if (nextState.HasValue)
            {
                gameStates[nextState.Value].Reset();
                curState = nextState.Value;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();

            gameStates[curState].Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}