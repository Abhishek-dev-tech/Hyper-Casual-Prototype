using System.Collections.Generic;
using UnityEngine;

public class RandomMap : MonoBehaviour
{
    public static RandomMap instance;

    public GameObject spawnPoint;
    public GameObject square;
    public GameObject coin;
    public GameObject coinSpawnPoint;

    public float maxTime = 1;
    public float timer = 0;
    public float maxTimeCoin = 5;
    public float timerCoin = 0;

    public bool Go;

    public List<GameObject> Obstacles = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        Go = true;
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.gameState == GameManager.GameState.GamePlay)
        {
            if (Go)
            {
                RandomBlocksPlace();
                RandomCoin();
            }
            
        }

    }

    void RandomBlocksPlace()
    {
        if(timer >= maxTime)
        {
            int rand = Random.Range(0, 6);

            if(rand >=0 && rand <= 3)
            {
                GameObject temp1 = Instantiate(square);
                Obstacles.Add(temp1);
                temp1.name = "Square";
                temp1.transform.position =  new Vector3(Random.Range(-5.17f, 5.17f), spawnPoint.transform.position.y, 0);
            }
            else
            {
                GameObject temp2 = Instantiate(square);
                Obstacles.Add(temp2);
                temp2.name = "Square";
                int i = Random.Range(0, 2);
                temp2.transform.position = i == 0 ?  new Vector3(-7.3f, spawnPoint.transform.position.y, 0) :  new Vector3(7.3f, spawnPoint.transform.position.y, 0);
                temp2.transform.localScale = new Vector3(Random.Range(8, 18), 1.2f, 1.2f);
            }
            timer = 0;
        }
        timer += Time.deltaTime;
        
    }
    //4.8
    void RandomCoin()
    {
        if (timerCoin >= maxTimeCoin)
        {
            GameObject temp = Instantiate(coin);
            temp.transform.position = coinSpawnPoint.transform.position + new Vector3(Random.Range(-6.483f, 6.483f), 0, 0);
            timerCoin = 0;
            Obstacles.Add(temp);
        }
        timerCoin += Time.deltaTime;
    }
}
