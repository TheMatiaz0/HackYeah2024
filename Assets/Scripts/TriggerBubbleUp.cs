using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Lemur
{
    public class TriggerBubbleUp : MonoBehaviour
    {
        private ITriggerBubbleUpListener listener;

        private void Start()
        {
            Transform father=this.transform.parent;
            while (father != null)
            {
                ITriggerBubbleUpListener listener = father.GetComponent<ITriggerBubbleUpListener>();
                if (listener != null)
                {
                    this.listener = listener;
                    break;
                }

                father = father.parent;
            }

            if (listener == null)
            {
                Debug.LogError("No one to bubble the trigger to");
                
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            listener.OnBubbleUpTriggerEnter(this.gameObject,other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            listener.OnBubbleUpTriggerStay(this.gameObject,other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            listener.OnBubbleUpTriggerExit(this.gameObject,other);
        }
    }
}