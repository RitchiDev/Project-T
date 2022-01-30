using UnityEngine;

[CreateAssetMenu(fileName = "New Entity Settings", menuName = "Create Entity Settings")]
public class EntitySettings : ScriptableObject
{
    [Header("Entity")]
    [SerializeField] private bool m_CanDie = false;
    [Range(1f, 100f)]
    [SerializeField] private float m_StartHealth = 6f;
    [Range(1f, 100f)]
    [SerializeField] private float m_MaxHealth = 20f;
    [Range(0f, 100f)]
    [SerializeField] private float m_StartArmor = 5f;
    [Range(0f, 100f)]
    [SerializeField] private float m_MaxArmor = 5f;
    [Range(0f, 10f)]
    [SerializeField] private float m_TimeInvincible = 0.6f;
    public bool CanDie => m_CanDie;
    public float StartHealth => m_StartHealth;
    public float MaxHealth => m_MaxHealth;
    public float StartArmor => m_StartArmor;
    public float MaxArmor => m_MaxArmor;
    public float TimeInvincible => m_TimeInvincible;

    [Header("Avatar")]
    [SerializeField] private bool m_CanSqueezeSprite = false;
    [SerializeField] private Vector2 m_JumpSqueezeSpriteSize = Vector2.zero;
    [Range(0f, 2f)]
    [SerializeField] private float m_JumpSqueezeSpriteLerp = 0.115f;
    [Range(0f, 2f)]
    [SerializeField] private float m_JumpUnSqueezeSpriteLerp = 0.1f;
    [SerializeField] private Vector2 m_HitGroundSqueezeSpriteSize = Vector2.zero;
    [Range(0f, 2f)]
    [SerializeField] private float m_HitGroundSqueezeSpriteLerp = 0.05f;
    [Range(0f, 2f)]
    [SerializeField] private float m_HitGroundUnSqueezeSpriteLerp = 0.05f;
    public bool CanSqueezeSprite => m_CanSqueezeSprite;
    public Vector2 JumpSqueezeSpriteSize => m_JumpSqueezeSpriteSize;
    public Vector2 HitGroundSqueezeSpriteSize => m_HitGroundSqueezeSpriteSize;
    public float JumpSqueezeSpriteLerp => m_JumpSqueezeSpriteLerp;
    public float JumpUnSqueezeSpriteLerp => m_JumpUnSqueezeSpriteLerp;
    public float HitGroundSqueezeSpriteLerp => m_HitGroundSqueezeSpriteLerp;
    public float HitGroundUnSqueezeSpriteLerp => m_HitGroundUnSqueezeSpriteLerp;

    [Header("Mover")]
    [SerializeField] private bool m_CanMove = false;
    [Range(1f, 100f)]
    [SerializeField] private float m_RunSpeed = 7f;
    [Range(0f, 100f)]
    [SerializeField] private float m_StickTime = 0.5f;
    public bool CanMove => m_CanMove;
    public float RunSpeed => m_RunSpeed;
    public float StickTime => m_StickTime;

    [Header("Jumper")]
    [SerializeField] private bool m_CanJump = false;
    [SerializeField] private bool m_CanWallJump = false;
    [SerializeField] private bool m_CanClimbJump = false;
    [Tooltip("Coyote jump is being able to jump after leaving the ground")]
    [SerializeField] private bool m_CanCoyoteJump = false;
    [SerializeField] private bool m_CanLedgeHop = false;
    [SerializeField] private bool m_CanWallJumpDirectionBuffer = false;
    [Range(1f, 50f)]
    [SerializeField] private float m_JumpForce = 12f;
    [Range(0f, 10f)]
    [SerializeField] private float m_JumpGraceTime = 0.1f;
    [Range(0.05f, 10f)]
    [SerializeField] private float m_CoyoteTime = 0.08f;
    [Range(0.001f, 20f)]
    [SerializeField] private float m_LowJumpMultiplier = 0.1f;
    [Range(0.001f, 20f)]
    [SerializeField] private float m_FallMultiplier = 0.05f;
    [Range(0f, 20f)]
    [SerializeField] private float m_DefaultGravity = 3f;
    [Range(0f, 50f)]
    [Tooltip("Amount of time it takes for the player to reach his normal horizontal speed after a wall jump")]
    [SerializeField] private float m_WallJumpLerp = 4f;
    [Range(0f, 10f)]
    [SerializeField] private float m_BlockRunTime = 0.15f;
    [SerializeField] private Vector2 m_NeutralWallJumpMultiplier = Vector2.zero;
    [SerializeField] private Vector2 m_PushWallJumpMultiplier = Vector2.zero;
    public bool CanJump => m_CanJump;
    public bool CanWallJump => m_CanWallJump;
    public bool CanClimbJump => m_CanClimbJump;
    public bool CanCoyoteJump => m_CanCoyoteJump;
    public bool CanLedgeHop => m_CanLedgeHop;
    public bool CanWallJumpDirectionBuffer => m_CanWallJumpDirectionBuffer;
    public float JumpForce => m_JumpForce;
    public float JumpGraceTime => m_JumpGraceTime;
    public float CoyoteTime => m_CoyoteTime;
    public float LowJumpMultiplier => m_LowJumpMultiplier;
    public float FallMultiplier => m_FallMultiplier;
    public float DefaultGravity => m_DefaultGravity;
    public float WallJumpLerp => m_WallJumpLerp;
    public float BlockRunTime => m_BlockRunTime;
    public Vector2 NeutralWallJumpMultiplier => m_NeutralWallJumpMultiplier;
    public Vector2 PushWallJumpMultiplier => m_PushWallJumpMultiplier;

    [Header("Climber")]
    [SerializeField] private bool m_CanClimb = false;
    [Range(1f, 100f)]
    [SerializeField] private float m_ClimbUpSpeed = 3.5f;
    [Range(1f, 100f)]
    [SerializeField] private float m_ClimbDownSpeed = 5f;
    [Range(0f, 100f)]
    [SerializeField] private float m_BlockClimbTime = 0.15f;
    [SerializeField] private Vector2 m_LedgeHopMultiplier;
    [SerializeField] private Vector2 m_ClimbJumpMultiplier;
    public bool CanClimb => m_CanClimb;
    public float ClimbUpSpeed => m_ClimbUpSpeed;
    public float ClimbDownSpeed => m_ClimbDownSpeed;
    public float BlockClimbTime => m_BlockClimbTime;
    public Vector2 LedgeHopMultiplier => m_LedgeHopMultiplier;
    public Vector2 ClimbJumpMultiplier => m_ClimbJumpMultiplier;

    [Header("Wall Slider")]
    [SerializeField] private bool m_CanWallSlide = false;
    [Range(1f, 100f)]
    [SerializeField] private float m_SlideSpeed = 5f;
    [Range(1f, 10f)]
    [SerializeField] private float m_SlideSpeedMultiplier = 1.5f;
    [Range(0f, 10f)]
    [SerializeField] private float m_SlideThreshold = 2f;
    public bool CanWallSlide => m_CanWallSlide;
    public float SlideSpeed => m_SlideSpeed;
    public float SlideSpeedMultiplier => m_SlideSpeedMultiplier;
    public float SlideTreshold => m_SlideThreshold;

    [Header("Dasher")]
    [SerializeField] private bool m_CanDash = false;
    [SerializeField] private bool m_IsInvincibleDuringDash = false;
    [Range(1, 3)]
    [SerializeField] private int m_MaxDashes = 2;
    [Range(1f, 100f)]
    [SerializeField] private float m_DashSpeed = 12f;
    [Range(0.1f, 100f)]
    [SerializeField] private float m_DashTime = 0.3f;
    public bool CanDash => m_CanDash;
    public bool IsInvincibleDuringDash => m_IsInvincibleDuringDash;
    public int MaxDashes => m_MaxDashes;
    public float DashSpeed => m_DashSpeed;
    public float DashTime => m_DashTime;

    [Header("Pusher")]
    [SerializeField] private bool m_CanPush = false;
    [Range(0f, 100f)]
    [SerializeField] private float m_PushForce = 20f;
    public bool CanPush => m_CanPush;
    public float PushForce => m_PushForce;

    [Header("Ducker")]
    [SerializeField] private bool m_CanDuck = false;
    [SerializeField] private bool m_CanDuckAttack = false;
    public bool CanDuck => m_CanDuck;
    public bool CanDuckAttack => m_CanDuckAttack;

    [Header("Stomper")]
    [SerializeField] private bool m_CanStomp = false;
    [Range(0f, 100)]
    [SerializeField] private float m_StompDamage = 16f;
    public bool CanStomp => m_CanStomp;
    public float StompDamage => m_StompDamage;

    [Header("Collision Layers")]
    [SerializeField] private LayerMask m_GroundLayer = 6;
    [SerializeField] private LayerMask m_PlatformLayer = 8;
    [SerializeField] private LayerMask m_ClimbAbleLayer = 7;
    public LayerMask GroundLayer => m_GroundLayer;
    public LayerMask PlatformLayer => m_PlatformLayer;
    public LayerMask ClimbAbleLayer => m_ClimbAbleLayer;

    [Header("Collision Size")]
    [SerializeField] private Vector2 m_GroundDetectionSize = Vector2.zero;
    [SerializeField] private Vector2 m_LedgeDetectionSize = Vector2.zero;
    [SerializeField] private Vector2 m_WallDetectionSize = Vector2.zero;
    public Vector2 GroundDetectionSize => m_GroundDetectionSize;
    public Vector2 LedgeDetectionSize => m_LedgeDetectionSize;
    public Vector2 WallDetectionSize => m_WallDetectionSize;

    [Header("Collision Offset")]
    [SerializeField] private Vector2 m_GroundDetectionOffset = Vector2.zero;
    [SerializeField] private Vector2 m_RightLedgeDetectionOffset = Vector2.zero;
    [SerializeField] private Vector2 m_LeftLedgeDetectionOffset = Vector2.zero;
    [SerializeField] private Vector2 m_RightWallDetectionOffset = Vector2.zero;
    [SerializeField] private Vector2 m_LeftWallDetectionOffset = Vector2.zero;
    public Vector2 GroundDetectionOffset => m_GroundDetectionOffset;
    public Vector2 RightLedgeDetectionOffset => m_RightLedgeDetectionOffset;
    public Vector2 LeftLedgeDetectionOffset => m_LeftLedgeDetectionOffset;
    public Vector2 RightWallDetectionOffset => m_RightWallDetectionOffset;
    public Vector2 LeftWallDetectionOffset => m_LeftWallDetectionOffset;

    [Header("Generous Collision Size")]
    [Range(0f, 10f)]
    [SerializeField] private float m_RayDistance = 0.2f;
    [Range(0f, 10f)]
    [SerializeField] private float m_CircleRadius = 0.2f;
    [SerializeField] private Vector2 m_GenerousGroundDetectionSize = Vector2.zero;
    [SerializeField] private Vector2 m_GenerousWallDetectionSize = Vector2.zero;
    public float RayDistance => m_RayDistance;
    public float CircleRadius => m_CircleRadius;
    public Vector2 GenerousGroundDetectionSize => m_GenerousGroundDetectionSize;
    public Vector2 GenerousWallDetectionSize => m_GenerousWallDetectionSize;

    [Header("Generous Collision Offset")]
    [SerializeField] private Vector2 m_GenerousGroundDetectionOffset = Vector2.zero;
    [SerializeField] private Vector2 m_GenerousRightWallDetectionOffset = Vector2.zero;
    [SerializeField] private Vector2 m_GenerousLeftWallDetectionOffset = Vector2.zero;
    public Vector2 GenerousGroundDetectionOffset => m_GenerousGroundDetectionOffset;
    public Vector2 GenerousRightWallDetectionOffset => m_GenerousRightWallDetectionOffset;
    public Vector2 GenerousLeftWallDetectionOffset => m_GenerousLeftWallDetectionOffset;

    [Header("Clamp Veloctiy")]
    [SerializeField] private bool m_ClampRigidbodyVelocity = false;
    [SerializeField] private Vector2 m_MinVelocity = Vector2.zero;
    [SerializeField] private Vector2 m_MaxVelocity = Vector2.zero;
    public bool ClampRigidbodyVelocity => m_ClampRigidbodyVelocity;
    public Vector2 MinVelocity => m_MinVelocity;
    public Vector2 MaxVelocity => m_MaxVelocity;

    [Header("Camera Effects")]
    [SerializeField] private bool m_CanScreenShake = true;
    [SerializeField] private bool m_CanScreenRipple = true;
    [SerializeField] private bool m_CanShakeFadeOut = true;
    [SerializeField] private bool m_CanShakeSnap = false;
    [SerializeField] private float m_ShakeDuration = 0.2f;
    [SerializeField] private float m_ShakeStrength = 0.5f;
    [SerializeField] private int m_ShakeVibrato = 14;
    [SerializeField] private float m_ShakeRandomness = 90f;
    public bool CanScreenShake => m_CanScreenShake;
    public bool CanScreenRipple => m_CanScreenRipple;
    public bool CanShakeFadeOut => m_CanShakeFadeOut;
    public bool CanShakeSnap => m_CanShakeSnap;
    public float ShakeDuration => m_ShakeDuration;
    public float ShakeStrength => m_ShakeStrength;
    public int ShakeVibrato => m_ShakeVibrato;
    public float ShakeRandomness => m_ShakeRandomness;

}
