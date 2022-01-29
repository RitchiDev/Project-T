using DG.Tweening;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerEntity : MonoBehaviour
{
    #region Variables
    [Header("States")]
    private PlayerStates m_CurrentState;
    private PlayerStates m_CurrentActionState;
    private PlayerStates m_PreviousActionState;

    [Header("Settings")]
    [SerializeField] private EntitySettings m_Settings;

    [Header("Avatar")]
    [SerializeField] private Camera m_Camera;
    [SerializeField] private GameObject m_Avatar;
    private SpriteRenderer m_SpriteRenderer;
    private Rigidbody2D m_Rigidbody;
    private Vector2 m_OriginalSize;
    private Animator m_Animator;
    private bool m_FacingRight;

    [Header("Entity")]
    private bool m_IsInvincible;
    private float m_MaxHealth;
    private float m_Health;
    private float m_Armor;
    #endregion

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_SpriteRenderer = m_Avatar.GetComponent<SpriteRenderer>();
        m_Animator = m_Avatar.GetComponent<Animator>();
        m_OriginalSize = m_Avatar.transform.localScale;

        m_CurrentState = PlayerStates.inAir;

        //if(true)
        //{
        //    m_SpriteRenderer.color = UnityEngine.Random.ColorHSV();
        //}
    }

    private void Start()
    {
        m_MaxHealth = m_Settings.StartHealth;
        m_Health = m_MaxHealth;
    }

    private IEnumerator GiveInvincibility(float timeInvincible)
    {
        m_IsInvincible = true;

        yield return new WaitForSeconds(timeInvincible);

        m_IsInvincible = false;
    }

    public void ChangeVitality(float amount)
    {
        float remainingAmount = amount;

        if (amount < 0)
        {
            if (m_IsInvincible)
            {
                //Debug.Log("Can't be damaged");
                return;
            }

            //Debug.Log("Has been hit");

            float damage = amount; // Hoeveelheid damage

            if (m_Armor > 0) // Als de speler armor heeft
            {
                if (amount > m_Armor) // Als de damage groter is dan de hoeveelheid armor
                {
                    remainingAmount = m_Armor + damage; // Overgebleven damage is armor hoeveelheid - damge ( + omdat het een negatief getal is)
                }

                m_Armor = Mathf.Clamp(m_Armor + amount, 0, m_Settings.MaxArmor); // Houdt het getal tussen de minimum en het maximum
            }

            StartCoroutine(GiveInvincibility(m_Settings.TimeInvincible));
        }

        m_Health = Mathf.Clamp(m_Health + remainingAmount, 0, m_MaxHealth); // Houdt het getal tussen de minimum en het maximum


        if(!m_Settings.CanDie)
        {
            return;
        }

        if (m_Health <= 0 && m_CurrentState != PlayerStates.dead)
        {
            Die();
        }
    }

    public void TakeKnockback(Vector2 force, ForceMode2D mode)
    {
        m_Rigidbody.AddForce(force, mode);
    }

    private void Die()
    {
        m_CurrentState = PlayerStates.dead;
    }
}
