using System;
using DPSF;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Constriction.Particle
{
    [Serializable]
    public class DeathParticleSystem : DefaultSpriteParticleSystem
    {
        public const int DEATH_NUM_PARTICLES = 100;

        public DeathParticleSystem(Game cGame) : base(cGame) { }

        public void Initialize(GraphicsDevice cGraphicsDevice, ContentManager cContentManager, SpriteBatch cSpriteBatch)
        {
            base.AutoInitialize(cGraphicsDevice, cContentManager, cSpriteBatch);
            base.InitializeSpriteParticleSystem(cGraphicsDevice, cContentManager, 1000, 50000, "Graphic/Empty", cSpriteBatch);

            Name = "Death Particle System";

            LoadParticleSystem();
        }

        public void Reset()
        {
            this.RemoveAllParticles();
        }

        public void LoadParticleSystem()
        {
            ParticleInitializationFunction = InitializeParticleUsingInitialProperties;

            InitialProperties.LifetimeMin = 0.5f;
            InitialProperties.LifetimeMax = 1.0f;
            InitialProperties.PositionMin = Vector3.Zero;
            InitialProperties.PositionMax = Vector3.Zero;
            InitialProperties.VelocityMin = new Vector3(150, 150, 0);
            InitialProperties.VelocityMax = new Vector3(-150, -150, 0);
            InitialProperties.StartSizeMin = 2;
            InitialProperties.StartSizeMax = 4;
            InitialProperties.EndSizeMin = 2;
            InitialProperties.EndSizeMax = 4;
            InitialProperties.StartColorMin = Color.Red;
            InitialProperties.StartColorMax = Color.Yellow;
            InitialProperties.EndColorMin = Color.Gray;
            InitialProperties.EndColorMax = Color.Gray;

            ParticleEvents.RemoveAllEvents();
            ParticleSystemEvents.RemoveAllEvents();

            ParticleEvents.AddEveryTimeEvent(UpdateParticlePositionUsingVelocity);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleTransparencyToFadeOutUsingLerp);

            Emitter.EmitParticlesAutomatically = true;
            Emitter.ParticlesPerSecond = DEATH_NUM_PARTICLES;
            Emitter.Enabled = true;
        }

        public void Explode(Point location)
        {
            Emitter.PositionData.Position = new Vector3(location.X, location.Y, 0);
            this.AddParticles(DEATH_NUM_PARTICLES);
        }
    }
}
