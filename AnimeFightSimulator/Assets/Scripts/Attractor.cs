using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    public Vector3 point;
    public float p;
    public float i;
    public float d;
    private PID pidx;
    private PID pidy;
    private PID pidz;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pidx = new PID(p, i, d);
        pidy = new PID(p, i, d);
        pidz = new PID(p, i, d);
    }

    private void FixedUpdate()
    {
        Vector3 xyz = Vector3.zero;
        xyz.x = pidx.Update(point.x, rb.position.x, Time.fixedDeltaTime);
        xyz.y = pidy.Update(point.y, rb.position.y, Time.fixedDeltaTime);
        xyz.z = pidz.Update(point.z, rb.position.z, Time.fixedDeltaTime);
        rb.AddForce(xyz, ForceMode.Acceleration);
    }
}
