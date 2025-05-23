using UnityEngine;

public class Attackable : MonoBehaviour
{
    public float power;

    public virtual void Attack()
    {
        Debug.Log("때리는 중!!");
    }
}
