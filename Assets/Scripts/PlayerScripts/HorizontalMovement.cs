using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class HorizontalMovement : Abilities
    {
        [SerializeField]
        protected float timeTillMaxSpeed;
        [SerializeField]
        protected float maxSpeed;
        [SerializeField]
        protected float sprintMultiplier;
        [SerializeField]
        protected float crouchSpeedMultiplier;
        [SerializeField]
        protected float hookSpeedMultiplier;

        private float acceleraion;
        private float currentSpeed;
        private float horizontalInput;
        private float runTime;

        protected override void Initialization()
        {
            base.Initialization();
        }

        protected virtual void Update()
        {

            MovementPressed();
        }

        public virtual bool MovementPressed()
        {
            if (Input.GetAxis("Horizontal") != 0)
            {
                horizontalInput = Input.GetAxis("Horizontal");
                return true;
            }
            else
                return false;
        }
        protected virtual void FixedUpdate()
        {
            Movement();
            RemoveFromGrapple();
        }
        protected virtual void Movement()
        {
            if (MovementPressed())
            {
                anim.SetBool("Moving", true);
                acceleraion = maxSpeed / timeTillMaxSpeed;
                runTime += Time.deltaTime;
                currentSpeed = horizontalInput * acceleraion * runTime;
                CheckDirection();
            }
            else
            {
                anim.SetBool("Moving", false);
                acceleraion = 0;
                runTime = 0;
                currentSpeed = 0;
            }
            SpeedMultiplier();
            anim.SetFloat("CurrentSpeed", currentSpeed);
            rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
        }
        protected virtual void RemoveFromGrapple()
        {
            if (grapplingHook.removed)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.identity, Time.deltaTime * 500);
                if(transform.rotation == Quaternion.identity)
                {
                    grapplingHook.removed = false;
                    rb.freezeRotation = true;
                }
            }
        }
        protected virtual void CheckDirection()
        {
            if (currentSpeed > 0)
            {
                if (character.isFacingLeft)
                {
                    character.isFacingLeft = false;
                    Flip();
                }
                if (currentSpeed > maxSpeed)
                {
                    currentSpeed = maxSpeed;
                }
            }
            if (currentSpeed < 0)
            {
                if (!character.isFacingLeft)
                {
                    character.isFacingLeft = true;
                    Flip();
                }
                if (currentSpeed < -maxSpeed)
                {
                    currentSpeed = -maxSpeed;
                }
            }
        }

        protected virtual void SpeedMultiplier()
        {
            if (input.SprintingHeld())
            {
                currentSpeed *= sprintMultiplier;
            }
            if (character.isCrouching)
            {
                currentSpeed *= crouchSpeedMultiplier;
            }
            if (grapplingHook.connected)
            {
                if(input.UpHeld() || input.DownHeld() || CollisionCheck(Vector2.right, .1f, jump.collisionLayer) || CollisionCheck(Vector2.left, .1f, jump.collisionLayer) || CollisionCheck(Vector2.down, .1f, jump.collisionLayer) || CollisionCheck(Vector2.up, .1f, jump.collisionLayer) || character.isGrounded)
                {
                    return;
                }
                currentSpeed *= hookSpeedMultiplier;
                if(grapplingHook.hookTrail.transform.position.y > grapplingHook.objectConnectedTo.transform.position.y)
                {
                    currentSpeed *= -hookSpeedMultiplier;
                }
                rb.rotation -= currentSpeed;
            }
            if (character.isWallSliding)
            {
                currentSpeed = 0;
            }
            if(!character.isFacingLeft && CollisionCheck(Vector2.right, .05f, jump.collisionLayer) || character.isFacingLeft && CollisionCheck(Vector2.left, .05f, jump.collisionLayer))
            {
                currentSpeed = .01f;
            }
        }
    }
}