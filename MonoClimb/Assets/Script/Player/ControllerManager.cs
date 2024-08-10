using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    public float speed;
    public float upForce;

    private float _currentSpeed;
    private Rigidbody2D _rb;
    private Animator _animator;

    public bool isStop;
    private bool isJumpEnabled = true;
    private bool isJumping = false;
    private bool isBow = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _currentSpeed = speed;
    }

    private void FixedUpdate()
    {
        if (!isStop)
        {
            PlayerRun();

            if (isJumping && isJumpEnabled)
            {
                Jump();
            }
        }
    }

    private void PlayerRun()
    {
        _rb.velocity = Vector2.right * _currentSpeed * Time.deltaTime;
    }

    private void Jump()
    {
        _rb.AddForce(Vector2.up * upForce * Time.deltaTime, ForceMode2D.Impulse);
        isJumpEnabled = false;
        _animator.SetBool("isJump", true);
        Invoke("EnableJump", 1f);
    }

    private void EnableJump()
    {
        isJumpEnabled = true;
        isJumping = false;
        _animator.SetBool("isJump", false);
    }

    public void PlayerJump()
    {
        if (isJumpEnabled)
        {
            isJumping = true;
        }
    }

    public void Bow()
    {
        UpdateBowState(true, speed / 2);
    }

    public void NotBow()
    {
        UpdateBowState(false, speed);
    }

    private void UpdateBowState(bool bowState, float newSpeed)
    {
        isBow = bowState;
        _currentSpeed = newSpeed;
        _animator.SetBool("isBow", isBow);
    }

    public void Stop()
    {
        UpdateStopState(true);
    }

    public void Resume()
    {
        UpdateStopState(false);
    }

    private void UpdateStopState(bool stopState)
    {
        isStop = stopState;
        _animator.SetBool("isStop", isStop);
        _currentSpeed = isStop ? 0 : speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            EnableJump();
        }
    }
}
