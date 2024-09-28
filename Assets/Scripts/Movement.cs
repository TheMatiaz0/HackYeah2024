using System;
using Rubin;
using UnityEngine;
using UnityEngine.Serialization;

namespace Lemur
{
    public class Movement : MonoBehaviour
    {
        [FormerlySerializedAs("jumpRadius")] [SerializeField] private float jumpColliderRadius=0.2f;
        [SerializeField] private Transform feet;
        
        [SerializeField]
        private float jumpVelocity;

        [SerializeField]
        private LayerMask groundMask;
        
        private Rigidbody2D rigi;
        [SerializeField]
        private float accSpeed=3;
        [SerializeField]
        private float decSpeed = 5;
        [SerializeField]
        private float maxSpeed = 5;

        [FormerlySerializedAs("maxJumptime")] [SerializeField]
        private float maxJumpTime = 1;


        private Ticker jumpProgress ;


        [Tooltip("If false, when object is moving in the right direction and left key is pressed, untill velocity = 0, dec is used\n If true acc is used")]
        [SerializeField] private bool fastRotation=false;
        

        private void Awake()
        {
            this.rigi = this.GetComponent<Rigidbody2D>();
            jumpProgress= TickerCreator.CreateNormalTime(maxJumpTime); 
            jumpProgress.ForceFinish();
        }


        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                PushIn(-1,true);
            }
            else if(Input.GetKey(KeyCode.RightArrow))
            {
                PushIn(1,true);
            }
            else
            {
                PushIn( - Mathf.Sign(this.rigi.velocity.x) ,false);
            }

            if (Input.GetKey(KeyCode.Space))
            {
                TryJump();
            }
            else
            {
                LetGoJump();
            }
        }

        public void LetGoJump()
        {
            jumpProgress.ForceFinish();
        }


        private void OnDrawGizmos()
        {
           Gizmos.DrawWireSphere(feet.transform.position, jumpColliderRadius); 
        }

        private void TryJump()
        {
            if (Physics2D.OverlapCircle(feet.transform.position, jumpColliderRadius, groundMask) && jumpProgress.Done )
            {
                jumpProgress.Reset();
                KeepJumping();
            }
            else if (!jumpProgress.Done)
            {
                KeepJumping();
            }
        }

        void KeepJumping()
        {
            this.rigi.velocity = new Vector2(this.rigi.velocity.x, jumpVelocity);
            
        }

        private void PushIn(float dir, bool isManual)
        {


            float sign = Mathf.Sign( this.rigi.velocity.x);

            if (dir == Mathf.Sign(this.rigi.velocity.x) )
                this.rigi.velocity = this.rigi.velocity.Mx(  RHelper.CapMaxAbs(  this.rigi.velocity.x+ Time.deltaTime * accSpeed * dir,maxSpeed));
            else
                this.rigi.velocity = this.rigi.velocity.Mx(  RHelper.CapMaxAbs(  this.rigi.velocity.x+ Time.deltaTime * ((fastRotation && isManual)? accSpeed : decSpeed) * dir,maxSpeed));


            if (sign != Mathf.Sign(this.rigi.velocity.x) && !isManual)
            {
                this.rigi.velocity = this.rigi.velocity.Mx(0);
            }
            
        }
    }
}