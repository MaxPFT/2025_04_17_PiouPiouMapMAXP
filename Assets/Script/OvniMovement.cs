using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class OvniMovement : MonoBehaviour
{
    [SerializeField] private Transform m_whatIMove;
    [SerializeField] private Transform m_launchPoint;
    [SerializeField] private Transform m_container;
    [SerializeField] private GameObject m_projectile;

    [SerializeField] private float m_timeBeforeDestruction;
    [SerializeField] private float m_launchVelocity;
    [SerializeField] private float m_fireRate, m_nextFireRate;

    [Header("Speed")]
    [SerializeField] private float m_speedDirection;
    [SerializeField] private float m_speedRotation;
    [SerializeField] private float m_speedGoUp;
    [SerializeField] private float m_speedMoveLR;

    [Header("Percent")]
    [SerializeField, Range(-1,1)] private float m_percentSpeedDirection;
    [SerializeField, Range(-1,1)] private float m_percentSpeedRotation;
    [SerializeField, Range(-1,1)] private float m_percentSpeedGoUp;
    [SerializeField, Range(-1,1)] private float m_percentSpeedLR;

    void Update()
    {
        float _delta = Time.deltaTime;
        m_whatIMove.Translate(Vector3.forward *  m_speedDirection * m_percentSpeedDirection * _delta, Space.Self);
        m_whatIMove.Rotate(0, m_speedRotation * m_percentSpeedRotation * _delta,0, Space.Self);

        m_whatIMove.Translate(Vector3.up * m_speedGoUp * m_percentSpeedGoUp * _delta, Space.Self);
        m_whatIMove.Translate(Vector3.right * m_speedMoveLR * m_percentSpeedLR * _delta, Space.Self);
    }

    public void Shoot()
    {
        var _projectile = Instantiate(m_projectile, m_launchPoint.position, m_launchPoint.rotation, m_container);
        _projectile.GetComponent<Rigidbody>().linearVelocity = m_launchPoint.up * m_launchVelocity;
        Destroy(_projectile, m_timeBeforeDestruction);
    }

    public void SetAxisLeftToRight(float percentSpeedLR)
    {
        m_percentSpeedLR = percentSpeedLR;
    }

    public void SetAxisDirection(float percentSpeedDirection)
    {
        m_percentSpeedDirection = percentSpeedDirection;
    }

    public void SetAxisRotation(float percentSpeedRotation)
    {
        m_percentSpeedRotation = percentSpeedRotation;
    }

    public void SetAxisGoUp(float percentGoUp)
    {
        m_percentSpeedGoUp = percentGoUp;
    }


}
