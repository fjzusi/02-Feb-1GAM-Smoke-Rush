using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Constriction
{
    public class HUD
    {
        public const int PLAYER_HEALTH_MAX = 100;
        public const int PLAYER_DAMAGE_DECREMENT = 10;

        private Vector2 position;
        public int curScore;

        public int playerHealth;

        public HUD()
        {
            position = new Vector2(16, 0);
            Reset();
        }

        public void Reset()
        {
            curScore = 0;

            playerHealth = PLAYER_HEALTH_MAX;
        }

        public void HurtPlayerHealth()
        {
            playerHealth -= PLAYER_DAMAGE_DECREMENT;
        }

        public void Draw(SpriteBatch batch)
        {
            batch.DrawString(MainGame.hudFont, string.Format("Score: {0}", curScore), position, Color.White);

            batch.Draw(MainGame.emptyTex, new Rectangle(100, 4, PLAYER_HEALTH_MAX, 8), Color.Red);
            batch.Draw(MainGame.emptyTex, new Rectangle(100, 4, playerHealth, 8), Color.GreenYellow);
        }
    }
}
