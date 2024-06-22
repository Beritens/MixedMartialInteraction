using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity.Sample.HandTracking;
using UnityEngine;

public class PhyHands : MonoBehaviour
{
    public HandTracking tracking;
    // Start is called before the first frame update
    private List<List<List<PID>>> pids = new List<List<List<PID>>>();
    public GameObject prefab;
    private List<List<Rigidbody>> rbs = new List<List<Rigidbody>>();
    void Start()
    {
        rbs.Add(new List<Rigidbody>());
        rbs.Add(new List<Rigidbody>());
        pids.Add(new List<List<PID>>());
        pids.Add(new List<List<PID>>());
        for (int i = 0; i < tracking.hands.Count; i++)
        {
            for (int j = 0; j < tracking.hands[i].Count; j++)
            {
                var rb = Instantiate(prefab);
                rbs[i].Add(rb.GetComponent<Rigidbody>());
                List<PID> xyz = new List<PID>();
                for (int b = 0; b < 3; b++)
                {
                    xyz.Add(new PID(20f,0f,19f));
                }
                pids[i].Add(xyz);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < tracking.hands.Count; i++)
        {
            for (int j = 0; j < tracking.hands[i].Count; j++)
            {
                Vector3 pos = tracking.hands[i][j];
                Vector3 curr = rbs[i][j].position;
                float x = pids[i][j][0].Update(pos.x, curr.x, Time.deltaTime);
                float y = pids[i][j][1].Update(pos.y, curr.y, Time.deltaTime);
                float z = pids[i][j][2].Update(pos.z, curr.z, Time.deltaTime);
                rbs[i][j].AddForce(new Vector3(x,y,z));
            }
        }
    }
}
