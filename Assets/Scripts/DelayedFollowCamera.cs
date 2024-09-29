using System;
using Rubin;
using UnityEngine;

namespace Lemur
{
    public class DelayedFollowCamera : MonoBehaviour
    {
        public float xDifLimit;
        public float acc;
        public float terminalSpeed;
        public float yOffset;
        
        private Ticker delay;
        private Vector2 lastFramePos;

        [SerializeField]
        private Transform followObj;
        private Vector2 startPos;


        private float curSpeed;
        

        private void Awake()
        {
            delay = TickerCreator.CreateNormalTime(1f);
            startPos = this.transform.position;
        }

        private Vector2 GetRealTargetPos()
        {
            float difX = Mathf.Clamp((followObj.transform.Get2Pos()).x - startPos.x,-xDifLimit,xDifLimit);
            float realX = startPos.x + difX;
            return new Vector2(realX, followObj.transform.position.y+yOffset);


        }

        private void Update()
        {

            if (Vector2.Distance( this.transform.position, (Vector2) GetRealTargetPos() )<0.1f)
            {
                curSpeed = 0;
                Debug.Log("Reset delay");
                delay.Reset();
            }
            if (GetRealTargetPos().y < this.transform.position.y - 0.5f)
            {
                delay.ForceFinish();   
            }
            if (delay.Done)
            {
                curSpeed = Mathf.Clamp( curSpeed+ acc * Time.deltaTime,0,terminalSpeed);
                this.transform.position += (Vector3) (Time.deltaTime *curSpeed*
                                           (GetRealTargetPos() - this.transform.Get2Pos()));
            }
            
            
            
            
        }
    }
}