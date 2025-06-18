using UnityEngine;

/// <summary>
/// 敵：空中敵
/// </summary>
public class EN002 : EnemyBase
{
    private float _initY;
    
    protected override void Start()
    {
        base.Start();
        _initY = transform.position.y;
    }

    protected override void Update()
    {
        base.Update();
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
        var isFlash1Hash = Animator.StringToHash("IsFlash1");
        ObjAnimator.SetBool(isFlash1Hash, true);
    }
}