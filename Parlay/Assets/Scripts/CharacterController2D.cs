using UnityEngine;
using UnityEngine.Events;

/**
* This code was inspired from a Brackey's tutorial
* https://www.youtube.com/watch?v=dwcT-Dch0bA
*/

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
	[SerializeField] private bool m_AirControl = false;
	[SerializeField] private LayerMask m_WhatIsGround;
	[SerializeField] private BoxCollider2D m_FootCollider;

	private bool m_IsGrounded;
	[SerializeField] private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;
	private Vector3 m_Velocity = Vector3.zero;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	private void Awake()
	{
		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_IsGrounded;
		m_IsGrounded = false;
		Vector2 colliderSize = new Vector2(m_FootCollider.bounds.size.x, m_FootCollider.bounds.size.y + 0.2f);
		Collider2D[] colliders = Physics2D.OverlapBoxAll(m_FootCollider.bounds.center, m_FootCollider.bounds.size, 0f, m_WhatIsGround);

		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_IsGrounded = true;
				if (!wasGrounded && m_Rigidbody2D.velocity.y < 0)
					OnLandEvent.Invoke();
			}
		}
	}

    public Vector3 GetPosition()
    {
        return m_Rigidbody2D.position;
    }

	public void Move(float move, bool jump)
	{

		if (m_IsGrounded || m_AirControl)
		{
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			if ((move > 0 && !m_FacingRight) || (move < 0 && m_FacingRight))
			{
				Flip();
			}
		}
		if (m_IsGrounded && jump)
		{
			m_IsGrounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}

	public bool CanJump()
	{
		return m_IsGrounded;
	}

	private void Flip()
	{
		m_FacingRight = !m_FacingRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
