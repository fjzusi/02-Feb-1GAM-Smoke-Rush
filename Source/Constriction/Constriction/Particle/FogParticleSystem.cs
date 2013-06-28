using System;
using Constriction.Constrict;
using DPSF;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Constriction.Particle
{
	/// <summary>
	/// Create a new Particle System class that inherits from a Default DPSF Particle System.
	/// </summary>
	[Serializable]
	class FogParticleSystem : DefaultSpriteParticleSystem
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="cGame">Handle to the Game object being used. Pass in null for this 
		/// parameter if not using a Game object.</param>
        public FogParticleSystem(Game cGame) : base(cGame) { }

		/// <summary>
		/// Function to Initialize the Particle System with default values.
		/// Particle system properties should not be set until after this is called, as 
		/// they are likely to be reset to their default values.
		/// </summary>
		/// <param name="cGraphicsDevice">The Graphics Device the Particle System should use</param>
		/// <param name="cContentManager">The Content Manager the Particle System should use to load resources</param>
		/// <param name="cSpriteBatch">The Sprite Batch that the Sprite Particle System should use to draw its particles.
		/// If this is not initializing a Sprite particle system, or you want the particle system to use its own Sprite Batch,
		/// pass in null.</param>
		public void Initialize(GraphicsDevice cGraphicsDevice, ContentManager cContentManager, SpriteBatch cSpriteBatch, ConstrictionManager blockManager)
		{
            base.AutoInitialize(cGraphicsDevice, cContentManager, cSpriteBatch);
			
			base.InitializeSpriteParticleSystem(cGraphicsDevice, cContentManager, 1000, 50000, "Particle/FogParticle", cSpriteBatch);
			Name = "Fog Particle System";

			// Finish loading the Particle System in a separate function call, so if
			// we want to reset the Particle System later we don't need to completely 
			// re-initialize it, we can just call this function to reset it.
			LoadParticleSystem();
            InitializeEmitters(blockManager);
		}

        public void Reset()
        {
            this.RemoveAllParticles();
        }

		/// <summary>
		/// Load the Particle System Events and any other settings
		/// </summary>
		public void LoadParticleSystem()
		{
			ParticleInitializationFunction = InitializeParticleUsingInitialProperties;

			// Setup the Initial Properties of the Particles.
			// These are only applied if using InitializeParticleUsingInitialProperties 
			// as the Particle Initialization Function.
			InitialProperties.LifetimeMin = 1.0f;
			InitialProperties.LifetimeMax = 1.5f;
			InitialProperties.PositionMin = Vector3.Zero;
			InitialProperties.PositionMax = Vector3.Zero;
			InitialProperties.VelocityMin = new Vector3(10, 10, 0);
			InitialProperties.VelocityMax = new Vector3(-10, -10, 0);
            InitialProperties.StartSizeMin = 8;
            InitialProperties.StartSizeMax = 32;
			InitialProperties.StartColorMin = Color.Black;
            InitialProperties.StartColorMax = Color.Black;
			InitialProperties.EndColorMin = Color.Black;
            InitialProperties.EndColorMax = Color.Black;

			// Remove all Events first so that none are added twice if this function is called again
			ParticleEvents.RemoveAllEvents();
			ParticleSystemEvents.RemoveAllEvents();

			// Allow the Particle's Position, Rotation, Width and Height, Color, and Transparency to be updated each frame
            ParticleEvents.AddEveryTimeEvent(UpdateParticlePositionUsingVelocity);
			ParticleEvents.AddEveryTimeEvent(UpdateParticleTransparencyToFadeOutUsingLerp);
		}

        private void InitializeEmitters(ConstrictionManager blockManager)
        {
            foreach (var block in blockManager.constrictBlocksLeft)
            {
                var emitter = new ParticleEmitter();
                emitter.ParticlesPerSecond = 100;
                emitter.PositionData.Position = new Vector3(block.boundedBox.Right, block.boundedBox.Center.Y, 0);
                this.Emitters.Add(emitter);
            }

            foreach (var block in blockManager.constrictBlocksRight)
            {
                var emitter = new ParticleEmitter();
                emitter.ParticlesPerSecond = 100;
                emitter.PositionData.Position = new Vector3(block.boundedBox.Left, block.boundedBox.Center.Y, 0);
                this.Emitters.Add(emitter);
            }

            foreach (var block in blockManager.constrictBlocksTop)
            {
                var emitter = new ParticleEmitter();
                emitter.ParticlesPerSecond = 100;
                emitter.PositionData.Position = new Vector3(block.boundedBox.Center.X, block.boundedBox.Bottom, 0);
                this.Emitters.Add(emitter);
            }

            foreach (var block in blockManager.constrictBlocksBottom)
            {
                var emitter = new ParticleEmitter();
                emitter.ParticlesPerSecond = 100;
                emitter.PositionData.Position = new Vector3(block.boundedBox.Center.X, block.boundedBox.Top, 0);
                this.Emitters.Add(emitter);
            }
        }

        public void Update(GameTime time, ConstrictionManager blockManager)
        {
            UpdateEmitters(blockManager);
            base.Update((float)time.ElapsedGameTime.TotalSeconds);
        }

        private void UpdateEmitters(ConstrictionManager blockManager)
        {
            var index = 0;

            foreach (var block in blockManager.constrictBlocksLeft)
            {
                var emitter = this.Emitters[index];
                emitter.Enabled = block.isMoving;
                emitter.PositionData.Position = new Vector3(block.boundedBox.Right, block.boundedBox.Center.Y, 0);

                index++;
            }

            foreach (var block in blockManager.constrictBlocksRight)
            {
                var emitter = this.Emitters[index];
                emitter.Enabled = block.isMoving;
                emitter.PositionData.Position = new Vector3(block.boundedBox.Left, block.boundedBox.Center.Y, 0);

                index++;
            }

            foreach (var block in blockManager.constrictBlocksTop)
            {
                var emitter = this.Emitters[index];
                emitter.Enabled = block.isMoving;
                emitter.PositionData.Position = new Vector3(block.boundedBox.Center.X, block.boundedBox.Bottom, 0);

                index++;
            }

            foreach (var block in blockManager.constrictBlocksBottom)
            {
                var emitter = this.Emitters[index];
                emitter.Enabled = block.isMoving;
                emitter.PositionData.Position = new Vector3(block.boundedBox.Center.X, block.boundedBox.Top, 0);

                index++;
            }
        }
	}
}
