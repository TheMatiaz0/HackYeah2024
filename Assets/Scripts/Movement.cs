using System;
using System.Linq;
using Honey;
using Rubin;
using Unity.Burst.Intrinsics;
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

        [Header("Anims")] [SerializeField] private AnimationClip running;

        private SimpleAnimation animer;
            
            

         private Camera camera;
        
        [Header("References")]
        [SerializeField] private Collider2D feetCollider;
        [SerializeField]
        private LayerMask groundMask;
        [SerializeField] private ParticleSystem walkingPatricles;
        
        
        [Header("jumping")]
        [SerializeField]
        private float jumpVelocity;
        [SerializeField]
        private float maxJumpTime = 1;
        [SerializeField] private float lostControlAfterBounceTime=0.5f;
        [SerializeField] private float maxHoldingWtime = 0.03f;
        [SerializeField] private float climbingMultiplication = 1.5f;
        [SerializeField] private float minimalClimbingVelocity = 0.4f;
        [SerializeField] private float maximumYVelocity = 11f;
        
        [Header("Left/Right movement")]
        [SerializeField]
        private float accSpeed=3;
        [SerializeField]
        private float decSpeed = 5;
        [SerializeField]
        private float maxSpeed = 5;
        
  

        [Tooltip("If false, when object is moving in the right direction and left key is pressed, untill velocity = 0, dec is used\n If true acc is used")]
        [SerializeField] private bool fastRotation=false;
        
        
        [Tooltip("template direction is right")] [SerializeField]
        private Vector2 bounceTemplateVector= new Vector2(2,3);

        private Rigidbody2D rigi;
        private SpriteRenderer renderer;
        
        private Ticker jumpProgress ;

        private bool isOnTheGround;
        private Ticker blockMoveTimer;

        private float holdingW = 0f;


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
            blockMoveTimer= TickerCreator.CreateNormalTime(lostControlAfterBounceTime); 
            blockMoveTimer.ForceFinish();
            jumpProgress.ForceFinish();
            animer = this.GetComponent<SimpleAnimation>();
        }


        
        private void Move(float dir)
        {
            if (!blockMoveTimer.Done)
                return;
           PushIn(dir,true);
        }

        private void BounceOff(float dir)
        {
            //this.rigi.velocity += new Vector2(dir*bounceTemplateVector.x, bounceTemplateVector.y);
            //blockMoveTimer.Reset();
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

            if (Input.GetKeyDown(jump))
            {
                TryJump();
                
            }
            else
            {
                LetGoJump();
            }


        }

        private void PlayIfExists(string nm)
        {
            if (animer.GetState(nm)!=null)
            {
                animer.Play(nm);
            }
            else
            {
                Debug.LogWarning($"Mising an animation {nm}");
                animer.Stop();
            }
        }
        private void FixedUpdate()
        {
            if (Mathf.Abs(rigi.velocity.x) > 0.2f && Mathf.Abs(rigi.velocity.y) < 0.1f)
            {
                PlayIfExists("Running");
            }
            else if(Mathf.Abs(rigi.velocity.y)<0.1f)
            {
                PlayIfExists("Idle");
            } 
            else
            {
                PlayIfExists("Flying");
            }
            
        }


        public void LetGoJump()
        {
            jumpProgress.ForceFinish();
        }



        private void TryJump()
        {
            if (isOnTheGround)
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

        private void OnCollisionStay2D(Collision2D other)
        {
            if (Input.GetKeyUp(KeyCode.W) || holdingW >= maxHoldingWtime || this.rigi.velocity.y <= minimalClimbingVelocity)
                return;
            if (!isOnTheGround && Input.GetKey(KeyCode.W))
            {
                this.rigi.velocity = Vector2.up * this.rigi.velocity.magnitude * climbingMultiplication;
                if (this.rigi.velocity.y > maximumYVelocity)
                    this.rigi.velocity = new Vector2(this.rigi.velocity.x, maximumYVelocity);
                holdingW += Time.deltaTime;

                // BounceOff(Mathf.Sign(this.transform.position.x-other.transform.position.x));
            }
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
                if (!isOnTheGround)
                {
                    walkingPatricles.Play();
                }
                isOnTheGround = true;
                holdingW = 0f;
            }
        }


        public void OnBubbleUpTriggerExit(GameObject internalObj, Collider2D incoming)
        {
            
            if ( ( groundMask.value &   (1<<incoming.gameObject.layer)) !=0 && feetCollider.gameObject == internalObj)
            {
                
                if (isOnTheGround)
                {
                    walkingPatricles.Stop();
                }
                isOnTheGround = false;
            }
        }

     
    }
}