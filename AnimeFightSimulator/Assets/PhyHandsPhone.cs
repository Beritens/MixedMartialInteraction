using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhyHandsPhone : MonoBehaviour
{
    public Transform bone;

    public Transform goal;
    
    // Start is called before the first frame update
    private List<List<PID>> pids = new List<List<PID>>();
    public float p = 30;
    public float i = 0.1f;
    public float d = 20;
    [SerializeField]
    private List<Rigidbody> rbs;
    
    void Start()
    {
        
        for (int i = 0; i < 2; i++)
        {
                List<PID> xyz = new List<PID>();
                for (int b = 0; b < 3; b++)
                {
                    xyz.Add(new PID(p,i,d));
                }
                pids.Add(xyz);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
            Vector3 pos = goal.position;
            Vector3 curr = rbs[0].position;
            float x = pids[0][0].Update(pos.x, curr.x, Time.deltaTime);
            float y = pids[0][1].Update(pos.y, curr.y, Time.deltaTime);
            float z = pids[0][2].Update(pos.z, curr.z, Time.deltaTime);
            rbs[0].AddForce(new Vector3(x,y,z));
            bone.position = rbs[0].position;
    }
}
