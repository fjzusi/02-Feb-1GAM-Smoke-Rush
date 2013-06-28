using System;
using DPSF;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Constriction.Particle
{
    [Serializable]
    public class DamageParticleSystem : DefaultSpriteParticleSystem
    {
        public const int DAMAGE_NUM_PARTICLES = 100;

        public DamageParticleSystem(Game cGame) : base(cGame) { }

        public void Initialize(GraphicsDevice cGraphicsDevice, ContentManager cContentManager, SpriteBatch cSpriteBatch)
        {
            base.AutoInitialize(cGraphicsDevice, cContentManager, cSpriteBatch);
            base.InitializeSpriteParticleSystem(cGraphicsDevice, cContentManager, 100, 100, "Graphic/Empty", cSpriteBatch);

            Name = "Damage Particle System";

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
            InitialProperties.VelocityMin = new Vector3(250, 250, 0);
            InitialProperties.VelocityMax = new Vector3(-250, -250, 0);
            InitialProperties.StartSizeMin = 1;
            InitialProperties.StartSizeMax = 3;
            InitialProperties.EndSizeMin = 1;
            InitialProperties.EndSizeMax = 3;
            InitialProperties.StartColorMin = Color.Yellow;
            InitialProperties.StartColorMax = Color.Red;
            InitialProperties.EndColorMin = Color.White;
            InitialProperties.EndColorMax = Color.White;

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
            this.AddParticles(DAMAGE_NUM_PARTICLES);
        }
    }
}
