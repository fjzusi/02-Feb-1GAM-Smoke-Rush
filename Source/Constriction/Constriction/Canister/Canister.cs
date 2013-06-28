using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Constriction.Canister
{
    public class Canister
    {
        public const int WIDTH = 16;
        public const int HEIGHT = 16;

        public static Color fillColor = new Color(0, 255, 228);
        public static Vector2 fillOffset = new Vector2(1, 6);

        public const int FILL_WIDTH = 14;
        public const int FILL_HEIGHT = 7;
        public const float FILLED_AMOUNT = 100;
        public const float DRAIN_RATE = 10;

        public Rectangle boundedBox;

        public float fillAmount;

        public Canister(int x, int y)
        {
            boundedBox = new Rectangle(x, y, WIDTH, HEIGHT);
            fillAmount = FILLED_AMOUNT;
        }

        public void Update(GameTime time)
        {
            if (fillAmount > 0)
            {
                fillAmount -= DRAIN_RATE * (float)time.ElapsedGameTime.TotalSeconds;

                if (fillAmount < 0)
                {
                    fillAmount = 0;
                }
            }
        }

        public void Draw(SpriteBatch batch)
        {
            float fillHeight = FILL_HEIGHT * fillAmount / FILLED_AMOUNT;
            float fillWidth = FILL_WIDTH;
            float fillX = boundedBox.X + fillOffset.X;
            float fillY = boundedBox.Y + fillOffset.Y + (FILL_HEIGHT - fillHeight);

            batch.Draw(MainGame.emptyTex, new Rectangle((int)(boundedBox.X + fillOffset.X), (int)(boundedBox.Y + fillOffset.Y), (int)FILL_WIDTH, (int)FILL_HEIGHT), Color.Black);
            batch.Draw(MainGame.emptyTex, new Rectangle((int)fillX, (int)fillY, (int)fillWidth, (int)fillHeight), fillColor);

            batch.Draw(CanisterManager.tex, boundedBox, Color.White);
        }
    }
}
