using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Constriction.Walls
{
    public class WallManager
    {
        public List<Wall> walls;

        public WallManager()
        {
            walls = new List<Wall>();

            walls.Add(new Wall(0, 0, MainGame.SCREEN_WIDTH, MainGame.WALL_SIZE));//Top
            walls.Add(new Wall(0, MainGame.SCREEN_HEIGHT - MainGame.WALL_SIZE, MainGame.SCREEN_WIDTH, MainGame.WALL_SIZE));//Bottom
            walls.Add(new Wall(0, 0, MainGame.WALL_SIZE, MainGame.SCREEN_HEIGHT));//Left
            walls.Add(new Wall(MainGame.SCREEN_WIDTH - MainGame.WALL_SIZE, 0, MainGame.WALL_SIZE, MainGame.SCREEN_HEIGHT));//Right
        }

        public Rectangle? IsSpaceFree(Rectangle box)
        {
            foreach (var wall in walls)
            {
                if (box.Intersects(wall.boundedBox))
                {
                    return wall.boundedBox;
                }
            }

            return null;
        }

        public void Draw(SpriteBatch batch)
        {
            foreach (var wall in walls)
            {
                wall.Draw(batch);
            }
        }
    }
}
