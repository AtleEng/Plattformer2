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
        float wallJumpTime = 0.2f;
        float wallJumpTimer;

        public Collider wallCheck;
        #endregion

        //Physics
        float maxVelocityX = 10;
        float maxVelocityY = 30;
        public PhysicsBody pB;

        //Sprite & Animation
        Sprite sprite;
        Animator anim;
        PlayerStates playerState = PlayerStates.idle;

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
                    if (playerState != PlayerStates.running)
                    {
                        playerState = PlayerStates.running;
                        anim.PlayAnimation("Run");
                    }
                }
                else
                {
                    if (playerState != PlayerStates.idle)
                    {
                        playerState = PlayerStates.idle;
                        anim.PlayAnimation("Idle");
                    }
                }
            }
            else
            {
                if (pB.velocity.Y < 0)
                {
                    if (playerState != PlayerStates.jump)
                    {
                        playerState = PlayerStates.jump;
                        anim.PlayAnimation("Jump");
                    }
                }
                else
                {
                    if (wallCheck.isColliding && moveInput != 0)
                    {
                        if (playerState != PlayerStates.wallGliding)
                        {
                            playerState = PlayerStates.wallGliding;
                        }
                    }
                    else
                    {
                        if (playerState != PlayerStates.fall)
                        {
                            playerState = PlayerStates.fall;
                            anim.PlayAnimation("Fall");
                        }
                    }
                }
            }
        }
        public override void Update(float delta)
        {
            HandleAnimation();

            Inputs(delta);

            if (isWallJumping)
            {
                WallJump(delta);
            }
            else
            {
                XInput();
            }
            xMovement(delta);
            Jump(delta);

            System.Console.WriteLine(moveInput);

            //Hanterar spelarens hastighet
            if (Math.Abs(pB.velocity.X) < 1) { pB.velocity.X = 0; }

            pB.velocity.X = Math.Clamp(pB.velocity.X, -maxVelocityX, maxVelocityX);
            pB.velocity.Y = Math.Clamp(pB.velocity.Y, -maxVelocityY, maxVelocityY);
        }
        void XInput()
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
        }
        void Inputs(float delta)
        {
            //manage jump buffer
            if (Raylib.IsKeyPressed(KeyboardKey.Space))
            {
                jumpBufferLengthCounter = jumpBufferLength;
            }
            else
            {
                jumpBufferLengthCounter -= delta;
            }

            if (playerState == PlayerStates.wallGliding)
            {
                pB.dragY = 20;
                if (jumpBufferLengthCounter >= 0)
                {
                    moveInput *= -1;
                    pB.dragY = 0;
                    isWallJumping = true;
                }
            }
            else
            {
                pB.dragY = 0;
                //manage hangtime
                if (groundCheck.isColliding)
                {
                    hangTimeCounter = hangTime;
                }
                else
                {
                    hangTimeCounter -= delta;
                }
            }
            if (jumpBufferLengthCounter >= 0 && (hangTimeCounter > 0 || playerState == PlayerStates.wallGliding))
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
            else
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

                sprite.isFlipedX = false;
            }
            else if (moveInput < 0)
            {
                wallCheck.gameEntity.transform.position.X = -0.3f;

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
            wallJumpTimer -= delta;
            if (wallJumpTimer <= 0)
            {
                wallJumpTimer = wallJumpTime;
                isWallJumping = false;
            }
        }
        public enum PlayerStates
        {
            idle, running, jump, fall, wallGliding
        }
    }
}
