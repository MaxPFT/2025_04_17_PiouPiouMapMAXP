using System.Collections.Generic;
using UnityEngine;

public class ThugEnemyGenerator : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private Transform centerPoint;
    [SerializeField] private float minDistance = 3f;
    [SerializeField] private float maxDistance = 5f;
    [SerializeField] private int numberOfCubes = 10;
    [SerializeField] private bool randomizeScale = false;
    [SerializeField] private Vector2 scaleRange = new Vector2(0.5f, 1.5f);
    [SerializeField] private int maxPlacementAttempts = 30;

    private List<Vector3> placedCubePositions = new List<Vector3>();
    private List<float> placedCubeScales = new List<float>();

    public void GenerateCubes()
    {
        ClearCubes();
        placedCubePositions.Clear();
        placedCubeScales.Clear();

        if (centerPoint == null)
            centerPoint = this.transform;

        int cubesPlaced = 0;
        int totalAttempts = 0;
        int maxTotalAttempts = numberOfCubes * maxPlacementAttempts;

        while (cubesPlaced < numberOfCubes && totalAttempts < maxTotalAttempts)
        {
            Vector3 randomDirection = Random.onUnitSphere;

            float randomDistance = Random.Range(minDistance, maxDistance);

            Vector3 cubePosition = centerPoint.position + (randomDirection * randomDistance);

            float cubeScale = 1f;
            if (randomizeScale)
            {
                cubeScale = Random.Range(scaleRange.x, scaleRange.y);
            }

            if (IsPositionValid(cubePosition, cubeScale))
            {
                GameObject cube = Instantiate(cubePrefab, cubePosition, Quaternion.identity);

                if (randomizeScale)
                {
                    cube.transform.localScale = new Vector3(cubeScale, cubeScale, cubeScale);
                }

                cube.transform.parent = centerPoint;

                cube.name = "Cube_" + cubesPlaced;

                placedCubePositions.Add(cubePosition);
                placedCubeScales.Add(cubeScale);

                cubesPlaced++;
            }

            totalAttempts++;
        }

        if (cubesPlaced < numberOfCubes)
        {
            Debug.LogWarning("Impossible de placer tous les cubes sans chevauchement. " + cubesPlaced + "/" + numberOfCubes + " cubes placés.");
        }
    }

    private bool IsPositionValid(Vector3 position, float scale)
    {
        for (int i = 0; i < placedCubePositions.Count; i++)
        {
            Vector3 existingPosition = placedCubePositions[i];
            float existingScale = placedCubeScales[i];

            float diagonalThis = Mathf.Sqrt(3) * scale;
            float diagonalExisting = Mathf.Sqrt(3) * existingScale;
            float minRequiredDistance = (diagonalThis + diagonalExisting) / 2;

            float actualDistance = Vector3.Distance(position, existingPosition);
            if (actualDistance < minRequiredDistance)
            {
                return false;
            }
        }

        return true;
    }

    public void ClearCubes()
    {
        if (centerPoint != null)
        {
            int childCount = centerPoint.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                if (Application.isPlaying)
                    Destroy(centerPoint.GetChild(i).gameObject);
                else
                    DestroyImmediate(centerPoint.GetChild(i).gameObject);
            }
        }
    }

    private void Start()
    {
        if (centerPoint == null)
            centerPoint = this.transform;
        GenerateCubes();
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 center = centerPoint != null ? centerPoint.position : transform.position;

        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawWireSphere(center, minDistance);

        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawWireSphere(center, maxDistance);

        if (!Application.isPlaying && placedCubePositions != null)
        {
            Gizmos.color = Color.blue;
            for (int i = 0; i < placedCubePositions.Count; i++)
            {
                float scale = placedCubeScales[i];
                Gizmos.DrawWireCube(placedCubePositions[i], Vector3.one * scale);
            }
        }
    }
}
