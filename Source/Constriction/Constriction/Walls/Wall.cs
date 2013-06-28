using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Constriction.Walls
{
    public class Wall
    {
        public Rectangle boundedBox;
        public Texture2D tex;

        public Wall(int x, int y, int width, int height)
        {
            boundedBox = new Rectangle(x, y, width, height);
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(MainGame.emptyTex, boundedBox, Color.Black);
        }
    }
}
