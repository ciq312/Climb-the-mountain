using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 moveInput;

    public float moveSpeed = 5f;
    public float rotateAngle;
    public float borderRange = 4f;
    public float bottomBorder;
    public float topBorder;
    private float InvicibilityDuration = 5f;
    private float invisibleDuration = 5f;
    private float accelerationDuration = 5f;
    public float force = 1000f;
    private float fastCrawlSpeed = 3f;
    private float normalCrawlSpeed = 1f;

    public int HP;
    public int HPToHeal;

    public bool isInvincible;
    public bool isInvisible;
    public bool isNoObstacles;
    public bool isAccelerating;
    public bool gotBuff;

    public AudioClip collectingSound;
    public AudioClip boomSound;
    public AudioClip hurtSound;

    private Rigidbody playerRb;

    public GameObject indicator;
    public GameObject model;

    public AudioSource audioSource;

    private Animator animator;

    private GameManager gameManager;

    private RepeatBackground repeatBackground;

    private SpawnManager spawnManager;

    public TextMeshProUGUI HPText;

    public ParticleSystem smokeParticle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gotBuff = false;
        audioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        animator = model.GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody>();
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        repeatBackground = GameObject.Find("Background").GetComponent<RepeatBackground>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        isInvincible = false;
        isAccelerating = false;
        HP = 100;
        animator.SetFloat("AnimSpeedMultiplier", normalCrawlSpeed);
        UpdateHP();
        

    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameIsProccessing)
        {
            Movement();
            Borders();
            Rotation();
        }


        if (isAccelerating)
        {
            animator.SetFloat("AnimSpeedMultiplier", fastCrawlSpeed);
        }

    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    
    private void Borders()
    {
        if (transform.position.x <= -borderRange)
        {
            transform.position = new Vector3(-borderRange, transform.position.y, transform.position.z);
        }
        if (transform.position.x >= borderRange)
        {
            transform.position = new Vector3(borderRange, transform.position.y, transform.position.z);
        }
        if (transform.position.y >= topBorder)
        {
            transform.position = new Vector3(transform.position.x, topBorder, transform.position.z);
        }
        if (transform.position.y <= bottomBorder)
        {
            transform.position = new Vector3(transform.position.x, bottomBorder, transform.position.z);
        }
    }
    private void Rotation()
    {
        if (moveInput.x == -1)
        {
            Quaternion rotateLeft = Quaternion.Euler(0, 0, rotateAngle);
            playerRb.MoveRotation(rotateLeft);
        }
        if (moveInput.x == 0)
        {
            Quaternion dontRotate = Quaternion.Euler(0, 0, 0);
            playerRb.MoveRotation(dontRotate);
        }
        if (moveInput.x == 1)
        {
            Quaternion rotateRight = Quaternion.Euler(0, 0, -rotateAngle);
            playerRb.MoveRotation(rotateRight);
        }
    }
    private void Movement()
    {
        Vector3 movement = new Vector3(moveInput.x, moveInput.y, 0) * moveSpeed * Time.fixedDeltaTime;
        playerRb.MovePosition(playerRb.position + movement);
    }

    public void Hit(int damage)
    {
        HP -= damage;

        UpdateHP();
        if (HP <= 0)
        {
            HP = 0;
            audioSource.PlayOneShot(boomSound);
            UpdateHP();
            smokeParticle.Play();
            animator.SetBool("Death_b", true);
            transform.eulerAngles = new Vector3(90, transform.eulerAngles.y, transform.eulerAngles.z);
            StartCoroutine(TurnOnGravityRoutine());
            gameManager.EndGame();
        }
        else
        {
            audioSource.PlayOneShot(hurtSound);
        }
    }

    public void Invincible()
    {
        StartCoroutine(InvincibleRoutine());
    }

    IEnumerator InvincibleRoutine()
    {
        isInvincible = true;

        indicator.SetActive(true);

        yield return new WaitForSeconds(InvicibilityDuration);

        indicator.SetActive(false);

        isInvincible = false;
    }

    public void Invisible()
    {
        StartCoroutine(InvisibleRoutine());
    }

    IEnumerator InvisibleRoutine()
    {
        isInvisible = true;
        SkinnedMeshRenderer skinnedMeshRenderer = GameObject.Find("CH_BusinessMan").GetComponent<SkinnedMeshRenderer>();
        skinnedMeshRenderer.enabled = false;
        yield return new WaitForSeconds(invisibleDuration);
        skinnedMeshRenderer.enabled = true;
        isInvisible = false;
    }

    public void Heal()
    {
        HP += HPToHeal;

        UpdateHP();
        if (HP >= 100)
        {
            HP = 100;
            UpdateHP();
        }
    }

    public void Acceleration()
    {
        isAccelerating = true;

        StartCoroutine(AccelerationRoutine());
    }

    IEnumerator AccelerationRoutine()
    {

        repeatBackground.speed = repeatBackground.acceleratedSpeed;

        yield return new WaitForSeconds(accelerationDuration);

        repeatBackground.speed = repeatBackground.defaultSpeed;

        animator.SetFloat("AnimSpeedMultiplier", normalCrawlSpeed);

        isAccelerating = false;

    }

    public void UpdateHP()
    {
        HPText.text = "HP: " + HP;    
    }

    IEnumerator TurnOnGravityRoutine()
    {
        yield return new WaitForSeconds(1f);
        Physics.gravity *= 0.2f;
        playerRb.useGravity = true;
    }
}
