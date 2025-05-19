using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Rigidbody), typeof(ConstantForce))]
public class FeatherFall : MonoBehaviour
{
    [SerializeField, Range(0.01f, 1f)] private float m_FloatForce = 0.8f;
    [SerializeField, Range(0.01f, 3f)] private float m_SlidePower = 0.2f;
    [SerializeField, Range(0.01f, 1f)] private float m_PuffPower = 0.05f;
    [SerializeField, Range(0.01f, 1f)] private float m_PuffDelayMin = 0.2f;
    [SerializeField, Range(0.01f, 1f)] private float m_PuffDelayMax = 0.3f;

    private Rigidbody m_Rigidbody;
    private BoxCollider m_Collider;
    private Vector3 m_AntigravityForce;
    private float m_LastTime;
    private float m_Delay;
    private Vector3[] m_EdgePoints;
    private Vector3 m_SlideVector;
    private Vector3 m_LastPuffPosition;
    private Vector3 m_LastPuffPower;
    private bool m_TouchingGround;

    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<BoxCollider>();
        m_AntigravityForce = GetAntigravityForce();
        GetComponent<ConstantForce>().force = m_AntigravityForce * m_FloatForce;

        // Build edge-points on the XY plane (treat Z as the 'thin' axis)
        Vector3 center = m_Collider.center;
        Vector3 size = m_Collider.size * 0.5f;

        m_EdgePoints = new[]
        {
            new Vector3( 0f, -size.y, 0f) + center,  // bottom
            new Vector3( 0f,  size.y, 0f) + center,  // top
            new Vector3(-size.x,  0f,   0f) + center,  // left
            new Vector3( size.x,  0f,   0f) + center,  // right
            new Vector3(-size.x, -size.y, 0f) + center,  // bottom-left
            new Vector3( size.x, -size.y, 0f) + center,  // bottom-right
            new Vector3(-size.x,  size.y, 0f) + center,  // top-left
            new Vector3( size.x,  size.y, 0f) + center   // top-right
        };
    }

    private void Update()
    {
        if (!m_TouchingGround)
        {
            UpdateSlide();
            UpdatePuffs();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        m_TouchingGround = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        m_TouchingGround = false;
    }
    private void UpdateSlide()
    {
        // Slide across the XY face; its normal is local Z
        Vector3 normal = transform.forward;

        m_SlideVector.x = normal.x * normal.y;
        m_SlideVector.z = normal.z * normal.y;
        m_SlideVector.y = -(normal.x * normal.x) - (normal.z * normal.z);

        m_Rigidbody.AddForce(m_SlideVector.normalized * m_SlidePower);
    }

    private void UpdatePuffs()
    {
        if (m_LastTime + m_Delay < Time.time)
            Puff();
    }

    private void Puff()
    {
        float downwardVelocity = -m_Rigidbody.linearVelocity.y;
        if (downwardVelocity > 0.001f)
        {
            m_LastTime = Time.time;
            m_Delay = Random.Range(m_PuffDelayMin, m_PuffDelayMax);

            Vector3 puffPosition = GetPuffPosition();
            m_LastPuffPower = m_AntigravityForce * m_PuffPower * downwardVelocity;
            m_LastPuffPosition = transform.InverseTransformPoint(puffPosition);

            m_Rigidbody.AddForceAtPosition(m_LastPuffPower, puffPosition, ForceMode.Impulse);
        }
    }

    private Vector3 GetPuffPosition()
    {
        Vector3 worldOffset = m_Collider.bounds.center;
        List<Vector3> worldEdges = m_EdgePoints
            .Select(pt => transform.TransformPoint(pt))
            .ToList();

        // Pick only those on the 'underside' relative to Z
        var validEdges = worldEdges.Where(v => v.z <= worldOffset.z).ToList();
        if (validEdges.Count == 0)
            validEdges = worldEdges;

        int idx = Random.Range(0, validEdges.Count);
        return validEdges[idx];
    }

    private Vector3 GetAntigravityForce()
    {
        float totalMass = transform
            .GetComponentsInChildren<Rigidbody>()
            .Sum(rb => rb.mass);
        return Physics.gravity * totalMass * -1f;
    }

    private void OnDrawGizmos()
    {
        if (m_EdgePoints == null) return;

        // Slide vector (blue)
        Debug.DrawRay(transform.position, m_SlideVector, Color.blue);

        // Last puff (fading white)
        Vector3 puffWorld = transform.TransformPoint(m_LastPuffPosition);
        Color fadeCol = Color.white;
        float t = (Time.time - m_LastTime) / m_Delay;
        fadeCol.a = 1f - Mathf.Clamp01(t);
        Debug.DrawRay(puffWorld, m_LastPuffPower * 10f, fadeCol);

        // Edge?point gizmos
        foreach (var pt in m_EdgePoints)
        {
            Vector3 worldPt = transform.TransformPoint(pt);
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(worldPt, 0.01f);
        }
    }
}
