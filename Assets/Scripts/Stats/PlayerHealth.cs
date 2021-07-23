using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class PlayerHealth : Health
    {
        [SerializeField]
        protected float iFrameTime;
        protected SpriteRenderer[] sprites;
        [HideInInspector]
        public bool invulnerable;

        protected override void Initialization()
        {
            base.Initialization();
            sprites = GetComponentsInChildren<SpriteRenderer>();
        }
        protected virtual void FixedUpdate()
        {
            HandleFrames();
        }
        public override void DealDamage(int amount)
        {
            if (invulnerable)
            {
                return;
            }
            base.DealDamage(amount);
            invulnerable = true;
            Invoke("Cancel", iFrameTime);
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
        protected virtual void Cancel()
        {
            invulnerable = false;
        }
    }
}