using UnityEngine;

/// <summary>
/// 敵：空中敵
/// </summary>
public class EN002 : EnemyBase
{
    private float _initY;
    [SerializeField, Label("斬られてから撃破までの時間")] private float _explosionTime;
    
    protected override void Start()
    {
        base.Start();
        _initY = transform.position.y;
        ChaseVelocity = 2.0f;
                
        // 撃破時間設定
        ExplosionTime = _explosionTime;
    }

    protected override void ObjectUpdate()
    {
        base.ObjectUpdate();
        if (!IsSlashed)
        {
            SinMove();
        }
    }

    /// <summary>
    /// 上下移動
    /// </summary>
    private void SinMove()
    {
        float sin = Mathf.Sin(Time.time*2) * 0.2f;
        transform.position = new Vector3(transform.position.x,_initY + sin,transform.position.z);
    }

    /// <summary>
    /// 斬られた
    /// </summary>
    public override void SetSlashed()
    {
        base.SetSlashed();
        Utility.SetAnimationFlg(ObjAnimator, "IsFlash1");
    }
}