using UnityEngine;

public class TestAttacker : Attackable
{
    void Awake()
    {
        power = 5;
    }

    public override void Attack()
    {
        Debug.Log("때리는 중!! 아프지?!");
    }
}
