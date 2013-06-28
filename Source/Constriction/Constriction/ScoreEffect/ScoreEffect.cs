using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Constriction.ScoreEffect
{
    public class ScoreEffect
    {
        public const float EFFECT_TIMER = 1;
        public const float MOVE_SPEED = 50;

        private Vector2 position;

        public float effectTimer;

        private int displayNumber;

        public ScoreEffect(float x, float y, int displayNumber)
        {
            position = new Vector2(x - 12, y - 14);
            effectTimer = EFFECT_TIMER;
            this.displayNumber = displayNumber;
        }

        public void Update(GameTime time)
        {
            if (effectTimer > 0)
            {
                effectTimer -= (float)time.ElapsedGameTime.TotalSeconds;
                position.Y -= MOVE_SPEED * (float)time.ElapsedGameTime.TotalSeconds;
            }
        }

        public void Draw(SpriteBatch batch)
        {
            if (effectTimer > 0)
            {
                batch.DrawString(MainGame.scoreFont, displayNumber.ToString(), position, Color.White);
            }
        }
    }
}
