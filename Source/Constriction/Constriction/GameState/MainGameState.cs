using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Constriction.Particle;
using Constriction.Walls;
using Constriction.Constrict;
using Constriction.Player;
using Constriction.Canister;
using Constriction.ScoreEffect;

namespace Constriction.GameState
{
    public class MainGameState : IGameState
    {
        private Texture2D texFloor, texSmoke;

        private SoundEffect sndGoalPickup;
        private SoundEffect sndPlayerHurt;
        private SoundEffectInstance sndPlayerDying;

        private FogParticleSystem fogParticleSystem;
        private DamageParticleSystem damageParticleSystem;
        private CanisterParticleSystem canisterParticleSystem;
        private ScoreEffectManager scoreEffectManager;

        private WallManager wallManager;
        private ConstrictionManager constrictionManager;
        private CanisterManager canisterManager;
        private HUD hud;

        private AlivePlayer player;
        private DyingPlayer dyingPlayer;

        private bool playerIsDying;
        private bool playerIsDead;

        public void Initialize(Game game)
        {
            player = new AlivePlayer();
            dyingPlayer = new DyingPlayer();
            wallManager = new WallManager();
            constrictionManager = new ConstrictionManager();
            canisterManager = new CanisterManager();
            scoreEffectManager = new ScoreEffectManager();

            player.wallManager = wallManager;

            hud = new HUD();

            fogParticleSystem = new FogParticleSystem(game);
            damageParticleSystem = new DamageParticleSystem(game);
            canisterParticleSystem = new CanisterParticleSystem(game);

            playerIsDying = false;
            playerIsDead = false;
        }

        public void LoadContent(ContentManager content)
        {
            texFloor = content.Load<Texture2D>("Graphic/Screen/MainGameScreenFloor");
            texSmoke = content.Load<Texture2D>("Graphic/Screen/MainGameScreenSmoke");

            player.tex = content.Load<Texture2D>("Graphic/Player");
            dyingPlayer.tex = content.Load<Texture2D>("Graphic/DeadPlayer");
            CanisterManager.tex = content.Load<Texture2D>("Graphic/Goal");

            sndGoalPickup = content.Load<SoundEffect>("Sound/GoalPickup");
            sndPlayerHurt = content.Load<SoundEffect>("Sound/PlayerHurt");

            sndPlayerDying = content.Load<SoundEffect>("Sound/PlayerDying").CreateInstance();
            sndPlayerDying.IsLooped = true;
        }

        public void LoadParticleContent(MainGame game, SpriteBatch spriteBatch)
        {
            fogParticleSystem.Initialize(game.GraphicsDevice, game.Content, spriteBatch, constrictionManager);
            damageParticleSystem.Initialize(game.GraphicsDevice, game.Content, spriteBatch);
            canisterParticleSystem.Initialize(game.GraphicsDevice, game.Content, spriteBatch);

            dyingPlayer.Initialize(game, spriteBatch);
        }

        public void HandleInput(KeyboardState state)
        {
            player.upPressed = state.IsKeyDown(Keys.Up);
            player.downPressed = state.IsKeyDown(Keys.Down);
            player.leftPressed = state.IsKeyDown(Keys.Left);
            player.rightPressed = state.IsKeyDown(Keys.Right);

            player.runPressed = state.IsKeyDown(Keys.LeftShift);
            player.walkPressed = state.IsKeyDown(Keys.LeftControl);
        }

        public void Update(GameTime time)
        {
            if (playerIsDying)
            {
                dyingPlayer.Update(time);

                if (dyingPlayer.isDead)
                {
                    playerIsDead = true;
                    sndPlayerDying.Stop();
                }
            }
            else
            {
                player.Update(time);
            }

            canisterManager.Update(time);
            scoreEffectManager.Update(time);
            constrictionManager.Update(time);
            UpdateParticles(time);

            //Collisions
            if (player.canBeHurt)
            {
                var canister = canisterManager.DoesCollide(player.boundedBox);

                if (canister != null)
                {
                    sndGoalPickup.Play(0.5f, 0.0f, 0.0f);

                    canisterParticleSystem.Explode(canister.boundedBox.Center);
                    hud.curScore += (int)canister.fillAmount;
                    constrictionManager.UpdateDifficulty(hud.curScore);
                    scoreEffectManager.AddScoreEffect(canister.boundedBox.Center, canister.fillAmount);
                }
            }

            if (player.canBeHurt && constrictionManager.DoesContactPlayer(player.boundedBox))
            {
                sndPlayerHurt.Play(0.5f, 0.0f, 0.0f);

                player.Hurt();
                hud.HurtPlayerHealth();

                if (hud.playerHealth > 0)
                {
                    damageParticleSystem.Explode(player.boundedBox.Center);
                }
            }

            //Check end state
            if (hud.playerHealth <= 0 && !playerIsDying)
            {
                KillPlayer();
            }
        }

        private void UpdateParticles(GameTime time)
        {
            fogParticleSystem.Update(time, constrictionManager);
            damageParticleSystem.Update((float)time.ElapsedGameTime.TotalSeconds);
            canisterParticleSystem.Update((float)time.ElapsedGameTime.TotalSeconds);
        }

        private void KillPlayer()
        {
            playerIsDying = true;
            dyingPlayer.Activate(player.GetLocation(), player.rotation);
            sndPlayerDying.Play();
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(texFloor, Vector2.Zero, Color.White);
            //constrictionManager.Draw(batch);

            if (playerIsDying)
            {
                dyingPlayer.Draw(batch);
            }
            else
            {
                player.Draw(batch);
            }
            
            wallManager.Draw(batch);

            fogParticleSystem.Draw();
            canisterParticleSystem.Draw();
            damageParticleSystem.Draw();

            canisterManager.Draw(batch);

            batch.Draw(texSmoke, Vector2.Zero, Color.White);

            scoreEffectManager.Draw(batch);
            hud.Draw(batch);
        }

        public void Reset()
        {
            fogParticleSystem.Reset();
            damageParticleSystem.Reset();
            canisterParticleSystem.Reset();
            constrictionManager.Reset();
            scoreEffectManager.Reset();
            player.Reset();
            dyingPlayer.Reset();
            canisterManager.Reset();
            hud.Reset();

            playerIsDying = false;
            playerIsDead = false;
        }

        public GameStateEnum? GetNextState()
        {
            GameStateEnum? nextState = null;

            if (playerIsDead)
            {
                nextState = GameStateEnum.GameOver;
            }

            return nextState;
        }
    }
}
