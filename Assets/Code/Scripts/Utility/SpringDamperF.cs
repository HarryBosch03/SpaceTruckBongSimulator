using UnityEngine;

namespace SpaceTruckBongSimulator.Utility
{
    [System.Serializable]
    public sealed class SpringDamperF
    {
        public float mass = 1.0f;
        public float spring = 100.0f;
        public float damper = 10.0f;
        public bool useLimits;
        public Vector2 limits = Vector2.right;
        [Range(0.0f, 1.0f)]public float bounce;
        
        [HideInInspector] public float position, velocity, frameAcceleration;

        public void AddImpulse(float force) => frameAcceleration += force / mass;
        public void AddForce(float force) => frameAcceleration += force * Time.deltaTime / mass;

        public void Update(float target)
        {
            frameAcceleration += (target - position) * spring - velocity * damper;
            
            Integrate();
            Collide();
        }

        private void Collide()
        {
            if (!useLimits) return;

            var min = Mathf.Min(limits.x, limits.y);
            var max = Mathf.Max(limits.x, limits.y);
            
            if (position > max)
            {
                position = max;
                if (velocity > 0.0f) velocity = -velocity * bounce;
            }

            if (position < min)
            {
                position = min;
                if (velocity < 0.0f) velocity = -velocity * bounce;
            }
        }

        private void Integrate()
        {
            position += velocity * Time.deltaTime;
            velocity += frameAcceleration * Time.deltaTime;

            frameAcceleration = 0.0f;
        }
    }
}
