using Honey;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Screenshotter : MonoBehaviour
{
    [SerializeField]
    private int superSize = 2;
    [SerializeField]
    private string targetDirectory = "Screenshots";

    private void Awake()
    {
        if (GameObject.FindObjectsOfType<Screenshotter>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        TakeScreenshot();
    }

    [EMethodButton]
    private void TakeScreenshot()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            string path = DateTime.Now.ToString("yy=MM-dd hh-mm-ss");
            if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
            }

            path = Path.Join(Directory.GetCurrentDirectory(), targetDirectory, $"{path}.png");
            ScreenCapture.CaptureScreenshot(path, superSize);
        }
    }
}
