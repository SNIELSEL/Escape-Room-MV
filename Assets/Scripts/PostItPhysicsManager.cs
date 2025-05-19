using UnityEngine;

public class PostItPhysicsManager : MonoBehaviour
{
    public void ToggleFall(bool state)
    {
        GetComponent<FeatherFall>().enabled = state;
    }
}
