using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
	private List<SlashBase> _slashList = new List<SlashBase>(50);

	public static ObjectManager Current;
	public List<SlashBase> SlashList => _slashList;

	private void Awake()
	{
		Current = this;
	}

	/// <summary>
	/// ������
	/// </summary>
	public void SetSlashObjectList(SlashBase slashObj)
	{
		_slashList.Add(slashObj);
	}

	/// <summary>
	/// �S�Ŕ���
	/// </summary>
	public bool GetDestroyCompletely()
	{
		return !_slashList.Any(x => !x.IsSlashed);
	}

	/// <summary>
	/// ���X�g������
	/// </summary>
	public void ClearSlashObjectList()
	{
		_slashList.Clear();
	}
}
