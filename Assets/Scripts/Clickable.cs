using System;
using DG.Tweening;
using Honey;
using Honey.Helper;
using Rubin;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Lemur
{
    public class Clickable : MonoBehaviour
    {

        [SearchableEnum]
        [SerializeField] private KeyCode key;
        [SerializeField] private float fadeTime;
         [SerializeField] private Graphic[] graphics;
        
        private  bool isIn;
        public UnityEvent onPressed;
        

        private void Awake()
        {
            this.graphics.Foreach(item => item.color = new Clr(item.color, 0));
            
        }

        private void Update()
        {
            if (Input.GetKeyDown(key))
            {
                onPressed.Invoke();
            }

            float dir = ((isIn) ? 1 : 0) - graphics[0].color.a;
            if (Mathf.Abs(dir) < 0.001f)
            {
                return;
            }
            foreach (var item in graphics)
            {
                
                item.color = new Clr(item.color, Mathf.Clamp( item.color.a + Time.deltaTime * Mathf.Sign( dir) *(1/fadeTime),0,1));
            }
        }
        private void Change(bool v)
        {
            isIn = v;
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Change(true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Change(false);
            }
        }
        
        
        
    }
}