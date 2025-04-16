using UnityEngine;

public class ThugEnemyChase : MonoBehaviour
{
    public Transform target;
    public float rotationSpeed = 5f;
    public float moveSpeed = 3f;
    public float minDistance = 1.5f;
    public LayerMask playerLayer;
    private bool playerFound = false;

    void Start()
    {
        if (target == null)
        {
            FindPlayerOnLayer();
        }
        else
        {
            playerFound = true;
        }
    }

    void Update()
    {
        if (playerFound && target != null)
        {
            Vector3 direction = target.position - transform.position;
            float distance = direction.magnitude;

            Vector3 rotationDirection = direction;

            if (rotationDirection != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(rotationDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
            }

            if (distance > minDistance)
            {
                direction.Normalize();

                transform.position += direction * moveSpeed * Time.deltaTime;
            }
        }
        else if (target == null)
        {
            playerFound = false;
            FindPlayerOnLayer();
        }
    }

    void FindPlayerOnLayer()
    {
        GameObject[] allObjects = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        int layerValue = LayerMaskToLayer(playerLayer);

        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == layerValue)
            {
                target = obj.transform;
                playerFound = true;
                break;
            }
        }
    }

    int LayerMaskToLayer(LayerMask layerMask)
    {
        int layerNumber = 0;
        int layer = layerMask.value;
        while (layer > 0)
        {
            layer = layer >> 1;
            layerNumber++;
        }
        return layerNumber - 1;
    }
}
