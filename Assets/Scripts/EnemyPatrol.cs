using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyPatrol : MonoBehaviour
{
    public float patrolRadius = 3f;
    public float speed = 2f;
    public float waitTime = 1f;

    private Vector3 startPosition;
    private Vector3 targetPoint;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool waiting = false;
    private float waitTimer = 0f;

    void Start()
    {
        startPosition = transform.position;

        PickRandomPoint();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            Debug.LogError("Enemy needs a SpriteRenderer!");
    }

    void Update()
    {
        if (waiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                waiting = false;
                PickRandomPoint();
            }
            return;
        }

        Vector3 dir = targetPoint - transform.position;
        Vector3 moveDir = dir.normalized;

        // Calculate next position
        Vector3 nextPos = transform.position + moveDir * speed * Time.deltaTime;

        // Check collision before moving
        if (!Physics2D.OverlapCircle(nextPos, 0.1f, LayerMask.GetMask("Solid")))
        {
            transform.position = nextPos;
        }
        else
        {
            // hit wall? pick new random point
            PickRandomPoint();
            return; // skip rest of frame
        }

        // Update Animator for walk
        if (animator != null)
        {
            animator.SetFloat("moveX", Mathf.Abs(moveDir.x));
            animator.SetFloat("moveY", moveDir.y);
            animator.SetBool("isAttacking", false);
            animator.SetBool("isDead", false);
        }

        // Flip sprite for left/right
        if (spriteRenderer != null)
        {
            if (moveDir.x > 0.01f)
                spriteRenderer.flipX = true;
            else if (moveDir.x < -0.01f)
                spriteRenderer.flipX = false;
        }

        // Reached target point
        if (Vector3.Distance(transform.position, targetPoint) < 0.05f)
        {
            waiting = true;
            waitTimer = waitTime;
        }
    }

    void PickRandomPoint()
    {
        int maxAttempts = 10; // try 10 times to find a free spot
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * patrolRadius;
            Vector3 potentialPoint = startPosition + new Vector3(randomCircle.x, randomCircle.y, 0f);

            // check if point is free
            if (!Physics2D.OverlapCircle(potentialPoint, 0.2f, LayerMask.GetMask("Solid")))
            {
                targetPoint = potentialPoint;
                return;
            }
        }

        // fallback: if can't find a free spot, stay at start
        targetPoint = startPosition;
    }

    // Draw patrol radius and next target in Scene view
    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
            return; // only draw during play so we have a valid targetPoint

        // 1️ Green circle for patrol radius
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startPosition, patrolRadius);

        // 2️ Red line to next target point
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, targetPoint);

        // Optional: draw a small red sphere at the target point
        Gizmos.DrawSphere(targetPoint, 0.05f);
    }
}
