using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    #region Variables
    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }

    public float HorizontalAim { get; private set; }
    public float VerticalAim { get; private set; }

    public bool GetJump { get; private set; }
    public bool GetJumpDown { get; private set; }
    public bool GetJumpUp { get; private set; }

    public bool GetDash { get; private set; }
    public bool GetDashDown { get; private set; }
    public bool GetDashUp { get; private set; }

    public bool GetClimb { get; private set; }
    public bool GetClimbDown { get; private set; }
    public bool GetClimbUp { get; private set; }

    public bool GetUseAbility { get; private set; }
    public bool GetUseAbilityDown { get; private set; }
    public bool GetUseAbilityUp { get; private set; }
    #endregion

    #region Input Context
    public void MovementContext(InputAction.CallbackContext context)
    {
        Horizontal = context.ReadValue<Vector2>().x;
        Vertical = context.ReadValue<Vector2>().y;

        if(context.canceled)
        {
            Horizontal = 0f;
            Vertical = 0f;
        }
    }

    public void AimContext(InputAction.CallbackContext context)
    {
        HorizontalAim = context.ReadValue<Vector2>().x;
        VerticalAim = context.ReadValue<Vector2>().y;
    }

    public void JumpContext(InputAction.CallbackContext context)
    {
        GetJump = context.performed;

        Debug.Log("Performed " + GetJumpDown);

        if (context.performed && !GetJumpDown)
        {
            StartCoroutine(GetJumpButtonDownTimer());
        }

        GetJumpUp = context.canceled;
    }

    public void DashContext(InputAction.CallbackContext context)
    {
        GetDash = context.performed;

        if (context.performed && !GetDashDown)
        {
            StartCoroutine(GetDashButtonDownTimer());
        }

        GetDashUp = context.canceled;
    }

    public void ClimbContext(InputAction.CallbackContext context)
    {
        GetClimb = context.performed;

        if (context.performed && !GetDashDown)
        {
            StartCoroutine(GetClimbButtonDownTimer());
        }

        GetClimbUp = context.canceled;
    }

    public void UseAbilityContext(InputAction.CallbackContext context)
    {
        GetUseAbility = context.performed;

        if (context.performed && !GetUseAbilityDown)
        {
            StartCoroutine(GetUseAbilityButtonDownTimer());
        }

        GetUseAbilityUp = context.canceled;
    }

    #endregion

    #region Coroutines

    private IEnumerator GetJumpButtonDownTimer()
    {
        GetJumpDown = true;

        Debug.Log(GetJumpDown);

        yield return new WaitForEndOfFrame();

        GetJumpDown = false;
    }

    private IEnumerator GetDashButtonDownTimer()
    {
        GetDashDown = true;

        yield return new WaitForEndOfFrame();

        GetDashDown = false;
    }

    private IEnumerator GetClimbButtonDownTimer()
    {
        GetClimbDown = true;

        yield return new WaitForEndOfFrame();

        GetClimbDown = false;
    }

    private IEnumerator GetUseAbilityButtonDownTimer()
    {
        GetUseAbilityDown = true;

        yield return new WaitForEndOfFrame();

        GetUseAbilityDown = false;
    }

    #endregion
}
