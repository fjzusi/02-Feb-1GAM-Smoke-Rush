using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Constriction.Constrict
{
    public class ConstrictBlock
    {
        public const int CONSTRICT_BLOCK_LONG = 64;
        public const int CONSTRICT_BLOCK_SHORT = 16;

        public Rectangle boundedBox;

        private ConstrictBlockSideEnum side;
        private float pos;

        public float constrictSpeed;
        public bool isMoving;

        public ConstrictBlock(int x, int y, ConstrictBlockSideEnum side)
        {
            switch (side)
            {
                case ConstrictBlockSideEnum.Left:
                case ConstrictBlockSideEnum.Right:
                    boundedBox = new Rectangle(x, y, CONSTRICT_BLOCK_LONG, CONSTRICT_BLOCK_SHORT);
                    break;
                case ConstrictBlockSideEnum.Top:
                case ConstrictBlockSideEnum.Bottom:
                    boundedBox = new Rectangle(x, y, CONSTRICT_BLOCK_SHORT, CONSTRICT_BLOCK_LONG);
                    break;
            }

            this.side = side;
            pos = 0;

            constrictSpeed = 100;
            isMoving = false;
        }

        public void Update(GameTime time)
        {
            if (isMoving)
            {
                UpdatePosition(time);
                CheckState();
            }
        }

        private void UpdatePosition(GameTime time)
        {
            pos += constrictSpeed * (float)time.ElapsedGameTime.TotalSeconds;

            switch (side)
            {
                case ConstrictBlockSideEnum.Left:
                    boundedBox.X = (int)pos - CONSTRICT_BLOCK_LONG;
                    break;
                case ConstrictBlockSideEnum.Right:
                    boundedBox.X = MainGame.SCREEN_WIDTH - MainGame.WALL_SIZE - (int)pos;
                    break;
                case ConstrictBlockSideEnum.Top:
                    boundedBox.Y = (int)pos - CONSTRICT_BLOCK_LONG;
                    break;
                case ConstrictBlockSideEnum.Bottom:
                    boundedBox.Y = MainGame.SCREEN_HEIGHT - MainGame.WALL_SIZE - (int)pos;
                    break;
            }
        }

        private void CheckState()
        {
            float targetPos = 0;

            switch (side)
            {
                case ConstrictBlockSideEnum.Left:
                case ConstrictBlockSideEnum.Right:
                    targetPos = MainGame.SCREEN_WIDTH + CONSTRICT_BLOCK_LONG;
                    break;
                case ConstrictBlockSideEnum.Top:
                case ConstrictBlockSideEnum.Bottom:
                    targetPos = MainGame.SCREEN_HEIGHT + CONSTRICT_BLOCK_LONG;;
                    break;
            }

            if (pos > targetPos)
            {
                isMoving = false;
                pos = 0;
            }
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(MainGame.emptyTex, boundedBox, Color.Black);
        }
    }
}
