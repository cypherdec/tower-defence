using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    [SerializeField] bool isPlaceable;
    public bool IsPlaceable { get { return isPlaceable; } }

    [SerializeField] Tower towerPrefab;

    GridManager gridManager;
    PathFinder pathFinder;
    Vector2Int coordinates = new Vector2Int();

    void Awake(){
        gridManager = FindObjectOfType<GridManager>();
        pathFinder = FindObjectOfType<PathFinder>();
    }

    void Start(){
        if(gridManager != null){
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);

            if(!isPlaceable){
                gridManager.BlockNode(coordinates);
            }
        }
    }

    void OnMouseDown()
    {
        if (gridManager.GetNode(coordinates).isWalkable && !pathFinder.WillBlockPath(coordinates))
        {
            Vector3 mousePos = transform.position;
            bool isSuccessfull = towerPrefab.CreateTower(towerPrefab, mousePos);

            if (isSuccessfull)
            {
                gridManager.BlockNode(coordinates);
                pathFinder.NotifyRecievers();
            }            
        }
    }
}
