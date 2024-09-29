using System;
using Rubin;
using UnityEngine;

namespace Lemur
{
    public class Paralax : MonoBehaviour
    {
        private Vector2 startPos;
        [SerializeField] private Transform cameraTransform;
        [Range(0,1)]
        [SerializeField] private float speed=0.3f;

        private void Start()
        {
            startPos = this.transform.position;
        }

        private void Update()
        {
            this.transform.position=  (cameraTransform.Get2Pos() - startPos) * speed + startPos;
        }
    }
}