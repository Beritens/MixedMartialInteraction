using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image healthBarFill;

    void Start()
    {
        healthBarFill = GetComponent<Image>();
    }

    public void SetHealth(float healthNormalized)
    {
        healthBarFill.fillAmount = healthNormalized;  
    }
}