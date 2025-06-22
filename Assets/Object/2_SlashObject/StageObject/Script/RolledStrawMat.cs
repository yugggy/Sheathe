using UnityEngine;

/// <summary>
/// 巻き藁
/// </summary>
public class RolledStrawMat : StageObjectBase
{
    [SerializeField, Label("斬られてから撃破までの時間")] private float _explosionTime;
    
    protected override void Start()
    {
        base.Start();
        
        // 撃破時間設定
        ExplosionTime = _explosionTime;
    }
}
