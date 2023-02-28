using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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

        public AudioClip runClip;
        public AudioClip jumpClip;
        public AudioClip dieClip;

        public float runPitch = 1f;
        public float jumpPitch = 1f;
        public float diePitch = 1f;

        public GameObject shadow;

        private AudioSource audioSource;
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
            audioSource = GetComponent<AudioSource>();
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

            Audio();
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

        public void Audio()
        {
            if (!anim.GetBool("isRun"))
                if (audioSource.clip == runClip)
                    audioSource.clip = null;
        }

        public void PlayJump()
        {
            audioSource.pitch = jumpPitch;
            audioSource.PlayOneShot(jumpClip);
        }
        public void PlayRun()
        {
            if (!audioSource.isPlaying)
            {
                audioSource.pitch = runPitch;
                audioSource.clip = runClip;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        public void PlayDie()
        {
            audioSource.pitch = diePitch;
            audioSource.PlayOneShot(dieClip);
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
                {
                    anim.SetBool("isRun", true);
                    PlayRun();
                }

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
                {
                    anim.SetBool("isRun", true);
                    PlayRun();
                }

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

            PlayJump();

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
            PlayDie();
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

            if (anim.GetBool("isJump") && isSurface)
                PlayJump();

            shadow.SetActive(isSurface);
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