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

	enum Direnction
    {
        [InspectorName("�E")] Right,
		[InspectorName("��")] Left,
	}

	public enum Landing
	{
		[InspectorName("�n��")] Ground,
		[InspectorName("��")] Air,
	}

	protected virtual void Start()
    {
		// Scale
		var scale = transform.Find("Scale");
		_scale = scale.localScale;

		// Animation
		if (scale.Find("Animation").TryGetComponent<Animator>(out var animator))
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

	/// <summary>
	/// �A�j�����I������܂őҋ@
	/// </summary>
	protected IEnumerator WaitAnimeFinish()
	{
		// �A�j���̐؂�ւ��̂���1�t���[���ҋ@
		yield return null;

		// �A�j�����I������܂őҋ@
		while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
		{
			//Debug.Log("normalizedTime" + _animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
			yield return null;
		}
	}

}
