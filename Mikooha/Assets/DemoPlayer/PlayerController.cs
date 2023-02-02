using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClearSky
{
    public class PlayerController : MonoBehaviour
    {
        public float movePower = 10f;
        public float KickBoardMovePower = 15f;
        public float jumpPower = 20f; //Set Gravity Scale in Rigidbody2D Component to 5
        public Transform contactRadar;
        public Vector2 contactSize = new Vector2(1f, 0.1f);
        public LayerMask surfaceLayer;
        public string playerName;

        public float rotationAngle = 25f;
        public float rotationSpeed = 0.1f;

        private Rigidbody2D _rigidbody;
        private Animator anim;
        private int direction = 1;
        private bool isJumping = false;
        private bool isSurface = false;
        private bool alive = true;
        private bool allowMovement = true;
        
        // Start is called before the first frame update
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.simulated = true;
            anim = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
           CheckSurface();
        }

        private void Update()
        {
            if (alive)
                if (allowMovement)
                {
                    //Attack();
                    Jump();
                    Run();
                }

            RotateBody();
            Restart();
        }

        public void DisableMovement()
        {
            allowMovement = false;
            anim.SetTrigger("idle");
            isJumping = false;
            anim.SetBool("isRun", false);
            anim.SetBool("isJump", false);
        }
        public void EnableMovement()
        {
            allowMovement = true;
            anim.SetTrigger("idle");
            isJumping = false;
            anim.SetBool("isRun", false);
            anim.SetBool("isJump", false);
        }

        void Run()
        {
            Vector3 moveVelocity = Vector3.zero;
            anim.SetBool("isRun", false);

            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                if (direction > 0)
                {
                    direction = -1;
                    var scale = transform.localScale.y;

                    transform.localScale = new Vector3(direction * scale, scale, scale);
                    _rigidbody.rotation *= direction;
                }
                if (!anim.GetBool("isJump"))
                    anim.SetBool("isRun", true);

                moveVelocity = Vector3.left;
            }

            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                if (direction < 0)
                {
                    direction = 1;
                    var scale = transform.localScale.y;

                    transform.localScale = new Vector3(direction * scale, scale, scale);
                    _rigidbody.rotation *= direction;
                }

                if (!anim.GetBool("isJump"))
                    anim.SetBool("isRun", true);

                moveVelocity = Vector3.right;
            }

            transform.position += moveVelocity * movePower * Time.deltaTime;
            
        }
        void Jump()
        {
            if (Input.GetButtonDown("Jump"))
                if (!anim.GetBool("isJump"))
                    if (isSurface)
                        isJumping = true;

            if (!isJumping)
                return;

            _rigidbody.velocity = Vector2.zero;

            Vector2 jumpVelocity = new Vector2(0, jumpPower);
            _rigidbody.AddForce(jumpVelocity, ForceMode2D.Impulse);

            _rigidbody.rotation = rotationAngle * direction;

            isJumping = false;
        }
        void Attack()
        {
            if (Input.GetButtonDown("Attack"))
            {
                anim.SetTrigger("attack");
            }
        }
        void Hurt()
        {
            anim.SetTrigger("hurt");
            if (direction == 1)
                _rigidbody.AddForce(new Vector2(-5f, 1f), ForceMode2D.Impulse);
            else
                _rigidbody.AddForce(new Vector2(5f, 1f), ForceMode2D.Impulse);
        }
        public void Die()
        {
            anim.SetBool("isRun", false);
            anim.SetBool("isKickBoard", false);
            anim.SetTrigger("die");
            alive = false;
        }
        void Restart()
        {
            if (!alive && Input.anyKeyDown)
            {
                var respawn = GameObject.FindGameObjectWithTag("Respawn");

                if(respawn != null)
                    gameObject.transform.SetPositionAndRotation(respawn.transform.position, Quaternion.identity);

                anim.SetBool("isKickBoard", false);
                anim.SetTrigger("idle");
                alive = true;
            }
        }

        public void CheckSurface()
        {
            isSurface = Physics2D.OverlapBox(contactRadar.position, contactSize, 0f, surfaceLayer);
            anim.SetBool("isJump", !isSurface);
        }

        public void RotateBody()
        {
            if (isSurface) 
                if(_rigidbody.rotation != 0)
                    if(_rigidbody.rotation != rotationAngle)
                        if(_rigidbody.rotation != -rotationAngle)
                            _rigidbody.rotation = 0;

            if (!isSurface)
                if (_rigidbody.rotation <= rotationAngle)
                    if (_rigidbody.rotation >= -rotationAngle)
                        _rigidbody.rotation += rotationSpeed * -direction;
        }
    }

}