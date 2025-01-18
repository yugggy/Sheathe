using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour
{
    [SerializeField] private Text velocityText;

    public static DebugManager Current;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
		Current = this;
	}

    // Update is called once per frame
    void Update()
    {
	}

    public void SetVelocityText(Vector3 velocity)
    {
		velocityText.text = $"velocityÅF({velocity.x}, {velocity.y}, {velocity.z})";
	}
}
