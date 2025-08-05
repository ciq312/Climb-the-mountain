using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class PickUpLogic : MonoBehaviour
{
    public int pickUpType;

    public float force;

    public float BorderPosY;

    public float torque;

    private GameManager gameManager;

    private Rigidbody pickupRb;

    private SpawnManager spawnManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        pickupRb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        pickupRb.AddTorque(Vector3.up * torque, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= BorderPosY)
        {
            Destroy(gameObject);
        }
        if (gameManager.gameIsProccessing)
        {
            pickupRb.AddForce(Vector3.down * Time.fixedDeltaTime * force);
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player playerScript = other.gameObject.GetComponent<Player>();

            playerScript.audioSource.PlayOneShot(playerScript.collectingSound, 5f);

            spawnManager.PickupInterval();

            if (pickUpType == 1)
            {
                pickUpType = Random.Range(1, 5);
                if (pickUpType == 1)
                {
                    playerScript.Invisible();
                }
            }
            if (pickUpType == 2)
            {
                playerScript.Invincible();
            }

            if (pickUpType == 3)
            {
                playerScript.Heal();
            }
            if (pickUpType == 4)
            {
                playerScript.Acceleration();
            }
            Destroy(gameObject);
        }
    }
}
