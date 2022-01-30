using DG.Tweening;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerMovement : MonoBehaviour
{
    #region Variables
    [Header("States")]
    private PlayerStates m_CurrentState;
    private PlayerStates m_CurrentActionState;
    private PlayerStates m_PreviousActionState;

    [Header("Settings")]
    [SerializeField] private EntitySettings m_Settings;

    [Header("Input")]
    [SerializeField] private PlayerInputHandler m_InputHandler;

    [Header("Avatar")]
    [SerializeField] private Camera m_Camera;
    [SerializeField] private GameObject m_Avatar;
    private SpriteRenderer m_SpriteRenderer;
    private Rigidbody2D m_Rigidbody;
    private Vector2 m_OriginalSize;
    private bool m_FacingRight;

    [Header("Run")]
    private bool m_AllowRun;

    [Header("Jump")]
    private bool m_IsJumping;
    private bool m_GoingToJump;
    private bool m_IsWallJumping;
    private bool m_AllowCoyoteJump;
    private bool m_CoyoteJumpAvailable;
    private bool m_IsFalling;

    [Header("Climb")]
    private bool m_IsClimbing;
    private bool m_AllowClimbing;
    private bool m_IsClimbJumping;
    private bool m_IsLedgeHopping;

    [Header("Wall Slide")]

    [Header("Dash")]
    private int m_DashesAvailable;
    private bool m_IsDashing;

    [Header("Push")]

    [Header("Duck")]

    [Header("Collision")]
    private bool m_HitGround;
    private bool m_OnGround;
    private bool m_GenerousOnGround;
    private bool m_OnPlatform;

    private bool m_GenerousHitWall;
    private bool m_HitWall;
    private bool m_OnWall;
    private bool m_OnRightWall;
    private bool m_OnLeftWall;
    private bool m_GenerousOnWall;
    private bool m_GenerousOnRightWall;
    private bool m_GenerousOnLeftWall;

    private bool m_OnLedge;
    private bool m_OnRightLedge;
    private bool m_OnLeftLedge;

    private bool m_OnClimbAble;
    #endregion

    #region Unity Functions
    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_SpriteRenderer = m_Avatar.GetComponent<SpriteRenderer>();
        //m_Animator = m_Avatar.GetComponent<Animator>();
        m_OriginalSize = m_Avatar.transform.localScale;

        m_CurrentState = PlayerStates.inAir;
    }

    private void Start()
    {
        m_AllowRun = true;
        m_AllowClimbing = true;

        m_FacingRight = true;
        m_SpriteRenderer.flipX = false;

        m_DashesAvailable = m_Settings.MaxDashes;

        m_Rigidbody.gravityScale = m_Settings.DefaultGravity;
    }

    private void Update()
    {
        FlipSprite();

        UpdateOverlapShapes();

        CheckForState();
        GravityScaler();

        CheckForGroundHit();
        CheckForJumpBuffer();

        switch (m_CurrentState)
        {
            case PlayerStates.onGround:

                CheckForWallHit();

                CheckForDash();
                CheckForClimb();
                CheckForJump();

                Duck();
                Run();

                break;
            case PlayerStates.inAir:

                CheckForFalling();
                CheckForWallHit();

                CheckForCoyoteJump();

                CheckForDash();
                CheckForClimb();
                CheckForJump();
                CheckForWallSlide();

                Run();

                BetterJump();

                break;
            case PlayerStates.climbing:

                CheckForWallHit();
                CheckForWallSlide();

                CheckForDash();
                CheckForLedgeHop();
                CheckForClimb();
                CheckForJump();

                Climb();

                break;
            case PlayerStates.dashing:

                break;
            default:
                break;
        }

        CheckForAnimation();
        ClampRigidbodyVelocity();
    }
    #endregion

    #region Behaviour/Action Functions
    private void Run()
    {
        if (!m_AllowRun || !m_Settings.CanMove || m_IsLedgeHopping)
        {
            return;
        }

        if (m_IsWallJumping)
        {
            m_Rigidbody.velocity = Vector2.Lerp(m_Rigidbody.velocity, new Vector2(m_InputHandler.Horizontal * m_Settings.RunSpeed, m_Rigidbody.velocity.y), m_Settings.WallJumpLerp * Time.deltaTime);
        }
        else
        {
            m_Rigidbody.velocity = new Vector2(m_InputHandler.Horizontal * m_Settings.RunSpeed, m_Rigidbody.velocity.y);
        }
    }

    private void Duck()
    {
        if (!m_Settings.CanDuck)
        {
            return;
        }

    }

    private void Climb()
    {
        if (!m_OnClimbAble || !m_AllowClimbing)
        {
            return;
        }

        m_IsJumping = false;
        m_IsWallJumping = false;
        m_IsClimbJumping = false;

        float climbSpeed = m_InputHandler.Vertical > 0 ? m_Settings.ClimbUpSpeed : m_Settings.ClimbDownSpeed;

        m_Rigidbody.velocity = new Vector2(m_Rigidbody.velocity.x, m_InputHandler.Vertical * climbSpeed);

        if (m_GenerousOnRightWall)
        {
            m_FacingRight = true;
            m_SpriteRenderer.flipX = false;
        }
        else if (m_GenerousOnLeftWall)
        {
            m_FacingRight = false;
            m_SpriteRenderer.flipX = true;
        }
    }

    private void Jump(Vector2 direction)
    {
        if (m_Settings.CanSqueezeSprite)
        {
            StartJumpSqueezeSpriteTimer();
        }

        m_Rigidbody.velocity = Vector2.zero;
        m_Rigidbody.AddForce(direction * m_Settings.JumpForce, ForceMode2D.Impulse);

        m_CoyoteJumpAvailable = false;
        m_AllowCoyoteJump = false;
        m_GoingToJump = false;
        m_IsJumping = true;
    }

    private void WallJump()
    {
        Vector2 jumpDirection = m_GenerousOnRightWall ? Vector2.left : Vector2.right;

        Vector2 multiplier = true ? m_Settings.PushWallJumpMultiplier : m_Settings.NeutralWallJumpMultiplier;

        Vector2 direction = Vector2.up * multiplier.y + jumpDirection * multiplier.x;

        m_IsWallJumping = true;

        if (m_GenerousOnLeftWall)
        {
            m_FacingRight = true;
            m_SpriteRenderer.flipX = false;
        }
        else if (m_GenerousOnRightWall)
        {
            m_FacingRight = false;
            m_SpriteRenderer.flipX = true;
        }

        Jump(direction);
    }

    private void ClimbJump()
    {
        Vector2 jumpDirection = m_GenerousOnRightWall ? Vector2.left : Vector2.right;

        Vector2 direction = Vector2.up * m_Settings.ClimbJumpMultiplier.y + jumpDirection * m_Settings.ClimbJumpMultiplier.x;

        m_IsClimbJumping = true;

        Jump(direction);
    }

    private void LedgeHop()
    {
        if (m_Settings.CanSqueezeSprite)
        {
            StartJumpSqueezeSpriteTimer();
        }

        Vector2 hopDirection = m_GenerousOnRightWall ? Vector2.right : Vector2.left;

        m_CoyoteJumpAvailable = false;
        m_GoingToJump = false;

        StartCoroutine(LedgeHopTimer(hopDirection));
    }

    private void Dash(Vector2 direction)
    {
        if (m_Settings.CanScreenShake)
        {
            m_Camera.transform.DOComplete();
            m_Camera.transform.DOShakePosition(m_Settings.ShakeDuration, m_Settings.ShakeStrength, m_Settings.ShakeVibrato, m_Settings.ShakeRandomness, m_Settings.CanShakeSnap, m_Settings.CanShakeFadeOut);
        }

        m_Rigidbody.velocity = Vector2.zero;
        m_Rigidbody.AddForce(direction * m_Settings.DashSpeed, ForceMode2D.Impulse);

        m_CoyoteJumpAvailable = false;
        m_IsClimbing = false;
        m_GoingToJump = false;

        m_DashesAvailable = Mathf.Clamp(m_DashesAvailable - 1, 0, m_Settings.MaxDashes);
        StartDashingTimer(m_Settings.DashTime, direction);
    }

    private void WallSlide()
    {
        if (!m_Settings.CanWallSlide)
        {
            return;
        }

        float push = m_InputHandler.Horizontal != 0 ? 0 : m_Rigidbody.velocity.x;
        float slideSpeed = m_Settings.SlideSpeed;
        //float slideSpeed = m_MovementInput.y >= 0 ? m_Settings.SlideSpeed : m_Settings.SlideSpeed * m_Settings.SlideSpeedMultiplier;

        m_Rigidbody.velocity = new Vector2(push, -slideSpeed);
    }

    #endregion

    #region Check Functions
    private void CheckForState()
    {
        if (m_OnGround && !m_IsClimbing || m_OnPlatform && !m_IsClimbing) // && !m_IsDashing)
        {
            m_CurrentState = PlayerStates.onGround;
        }
        else if (!m_OnGround && !m_IsClimbing || !m_OnPlatform && !m_IsClimbing) // || m_IsDashing)
        {
            m_CurrentState = PlayerStates.inAir;
        }
        else if (m_IsClimbing && !m_IsDashing)
        {
            m_CurrentState = PlayerStates.climbing;
        }
        else if (m_IsDashing)
        {
            m_CurrentState = PlayerStates.dashing;
        }
    }

    private void GravityScaler()
    {
        if (!m_IsClimbing && !m_IsDashing)
        {
            m_Rigidbody.gravityScale = m_Settings.DefaultGravity;
        }
        else if (m_IsClimbing || m_IsDashing)
        {
            m_Rigidbody.gravityScale = 0f;
        }
    }

    private void CheckForJump()
    {
        if (m_IsJumping || m_IsDashing) // || m_IsDashing)
        {
            return;
        }

        if (m_GoingToJump) // Jump Buffer Started
        {
            if (m_GenerousOnWall && !m_GenerousOnGround && !m_InputHandler.GetClimb && !m_OnPlatform) // Check for wall jump
            {
                if (m_Settings.CanWallJump)
                {
                    StartDisableRunTimer(m_Settings.BlockRunTime);

                    WallJump(); // Wall Jump
                }
            }
            else if (m_OnWall && m_IsClimbing) // Check for climb jump
            {
                if (m_Settings.CanClimbJump)
                {
                    StartDisableClimbTimer(m_Settings.BlockClimbTime);
                    StartDisableRunTimer(m_Settings.BlockClimbTime);

                    ClimbJump(); // Climb Jump
                }
            }
            else if (m_OnGround || m_OnPlatform || m_AllowCoyoteJump) // Check for normal jump
            {
                if (m_Settings.CanJump)
                {
                    //if(m_AllowCoyoteJump)
                    //{
                    //    Debug.Log("Coyote Jumped");
                    //}

                    Jump(Vector2.up); // Normal Jump
                }
            }
        }
    }

    private void CheckForLedgeHop()
    {
        if (!m_Settings.CanLedgeHop || m_IsLedgeHopping || m_IsJumping || m_OnGround)
        {
            return;
        }

        if (!m_OnLedge && m_OnWall)
        {
            StartDisableClimbTimer(m_Settings.BlockClimbTime);
            StartDisableRunTimer(m_Settings.BlockRunTime);

            LedgeHop();
        }
    }

    private void CheckForFalling()
    {
        if (!m_OnGround && !m_OnPlatform && !m_IsClimbing && m_Rigidbody.velocity.y < 0) // Player is falling
        {
            m_IsJumping = false;
            m_IsWallJumping = false;
            m_IsClimbJumping = false;

            m_IsFalling = true;
        }
    }

    private void CheckForGroundHit()
    {
        if (!m_HitGround && m_OnGround || !m_HitGround && m_OnPlatform)
        {
            m_IsJumping = false;
            m_IsWallJumping = false;
            m_IsClimbJumping = false;
            m_IsFalling = false;
            m_DashesAvailable = m_Settings.MaxDashes;

            m_CoyoteJumpAvailable = true;

            if (m_GoingToJump || m_IsClimbing || m_Rigidbody.velocity.y > 0)
            {
                return;
            }

            if (m_Settings.CanSqueezeSprite)
            {
                StartHitGroundSqueezeSpriteTimer();
            }
        }
    }

    private void CheckForClimb()
    {
        if (!m_Settings.CanClimb)
        {
            return;
        }

        m_IsClimbing = m_InputHandler.GetClimb && m_OnClimbAble ? true : false;
    }

    private void CheckForDash()
    {
        if (!m_Settings.CanDash || m_IsDashing)
        {
            return;
        }

        if (m_InputHandler.GetDashDown && m_DashesAvailable > 0)
        {
            float xDirection = 0;
            if (m_InputHandler.Horizontal == 0 && m_InputHandler.Vertical == 0 || m_InputHandler.Vertical < 0 && m_OnGround || m_InputHandler.Vertical < 0 && m_OnPlatform)
            {
                xDirection = m_FacingRight ? 1f : -1f;
            }
            else
            {
                xDirection = m_InputHandler.Horizontal;
            }

            Vector2 dashDirection = new Vector2(xDirection, m_InputHandler.Vertical);

            Dash(dashDirection.normalized);
        }
    }

    private void CheckForWallSlide()
    {
        if (!m_Settings.CanWallSlide || m_IsJumping || m_InputHandler.GetClimb || m_IsDashing)
        {
            return;
        }

        if (!m_OnGround && !m_IsJumping && !m_OnPlatform)
        {
            if (m_InputHandler.Horizontal < 0 && m_OnLeftWall || m_InputHandler.Horizontal > 0 && m_OnRightWall)
            {
                WallSlide();
            }
        }
    }

    private void CheckForWallHit()
    {
        if (!m_GenerousHitWall && m_GenerousOnWall)
        {
            if (m_IsWallJumping || m_IsClimbJumping) // IsJumping
            {
                m_IsJumping = false;
                m_IsWallJumping = false;
                m_IsClimbJumping = false;
            }
        }
    }

    private void CheckForCoyoteJump()
    {
        if (!m_Settings.CanCoyoteJump)
        {
            return;
        }

        if (m_CoyoteJumpAvailable)
        {
            if (!m_AllowCoyoteJump && !m_OnGround && !m_IsJumping && !m_IsWallJumping && !m_IsClimbJumping && !m_OnWall && !m_IsDashing && !m_OnPlatform)
            {
                //Debug.Log("Coyote Timer Started!");
                StartCoyoteJumpTimer();
            }
        }
    }

    private void CheckForJumpBuffer()
    {
        if (m_InputHandler.GetJumpDown && m_InputHandler.GetJump)
        {
            if (!m_GoingToJump)
            {
                StartJumpBufferTimer();
            }
        }
    }

    private void CheckForAnimation()
    {
        if (m_IsDashing)
        {
            SetActionState(PlayerStates.dashing);
            return;
        }

        float xDirection = m_InputHandler.Horizontal;

        if (m_OnGround && !m_IsClimbing || m_OnPlatform && !m_IsClimbing)
        {
            if (m_Rigidbody.velocity.y <= 0.5f)
            {
                if (xDirection != 0)
                {
                    if (xDirection > 0 && m_OnRightWall || xDirection < 0 && m_OnLeftWall)
                    {
                        SetActionState(PlayerStates.pushing);
                    }
                    else if (xDirection > 0 && m_InputHandler.HorizontalAim < 0 || xDirection < 0 && m_InputHandler.HorizontalAim > 0)
                    {
                        SetActionState(PlayerStates.runningBackwards);
                    }
                    else
                    {
                        SetActionState(PlayerStates.runningForward);
                    }
                }
                else if (xDirection == 0)
                {
                    if (m_InputHandler.Vertical < 0)
                    {
                        if (m_FacingRight && m_InputHandler.HorizontalAim < 0 || !m_FacingRight && m_InputHandler.HorizontalAim > 0)
                        {
                            SetActionState(PlayerStates.duckingBackwards);
                        }
                        else
                        {
                            SetActionState(PlayerStates.duckingForward);
                        }
                    }
                    else if (m_FacingRight && m_InputHandler.HorizontalAim < 0 || !m_FacingRight && m_InputHandler.HorizontalAim > 0)
                    {
                        SetActionState(PlayerStates.idleBackwards);
                    }
                    else
                    {
                        SetActionState(PlayerStates.idleForward);
                    }
                }
            }
        }
        else if (!m_OnGround && !m_IsClimbing && !m_OnPlatform)
        {
            if (xDirection > 0 && m_OnRightWall && !m_IsJumping || xDirection < 0 && m_OnLeftWall && !m_IsJumping)
            {
                SetActionState(PlayerStates.wallSliding);
            }
            else
            {
                if (m_IsJumping || m_IsLedgeHopping || m_IsClimbJumping)
                {
                    SetActionState(PlayerStates.jumping);
                }
                else
                {
                    SetActionState(PlayerStates.falling);
                }
            }
        }
        else if (m_IsClimbing && m_OnWall)
        {
            if (m_InputHandler.Vertical == 0 || m_OnGround || m_OnPlatform)
            {
                SetActionState(PlayerStates.climbingIdle);
            }
            else
            {
                if (m_InputHandler.Vertical > 0)
                {
                    SetActionState(PlayerStates.climbingUp);
                }
                else if (m_InputHandler.Vertical < 0)
                {
                    SetActionState(PlayerStates.climbingDown);
                }
            }
        }
    }

    #endregion

    #region Timer Starters
    private void StartHitGroundSqueezeSpriteTimer()
    {
        IEnumerator hitGroundSqueezeSpriteCoroutine = SqueezeSpriteTimer(m_Settings.HitGroundSqueezeSpriteSize, m_Settings.HitGroundSqueezeSpriteLerp, m_Settings.HitGroundUnSqueezeSpriteLerp);
        StartCoroutine(hitGroundSqueezeSpriteCoroutine);
    }

    private void StartJumpSqueezeSpriteTimer()
    {
        IEnumerator jumpSqueezeSpriteCoroutine = SqueezeSpriteTimer(m_Settings.JumpSqueezeSpriteSize, m_Settings.JumpSqueezeSpriteLerp, m_Settings.JumpUnSqueezeSpriteLerp);
        StartCoroutine(jumpSqueezeSpriteCoroutine);
    }

    private void StartDisableRunTimer(float time)
    {
        IEnumerator disableRunCoroutine = DisableRunTimer(time);
        StartCoroutine(disableRunCoroutine);
    }

    private void StartDisableClimbTimer(float time)
    {
        IEnumerator disableClimbCoroutine = DisableClimbTimer(time);
        StartCoroutine(disableClimbCoroutine);
    }

    private void StartJumpBufferTimer()
    {
        IEnumerator jumpBufferCoroutine = JumpBufferTimer(m_Settings.JumpGraceTime);
        StartCoroutine(jumpBufferCoroutine);
    }

    private void StartDashingTimer(float time, Vector2 direction)
    {
        IEnumerator dashingCoroutine = DashingTimer(time, direction);
        StartCoroutine(dashingCoroutine);

        IEnumerator disableRunCoroutine = DisableRunTimer(time);
        StartCoroutine(disableRunCoroutine);

        IEnumerator disableClimbCoroutine = DisableClimbTimer(time);
        StartCoroutine(disableClimbCoroutine);
    }

    private void StartCoyoteJumpTimer()
    {
        IEnumerator coyoteJumpCoroutine = CoyoteJumpTimer();
        StartCoroutine(coyoteJumpCoroutine);
    }

    #endregion

    #region Coroutines

    private IEnumerator SqueezeSpriteTimer(Vector2 squeezeSize, float squeezeTime, float unSqueezeTime)
    {
        float elapsedTime = 0f;
        while (elapsedTime < squeezeTime)
        {
            elapsedTime += Time.deltaTime;
            m_Avatar.transform.localScale = Vector2.Lerp(m_OriginalSize, squeezeSize, elapsedTime / squeezeTime);

            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < unSqueezeTime)
        {
            elapsedTime += Time.deltaTime;
            m_Avatar.transform.localScale = Vector2.Lerp(squeezeSize, m_OriginalSize, elapsedTime / unSqueezeTime);

            yield return null;
        }
    }

    private IEnumerator DashingTimer(float time, Vector2 direction)
    {
        m_IsDashing = true;
        //m_DashEffect.Play();

        yield return new WaitForSeconds(time);

        //m_DashEffect.Stop();
        m_IsDashing = false;

        if (direction.y != -1)
        {
            m_Rigidbody.velocity = Vector2.zero;
        }

        if (m_OnGround || m_OnPlatform)
        {
            m_DashesAvailable = m_Settings.MaxDashes;
        }
    }

    private IEnumerator DisableRunTimer(float time)
    {
        m_AllowRun = false;

        yield return new WaitForSeconds(time);

        m_AllowRun = true;
    }

    private IEnumerator DisableClimbTimer(float time)
    {
        m_AllowClimbing = false;

        yield return new WaitForSeconds(time);

        m_AllowClimbing = true;
    }

    private IEnumerator JumpBufferTimer(float time)
    {
        m_GoingToJump = true;

        yield return new WaitForSeconds(time);

        m_GoingToJump = false;
    }

    private IEnumerator LedgeHopTimer(Vector2 direction)
    {
        m_Rigidbody.velocity = Vector2.zero;

        m_IsLedgeHopping = true;

        m_Rigidbody.AddForce(Vector2.up * m_Settings.LedgeHopMultiplier.y * m_Settings.JumpForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.1f);

        m_Rigidbody.velocity = Vector2.zero;

        m_Rigidbody.AddForce(direction * m_Settings.LedgeHopMultiplier.x * m_Settings.JumpForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.2f);

        m_IsLedgeHopping = false;
    }

    private IEnumerator CoyoteJumpTimer()
    {
        m_AllowCoyoteJump = true;

        yield return new WaitForSeconds(m_Settings.CoyoteTime);

        m_AllowCoyoteJump = false;
        m_CoyoteJumpAvailable = false;
    }

    #endregion

    #region Other Functions

    private void ClampRigidbodyVelocity()
    {
        m_Rigidbody.velocity = new Vector2(Mathf.Clamp(m_Rigidbody.velocity.x, m_Settings.MinVelocity.x, m_Settings.MaxVelocity.x), Mathf.Clamp(m_Rigidbody.velocity.y, m_Settings.MinVelocity.y, m_Settings.MaxVelocity.y));
    }

    private void BetterJump()
    {
        if (m_IsDashing)
        {
            return;
        }

        if (m_Settings.CanJump)
        {
            if (m_Rigidbody.velocity.y < 0)
            {
                float multiplier = m_InputHandler.GetJump ? 0.5f : 1f;

                m_Rigidbody.velocity += Vector2.up * (Physics2D.gravity.y * multiplier) * m_Settings.FallMultiplier;
            }
            else if (m_Rigidbody.velocity.y > 0 && !m_InputHandler.GetJump)
            {
                m_Rigidbody.velocity += Vector2.up * Physics2D.gravity.y * m_Settings.LowJumpMultiplier;
            }
        }
    }

    private void FlipSprite()
    {
        if (m_IsDashing || m_IsClimbing || m_IsWallJumping || m_IsLedgeHopping)
        {
            return;
        }

        if (m_InputHandler.Horizontal > 0)
        {
            m_FacingRight = true;
            m_SpriteRenderer.flipX = false;
        }
        else if (m_InputHandler.Horizontal < 0)
        {
            m_FacingRight = false;
            m_SpriteRenderer.flipX = true;
        }
    }

    private void SetActionState(PlayerStates state)
    {
        //if(m_PreviousActionState == state)
        //{
        //    return;
        //}

        m_PreviousActionState = m_CurrentActionState;
        m_CurrentActionState = state;

        //m_Animator.SetInteger("Action", (int)m_CurrentActionState);
    }

    private void UpdateOverlapShapes()
    {
        m_GenerousOnGround = Physics2D.OverlapBox(m_Rigidbody.position + m_Settings.GroundDetectionOffset, m_Settings.GenerousGroundDetectionSize, 0, m_Settings.GroundLayer); // The bottom OverlapCircle

        m_GenerousHitWall = m_GenerousOnWall;
        m_GenerousOnRightWall = Physics2D.OverlapBox(m_Rigidbody.position + m_Settings.GenerousRightWallDetectionOffset, m_Settings.GenerousWallDetectionSize, 0, m_Settings.GroundLayer); // Right OverlapBox
        m_GenerousOnLeftWall = Physics2D.OverlapBox(m_Rigidbody.position + m_Settings.GenerousLeftWallDetectionOffset, m_Settings.GenerousWallDetectionSize, 0, m_Settings.GroundLayer); // Left OverlapBox
        m_GenerousOnWall = m_GenerousOnRightWall || m_GenerousOnLeftWall;

        m_HitGround = m_OnGround || m_OnPlatform;
        m_OnGround = Physics2D.OverlapBox(m_Rigidbody.position + m_Settings.GroundDetectionOffset, m_Settings.GroundDetectionSize, 0, m_Settings.GroundLayer); // The bottom of the player
        m_OnPlatform = Physics2D.OverlapBox(m_Rigidbody.position + m_Settings.GroundDetectionOffset, m_Settings.GroundDetectionSize, 0, m_Settings.PlatformLayer); // The bottom of the player


        m_OnRightLedge = Physics2D.OverlapBox(m_Rigidbody.position + m_Settings.RightLedgeDetectionOffset, m_Settings.LedgeDetectionSize, 0, m_Settings.ClimbAbleLayer);
        m_OnLeftLedge = Physics2D.OverlapBox(m_Rigidbody.position + m_Settings.LeftLedgeDetectionOffset, m_Settings.LedgeDetectionSize, 0, m_Settings.ClimbAbleLayer);
        m_OnLedge = m_OnLeftLedge || m_OnRightLedge;

        m_HitWall = m_OnWall;
        m_OnRightWall = Physics2D.OverlapBox(m_Rigidbody.position + m_Settings.RightWallDetectionOffset, m_Settings.WallDetectionSize, 0, m_Settings.GroundLayer); // Right OverlapBox
        m_OnLeftWall = Physics2D.OverlapBox(m_Rigidbody.position + m_Settings.LeftWallDetectionOffset, m_Settings.WallDetectionSize, 0, m_Settings.GroundLayer); // Left OverlapBox
        m_OnWall = m_OnRightWall || m_OnLeftWall;

        m_OnClimbAble = m_OnWall; //|| Physics2D.OverlapCircle(m_Rigidbody.position, m_Settings.CircleRadius, m_Settings.ClimbAbleLayer); // Yellow circle on top
    }

    private void OnDrawGizmosSelected()
    {
        if (!m_Rigidbody)
        {
            m_Rigidbody = GetComponent<Rigidbody2D>();
            if (!m_Rigidbody)
            {
                Debug.LogError("There is no Rigidbody2D attached to this gameObject!");
                return;
            }
        }
        else
        {
            Gizmos.color = Color.yellow;
            //Gizmos.DrawWireSphere(m_Rigidbody.position, m_Settings.CircleRadius); // Circle for OnClimbAble

            Gizmos.color = Color.cyan;
            Gizmos.DrawCube(m_Rigidbody.position + m_Settings.GenerousGroundDetectionOffset, m_Settings.GenerousGroundDetectionSize); // Circle for GenerousOnGround
            Gizmos.DrawCube(m_Rigidbody.position + m_Settings.GenerousRightWallDetectionOffset, m_Settings.GenerousWallDetectionSize); // Box for GenerousOnRightWall
            Gizmos.DrawCube(m_Rigidbody.position + m_Settings.GenerousLeftWallDetectionOffset, m_Settings.GenerousWallDetectionSize); // Box for GenerousOnLeftWall

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(m_Rigidbody.position + m_Settings.GroundDetectionOffset, m_Settings.GroundDetectionSize); // Line/Box for OnGround
            Gizmos.DrawWireCube(m_Rigidbody.position + m_Settings.RightWallDetectionOffset, m_Settings.WallDetectionSize); // Line/Box for OnRightWall
            Gizmos.DrawWireCube(m_Rigidbody.position + m_Settings.LeftWallDetectionOffset, m_Settings.WallDetectionSize); // Line/Box for OnLeftWall

            Gizmos.DrawWireCube(m_Rigidbody.position + m_Settings.RightLedgeDetectionOffset, m_Settings.LedgeDetectionSize);
            Gizmos.DrawWireCube(m_Rigidbody.position + m_Settings.LeftLedgeDetectionOffset, m_Settings.LedgeDetectionSize);
        }
    }

    #endregion
}
