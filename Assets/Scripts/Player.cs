using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static event Action Damaged;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    [SerializeField]
    private AudioSource soundOfDamage;

    [SerializeField]
    private Transform checkGroundPoint;
    [SerializeField]
    private LayerMask layerGound;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpForce;

    private bool isGrounded;
    private bool invincibility = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        CheckForGound();
        Jump();

        float moveInput = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(moveInput, 0f, 0f) * speed;
        rb.velocity = new Vector2(movement.x, rb.velocity.y);

        if (rb.velocity.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (rb.velocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            animator.SetBool("run", true);
        }
        else animator.SetBool("run", false);

        if (!isGrounded && rb.velocity.y > 0) animator.SetBool("jump", true);
        else animator.SetBool("jump", false);
        if (!isGrounded && rb.velocity.y < 0) animator.SetBool("fall", true);
        else animator.SetBool("fall", false);
        animator.SetBool("land", isGrounded);
    }

    private void CheckForGound()
    {
        if (Physics2D.OverlapPoint(checkGroundPoint.position, layerGound))
        {
            isGrounded = true;
        }
        else isGrounded = false;
    }

    private void Jump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    public void GetDamage()
    {
        if (!invincibility)
        {
            soundOfDamage.Play();
            Damaged?.Invoke();
            StartCoroutine(InvincibilityOn());
        }
    }

    private IEnumerator InvincibilityOn()
    {
        invincibility = true;
        yield return new WaitForSecondsRealtime(0.3f);
        invincibility = false;
    }
}
