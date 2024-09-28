using UnityEngine;

namespace Lemur
{
    public interface ITriggerBubbleUpListener
    {
        void OnBubbleUpTriggerEnter(GameObject internalObj, Collider2D incoming);
        void OnBubbleUpTriggerExit(GameObject internalObj, Collider2D incoming);
    }
}