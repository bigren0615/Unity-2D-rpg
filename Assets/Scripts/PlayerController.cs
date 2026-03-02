using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public SpriteRenderer spriteRenderer;

    private Vector2 movementInput;
    private Rigidbody2D rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Automatically fetch SpriteRenderer if not assigned
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
            Debug.LogError("SpriteRenderer not found on Player! Please add one.");
    }

    private void Update()
    {
        ReadInput();
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        Move();
    }

    // 1️ Read input every frame (responsive)
    private void ReadInput()
    {
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");

        // Normalize so diagonal is not faster
        movementInput = movementInput.normalized;

        // Base sprite faces LEFT; flipX when moving right
        if (movementInput.x > 0) spriteRenderer.flipX = true;
        else if (movementInput.x < 0) spriteRenderer.flipX = false;
    }

    // 2️ Physics-based movement
    private void Move()
    {
        rb.MovePosition(
            rb.position + movementInput * moveSpeed * Time.fixedDeltaTime
        );
    }

    // 3️ Animation sync
    private void UpdateAnimation()
    {
        bool isMoving = movementInput != Vector2.zero;

        animator.SetBool("isMoving", isMoving);

        if (isMoving)
        {
            animator.SetFloat("moveX", Mathf.Abs(movementInput.x));
            animator.SetFloat("moveY", movementInput.y);
        }
    }
}
