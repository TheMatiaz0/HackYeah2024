using System;
using System.Linq;
using Honey;
using Rubin;
using UnityEngine;

namespace Lemur
{
    public class Movement : MonoBehaviour, ITriggerBubbleUpListener
    {
        
        [Header("Keys")]
        [SearchableEnum]
        [SerializeField] private KeyCode left = KeyCode.A;
        [SearchableEnum]
        [SerializeField] private KeyCode right = KeyCode.D;
        [SearchableEnum]
        [SerializeField] private KeyCode jump = KeyCode.Space;
        
        [Header("References")]
        [SerializeField] private Collider2D leftStickyCollider;
        [SerializeField] private Collider2D rightStickyCollider;
        [SerializeField] private Collider2D feetCollider;
        [SerializeField]
        private LayerMask groundMask;
        
        
        [Header("jumping")]
        [SerializeField]
        private float jumpVelocity;
        [SerializeField]
        private float maxJumpTime = 1;
        
        [Header("Left/Right movement")]
        [SerializeField]
        private float accSpeed=3;
        [SerializeField]
        private float decSpeed = 5;
        [SerializeField]
        private float maxSpeed = 5;

        [Tooltip("If false, when object is moving in the right direction and left key is pressed, untill velocity = 0, dec is used\n If true acc is used")]
        [SerializeField] private bool fastRotation=false;
        
        
        [Header("Wallrun")]
        [SerializeField] private float wallRunVelocity=2;
        [SerializeField] private float wallRunStartTime=0.5f;
        [SerializeField] private float wallRunTime=3f;
        
        [Tooltip("template direction is right")] [SerializeField]
        private Vector2 bounceTemplateVector= new Vector2(2,3);

        private Rigidbody2D rigi;
        private SpriteRenderer renderer;
        
        
        private Ticker jumpProgress ;
        private Ticker canWallRunTimer; 
        private Ticker wallRunTimer; 

        
        private Possibility ability;
        
        private float stickyFromSide;

        public enum Possibility
        {
            All,
            Jump,
            Wall,
            None,
        }

        private void Awake()
        {
            renderer = this.GetComponent<SpriteRenderer>();
            this.rigi = this.GetComponent<Rigidbody2D>();
            jumpProgress= TickerCreator.CreateNormalTime(maxJumpTime); 
            canWallRunTimer= TickerCreator.CreateNormalTime(wallRunStartTime);
            wallRunTimer = TickerCreator.CreateNormalTime(wallRunTime);
            jumpProgress.ForceFinish();
            canWallRunTimer.ForceFinish();
            wallRunTimer.ForceFinish();
        }


        private void Move(float dir)
        {
           PushIn(dir,true);
           if (!wallRunTimer.Done)
           {
               if (stickyFromSide == -dir)
               {
                   Debug.Log("bounceeee");
                   ForceStickyExit();
                   BounceOff(dir);
               }
           }
        }

        private void BounceOff(float dir)
        {
            this.rigi.velocity += new Vector2(dir*bounceTemplateVector.x, bounceTemplateVector.y);
        }

        private void Update()
        {
            if (Input.GetKey(left))
            {
                Move(-1);
            }
            else if(Input.GetKey(right))
            {
                Move(1);
            }
            else
            {
                PushIn( - Mathf.Sign(this.rigi.velocity.x) ,false);
            }

            if (Input.GetKey(jump))
            {
                if (!canWallRunTimer.Done )
                {

                    ability = Possibility.None;
                    canWallRunTimer.ForceFinish();
                    jumpProgress.ForceFinish();
                    wallRunTimer.Reset();
                }


                else if (!wallRunTimer.Done)
                {
                    this.rigi.velocity=this.rigi.velocity.My(wallRunVelocity);
                }
                else
                {
                    
                    TryJump();
                }
                
            }
            else
            {
                wallRunTimer.ForceFinish();
                //canWallRunTimer.ForceFinish();
                LetGoJump();
            }


            if (!wallRunTimer.Done)
            {
                
                this.renderer.color = Color.red;
            }
            else if (!jumpProgress.Done)
            {
                
                this.renderer.color = Color.blue;
            }
            else
            {
                this.renderer.color = Color.white;
            }
        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
        }

        public void LetGoJump()
        {
            jumpProgress.ForceFinish();
        }



        private void TryJump()
        {
            if (IsAtLeast(Possibility.Jump))
            {
                ability = Possibility.Wall;
                jumpProgress.Reset();
                Debug.Log("jump");
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

        public void OnBubbleUpTriggerEnter(GameObject internalObj, Collider2D incoming)
        {
            
            if ( ( groundMask.value &   (1<<incoming.gameObject.layer)) !=0 && feetCollider.gameObject == internalObj)
            {
                ability = Possibility.All;
            }
            if ( ( groundMask.value &   (1<<incoming.gameObject.layer)) !=0 ) {

                if (leftStickyCollider.gameObject == internalObj)
                {
                    stickyFromSide = -1;
                }
                else if (rightStickyCollider.gameObject == internalObj)
                {
                    stickyFromSide = 1;
                }
                else
                {
                    return;
                }
                if (IsAtLeast(Possibility.Wall))
                    canWallRunTimer.Reset();
            }
        }

        public bool IsAtLeast(Possibility pos)
        {
            return (int) ability <= (int) pos;
        }

        public void OnBubbleUpTriggerExit(GameObject internalObj, Collider2D incoming)
        {
            if ( ( groundMask.value &    (1<<incoming.gameObject.layer) ) !=0  && new Collider2D[]{leftStickyCollider,rightStickyCollider}.Any(item => item.gameObject == internalObj)) {
                ForceStickyExit();
            }
        }

        private void ForceStickyExit()
        {
            if (!canWallRunTimer.Done)
            {
                canWallRunTimer.ForceFinish();
            }
            if (!wallRunTimer.Done)
            {
                wallRunTimer.ForceFinish();
            }    
        }
    }
}