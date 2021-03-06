using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class Jump : Abilities
    {
        [SerializeField]
        protected bool limitAirJumps;
        [SerializeField]
        protected int maxJumps;
        [SerializeField]
        protected float jumpForce;
        [SerializeField]
        protected float holdForce;
        [SerializeField]
        protected float buttonHoldTime;
        [SerializeField]
        protected float distanceToCollider;
        [SerializeField]
        protected float horizontalWallJumpForce;
        [SerializeField]
        protected float verticalWallJumpForce;
        [SerializeField]
        protected float maxJumpSpeed;
        [SerializeField]
        protected float maxFallSpeed;
        [SerializeField]
        protected float acceptedFallSpeed;
        [SerializeField]
        protected float glideTime;
        [SerializeField]
        [Range(-2, 2)]
        protected float gravity;
        [SerializeField]
        protected float wallJumpTime;

        public LayerMask collisionLayer;

        private bool isWallJumping;
        private bool justWallJumped;
        private bool flipped;
        private int numberOfJumpsLeft;
        private float jumpCountDown;
        private float fallCountDown;
        private float wallJumpCountdown;

        protected override void Initialization()
        {
            base.Initialization();
            numberOfJumpsLeft = maxJumps;
            jumpCountDown = buttonHoldTime;
            fallCountDown = glideTime;
            wallJumpCountdown = wallJumpTime;
        }
        protected virtual void Update()
        {
            CheckForJump();
        }

        protected virtual bool CheckForJump()
        {
            if (gameManager.gamePaused)
            {
                return false;
            }
            if (input.JumpPressed())
            {
                if(currentPlatform != null && currentPlatform.GetComponent<OneWayPlatform>() && input.DownHeld())
                {
                    character.isJumpingThroughPlatform = true;
                    Invoke("NotJumpingThroughPlatform", .1f);
                    return false;
                }
                if (!character.isGrounded && numberOfJumpsLeft == maxJumps)
                {
                    character.isJumping = false;
                    return false;
                }
                if(limitAirJumps && character.Falling(acceptedFallSpeed))
                {
                    character.isJumping = false;
                    return false;
                }
                if (character.isWallSliding && wallJumpAbility)
                {
                    wallJumpTime = wallJumpCountdown;
                    isWallJumping = true;
                    return false;
                }
                numberOfJumpsLeft--;
                if(numberOfJumpsLeft >= 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    jumpCountDown = buttonHoldTime;
                    character.isJumping = true;
                    fallCountDown = glideTime;
                }
                return true;
            }
            else
                return false;
        }
        protected virtual void FixedUpdate()
        {
            IsJumping();
            Gliding();
            GroundCheck();
            WallSliding();
            WallJump();
        }

        protected virtual void IsJumping()
        {
            if (character.isJumping)
            {
                rb.AddForce(Vector2.up * jumpForce);
                AdditionalAir();
            }
            if(rb.velocity.y > maxJumpSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, maxJumpSpeed);
            }
        }
        protected virtual void Gliding()
        {
            if(character.Falling(0) && input.JumpHeld())
            {
                fallCountDown -= Time.deltaTime;
                if(fallCountDown > 0 && rb.velocity.y > acceptedFallSpeed)
                {
                    anim.SetBool("Gliding", true);
                    FallSpeed(gravity);
                    return;
                }
            }
            anim.SetBool("Gliding", false);
        }
        protected virtual void AdditionalAir()
        {
            if (input.JumpHeld())
            {
                jumpCountDown -= Time.deltaTime;
                if (jumpCountDown <= 0)
                {
                    jumpCountDown = 0;
                    character.isJumping = false;
                }
                else
                    rb.AddForce(Vector2.up * holdForce);
            }
            else
                character.isJumping = false;
        }
        protected virtual void GroundCheck()
        {
            if (CollisionCheck(Vector2.down, distanceToCollider, collisionLayer) && !character.isJumping)
            {
                if (currentPlatform.GetComponent<MoveablePlatform>())
                {
                    transform.parent = currentPlatform.transform;
                }
                anim.SetBool("Grounded", true);
                character.isGrounded = true;
                numberOfJumpsLeft = maxJumps;
                fallCountDown = glideTime;
                justWallJumped = false;
            }
            else
            {
                transform.parent = null;
                anim.SetBool("Grounded", false);
                character.isGrounded = false;
                if (character.Falling(0) && rb.velocity.y < maxFallSpeed)
                {
                    rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
                }
            }
            anim.SetFloat("VerticalSpeed", rb.velocity.y);
        }
        protected virtual bool WallCheck()
        {
            if ((!character.isFacingLeft && CollisionCheck(Vector2.right, distanceToCollider, collisionLayer) || character.isFacingLeft && CollisionCheck(Vector2.left, distanceToCollider, collisionLayer)) && movement.MovementPressed() && !character.isGrounded)
            {
                if (currentPlatform.GetComponent<OneWayPlatform>() || currentPlatform.GetComponent<Ladder>())
                {
                    return false;
                }
                if (justWallJumped)
                {
                    wallJumpTime = 0;
                    justWallJumped = false;
                    isWallJumping = false;
                    movement.enabled = true;
                }
                return true;
            }
            return false;
        }
        protected virtual bool WallSliding()
        {
            bool isTouchingWall = WallCheck();

            if (isTouchingWall)
            {
                if (!flipped)
                {
                    Flip();
                    flipped = true;
                }
                FallSpeed(gravity);
                character.isWallSliding = true;
                anim.SetBool("WallSliding", true);
                return true;
            }
            else
            {
                character.isWallSliding = false;
                anim.SetBool("WallSliding", false);
                if(flipped && !isWallJumping)
                {
                    Flip();
                    flipped = false;
                }
                return false;
            }
        }
        protected virtual void NotJumpingThroughPlatform()
        {
            character.isJumpingThroughPlatform = false;
        }
        protected virtual void WallJump()
        {
            if (isWallJumping)
            {
                rb.AddForce(Vector2.up * verticalWallJumpForce);
                if (!character.isFacingLeft)
                {
                    rb.AddForce(Vector2.left * horizontalWallJumpForce);
                }
                if (character.isFacingLeft)
                {
                    rb.AddForce(Vector2.right * horizontalWallJumpForce);
                }
                movement.enabled = false;
                Invoke("JustWallJumped", .05f);
            }
            if(wallJumpTime > 0)
            {
                wallJumpTime -= Time.deltaTime;
                if(wallJumpTime <= 0)
                {
                    movement.enabled = true;
                    isWallJumping = false;
                    wallJumpTime = 0;
                }
            }
        }
        protected virtual void JustWallJumped()
        {
            justWallJumped = true;
        }
    }
}
