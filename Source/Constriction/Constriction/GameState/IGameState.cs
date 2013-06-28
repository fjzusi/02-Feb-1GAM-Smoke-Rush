using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Constriction.GameState
{
    public interface IGameState
    {
        void Initialize(Game game);

        void LoadContent(ContentManager content);

        void HandleInput(KeyboardState state);

        void Update(GameTime time);

        void Draw(SpriteBatch batch);

        void Reset();

        GameStateEnum? GetNextState();
    }
}
