using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class SlashBase : ObjectBase
{
	[SerializeField] private GameObject slashAnime;
	[SerializeField] private bool isCanSlash = true; // 斬られるオブジェクトかどうか
	[SerializeField] private bool isExplosion = true; // 爆発するかどうか

	private bool isSlashed = false; // 斬られたフラグ

	public bool IsSlashed => isSlashed;
	public bool IsCanSlash => isCanSlash;

	/// <summary>
	/// 斬られた
	/// </summary>
	public virtual void SetSlashed()
	{
		isSlashed = true;
		slashAnime.SetActive(true);
		StageManager.Current.StartFirstAttack();
	}

	/// <summary>
	/// 撃破
	/// </summary>
	public async void Destroy()
	{
		// 爆発
		if (isExplosion)
		{
			var explosionHandle = Addressables.LoadAssetAsync<GameObject>($"Explosion");
			var explosion = await explosionHandle.Task;
			if (explosion == null)
			{
				Debug.Log($"爆発のエフェクトがありません");
			}
			Instantiate(explosion, transform.position, transform.rotation, transform.parent);
		}
		
		Destroy(gameObject);
	}
}
