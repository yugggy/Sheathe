using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// 斬られるオブジェクト基底クラス
/// </summary>
public class SlashBase : ObjectBase
{
	[SerializeField] private bool _isCanSlash = true; // 斬られるオブジェクトかどうか
	[SerializeField] private bool _isExplosion = true; // 爆発するかどうか
	
	private GameObject _slashAnime;
	private bool _isSlashed = false; // 斬られたフラグ

	public bool IsSlashed => _isSlashed;
	public bool IsCanSlash => _isCanSlash;

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
