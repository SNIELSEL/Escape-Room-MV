using UnityEngine;

public class ObjectRespawnPlane : MonoBehaviour
{
    PinBoardManager pinBoardManager;

    void Awake()
    {
        pinBoardManager = GameObject.FindGameObjectWithTag("PinBoard").GetComponent<PinBoardManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PostItNote"))
        {
            pinBoardManager.RespawnPiece(other.gameObject);
        }
    }
}
