using System.Collections;
using UnityEngine;


public class InputSetting
{
    public static KeyCode ReloadKey = KeyCode.R; // 换弹键
    public static string JumpKey = "Jump"; // 跳跃键
    public static string ShootKey = "Fire1"; // 射击键
    public static string OpenMirrorKey = "Fire2"; // 开镜键
    public static string ForwardOrBackKey = "Vertical"; // 前进后退键
    public static string LeftOrRightKey = "Horizontal"; // 左右键
    public static KeyCode GetMainWeapon = KeyCode.Alpha1; // 左边数字1 -> 切换主武器
    public static KeyCode GetSubWeapon = KeyCode.Alpha2; // 左边数字2 -> 切换副武器
    public static KeyCode GetGrenade = KeyCode.Alpha4; // 左边数字4
}