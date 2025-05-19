using UnityEngine;
public class SocketStatus : MonoBehaviour
{
    public socket[] sockets = new socket[15];
}
[System.Serializable]
public struct socket
{
    public bool Correct;
    public int CorrectSockets;
    public bool Occupied;
    public int OccupiedSockets;
}