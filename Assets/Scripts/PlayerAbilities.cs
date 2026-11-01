using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    public bool CanUsePowers => lockEndTime <= Time.time;
    private float lockEndTime;
    public void Springlock(float seconds) => lockEndTime = Mathf.Max(lockEndTime, Time.time + Mathf.Max(0, seconds));
    public void ClearSpringlock() => lockEndTime = 0;
}
