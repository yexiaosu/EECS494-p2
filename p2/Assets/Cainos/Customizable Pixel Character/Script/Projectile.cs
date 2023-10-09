using UnityEngine;

namespace Cainos.CustomizablePixelCharacter
{
    public class Projectile : MonoBehaviour
    {
        public float lifeTime = 10.0f;

        private float timer;

        public bool Launched
        {
            get { return launched; }
            set
            {
                launched = value;

                if ( launched) Rigidbody2D.simulated = true;
            }
        }
        private bool launched;

        public Vector2 Velocity
        {
            get
            {
                return Rigidbody2D.velocity;
            }
            set
            {
                Rigidbody2D.velocity = value;
            }
        }

        public virtual bool HasHit
        {
            get { return hasHit; }
            protected set
            {
                if (hasHit == value) return;
                hasHit = value;

                Rigidbody2D.gravityScale = 1.0f;
            }
        }
        protected bool hasHit;

        protected Rigidbody2D Rigidbody2D
        {
            get
            {
                if (rigidbody2D == null) rigidbody2D = GetComponent<Rigidbody2D>();
                return rigidbody2D;
            }
        }
        protected new Rigidbody2D rigidbody2D;

        private void Start()
        {
            Rigidbody2D.simulated = false;
        }

        protected virtual void Update()
        {
            if (Launched == false) return;

            timer += Time.deltaTime;
            if ( timer > lifeTime)
            {
                Destroy();
            }
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            HasHit = true;
        }

        protected virtual void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
