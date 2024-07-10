using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LocalNetworking;
using UnityEngine;

public class PhoneTracker : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform t;
    private Quaternion firstRot;
    private Vector3 velocity = Vector3.zero;
    private Vector3 lastAcceleration = Vector3.zero;
    private Vector3 position = Vector3.zero;
    public MyNetworkManager _nm;
    void Start()
    {
        // Enable the gyroscope
        Input.gyro.enabled = true;

    }

    void Update()
    {
        //rotation part
        Quaternion gyroAttitude = Input.gyro.attitude;

        if (Input.touches.Length > 0)
        {
            firstRot = gyroAttitude;
            velocity = Vector3.zero;
            position = Vector3.zero;

        }
        Quaternion diff = Quaternion.Euler(0,0,0) * Quaternion.Inverse(firstRot);
        // Convert the Quaternion to Euler angles (in degrees) for easier readability
        Quaternion q = diff * gyroAttitude;
        q = new Quaternion(q.x,
                                                    q.y,
                                                    q.z * -1.0f,
                                                    q.w * -1.0f);
         
        Vector3 gyroEulerAngles = (q).eulerAngles;

        t.eulerAngles = gyroEulerAngles;

        Vector3 gravity = Input.gyro.gravity;

        Vector3 currentAcceleration =  (Input.acceleration - gravity);
        currentAcceleration = Vector3.Scale(currentAcceleration, new Vector3(-1, -1, 1));
        

        velocity += (currentAcceleration + lastAcceleration)* 100f * 0.5f * Time.deltaTime;
        position += q *velocity * Time.deltaTime;
        lastAcceleration = currentAcceleration;

        t.position = position;
        
        
        
        //network stuff
        _nm.Send("pos", position.ToString());
        _nm.Send("rot", gyroEulerAngles.ToString());

        float rate = 1f;
        if ((q  * velocity).z < 0f)
        {
            velocity = Vector3.zero;
            
            rate = 30f;
        }
        float l = Mathf.Pow(0.5f, Time.deltaTime * rate);
        //float lv = Mathf.Pow(0.5f, Time.deltaTime * rate*2);
        Debug.Log(l);
        velocity = Vector3.Lerp(Vector3.zero, velocity, l);
        position = Vector3.Lerp(Vector3.zero,  position, l);
        lastAcceleration = Vector3.Lerp(Vector3.zero, lastAcceleration, l);

    }
}
