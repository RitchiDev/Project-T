using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deactivator : MonoBehaviour
{
    [SerializeField] private float m_TimeBeforeDeactivate = 3f;
    [SerializeField] private bool m_Destroy = false;
    private IEnumerator m_DeactivateTimer;

    private void OnEnable()
    {
        if(m_DeactivateTimer != null)
        {
            StopCoroutine(m_DeactivateTimer);
        }

        m_DeactivateTimer = DeactivateTimer();
        StartCoroutine(m_DeactivateTimer);
    }

    private void OnDisable()
    {
        if (m_DeactivateTimer != null)
        {
            StopCoroutine(m_DeactivateTimer);
        }
    }

    private void Deactivate()
    {
        if(m_Destroy)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator DeactivateTimer()
    {
        float totalTime = m_TimeBeforeDeactivate;

        while (totalTime >= 0)
        {
            totalTime -= Time.deltaTime;
            yield return null;
        }

        Deactivate();
    }
}
