using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] ObstaclePrefabs;
    public GameObject[] pickupPrefabs;
    
    public float SpawnPosY;
    public float SpawnPosZ;
    public float SpawnRangeX = 4.5f;
    public float gravityModify = 0.01f;
    private float obstacleSpawnMinInterval = 2f;
    private float obstacleSpawnMaxInterval = 3f;
    private float obstacleSpawnInterval;
    private float pickupInterval = 7f;

    private Vector2 spawnY = new Vector2(-1, 2);

    public bool spawnObstacles;
    private bool isNoPickups;
    public bool canSpawnPickup;

    private Player playerScript;

    private GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnObstacles = true;
        canSpawnPickup = true;
        Physics.gravity *= gravityModify;

        playerScript = GameObject.Find("Player").GetComponent<Player>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        Generate();

        isNoPickups = GameObject.FindGameObjectsWithTag("Pickup").Length == 0;

        if (isNoPickups && !playerScript.isAccelerating && gameManager.gameIsProccessing && canSpawnPickup)
        {
            GeneratePickUp();
        }

    }

    public void SpawnRandomObstacle()
    {
        int index = Random.Range(0, ObstaclePrefabs.Length);

        GameObject obstacle = ObstaclePrefabs[index];

        Instantiate(obstacle, new Vector3(GenerateRandomSpawnPosX(), SpawnPosY, SpawnPosZ), obstacle.transform.rotation);

    }

    public float GenerateRandomSpawnPosX()
    {
        return Random.Range(-SpawnRangeX, SpawnRangeX);
    }

    public float GenerateSpawnY()
    {
        return Random.Range(spawnY.x, spawnY.y);
    }
    public void GeneratePickUp()
    {
        int index = Random.Range(0, pickupPrefabs.Length);

        Vector3 position = new Vector3(GenerateRandomSpawnPosX(), GenerateSpawnY(), SpawnPosZ);

        Instantiate(pickupPrefabs[index], position, gameManager.transform.rotation);
    }

    IEnumerator SpawnObstacleRoutine()
    {
        obstacleSpawnInterval = Random.Range(obstacleSpawnMinInterval, obstacleSpawnMaxInterval);
        spawnObstacles = false;
        yield return new WaitForSeconds(obstacleSpawnInterval);
        spawnObstacles = true;
    }

    public void Generate()
    { 
        if (spawnObstacles && !playerScript.isAccelerating && gameManager.gameIsProccessing)
        {
            SpawnRandomObstacle();
            StartCoroutine(SpawnObstacleRoutine());
        }
    }

    public void PickupInterval()
    {
        StartCoroutine(PickupIntervalRoutine());
    }

    IEnumerator PickupIntervalRoutine()
    {
        canSpawnPickup = false;

        yield return new WaitForSeconds(pickupInterval);

        canSpawnPickup = true;
    }



}
