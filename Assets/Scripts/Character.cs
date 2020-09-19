using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class Character : MonoBehaviour
{
    public Collider2D playerColl;    
    public GameObject bulletPref;
    public Transform gunPosition;
    public GameObject minePref;
    public Transform groundChk;
    public Collider2D[] colliders;  
    public ParticleSystem partSyst;
    public AudioClip jumpClip;
    public AudioClip shootClip;
    public AudioClip deathClip;
    public Camera mCam;
    public Panel panel;

    public int health;
    public float speed;
    public float jumpForce;
    public float offSetX;
    public float offSetY;
    public int winCount;
    public int curCount;

    Animator anim;
    Rigidbody2D RB;
    Vector3 direction;

    float horizontal;
    float vertical;
    bool right;
    bool isGrounded;
    float camSpeed;
    float guiX;
    float guiY;
    Scene mainScene;

    void Start()
    {
        health = 100;
        RB = GetComponent<Rigidbody2D>();
        speed = 3;
        jumpForce = 10;
        camSpeed = 5;
        anim = GetComponent<Animator>();
        mainScene = SceneManager.GetActiveScene();
        winCount = 5;
        curCount = 0;
    }

    private void Move()
    {
        if (horizontal > 0 && right || horizontal < 0 && !right)
            Flip();

        direction = Vector3.right * horizontal;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);
    }
    
    private void Jump()
    {
        if (isGrounded)
        {
            RB.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            AudioSource.PlayClipAtPoint(jumpClip, transform.position);
        }
    }

    private void Shoot()
    {
        GameObject temp = Instantiate(bulletPref, gunPosition.position, Quaternion.identity);
        temp.name = gameObject.tag;
        temp.GetComponent<Bullet>().direction = (!right) ? 1 : -1;
        partSyst.Play();
        AudioSource.PlayClipAtPoint(shootClip, gunPosition.position);
    }

    private void LandMine()
    {
        Instantiate(minePref, transform.position, Quaternion.identity);
    }

    private void Flip()
    {
        right = !right;
        Vector2 sc = transform.localScale;
        sc.x *= -1;
        transform.localScale = sc;
    }

    private void CheckGround()
    {
        colliders = Physics2D.OverlapCircleAll(groundChk.position, 0.3f);

        isGrounded = colliders.Length > 1;
    }

    private void Death()
    {
        AudioSource.PlayClipAtPoint(deathClip, gunPosition.position);
        Destroy(gameObject);
        SceneManager.LoadScene(1);
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void UpdateCamera()
    {
        float X = transform.position.x;
        float Y = transform.position.y;

        mCam.transform.position = Vector3.Lerp(mCam.transform.position, new Vector3(X + offSetX, Y + offSetY, mCam.transform.position.z), camSpeed * Time.deltaTime);
    }

    private void UpdateGUI()
    {
        Vector3 cameraPosition = Camera.main.WorldToScreenPoint(transform.position);

        guiX = cameraPosition.x;
        guiY = cameraPosition.y;
    }

    private void UpdateAxises()
    {
        horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        vertical = CrossPlatformInputManager.GetAxis("Vertical");
    }

    private void CheckInput()
    {
        if (CrossPlatformInputManager.GetAxis("Horizontal") != 0)
            Move();    

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
            Jump();

        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
            Shoot();

        if (CrossPlatformInputManager.GetButtonDown("Fire2"))
            LandMine();
    }

    private void CheckPlayer()
    {
        panel.Refresh();

        if (health <= 0
            || transform.position.y < -6)
            Death();
    }

    void Update()
    {
        UpdateCamera();
        UpdateGUI();
        UpdateAxises();

        CheckInput();
        CheckPlayer();

        if(isGrounded)
        {
            if(RB.velocity.y == 0)
            {
                anim.SetBool("JumpUp", false);
                anim.SetBool("JumpDown", false);
            }

            if (Mathf.Abs(horizontal) > 0)
                anim.SetBool("Run", true);
            else
                anim.SetBool("Run", false);
        }
        else
        {
            if (RB.velocity.y > 0.3)
                anim.SetBool("JumpUp", true);
            if (RB.velocity.y < 0.3)
                anim.SetBool("JumpDown", true);
        }
    }
}
