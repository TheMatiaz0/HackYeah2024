using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Lemur
{
    public class TutorialWindow : MonoBehaviour
    {
        public void Go()
        {
            SceneManager.LoadScene("vacuum test");
        }

    }
}