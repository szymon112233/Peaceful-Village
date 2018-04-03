using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraCenter : MonoBehaviour {

    public float scrollSpeed = 10.0f;
    public NodeGrid grid = null;

    Camera mainCamera = null;
    
    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        CenterCamera();
    }

    public void CenterCamera()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        if (grid == null)
            grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<NodeGrid>();
        mainCamera.transform.position = new Vector3(grid.nodeSize * grid.gridSize.x /2.0f - grid.nodeSize/2.0f , grid.nodeSize * grid.gridSize.y / 2.0f - grid.nodeSize / 2.0f, - 10);
    }

    void Update () {
        
        if (mainCamera == null)
            mainCamera = Camera.main;
        
        mainCamera.orthographicSize -= Input.mouseScrollDelta.y * scrollSpeed;
        if (mainCamera.orthographicSize < 1)
            mainCamera.orthographicSize = 1;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Awake();
    }
}
