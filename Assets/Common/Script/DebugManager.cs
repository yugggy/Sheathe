using System;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Script
{
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
			_velocityText.text = $"velocity.x：{Math.Round(velocity.x, 4)}{Environment.NewLine}" +
			                     $"velocity.y：{Math.Round(velocity.y, 4)}";
		}
	}
}
