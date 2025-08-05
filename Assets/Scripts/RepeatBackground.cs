using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    public GameObject background;

    public float startPosY;
    public float backgroundSizeY;
    public float speed = 3f;
    public float acceleratedSpeed = 15f;
    public float defaultSpeed = 3f;

    private GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        backgroundSizeY = background.GetComponent<BoxCollider>().size.y;
        startPosY = transform.position.y;

    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameIsProccessing)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
            if (Mathf.Abs(transform.position.y - startPosY) >= backgroundSizeY)
            {
                transform.position = new Vector3(transform.position.x, startPosY, transform.position.z);
            }
        }
    }
}
