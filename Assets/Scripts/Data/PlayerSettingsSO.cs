using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Player/Data")]

public class PlayerSettingsSO : ScriptableObject
{
    [Header("General Configs: ")]
    public float mouseSens;
    public float speed;
    public float jumpForce;
    public float m1Damage;
    public float m2Damage;
}