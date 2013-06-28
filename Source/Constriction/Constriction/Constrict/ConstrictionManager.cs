using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Constriction.Constrict
{
    public class ConstrictionManager
    {
        public const int BASE_NUM_TO_MOVE = 5;
        public const int BASE_MIN_BLOCK_SPEED = 50;
        public const int BASE_MAX_BLOCK_SPEED = 250;

        public List<ConstrictBlock> constrictBlocksLeft;
        public List<ConstrictBlock> constrictBlocksRight;
        public List<ConstrictBlock> constrictBlocksTop;
        public List<ConstrictBlock> constrictBlocksBottom;

        private int numToMove;
        private int minBlockSpeed, maxBlockSpeed;

        public ConstrictionManager()
        {
            constrictBlocksLeft = new List<ConstrictBlock>();
            constrictBlocksRight = new List<ConstrictBlock>();
            constrictBlocksTop = new List<ConstrictBlock>();
            constrictBlocksBottom = new List<ConstrictBlock>();

            Reset();
        }

        public void Reset()
        {
            constrictBlocksLeft.Clear();
            constrictBlocksRight.Clear();
            constrictBlocksTop.Clear();
            constrictBlocksBottom.Clear();

            FillConstrictBlocks();

            numToMove = BASE_NUM_TO_MOVE;
            minBlockSpeed = BASE_MIN_BLOCK_SPEED;
            maxBlockSpeed = BASE_MAX_BLOCK_SPEED;
        }

        private void FillConstrictBlocks()
        {
            //Left & Right
            for (var y = 16; y < MainGame.SCREEN_HEIGHT - 16; y += ConstrictBlock.CONSTRICT_BLOCK_SHORT)
            {
                constrictBlocksLeft.Add(new ConstrictBlock(-ConstrictBlock.CONSTRICT_BLOCK_LONG, y, ConstrictBlockSideEnum.Left));
                constrictBlocksRight.Add(new ConstrictBlock(MainGame.SCREEN_WIDTH - MainGame.WALL_SIZE, y, ConstrictBlockSideEnum.Right));
            }

            //Top & Bottom
            for (var x = 16; x < MainGame.SCREEN_WIDTH - 16; x += ConstrictBlock.CONSTRICT_BLOCK_SHORT)
            {
                constrictBlocksTop.Add(new ConstrictBlock(x, -ConstrictBlock.CONSTRICT_BLOCK_LONG, ConstrictBlockSideEnum.Top));
                constrictBlocksBottom.Add(new ConstrictBlock(x, MainGame.SCREEN_HEIGHT - MainGame.WALL_SIZE, ConstrictBlockSideEnum.Bottom));
            }
        }

        public void Update(GameTime gameTime)
        {
            int numMoving = 0;

            for (var i = 0; i < constrictBlocksLeft.Count; i++)
            {
                constrictBlocksLeft[i].Update(gameTime);
                constrictBlocksRight[i].Update(gameTime);
                

                if (constrictBlocksLeft[i].isMoving)
                {
                    numMoving++;
                }

                if (constrictBlocksRight[i].isMoving)
                {
                    numMoving++;
                }
            }

            for (var i = 0; i < constrictBlocksTop.Count; i++)
            {
                constrictBlocksTop[i].Update(gameTime);
                constrictBlocksBottom[i].Update(gameTime);

                if (constrictBlocksTop[i].isMoving)
                {
                    numMoving++;
                }

                if (constrictBlocksBottom[i].isMoving)
                {
                    numMoving++;
                }
            }

            if (numMoving < numToMove)
            {
                StartMoving(numToMove - numMoving);
            }
        }

        private void StartMoving(int numToStartMoving)
        {
            for (var i = 0; i < numToStartMoving; i++)
            {
                int which = MainGame.rand.Next(4);
                float speed = MainGame.rand.Next(minBlockSpeed, maxBlockSpeed);
                ConstrictBlock block;
                int index;

                switch (which)
                {
                    case 0:
                        index = MainGame.rand.Next(constrictBlocksLeft.Count - 1);
                        block = constrictBlocksLeft[index];
                        break;
                    case 1:
                        index = MainGame.rand.Next(constrictBlocksRight.Count - 1);
                        block = constrictBlocksRight[index];
                        break;
                    case 2:
                        index = MainGame.rand.Next(constrictBlocksTop.Count - 1);
                        block = constrictBlocksTop[index];
                        break;
                    default:
                        index = MainGame.rand.Next(constrictBlocksBottom.Count - 1);
                        block = constrictBlocksBottom[index];
                        break;
                }

                MoveBlock(block, speed);
            }
        }

        private void MoveBlock(ConstrictBlock block, float speed) {
            if (!block.isMoving)
            {
                block.isMoving = true;
                block.constrictSpeed = speed;
            }
        }

        public void Draw(SpriteBatch batch)
        {
            for (var i = 0; i < constrictBlocksLeft.Count; i++)
            {
                constrictBlocksLeft[i].Draw(batch);
                constrictBlocksRight[i].Draw(batch);
            }

            for (var i = 0; i < constrictBlocksTop.Count; i++)
            {
                constrictBlocksTop[i].Draw(batch);
                constrictBlocksBottom[i].Draw(batch);
            }
        }

        public bool DoesContactPlayer(Rectangle box)
        {
            for (var i = 0; i < constrictBlocksLeft.Count; i++)
            {
                if (box.Intersects(constrictBlocksLeft[i].boundedBox) || box.Intersects(constrictBlocksRight[i].boundedBox))
                {
                    return true;
                }
            }

            for (var i = 0; i < constrictBlocksTop.Count; i++)
            {
                if (box.Intersects(constrictBlocksTop[i].boundedBox) || box.Intersects(constrictBlocksBottom[i].boundedBox))
                {
                    return true;
                }
            }

            return false;
        }

        public void UpdateDifficulty(int curScore)
        {
            numToMove = BASE_NUM_TO_MOVE + curScore / 200;

            minBlockSpeed = BASE_MIN_BLOCK_SPEED + curScore / 250;
            maxBlockSpeed = BASE_MAX_BLOCK_SPEED + curScore / 150;
        }
    }
}
