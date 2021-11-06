using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] [Range(0, 5)] float speed = 0.5f;

    List<Node> path = new List<Node>();
    Enemy enemy;

    PathFinder pathFinder;
    GridManager gridManager;

    void Awake()
    {
        enemy = GetComponent<Enemy>();
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<PathFinder>();
    }
    void OnEnable()
    {        
        ReturnToStart();
        RecalculatePath(true);
    }

    void RecalculatePath(bool resetPath)
    {
        Vector2Int coordinates = new Vector2Int();

        if(resetPath)
        {
            coordinates = pathFinder.StartCoordinate;
        }
        else
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        }

        StopAllCoroutines();

        path.Clear();
        path = pathFinder.GetNewPath(coordinates);   
        
        StartCoroutine(FollowPath());     
    }

    void ReturnToStart()
    {
        transform.position = gridManager.GetPositionFromCoordinates(pathFinder.StartCoordinate);
    }

    void FinishPath()
    {
        enemy.Steal();
        gameObject.SetActive(false);
    }

    IEnumerator FollowPath()
    {
        for (int i = 1; i< path.Count; i++)
        {
            Vector3 startPosition = this.transform.position;
            Vector3 endPosition = gridManager.GetPositionFromCoordinates(path[i].coordinates);
            float travelPercent = 0f;

            float rotationDegrees = degreesToRotate(this.transform.forward, startPosition, endPosition);

            while (travelPercent < 1f)
            {
                if (travelPercent < 0.25f)
                {
                    float rotationAngle = 4 * rotationDegrees * Time.deltaTime * speed;
                    this.transform.Rotate(new Vector3(0, rotationAngle, 0));
                }

                travelPercent += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(
                    startPosition,
                    endPosition,
                    travelPercent
                );
                yield return new WaitForEndOfFrame();
            }
        }

        FinishPath();
    }

    float degreesToRotate(Vector3 currentDir, Vector3 currentPosition, Vector3 endPosition)
    {
        Vector3 targetDir = endPosition - currentPosition;
        return Vector3.SignedAngle(currentDir, targetDir, Vector3.up);
    }
}
