using UnityEngine;

/// <summary>
/// 敵基底クラス
/// </summary>
public class EnemyBase : SlashBase
{
    protected float ChaseVelocity = 0.002f;
    
    protected override void Update()
    {
        base.Update();
        PlayerChase();
    }

    /// <summary>
    /// 攻撃されたらプレイヤーを追いかける
    /// </summary>
    private void PlayerChase()
    {
        if (IsSlashed)
        {
            var vec = ObjectManager.Current.Player.transform.position - transform.position;
            vec.z = 0;
            vec.Normalize();
            transform.position += vec * ChaseVelocity;
        }
    }
}
