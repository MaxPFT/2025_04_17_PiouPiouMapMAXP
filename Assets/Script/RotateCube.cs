using UnityEngine;

public class RotateCube : MonoBehaviour
{
    [SerializeField] private float _speedRotation;
    [SerializeField] private float _speedScale;
    [SerializeField] Vector3 _rotationAxis;

    void Update()
    {
        transform.Rotate(_rotationAxis * _speedRotation * Time.deltaTime);
    }

    public float SineAmount()
    {
        return Mathf.Sin(_speedScale * Time.time);
    }

}
