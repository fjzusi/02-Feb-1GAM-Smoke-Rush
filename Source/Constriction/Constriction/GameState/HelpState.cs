using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Constriction.GameState
{
    public class HelpState : IGameState
    {
        private Texture2D texHelpScreen;

        private bool keysPressed;
        private bool goToNextState;

        public void Initialize(Game game)
        {
            Reset();
        }

        public void LoadContent(ContentManager content)
        {
            texHelpScreen = content.Load<Texture2D>("Graphic/Screen/HelpScreen");
        }

        public void HandleInput(KeyboardState state)
        {
            var keysDown = state.GetPressedKeys().Length;

            if (keysDown > 0 && !keysPressed)
            {
                goToNextState = true;
            }
            else if (keysDown == 0)
            {
                keysPressed = false;
            }
        }

        public void Update(GameTime time)
        {
            //TODO Something cool and animation-y
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(texHelpScreen, Vector2.Zero, Color.White);
        }

        public void Reset()
        {
            keysPressed = true;
            goToNextState = false;
        }

        public GameStateEnum? GetNextState()
        {
            GameStateEnum? nextState = null;

            if (goToNextState)
            {
                nextState = GameStateEnum.MainGame;
            }

            return nextState;
        }
    }
}
