using System;
using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity.Sample.HandTracking;
using UnityEngine;

enum AttackState
{
    Idle,
    Attacking,
    Returning,
}
public class AttackTracker : MonoBehaviour
{

    public float attackPlane;

    public float attackSpeed;
    
    private AttackState rightAttackState = AttackState.Idle;

    private AttackState leftAttackState = AttackState.Idle;

    private bool blackFlashLeft = false;
    private bool blackFlashRight = false;

    public Rigidbody handLeft;

    public Rigidbody handRight;

    public event EventHandler OnBlackFlashRight; 
    public event EventHandler OnBlackFlashLeft; 
    public bool getRightAttacking()
    {
        return rightAttackState == AttackState.Attacking;
    }

    public bool getLeftAttacking()
    {
        return leftAttackState == AttackState.Attacking;
    }

    public void BlackFlash()
    {
        Debug.Log("Test");
        if (!blackFlashLeft && leftAttackState != AttackState.Idle)
        { 
            blackFlashLeft = true;
            OnBlackFlashLeft?.Invoke(this,EventArgs.Empty); 
        } 
        if (!blackFlashRight && rightAttackState != AttackState.Idle)
        {
            blackFlashRight = true;
            OnBlackFlashRight?.Invoke(this,EventArgs.Empty); 
        } 
    }
    // Update is called once per frame
    void StateHandling(ref AttackState attackState, Rigidbody rb, ref bool bf)
    {
        
        if (attackState == AttackState.Idle && rb.position.z >= attackPlane)
        {
            if (rb.velocity.magnitude >= attackSpeed)
            {
                attackState = AttackState.Attacking;
            }
            else
            {
                attackState = AttackState.Returning;
            }
        }
        else if(attackState == AttackState.Attacking)
        {
            if (rb.velocity.magnitude < attackSpeed)
            {
                attackState = AttackState.Returning;
            }
             
        }

        if (rb.position.z < attackPlane)
        {
            bf = false;
            attackState = AttackState.Idle;
        }
    }
    void FixedUpdate()
    {

        StateHandling(ref leftAttackState, handLeft, ref blackFlashLeft);
        StateHandling(ref rightAttackState, handRight, ref blackFlashRight);
    }
}