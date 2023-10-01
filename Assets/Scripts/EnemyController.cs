using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

enum State
{
    Idle,
    Active,
    Slow,
}
public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 0;
    [SerializeField] private float slowSpeed = 0;
    [SerializeField] private float closeDistance = 0;
    [SerializeField] private float farDistance = 0;
    [SerializeField] private bool hasIdle = true;

    private Rigidbody2D rigidbody;
    private SpriteRenderer renderer;
    private State currentState = State.Active;
    
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();

        if (hasIdle)
        {
            InvokeRepeating(nameof(ChooseState), 0, 0.1f);
        }
    }
    
    void FixedUpdate()
    {
        if (currentState == State.Active) 
        {
            Move(speed);
        }
        else if (currentState == State.Slow)
        {
            Move(slowSpeed);
        }
        else
        {
            Stop();
        }
    }

    private void ChooseState()
    {
        float dist = GetDistToTarget();
        Debug.DrawRay(target.position, -GetDirToTarget().normalized * dist, Color.green, 0.1f);

        if (dist > closeDistance && dist < farDistance)
        {
            currentState = State.Active;
        } 
        else if (dist >= farDistance)
        {
            currentState = State.Slow;
        }
        else 
        {
            currentState = State.Idle;
        }
    }

    private void Move(float moveSpeed)
    {
        if (rigidbody.bodyType == RigidbodyType2D.Static)
        {
            rigidbody.bodyType = RigidbodyType2D.Dynamic;
        }

        Vector2 dir = GetDirToTarget();
        rigidbody.velocity = moveSpeed * Time.deltaTime * dir.normalized;
    }

    private void Stop()
    {
        rigidbody.bodyType = RigidbodyType2D.Static;
    }

    private float GetDistToTarget()
    {
        return GetDirToTarget().magnitude - (this.renderer.bounds.size.x * transform.localScale.x / 2);
    }

    private Vector3 GetDirToTarget()
    {
        return new Vector2(target.position.x - transform.position.x, target.position.y - transform.position.y);
    }
}
