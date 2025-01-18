
public class LabelAttribute : UnityEngine.PropertyAttribute
{
#if (UNITY_EDITOR) && (!_NODEBUG)
	public readonly string Value;
#endif

	public LabelAttribute(string value)
	{
#if (UNITY_EDITOR) && (!_NODEBUG)
		Value = value;
#endif
	}
}
