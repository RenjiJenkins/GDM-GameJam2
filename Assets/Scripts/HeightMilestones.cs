using UnityEngine;
using UnityEngine.Events;

public class HeightMilestones : MonoBehaviour
{
    public Transform player;
    public float startHeight = 0f, heightPerLevel = 10f;
    public int maxLevel = 10;
    public UnityEvent onFinalLevelReached;
    public int CurrentLevel { get; private set; } = 1;
    void Update()
    {
        if (!player || heightPerLevel <= 0) return;
        int level = Mathf.Clamp(1 + Mathf.FloorToInt((player.position.y - startHeight) / heightPerLevel), 1, Mathf.Max(1, maxLevel));
        if (level <= CurrentLevel) return;
        CurrentLevel = level;
        Debug.Log("Reached level " + CurrentLevel);
        if (CurrentLevel >= maxLevel) onFinalLevelReached?.Invoke();
    }
}
