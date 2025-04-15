using UnityEngine;

public class RotateCube : MonoBehaviour
{
    [SerializeField] private float _speedRotation;
    [SerializeField] Vector3 _rotationAxis;

    void Update()
    {
        transform.Rotate(_rotationAxis * _speedRotation * Time.deltaTime);
    }
}
