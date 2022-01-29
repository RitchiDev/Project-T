using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsHandler : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] private TrailRenderer m_Trail;
    [SerializeField] private ParticleSystem m_RunEffect;
    [SerializeField] private ParticleSystem m_JumpEffect;
    [SerializeField] private ParticleSystem m_WallJumpEffect;
    [SerializeField] private ParticleSystem m_WallSlideEffect;
    [SerializeField] private ParticleSystem m_DashEffect;
}
