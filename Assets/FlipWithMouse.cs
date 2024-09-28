using System;
using System.Collections;
using System.Collections.Generic;
using Rubin;
using UnityEngine;

public class FlipWithMouse : MonoBehaviour
{


    private Camera camera;

    private void Awake()
    {
        this.camera = Camera.main;
    }

    private void Update()
    {
        Vector2 pos = camera.ScreenToWorldPoint(Input.mousePosition);
        this.transform.SetRawFlip(pos.x>this.transform.position.x? 1: -1);

    }
}
