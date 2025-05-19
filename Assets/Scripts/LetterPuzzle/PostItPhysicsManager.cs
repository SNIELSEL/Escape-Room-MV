using UnityEngine;

public class PostItPhysicsManager : MonoBehaviour
{
    public void ToggleFall(bool state)
    {
        //if not holding paper paper fall if not then paper not fall
        GetComponent<FeatherFall>().enabled = state;
    }
}
