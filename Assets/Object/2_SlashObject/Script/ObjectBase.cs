using UnityEngine;

public class ObjectBase : MonoBehaviour
{
	[SerializeField] private Direnction direnction;

	protected BoxCollider2D bodyCollider;
	protected BoxCollider2D attackCollider;
	protected BoxCollider2D damageCollider;

	enum Direnction
    {
        [InspectorName("âE")] Right,
		[InspectorName("ç∂")] Left,
	}
    
    protected virtual void Start()
    {
		bodyCollider = GetComponent<BoxCollider2D>();
		var collider = transform.Find("Collider");
		attackCollider = collider.Find("Attack").GetComponent<BoxCollider2D>();
		damageCollider = collider.Find("Damage").GetComponent<BoxCollider2D>();
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
