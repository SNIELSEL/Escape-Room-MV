using UnityEngine;

public class PinPostIt : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    public float m_Speed;

    bool active = true;

    public int id;
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        active = false;
    }

    private void Update()
    {
        if (active)
        {
            m_Rigidbody.linearVelocity = -transform.forward * m_Speed;
        }
    }
}
