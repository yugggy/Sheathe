using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// 斬られるオブジェクト基底クラス
/// </summary>
public class SlashBase : ObjectBase
{
	[SerializeField, Label("斬られるオブジェクトかどうか")] private bool _isCanSlash = true;
	[SerializeField,Label("爆発するかどうか")] private bool _isExplosion = true;
	[SerializeField,Label("復活するかどうか")] private bool _isRespawn = false;
	
	private GameObject _slashAnime;
	private bool _isSlashed = false; // 斬られたフラグ
	private float _explosiontimer;
	protected float ExplosionTime = 5;

	public bool IsSlashed => _isSlashed;
	public bool IsCanSlash => _isCanSlash;
	public bool IsRespawn => _isRespawn;

	protected override void Start()
	{
		base.Start();
		
		// SlashAnime
		var slashAnimeTrans = ImageTrans.Find("SlashAnime");
		if (slashAnimeTrans  == null)
		{
			Debug.Log($"{name}プレハブにslashAnimeがありません");
		}
		else
		{
			_slashAnime = slashAnimeTrans.gameObject;
		}
	}
	
	/// <summary>
	/// 斬られた
	/// </summary>
	public virtual void SetSlashed()
	{
		_isSlashed = true;
		_slashAnime.SetActive(true);
		_explosiontimer = ExplosionTime;
	}

	protected override void ObjectUpdate()
	{
		base.ObjectUpdate();
		OverTime();
	}

	/// <summary>
	/// 斬られてから一定時間で撃破
	/// </summary>
	private void OverTime()
	{
		if (_isSlashed)
		{
			_explosiontimer -= Time.deltaTime;
			if (_explosiontimer <= 0)
			{
				_isSlashed = false;
				DestroyAsync();
			}	
		}
	}


	/// <summary>
	/// 撃破
	/// </summary>
	public async Task DestroyAsync()
	{
		// 爆発
		if (_isExplosion)
		{
			var obj = await SceneGameManager.Current.LoadAsync("Explosion");
			Instantiate(obj, transform.position, transform.rotation, transform.parent);
			Destroy(gameObject);
		}
		// 撃破アニメ
		else
		{
			var isSlashedHash = Animator.StringToHash("IsSlashed");
			ObjAnimator.SetBool(isSlashedHash, true);
			
			// TODO：アニメ終了待機
			// await WaitAnimeFinish();
			await Task.Delay(1000);
			
			Destroy(gameObject);
		}
	}
}
