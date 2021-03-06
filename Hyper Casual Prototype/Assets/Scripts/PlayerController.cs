using UnityEngine;
using UnityEngine.Advertisements;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed;

    public float forwardMoveSpeed;

    [HideInInspector]
    public bool moveRight;

    [HideInInspector]
    public bool moveLeft;

    [HideInInspector]
    public bool Once;

    public GameObject CollisionFx;

    public GameObject CoinFx;

    public Camera cam;

    private float moveAmount;

    public GameObject rightScorer;

    public GameObject leftScorer;

    private void Awake()
    {
        DetectCameraEdges();
    }

    // Start is called before the first frame update
    void Start()
    {
        Once = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(GameManager.Instance.gameState != GameManager.GameState.GameOver)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (GameManager.Instance.gameState == GameManager.GameState.GamePlay)
        {
            transform.position += new Vector3(0, forwardMoveSpeed * Time.deltaTime, 0);
            Move();
        }
    }

    public void MoveLeftRight()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.GamePlay)
        {
            if (Once && Random.value <= 0.5f)
            {
                moveLeft = true;
                moveRight = false;
                Once = false;
            }
            else if (Once)
            {
                moveRight = true;
                moveLeft = false;
                Once = false;
            }

            if (!Once)
            {
                moveLeft = !moveLeft;
                moveRight = !moveRight;
            }

        }
    }

    private void Move()
    {
        

        if (moveRight)
        {
            transform.position = Vector2.Lerp(transform.position, new Vector2(-(moveAmount + transform.localScale.x / 2), transform.position.y), moveSpeed * Time.deltaTime);
        }

        if (moveLeft)
        {
            transform.position = Vector2.Lerp(transform.position, new Vector2(moveAmount + transform.localScale.x / 2, transform.position.y), moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Score")
        {
            AudioManager.Instance.Play("Score");
            GameManager.Instance.Score.transform.localScale = new Vector3(2, 2, 2);
            GameManager.Instance.score++;
            GameObject temp = Instantiate(CollisionFx, gameObject.transform.position,Quaternion.Euler(0,-90,0));
            Destroy(temp, 1f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            SaveManager.Instance.state.kill++;

            if (Advertisement.IsReady("rewardedVideo") && SaveManager.Instance.state.kill <= 7) //ad is loaded
            {
                GameManager.Instance.ChangeGameState(GameManager.GameState.WatchAd);

            }
            else
            {
                if (SaveManager.Instance.state.kill >= 7)
                {
                    SaveManager.Instance.state.kill = 0;
                    InitializeAds.Instance.ShowAd();
                }
                GameManager.Instance.ChangeGameState(GameManager.GameState.GameOver);
            }
            SaveManager.Instance.Save();

        }

        if (collision.gameObject.tag == "Coin")
        {
            AudioManager.Instance.Play("Coin");
            Destroy(collision.gameObject);
            GameObject temp = Instantiate(CoinFx, gameObject.transform.position, Quaternion.Euler(0, -90, 0));
            Destroy(temp, 1f);
            SaveManager.Instance.state.Coin++;
            SaveManager.Instance.Save();
        }
    }

    private void DetectCameraEdges()
    {
        if (!cam.orthographic) 
        { 
            Debug.LogError("Camera.main is not Orthographic, failed to find camera edge"); 
            return;
        }

        var bottomLeft = (Vector2)cam.ScreenToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));

        moveAmount = bottomLeft.x;

        rightScorer.transform.position = new Vector2(-(moveAmount + 0.5f), 0);
        leftScorer.transform.position = new Vector2(moveAmount + 0.5f, 0);

        Debug.Log(moveAmount);

    }


}
