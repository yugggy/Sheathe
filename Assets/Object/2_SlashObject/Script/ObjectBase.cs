using System.Collections;
using UnityEngine;

public class ObjectBase : MonoBehaviour
{
	[SerializeField] private Direnction direnction;
	[SerializeField] private Landing landing;

	protected Vector3 _scale;
	protected Animator _animator;
	protected BoxCollider2D _bodyCollider;
	protected BoxCollider2D _attackCollider;
	protected BoxCollider2D _damageCollider;

	public enum Direnction
    {
        [InspectorName("右")] Right,
		[InspectorName("左")] Left,
	}

	public enum Landing
	{
		[InspectorName("地上")] Ground,
		[InspectorName("空中")] Air,
	}

	protected virtual void Start()
    {
		// Scale
		var scale = transform.Find("Scale");
		_scale = scale.localScale;

		// Animation
		if (scale.Find("Image").TryGetComponent<Animator>(out var animator))
		{
			_animator = animator;
		}

		// Collider
		_bodyCollider = GetComponent<BoxCollider2D>();
		var collider = scale.transform.Find("Collider");
		_attackCollider = collider.Find("Attack").GetComponent<BoxCollider2D>();
		_damageCollider = collider.Find("Damage").GetComponent<BoxCollider2D>();
	}

	public Vector3 GetFootPos()
	{
		var a = _scale;
		return Vector3.zero;
	}

	public Landing GetLanding()
	{
		return landing;
	}

	private void OnValidate()
	{
		SetDirection();

		SetGravityScale();
	}

    private void SetDirection()
    {
		var eulerAngles = transform.eulerAngles;

		switch (direnction)
		{
			case Direnction.Right:
				eulerAngles.y = 0;
				break;
			case Direnction.Left:
				eulerAngles.y = 180;
				break;
			default:
				break;
		}

		transform.eulerAngles = eulerAngles;
	}
	public void SetDirection(bool isRight)
	{
		var eulerAngles = transform.eulerAngles;

		if (isRight)
		{
			eulerAngles.y = 0;
		}
		else
		{
			eulerAngles.y = 180;
		}

		transform.eulerAngles = eulerAngles;
	}

	private void SetGravityScale()
	{
		var rigidbody2D = transform.GetComponent<Rigidbody2D>();

		switch (landing)
		{
			case Landing.Ground:
				rigidbody2D.gravityScale = 1;
				break;
			case Landing.Air:
				rigidbody2D.gravityScale = 0;
				break;
			default:
				break;
		}
	}

	/// <summary>
	/// アニメが終了するまで待機
	/// </summary>
	protected IEnumerator WaitAnimeFinish()
	{
		// アニメの切り替えのため1フレーム待機
		yield return null;

		// アニメが終了するまで待機
		while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
		{
			//Debug.Log("normalizedTime" + _animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
			yield return null;
		}
	}

}
