using UnityEngine;

public class EN002 : EnemyBase
{
    private float initY;
    
    private void Start()
    {
        initY = transform.position.y;
    }

    protected override void Update()
    {
        base.Update();
        
        if (!IsSlashed)
        {
            SinMove();
        }
    }

    private void SinMove()
    {
        float sin = Mathf.Sin(Time.time*2) * 0.2f;
        transform.position = new Vector3(transform.position.x,initY + sin,transform.position.z);
    }
}