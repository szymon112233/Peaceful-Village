﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour {

    public GameObject objectOnNode = null;

    public bool isFree()
    {
        return objectOnNode == null;
    }
}