using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Constriction.Canister
{
    public class CanisterManager
    {
        public const int MAX_NUM_CANISTERS = 5;
        public const float MIN_DISTANCE = 70;
        public const float CANISTER_SPAWN_TIMER = 1.5f;

        public const int MIN_X = 128;
        public const int MAX_X = MainGame.SCREEN_WIDTH - MainGame.WALL_SIZE - Canister.WIDTH - 128;
        public const int MIN_Y = 128;
        public const int MAX_Y = MainGame.SCREEN_HEIGHT - MainGame.WALL_SIZE - Canister.HEIGHT - 128;

        public static Texture2D tex;

        public List<Canister> canisters;

        private float canisterSpawnTimer;

        public CanisterManager()
        {
            canisters = new List<Canister>();
        }

        public void Reset()
        {
            canisters.Clear();
            canisterSpawnTimer = 0;
        }

        public Canister DoesCollide(Rectangle playerBox)
        {
            foreach (var canister in canisters)
            {
                if (playerBox.Intersects(canister.boundedBox))
                {
                    canisters.Remove(canister);
                    return canister;
                }
            }

            return null;
        }

        public void Update(GameTime time)
        {
            var removeCanisters = new List<Canister>();
            UpdateTimers(time);

            foreach (var canister in canisters)
            {
                canister.Update(time);

                if (canister.fillAmount <= 0)
                {
                    removeCanisters.Add(canister);
                }
            }

            foreach (var removeCanister in removeCanisters)
            {
                canisters.Remove(removeCanister);
            }
        }

        private void UpdateTimers(GameTime time)
        {
            canisterSpawnTimer -= (float)time.ElapsedGameTime.TotalSeconds;

            if (canisterSpawnTimer < 0)
            {
                if (canisters.Count < MAX_NUM_CANISTERS)
                {
                    SpawnCanister();
                }
                
                canisterSpawnTimer = CANISTER_SPAWN_TIMER;
            }
        }

        private void SpawnCanister()
        {
            var position = new Vector2(MainGame.rand.Next(MIN_X, MAX_X), MainGame.rand.Next(MIN_Y, MAX_Y));

            if (IsTooClose(position))
            {
                position = new Vector2(MainGame.rand.Next(MIN_X, MAX_X), MainGame.rand.Next(MIN_Y, MAX_Y));

                // Try Again. Only try twice so we don't get stuck here forever, at the mercy of the random number generator
                if (IsTooClose(position))
                {
                    return;
                }
            }

            canisters.Add(new Canister((int)position.X, (int)position.Y));
        }

        private bool IsTooClose(Vector2 position)
        {
            bool tooClose = false;

            foreach (var canister in canisters)
            {
                if (Vector2.Distance(position, new Vector2(canister.boundedBox.X, canister.boundedBox.Y)) < MIN_DISTANCE)
                {
                    tooClose = true;
                }
            }

            return tooClose;
        }

        public void Draw(SpriteBatch batch)
        {
            foreach (var canister in canisters)
            {
                canister.Draw(batch);
            }
        }
    }
}
