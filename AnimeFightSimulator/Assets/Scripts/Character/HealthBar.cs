using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image healthBarFill;

    void Awake()
    {
        healthBarFill = GetComponent<Image>();
    }

    public void SetHealth(float healthNormalized)
    {
        healthBarFill.fillAmount = healthNormalized;  
    }
}