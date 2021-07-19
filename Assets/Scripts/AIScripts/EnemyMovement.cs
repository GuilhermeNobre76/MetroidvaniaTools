﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MetroidvaniaTools
{
    public class EnemyMovement : AIManagers
    {
        [SerializeField]
        protected enum MovementType { Normal, HugWalls }
        [SerializeField]
        protected MovementType type;
        [SerializeField]
        protected bool spawnFacingLeft;
        [SerializeField]
        protected bool turnAroundOnCollision;
        [SerializeField]
        protected bool avoidFalling;
        [SerializeField]
        protected float timeTillMaxSpeed;
        [SerializeField]
        protected float maxSpeed;
        [SerializeField]
        protected LayerMask collidersToTurnAroundOn;

        private bool spawning = true;
        private float acceleraion;
        private float direction;
        private float runTime;

        protected bool wait;

        protected float originalWaitTime = .1f;
        protected float currentWaitTime;
        protected float currentSpeed;

        protected override void Initialization()
        {
            base.Initialization();
            if (spawnFacingLeft)
            {
                enemyCharacter.facingLeft = true;
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            }
            currentWaitTime = originalWaitTime;
            Invoke("Spawning", .01f);
        }

        protected virtual void FixedUpdate()
        {
            Movement();
            CheckGround();
            EdgeOfFloor();
            HandleWait();
            HugWalls();
        }

        protected virtual void Movement()
        {
            if (!enemyCharacter.facingLeft)
            {
                direction = 1;
                if(CollisionCheck(Vector2.right, .5f, collidersToTurnAroundOn) && turnAroundOnCollision && !spawning)
                {
                    enemyCharacter.facingLeft = true;
                    transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                }
            }
            else
            {
                direction = -1;
                if (CollisionCheck(Vector2.left, .5f, collidersToTurnAroundOn) && turnAroundOnCollision && !spawning)
                {
                    enemyCharacter.facingLeft = false;
                    transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                }
            }
            acceleraion = maxSpeed / timeTillMaxSpeed;
            runTime += Time.deltaTime;
            currentSpeed = direction * acceleraion * runTime;
            CheckSpeed();
            rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
        }
        protected virtual void CheckSpeed()
        {
            if(currentSpeed > maxSpeed)
            {
                currentSpeed = maxSpeed;
            }
            if(currentSpeed < -maxSpeed)
            {
                currentSpeed = -maxSpeed;
            }
        }
        protected virtual void EdgeOfFloor()
        {
            if(rayHitNumber == 1 && avoidFalling && !wait)
            {
                currentWaitTime = originalWaitTime;
                wait = true;
                enemyCharacter.facingLeft = !enemyCharacter.facingLeft;
                transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
            }
        }
        protected virtual void Spawning()
        {
            spawning = false;
        }
        protected virtual void HugWalls()
        {
            if(type == MovementType.HugWalls)
            {
                turnAroundOnCollision = false;
                float newZValue = transform.localEulerAngles.z;
                rb.gravityScale = 0;
                if (rayHitNumber == 1 && !wait)
                {
                    wait = true;
                    currentWaitTime = originalWaitTime;
                    rb.velocity = Vector2.zero;
                    if (!enemyCharacter.facingLeft)
                    {
                        transform.localEulerAngles = new Vector3(0, 0, newZValue - 90);
                    }
                    else
                    {
                        transform.localEulerAngles = new Vector3(0, 0, newZValue + 90);
                    }
                }
                if(Mathf.Round(transform.eulerAngles.z) == 0)
                {
                    rb.velocity = new Vector2(currentSpeed, 0);
                }
                if (Mathf.Round(transform.eulerAngles.z) == 90)
                {
                    rb.velocity = new Vector2(0, currentSpeed);
                }
                if (Mathf.Round(transform.eulerAngles.z) == 180)
                {
                    rb.velocity = new Vector2(-currentSpeed, 0);
                }
                if (Mathf.Round(transform.eulerAngles.z) == 270)
                {
                    rb.velocity = new Vector2(0, -currentSpeed);
                }
                if(rayHitNumber == 0 && !wait)
                {
                    transform.localEulerAngles = Vector3.zero;
                    rb.gravityScale = 1;
                }
            }
        }
        protected virtual void HandleWait()
        {
            currentWaitTime -= Time.deltaTime;
            if(currentWaitTime <= 0)
            {
                wait = false;
                currentWaitTime = 0;
            }
        }
    }
}