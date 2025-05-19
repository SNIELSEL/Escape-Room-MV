using UnityEngine;

public class SpawnPin : MonoBehaviour
{
    public GameObject pin;
    public Transform parent;
    public Transform[] pinLoc = new Transform[15];
    public GameObject[] currentPins = new GameObject[15];
    public Vector3 offset;
    public void InstantiatePin(int id)
    {
        Vector3 pos = new Vector3(pinLoc[id].transform.position.x, pinLoc[id].transform.position.y, pinLoc[id].transform.position.z);
        pos += offset;
        Quaternion rot = Quaternion.Euler(0f, 90f, 0f);
        GameObject newPin = Instantiate(pin, pos, rot, parent);
        newPin.GetComponent<PinPostIt>().id = id;
    }
    public void DeletePin(int id)
    {
        currentPins = GameObject.FindGameObjectsWithTag("Pin");

        for(int i = 0;i < currentPins.Length;i++)
        {
            if (currentPins[i].GetComponent<PinPostIt>().id == id)
            {
                Destroy(currentPins[i]);
            }
        }
    }
}
