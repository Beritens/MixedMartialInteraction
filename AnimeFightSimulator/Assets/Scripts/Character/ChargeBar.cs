using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeBar : MonoBehaviour
{

    public int charge = 1; 
    public HealthBar chargeBar;
    public int chargeAmount;
    // Start is called before the first frame update
    void Start()
    {
        chargeBar.SetHealth((float)charge / 100);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))  // Check if Space key is pressed
        {
            addCharge();  // Reduce health by 10
        }
        
    }

    public void addCharge()
    {
        charge += chargeAmount;
        if (charge < 0) charge = 0;
        
        chargeBar.SetHealth((float)charge / 100);


    }

    public bool isCharged()
    {
        if (charge >= 100)
        {
            return true;
        }

        return false;
    }
    
    public void deplete()
    {
        charge = 0;
        chargeBar.SetHealth((float)charge / 100);
    }

}
