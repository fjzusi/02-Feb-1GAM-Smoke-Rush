using System;
using Constriction.Walls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Constriction.Player
{
    public class AlivePlayer
    {
        public const int SPRITE_WIDTH = 32;
        public const int SPRITE_HEIGHT = 32;

        public const int PLAYER_WIDTH = 16;
        public const int PLAYER_HEIGHT = 16;

        public const int PLAYER_RUN_SPEED = 300;
        public const int PLAYER_JOG_SPEED = 170;
        public const int PLAYER_WALK_SPEED = 100;

        public const float INVULN_TIMER = 1.5f;

        public Rectangle boundedBox;
        public Texture2D tex;

        public WallManager wallManager;

        public bool upPressed, downPressed;
        public bool leftPressed, rightPressed;
        public bool runPressed, walkPressed;

        private Vector2 position;
        private Vector2 velocity;
        public float rotation;

        public bool canBeHurt;
        private float invulnTimer;

        public AlivePlayer()
        {
            Reset();
        }

        public void Reset()
        {
            position = new Vector2(400, 300);
            boundedBox = new Rectangle((int)position.X, (int)position.Y, PLAYER_WIDTH, PLAYER_HEIGHT);
            canBeHurt = true;
            invulnTimer = 0;
        }

        public void Hurt()
        {
            canBeHurt = false;
            invulnTimer = INVULN_TIMER;
        }

        public void Update(GameTime time)
        {
            UpdateInvulnerability(time);

            HandleInput(time);
            UpdatePosition();
            UpdateBox();
        }

        private void UpdateInvulnerability(GameTime time)
        {
            if (invulnTimer > 0)
            {
                invulnTimer -= (float)time.ElapsedGameTime.TotalSeconds;

                if (invulnTimer < 0)
                {
                    invulnTimer = 0;
                    canBeHurt = true;
                }
            }
        }

        private void HandleInput(GameTime time)
        {
            velocity = Vector2.Zero;
            int speed;

            if (runPressed)
            {
                speed = PLAYER_RUN_SPEED;
            }
            else if (walkPressed)
            {
                speed = PLAYER_WALK_SPEED;
            }
            else
            {
                speed = PLAYER_JOG_SPEED;
            }

            if (upPressed)
            {
                velocity.Y -= speed;
            }
            if (downPressed)
            {
                velocity.Y += speed;
            }

            if (leftPressed)
            {
                velocity.X -= speed;
            }
            if (rightPressed)
            {
                velocity.X += speed;
            }

            if (velocity != Vector2.Zero)
            {
                rotation = (float)Math.Atan2(velocity.Y, velocity.X);
                velocity.Normalize();

                velocity *= speed * (float)time.ElapsedGameTime.TotalSeconds;
            }
        }

        private void UpdatePosition()
        {
            position.X += velocity.X;
            UpdateBox();

            var solidObstacle = wallManager.IsSpaceFree(this.boundedBox);

            if (solidObstacle.HasValue)
            {
                if (velocity.X > 0)//Right
                {
                    position.X = solidObstacle.Value.Left - PLAYER_WIDTH;
                }
                else if (velocity.X < 0)//Left
                {
                    position.X = solidObstacle.Value.Right;
                }
            }

            position.Y += velocity.Y;
            UpdateBox();
            solidObstacle = wallManager.IsSpaceFree(this.boundedBox);

            if (solidObstacle.HasValue)
            {
                if (velocity.Y > 0)//Down
                {
                    position.Y = solidObstacle.Value.Top - PLAYER_HEIGHT;
                }
                else if (velocity.Y < 0)//Up
                {
                    position.Y = solidObstacle.Value.Bottom;
                }
            }
            
            UpdateBox();
        }

        private void UpdateBox()
        {
            boundedBox.Location = new Point((int)position.X, (int)position.Y);
        }

        public void Draw(SpriteBatch batch)
        {
            float xPos = position.X + SPRITE_WIDTH / 2 - PLAYER_WIDTH / 2;
            float yPos = position.Y + SPRITE_HEIGHT / 2 - PLAYER_HEIGHT / 2;

            float alpha = 1 - (invulnTimer / INVULN_TIMER * 0.8f);
            var color = Color.White * alpha;

            batch.Draw(tex, new Vector2(xPos, yPos), null, color, rotation, new Vector2(SPRITE_WIDTH / 2, SPRITE_HEIGHT / 2), 1, SpriteEffects.None, 0);
        }

        public Point GetLocation()
        {
            float xPos = position.X + SPRITE_WIDTH / 2 - PLAYER_WIDTH / 2;
            float yPos = position.Y + SPRITE_HEIGHT / 2 - PLAYER_HEIGHT / 2;

            return new Point((int)xPos, (int)yPos);
        }
    }
}
