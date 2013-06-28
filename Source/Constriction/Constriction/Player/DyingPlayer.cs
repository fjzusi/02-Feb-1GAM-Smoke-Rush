using Constriction.Particle;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Constriction.Player
{
    public class DyingPlayer
    {
        public const int SPRITE_WIDTH = 32;
        public const int SPRITE_HEIGHT = 32;
        public const float DYING_TIMER = 5f;

        public Texture2D tex;

        private Rectangle boundedBox;
        private DeathParticleSystem deathParticleSystem;

        public bool isDead;

        private float dyingTimer;
        private float rotation;

        public DyingPlayer()
        {
            boundedBox = new Rectangle(0, 0, SPRITE_WIDTH, SPRITE_HEIGHT);
            isDead = false;
            dyingTimer = 0;
        }

        public void Initialize(MainGame game, SpriteBatch spriteBatch)
        {
            deathParticleSystem = new DeathParticleSystem(game);
            deathParticleSystem.Initialize(game.GraphicsDevice, game.Content, spriteBatch);
        }

        public void Activate(Point newLocation, float rotation)
        {
            dyingTimer = DYING_TIMER;
            boundedBox.Location = newLocation;
            this.rotation = rotation;
            deathParticleSystem.Emitter.PositionData.Position = new Vector3(newLocation.X, newLocation.Y, 0);
            deathParticleSystem.Emitter.Enabled = true;
        }

        public void Reset()
        {
            isDead = false;
            dyingTimer = 0;
            deathParticleSystem.Reset();
        }

        public void Update(GameTime time)
        {
            if (!isDead)
            {
                var elapsedTime = (float)time.ElapsedGameTime.TotalSeconds;

                deathParticleSystem.Update(elapsedTime);
                dyingTimer -= elapsedTime;

                if (dyingTimer <= 0)
                {
                    isDead = true;
                }
            }
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(tex, boundedBox, null, Color.White, rotation, new Vector2(SPRITE_WIDTH / 2, SPRITE_HEIGHT / 2), SpriteEffects.None, 0);
            deathParticleSystem.Draw();
        }
    }
}
