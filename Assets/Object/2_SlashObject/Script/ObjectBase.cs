using System.Collections;
using UnityEngine;

/// <summary>
/// オブジェクト基底クラス
/// </summary>
public class ObjectBase : MonoBehaviour
{
	[SerializeField] private Direction _direction;
	[SerializeField] private Landing _landing;

	protected bool IsInit;
	protected Rigidbody2D ObjRigidBody;
	protected BoxCollider2D ObjBodyCollider;
	protected Transform ScaleTrans;
	protected Vector3 ObjScale;
	protected Transform ImageTrans;
	protected Animator ObjAnimator;
	protected BoxCollider2D ObjAttackCollider;
	protected BoxCollider2D ObjDamageCollider;
	
	int _hitStopTimer;

	
	public enum Direction
    {
        [InspectorName("右")] Right,
		[InspectorName("左")] Left,
	}

	public enum Landing
	{
		[InspectorName("地上")] Ground,
		[InspectorName("空中")] Air,
	}

	public void SetHitStop()
	{
		_hitStopTimer = SceneGameManager.Current.HitStopTime;
	}

	protected virtual void Start()
    {
	    // RigidBody2D
	    if (TryGetComponent<Rigidbody2D>(out var rb))
	    {
		    ObjRigidBody = rb;
	    }
	    else
	    {
		    Debug.Log($"{name}プレハブにRigidbody2Dがありません");
	    }
	    
	    // BoxCollider2D
	    if (TryGetComponent<BoxCollider2D>(out var body))
	    {
		    ObjBodyCollider = body;
	    }
	    else
	    {
		    Debug.Log($"{name}プレハブにBoxCollider2Dがありません");
	    }
	    
	    // Scale
	    ScaleTrans = transform.Find("Scale");
	    if (ScaleTrans == null)
	    {
		    Debug.Log($"{name}プレハブにScaleがありません");
		    return;
	    }
	    ObjScale = ScaleTrans.localScale;
	    
	    // Image
	    ImageTrans = ScaleTrans.Find("Image");
	    if (ImageTrans == null)
	    {
		    Debug.Log($"{name}プレハブにScale > Imageがありません");
		    return;
	    }
		
	    // Animator
	    if (ImageTrans.TryGetComponent<Animator>(out var animator))
	    {
		    ObjAnimator = animator;
	    }
	    else
	    {
		    Debug.Log($"{name}プレハブのScale > ImageにAnimatorが付いていません");
	    }
	    
	    // Collider
	    var objCollider = ScaleTrans.transform.Find("Collider");
	    if (objCollider  == null)
	    {
		    Debug.Log($"{name}プレハブにScale > Colliderがありません");
	    }
	    
	    var attackTrans = objCollider.Find("Attack");
	    if (attackTrans == null)
	    {
		    Debug.Log($"{name}プレハブにScale > Collider > Attackがありません");
	    }
	    else
	    {
		    if (attackTrans.TryGetComponent<BoxCollider2D>(out var attack))
		    {
			    ObjAttackCollider =  attack;
		    }
		    else
		    {
			    Debug.Log($"{name}プレハブのScale > Collider > AttackにBoxCollider2Dがありません");
		    }
	    }
	    
	    var damageTrans = objCollider.Find("Damage");
	    if (damageTrans == null)
	    {
		    Debug.Log($"{name}プレハブにScale > Collider > Damageがありません");
	    }
	    else
	    {
		    if (damageTrans.TryGetComponent<BoxCollider2D>(out var damage))
		    {
			    ObjDamageCollider = damage;
		    }
		    else
		    {
			    Debug.Log($"{name}プレハブのScale > Collider > DamageにBoxCollider2Dがありません");
		    }
	    }

	    IsInit = true;
    }

	private void Update()
	{
		if (_hitStopTimer > 0)
		{
			_hitStopTimer--;	
		}
		else
		{
			ObjectUpdate();
		}
	}
	
	protected virtual void ObjectUpdate()
	{
		
	}

	private void OnValidate()
	{
		SetDirection();
		SetGravityScale();
	}

    private void SetDirection()
    {
		var eulerAngles = transform.eulerAngles;
		eulerAngles.y = (_direction == Direction.Right) ? 0 : 180;
		transform.eulerAngles = eulerAngles;
	}
    
	public void SetDirection(bool isRight)
	{
		var eulerAngles = transform.eulerAngles;
		eulerAngles.y = isRight ? 0 : 180;
		transform.eulerAngles = eulerAngles;
	}

	/// <summary>
	/// タイプによって重力変更
	/// </summary>
	private void SetGravityScale()
	{
		var rb = transform.GetComponent<Rigidbody2D>();

		switch (_landing)
		{
			case Landing.Ground:
				// rb.gravityScale = 1;
				break;
			case Landing.Air:
				rb.gravityScale = 0;
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
		// TODO：アニメ待機がうまくいかないので下記対応
		yield return new WaitForSeconds(1);
		
		// アニメの切り替えのため1フレーム待機
		yield return null;

		// アニメが終了するまで待機
		while (ObjAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
		{
			//Debug.Log("normalizedTime" + _animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
			yield return null;
		}
	}
}
