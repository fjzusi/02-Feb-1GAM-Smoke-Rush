using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Constriction.GameState
{
    public class TitleState : IGameState
    {
        private Texture2D texTitleScreen;

        private bool acceptingKeyPresses;
        private bool keysPressed;
        private bool goToNextState;

        public void Initialize(Game game)
        {
            Reset();
        }

        public void LoadContent(ContentManager content)
        {
            texTitleScreen = content.Load<Texture2D>("Graphic/Screen/TitleScreen");
        }

        public void HandleInput(KeyboardState state)
        {
            var keysDown = state.GetPressedKeys().Length;

            if (keysDown > 0 && !keysPressed && acceptingKeyPresses)
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
            batch.Draw(texTitleScreen, Vector2.Zero, Color.White);
        }

        public void Reset()
        {
            keysPressed = true;
            goToNextState = false;
            acceptingKeyPresses = true;
        }

        public GameStateEnum? GetNextState()
        {
            GameStateEnum? nextState = null;

            if (goToNextState)
            {
                nextState = GameStateEnum.Help;
            }

            return nextState;
        }
    }
}