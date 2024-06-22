using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mediapipe.Unity.Sample.HandTracking;

public class PhyHandsSimple : MonoBehaviour
{
   
    public HandTracking tracking;
    // Start is called before the first frame update
    private List<List<PID>> pids = new List<List<PID>>();
    [SerializeField]
    private List<Rigidbody> rbs;
    
    void Start()
    {
        
        for (int i = 0; i < 2; i++)
        {
                List<PID> xyz = new List<PID>();
                for (int b = 0; b < 3; b++)
                {
                    xyz.Add(new PID(30f,0.1f,20f));
                }
                pids.Add(xyz);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < tracking.middles.Count; i++)
        {
            Vector3 pos = tracking.middles[i];
            Vector3 curr = rbs[i].position;
            float x = pids[i][0].Update(pos.x, curr.x, Time.deltaTime);
            float y = pids[i][1].Update(pos.y, curr.y, Time.deltaTime);
            float z = pids[i][2].Update(pos.z, curr.z, Time.deltaTime);
            rbs[i].AddForce(new Vector3(x,y,z));
        }
    }

}
