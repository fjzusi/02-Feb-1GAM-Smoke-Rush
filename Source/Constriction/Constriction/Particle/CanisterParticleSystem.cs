using System;
using DPSF;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace Constriction.Particle
{
    [Serializable]
    public class CanisterParticleSystem : DefaultSpriteParticleSystem
    {

        public const int PICKUP_NUM_PARTICLES = 30;

        private Color canisterStartColor = new Color(0, 255, 228);
        private Color canisterEndColor = new Color(0, 255, 228, 0);

        public CanisterParticleSystem(Game cGame) : base(cGame) { }

        public void Initialize(GraphicsDevice cGraphicsDevice, ContentManager cContentManager, SpriteBatch cSpriteBatch)
        {
            base.AutoInitialize(cGraphicsDevice, cContentManager, cSpriteBatch);

            base.InitializeSpriteParticleSystem(cGraphicsDevice, cContentManager, 30, 30, "Particle/CanisterPickupParticle", cSpriteBatch);
            Name = "Canister Particle System";

            // Finish loading the Particle System in a separate function call, so if
            // we want to reset the Particle System later we don't need to completely 
            // re-initialize it, we can just call this function to reset it.
            LoadParticleSystem();
        }

        public void Reset()
        {
            this.RemoveAllParticles();
        }

        public void LoadParticleSystem()
        {
            ParticleInitializationFunction = InitializeParticleUsingInitialProperties;

            InitialProperties.LifetimeMin = 0.1f;
            InitialProperties.LifetimeMax = 0.4f;
            InitialProperties.PositionMin = Vector3.Zero;
            InitialProperties.PositionMax = Vector3.Zero;
            InitialProperties.VelocityMin = new Vector3(100, 100, 0);
            InitialProperties.VelocityMax = new Vector3(-100, -100, 0);
            InitialProperties.StartSizeMin = 4;
            InitialProperties.StartSizeMax = 8;
            InitialProperties.StartColorMin = canisterStartColor;
            InitialProperties.StartColorMax = canisterStartColor;
            InitialProperties.EndColorMin = canisterEndColor;
            InitialProperties.EndColorMax = canisterEndColor;

            ParticleEvents.RemoveAllEvents();
            ParticleSystemEvents.RemoveAllEvents();

            ParticleEvents.AddEveryTimeEvent(UpdateParticlePositionUsingVelocity);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleTransparencyToFadeOutUsingLerp);

            Emitter.EmitParticlesAutomatically = false;
            Emitter.Enabled = true;
        }

        public void Explode(Point location)
        {
            Emitter.PositionData.Position = new Vector3(location.X, location.Y, 0);
            this.AddParticles(PICKUP_NUM_PARTICLES);
        }
    }
}
