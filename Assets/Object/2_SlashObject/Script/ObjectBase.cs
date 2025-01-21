using UnityEngine;

public class ObjectBase : MonoBehaviour
{
	[SerializeField] private Direnction direnction;

	protected Vector3 _scale;
	protected BoxCollider2D _bodyCollider;
	protected BoxCollider2D _attackCollider;
	protected BoxCollider2D _damageCollider;

	enum Direnction
    {
        [InspectorName("âE")] Right,
		[InspectorName("ç∂")] Left,
	}
    
    protected virtual void Start()
    {
		_bodyCollider = GetComponent<BoxCollider2D>();
		var scale = transform.Find("Scale");
		_scale = scale.localScale;
		var collider = scale.transform.Find("Collider");
		_attackCollider = collider.Find("Attack").GetComponent<BoxCollider2D>();
		_damageCollider = collider.Find("Damage").GetComponent<BoxCollider2D>();
	}

	public Vector3 GetFootPos()
	{
		var a = _scale;
		return Vector3.zero;
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
}
