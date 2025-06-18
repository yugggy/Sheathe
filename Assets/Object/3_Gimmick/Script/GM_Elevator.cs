using UnityEngine;

/// <summary>
/// ギミック：エレベーター
/// </summary>
public class GM_Elevator : MonoBehaviour
{
    [SerializeField, Label("移動の速さ")] private float _velocityY;
    [SerializeField, Label("上方レイの長さ")] private float _upRayLength;
    [SerializeField, Label("下方レイの長さ")] private float _downRayLength;
    [SerializeField, Label("待機時間")] private float _waitTime = 1.0f; 
    
    private MoveState _moveState = MoveState.Wait;
    private float _waitCurrentTimer;
    
    private bool _isDown = true;
    private Vector3 _currentPos = Vector3.zero;

    enum MoveState
    {
        Wait,
        Down,
        Up,
    }
    
    private void Start()
    {
        _currentPos = transform.position;
    }
    
    private void Update()
    {
        Move();
    }

    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        switch (_moveState)
        {
            case MoveState.Wait:
                Wait();
                break;
            case MoveState.Down:
                UpDownMove();
                break;
            case MoveState.Up:
                UpDownMove();
                break;
        }

        // 待機
        void Wait()
        {
            _waitCurrentTimer += Time.deltaTime;
            if (_waitCurrentTimer >= _waitTime)
            {
                _waitCurrentTimer = 0;
                _moveState = _isDown ? MoveState.Down : MoveState.Up;
            }
        }
        
        // 上下に移動
        // TODO：現状突貫で降下させているため、修正して上下移動させる必要あり
        void UpDownMove()
        {
            int stageLayer = 1 << LayerMask.NameToLayer("Stage");
            if (Physics2D.Raycast(transform.position, Vector2.down, _downRayLength, stageLayer))
            {
                _isDown = false;
            }
        
            float velocityY = _isDown ? -_velocityY : 0;
            _currentPos.y += velocityY;
            transform.position = _currentPos;
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.up * _upRayLength);
        Gizmos.DrawRay(transform.position, Vector2.down * _downRayLength);
    }
}
