using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using Engine;
using Physics;
using Animation;

namespace Engine
{
    public class PlayerMovement : Component, IScript
    {
        #region XMovement
        private float moveSpeed = 100;
        private float moveInput;
        #endregion

        #region NormalJump
        private float jumpForce = 10;

        private float jumpTime = 0.3f;
        private float jumpTimeCounter;

        private bool isJumping;

        private float hangTime = 0.1f;
        private float hangTimeCounter;

        private float jumpBufferLength = 0.1f;
        private float jumpBufferLengthCounter;

        public Transform feetPos;

        public Collider groundCheck;
        #endregion

        #region WallJump
        bool isWallJumping;
        float wallJumpTime = 0.3f;
        float wallJumpTimer;

        float wallJumpForce = 15;

        int dir = 0;

        public Collider wallCheck;
        #endregion

        //Physics
        float maxVelocityX = 10;
        float maxVelocityY = 30;
        public PhysicsBody pB;

        //Sprite & Animation
        Sprite sprite;
        Animator anim;
        PlayerStates playerStates = PlayerStates.idle;


        public override void Start()
        {
            pB = gameEntity.GetComponent<PhysicsBody>();
            anim = gameEntity.GetComponent<Animator>();
            sprite = gameEntity.GetComponent<Sprite>();
        }

        void HandleAnimation()
        {
            if (groundCheck.isColliding)
            {
                if (pB.velocity.X != 0)
                {
                    if (playerStates != PlayerStates.running)
                    {
                        playerStates = PlayerStates.running;
                        anim.PlayAnimation("Run");
                    }
                }
                else
                {
                    if (playerStates != PlayerStates.idle)
                    {
                        playerStates = PlayerStates.idle;
                        anim.PlayAnimation("Idle");
                    }
                }
            }
            else
            {
                if (pB.velocity.Y < 0)
                {
                    if (playerStates != PlayerStates.jump)
                    {
                        playerStates = PlayerStates.jump;
                        anim.PlayAnimation("Jump");
                    }
                }
                else
                {
                    if (playerStates != PlayerStates.fall)
                    {
                        playerStates = PlayerStates.fall;
                        anim.PlayAnimation("Fall");
                    }
                }
            }
        }
        public override void Update(float delta)
        {
            HandleAnimation();

            if (!isWallJumping)
            {
                Inputs(delta);
                Jump(delta);
                xMovement(delta);

                if (wallCheck.isColliding && !groundCheck.isColliding && pB.velocity.Y > 0)
                {
                    pB.dragY = 20;
                    if (Raylib.IsKeyPressed(KeyboardKey.Space))
                    {
                        isWallJumping = true;
                        pB.dragY = 0;
                        pB.velocity.Y = -wallJumpForce;
                        moveInput = dir;
                    }
                }
                else
                {
                    pB.dragY = 0;
                }
            }
            else
            {
                WallJump(delta);
            }

            //Hanterar spelarens hastighet
            if (Math.Abs(pB.velocity.X) < 1) { pB.velocity.X = 0; }

            pB.velocity.X = Math.Clamp(pB.velocity.X, -maxVelocityX, maxVelocityX);
            pB.velocity.Y = Math.Clamp(pB.velocity.Y, -maxVelocityY, maxVelocityY);
        }
        void Inputs(float delta)
        {
            moveInput = 0;
            if (Raylib.IsKeyDown(KeyboardKey.D))
            {
                moveInput++;
            }
            if (Raylib.IsKeyDown(KeyboardKey.A))
            {
                moveInput--;
            }

            //manage hangtime
            if (groundCheck.isColliding)
            {
                hangTimeCounter = hangTime;
            }
            else
            {
                hangTimeCounter -= delta;
            }
            //manage jump buffer
            if (Raylib.IsKeyPressed(KeyboardKey.Space))
            {
                jumpBufferLengthCounter = jumpBufferLength;
            }
            else
            {
                jumpBufferLengthCounter -= delta;
            }

            //manage jump
            if (jumpBufferLengthCounter >= 0 && hangTimeCounter > 0)
            {
                isJumping = true;
                jumpTimeCounter = jumpTime;
                jumpBufferLengthCounter = 0;
                hangTimeCounter = 0;
            }

            if (Raylib.IsKeyDown(KeyboardKey.Space) && isJumping == true)
            {
                if (jumpTimeCounter > 0)
                {
                    jumpTimeCounter -= delta;
                }
                else
                {
                    isJumping = false;
                }
            }

            if (!Raylib.IsKeyDown(KeyboardKey.Space))
            {
                isJumping = false;
            }
        }
        void xMovement(float delta)
        {
            pB.velocity.X += moveInput * moveSpeed * delta;

            if (moveInput > 0)
            {
                wallCheck.gameEntity.transform.position.X = 0.3f;

                dir = -1;

                sprite.isFlipedX = false;
            }
            else if (moveInput < 0)
            {
                wallCheck.gameEntity.transform.position.X = -0.3f;

                dir = 1;

                sprite.isFlipedX = true;
            }
        }
        void Jump(float delta)
        {
            if (isJumping)
            {
                pB.velocity.Y = -jumpForce * delta * 60;
            }
        }

        void WallJump(float delta)
        {
            xMovement(delta);

            wallJumpTimer -= delta;
            if (wallJumpTimer <= 0)
            {
                wallJumpTimer = wallJumpTime;
                isWallJumping = false;
            }
        }
        public enum PlayerStates
        {
            idle, running, jump, fall
        }
    }
}
