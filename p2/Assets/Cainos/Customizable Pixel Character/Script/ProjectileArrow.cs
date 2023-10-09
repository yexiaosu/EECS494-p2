using UnityEngine;

namespace Cainos.CustomizablePixelCharacter
{

    public class ProjectileArrow : Projectile
    {
        public float hitZPos = 1.0f;
        public float insertDepth = 0.1f;
        public float insertAngle = 60.0f;

        private Vector2 hitVel;

        private bool hasInsertedIntoTarget;
        private float curInsertDepth;

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            if (HasHit == true) return;
            base.OnCollisionEnter2D(collision);

            if ( Vector2.Angle(collision.contacts[0].normal, -transform.right) < insertAngle)
            {
                hasInsertedIntoTarget = true;

                transform.SetParent(collision.collider.transform, true);
                hitVel = -collision.relativeVelocity;
                Rigidbody2D.simulated = false;
            }

            Vector3 pos = transform.localPosition;
            pos.z = hitZPos;
            transform.localPosition = pos;
        }

        protected override void Update()
        {
            if (Launched == false) return;

            base.Update();

            if (hasHit == false)
            {
                float angle = Mathf.Atan2(Rigidbody2D.velocity.y, Rigidbody2D.velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

            if ( hasInsertedIntoTarget)
            {
                if (curInsertDepth < insertDepth)
                {
                    transform.Translate(hitVel * Time.deltaTime, Space.World);
                    curInsertDepth += hitVel.magnitude * Time.deltaTime;
                }


            }
        }
    }
}
