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
        mainCamera.transform.position = new Vector3(grid.nodeSize * grid.gridSize.x /2, grid.nodeSize * grid.gridSize.y / 2, - 200);
    }

    // Update is called once per frame
    void Update () {
        Vector3 newVectror = mainCamera.transform.position;
        newVectror.z += Input.mouseScrollDelta.y;
        mainCamera.transform.position = newVectror;

    }
}
