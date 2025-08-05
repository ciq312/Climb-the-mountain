using UnityEngine;
using UnityEngine.Rendering;

public class ObstacleLogic : MonoBehaviour
{
    public float borderPosY;
    public int damage;
    public float force;
    public float torque;

    private Rigidbody objectRb;

    private GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objectRb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame 
    void Update()
    {
        if (transform.position.y <= borderPosY)
        {
            Destroy(gameObject);
        }

        objectRb.AddForce(Vector3.down * force * Time.deltaTime);

        objectRb.AddTorque(Vector3.right * torque * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !other.gameObject.GetComponent<Player>().isInvincible && gameManager.gameIsProccessing)
        {
            other.gameObject.GetComponent<Player>().Hit(damage);
        }
    }
}
