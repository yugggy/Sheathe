using UnityEngine;

public static class Utility
{
    /// <summary>
    /// オブジェクトのアニメフラグ変更
    /// </summary>
    public static void SetAnimationFlg(Animator animator, string animFlgName, bool value = true)
    {
        var animFlgNameHash = Animator.StringToHash(animFlgName);
        animator.SetBool(animFlgNameHash, value);
    }
}
