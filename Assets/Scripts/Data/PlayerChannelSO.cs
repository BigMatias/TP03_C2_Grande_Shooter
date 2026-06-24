using UnityEngine;

// IA
[CreateAssetMenu(menuName = "Channels/Player Channel")]
public class PlayerChannelSO : ScriptableObject
{
    public Transform PlayerTransform { get; private set; }

    public void Register(Transform player)
    {
        PlayerTransform = player;
    }
}