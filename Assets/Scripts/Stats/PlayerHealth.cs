using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class PlayerHealth : Health
    {
        [SerializeField]
        protected float iFrameTime;
        [SerializeField]
        protected float verticalDamageForce;
        [SerializeField]
        protected float horizontalDamageForce;
        [SerializeField]
        protected float slowDownTimeAmount;
        [SerializeField]
        protected float slowDownSpeed;
        protected SpriteRenderer[] sprites;
        protected Rigidbody2D rb;
        [HideInInspector]
        public bool invulnerable;
        [HideInInspector]
        public bool hit;
        [HideInInspector]
        public bool left;
        protected float originalTimeScale;

        protected override void Initialization()
        {
            base.Initialization();
            rb = GetComponent<Rigidbody2D>();
            sprites = GetComponentsInChildren<SpriteRenderer>();
        }
        protected virtual void FixedUpdate()
        {
            HandleFrames();
            HandleDamageMovement();
        }
        public override void DealDamage(int amount)
        {
            if (invulnerable || character.isDashing)
            {
                return;
            }
            base.DealDamage(amount);
            originalTimeScale = Time.timeScale;
            invulnerable = true;
            Invoke("Cancel", iFrameTime);
        }
        public virtual void HandleDamageMovement()
        {
            if (hit)
            {
                Time.timeScale = slowDownSpeed;
                rb.AddForce(Vector2.up * verticalDamageForce);
                if (!left)
                {
                    rb.AddForce(Vector2.right * horizontalDamageForce);
                }
                else
                {
                    rb.AddForce(Vector2.left * horizontalDamageForce);
                }
                Invoke("HitCancel", slowDownTimeAmount);
            }
        }
        protected virtual void HandleFrames()
        {
            Color spriteColors = new Color();
            if (invulnerable)
            {
                foreach(SpriteRenderer sprite in sprites)
                {
                    spriteColors = sprite.color;
                    spriteColors.a = .5f;
                    sprite.color = spriteColors;
                }
            }
            else
            {
                foreach (SpriteRenderer sprite in sprites)
                {
                    spriteColors = sprite.color;
                    spriteColors.a = 1;
                    sprite.color = spriteColors;
                }
            }
        }
        protected virtual void HitCancel()
        {
            hit = false;
            Time.timeScale = originalTimeScale;
        }
        protected virtual void Cancel()
        {
            invulnerable = false;
        }
        public virtual void GainCurrentHealth(int amount)
        {
            healthPoints += amount;
            if(healthPoints > maxHealthPoints)
            {
                healthPoints = maxHealthPoints;
            }
        }
    }
}