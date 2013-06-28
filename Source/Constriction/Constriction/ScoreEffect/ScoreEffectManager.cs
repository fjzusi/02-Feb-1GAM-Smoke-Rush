using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Constriction.ScoreEffect
{
    public class ScoreEffectManager
    {
        private List<ScoreEffect> scoreEffects;

        public ScoreEffectManager()
        {
            scoreEffects = new List<ScoreEffect>();
        }

        public void Reset()
        {
            scoreEffects.Clear();
        }

        public void AddScoreEffect(Point startPosition, float displayNumber)
        {
            scoreEffects.Add(new ScoreEffect(startPosition.X, startPosition.Y, (int)displayNumber));
        }

        public void Update(GameTime time)
        {
            var removeScoreEffects = new List<ScoreEffect>();

            foreach (var scoreEffect in scoreEffects)
            {
                scoreEffect.Update(time);

                if (scoreEffect.effectTimer <= 0)
                {
                    removeScoreEffects.Add(scoreEffect);
                }
            }

            foreach (var removeScoreEffect in removeScoreEffects)
            {
                scoreEffects.Remove(removeScoreEffect);
            }
        }

        public void Draw(SpriteBatch batch)
        {
            foreach (var scoreEffect in scoreEffects)
            {
                scoreEffect.Draw(batch);
            }
        }
    }
}
