using UnityEngine;

public class DamageCheck : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.name != "Attack")
		{
			return;
		}
        var attackChara = collision.transform.parent.parent.parent;
		var damageChara = transform.parent.parent.parent;
		if (attackChara == null || damageChara == null)
		{
			return;
		}
		SlashBase slashObj;
        var isAttackCharaEnemy = attackChara.TryGetComponent<SlashBase>(out var temp);
		var isDamageCharaEnemy = damageChara.TryGetComponent<SlashBase>(out slashObj);

		// スキップ判定
		// ・自分自身だった場合
		// ・敵同士のフレンドリーファイヤー
		// ・既にダメージを受けている場合
		if (attackChara == damageChara ||
			(isAttackCharaEnemy && isDamageCharaEnemy) ||
			slashObj.IsSlashed)
		{
            return;
        }

		// 斬られた
		slashObj.SetSlashed();

		Debug.Log($"攻撃:{attackChara.name}, 被ダメージ:{damageChara.name}");
	}
}
