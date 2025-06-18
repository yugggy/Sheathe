using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// デバッグ用表示
/// </summary>
public class DebugManager : MonoBehaviour
{
    [SerializeField] private Text _velocityText;

    public static DebugManager Current;
	
    void Start()
    {
		Current = this;
	}

    public void SetVelocityText(Vector3 velocity)
    {
		_velocityText.text = $"velocity：({velocity.x}, {velocity.y}, {velocity.z})";
	}
}
