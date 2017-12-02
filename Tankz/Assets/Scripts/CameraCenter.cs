using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCenter : MonoBehaviour {

    public float scrollSpeed = 10.0f;
    public NodeGrid grid = null;

    Camera mainCamera = null;
    
    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Start()
    {
        mainCamera.transform.position = new Vector3(grid.nodeSize * grid.gridSize.x /2.0f - grid.nodeSize/2.0f , grid.nodeSize * grid.gridSize.y / 2.0f - grid.nodeSize / 2.0f, - 10);
    }

    void Update () {
        mainCamera.orthographicSize -= Input.mouseScrollDelta.y * scrollSpeed;
        if (mainCamera.orthographicSize < 1)
            mainCamera.orthographicSize = 1;
    }
}
