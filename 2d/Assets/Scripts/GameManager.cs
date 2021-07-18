using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        MainMenu,
        Setting,
        GamePlay,
        Shop,
        ChangeTheme,
        Pause,
        WatchAd,
        WatchingAd,
        GameOver
    }

    public static GameManager Instance;

    public GameState gameState = GameState.MainMenu;

    public GameObject player;
    public GameObject menuPanel;
    public GameObject gamePanel;
    public GameObject gameOver;
    public GameObject pausePanel;
    public GameObject watchAdPanel;
    public GameObject noThanks;
    public GameObject pausePlayButton;
    public GameObject SettingPanel;
    public GameObject shopPanel;
    public GameObject newImage;
    public GameObject dotNewImage;
    public GameObject SecondPanel;
    public GameObject Score;
    public GameObject RewardCoinButton;
    public GameObject showCoinText;

    public List<GameObject> currentShopItems = new List<GameObject>();

    public List<Sprite> AllPlayers = new List<Sprite>();

    [HideInInspector]
    public int score;

    public Text gameTextScore;
    public Text mainMenuBestScore;
    public Text gameOverTextScore;
    public Text gameOverBestScore;
    public Text WatchAdSecond;
    public Text RewardSecond;
    public Text coinsText;

    private float currentTime = 0;
    private float RewardcurrentTime = 0;
    private float currentCircleTime = 0;
    private float startingTime = 5f;
    private float RewardstartingTime = 3f;

    public Image circle;

    public Sprite pause;
    public Sprite lockSprite;
    public Sprite PlaySprite;

    [HideInInspector]
    public bool Right;

    public Slider Sound;
    public Slider Music;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 300;
        if (!SaveManager.Instance.state.SFX)
        {
            Sound.value = 1;
        }
        else
        {
            Sound.value = 0;
        }          
        if (!SaveManager.Instance.state.Music)
        {
            Music.value = 1;
        }
        else
        {
            Music.value = 0;
        }       
        

        player.GetComponent<SpriteRenderer>().sprite = AllPlayers[SaveManager.Instance.state.LastIndex];
        currentTime = startingTime;
        RewardcurrentTime = RewardstartingTime;
        currentCircleTime = 0;
        Time.timeScale = 1;
        mainMenuBestScore.text = "BEST : " + SaveManager.Instance.state.BestScore.ToString();
        coinsText.text =  SaveManager.Instance.state.Coin.ToString();
        newImage.SetActive(false);
        for (int i = 0; i < SaveManager.Instance.state.seen.Length; i++)
        {
            if (!SaveManager.Instance.state.seen[i])
            {
                
                if(i != 0)
                {
                    dotNewImage.SetActive(false);
                    currentShopItems[i].transform.GetChild(3).gameObject.SetActive(false);
                }
                    
                //shopNewImage[i].SetActive(false);

            }
            else
            {
                
                if (i != 0)
                {
                    dotNewImage.SetActive(true);
                    currentShopItems[i].transform.GetChild(3).gameObject.SetActive(true);
                }
                    
                //shopNewImage[i].SetActive(true);
                break;
            }
                
        }
        for (int i = 0; i < currentShopItems.Count; i++)
        {
            if (SaveManager.Instance.state.Items[i])
            {
                currentShopItems[i].transform.GetChild(1).GetComponent<Text>().text = "";
            }
        }
        
    }

    public void ChangeGameState(GameState state)
    {
        gameState = state;
    }

    void Update()
    {
        coinsText.text = SaveManager.Instance.state.Coin.ToString();
        gameTextScore.text =  score.ToString();
        print("Music : " + SaveManager.Instance.state.Music + " Sfx : " + SaveManager.Instance.state.SFX);
        if(gameState == GameState.MainMenu && Advertisement.IsReady("rewardedVideo2"))
        {
            RewardCoinButton.SetActive(true); 
        }
        if(gameState == GameState.Shop)
        {
            for (int i = 0; i < currentShopItems.Count; i++)
            {
                if (SaveManager.Instance.state.Items[i])
                {
                    currentShopItems[i].transform.GetChild(1).GetComponent<Text>().text = "";
                }
            }
        }

        Unlock();
        if ((gameState == GameState.GamePlay || gameState == GameState.Pause) && Input.GetButtonDown("Cancel"))
        {
            Pause();
            AudioManager.Instance.Play("Button");
        }        
        if (gameState == GameState.Setting && Input.GetButtonDown("Cancel"))
        {
            Back();
            AudioManager.Instance.Play("Button");
        }        
        if (gameState == GameState.Shop && Input.GetButtonDown("Cancel"))
        {
            ShopBack();
            AudioManager.Instance.Play("Button");
        }

        if (gameState == GameState.Pause)
        { 
            gamePanel.SetActive(false);
            pausePanel.SetActive(true);
        }

        if (currentTime <= 0 && gameState != GameState.WatchingAd)
        {
            ChangeGameState(GameState.GameOver);
            watchAdPanel.SetActive(false);
            currentTime = 0;
        }        

        if(gameState == GameState.GamePlay && !RandomMap.instance.Go)
        {
            RewardcurrentTime -= 1 * Time.deltaTime;
            if(RewardcurrentTime < 0.5f)
            {
                RewardSecond.text = "GO!";
            }
            else
            {
                RewardSecond.text = RewardcurrentTime.ToString("0");
            }
        }

        if (RewardcurrentTime < 0)
        {
            RewardcurrentTime = 0;
            RandomMap.instance.Go = true;
            SecondPanel.SetActive(false);
            gamePanel.SetActive(true);
        }

        if (currentTime <= 2)
        {
            noThanks.SetActive(true);
        }
        if (gameState == GameState.WatchAd)
        {
            watchAdPanel.SetActive(true);
            gamePanel.SetActive(false);
            currentTime -= 1 * Time.deltaTime;
            currentCircleTime += 1f * Time.deltaTime / 5;
            circle.fillAmount = currentCircleTime;
            WatchAdSecond.text = currentTime.ToString("0");
        }
        if (gameState == GameState.GameOver)
        {
            gameOverTextScore.text = "Score : " + gameTextScore.text;
            gameOverBestScore.text = "BEST : " + SaveManager.Instance.state.BestScore.ToString();
            gamePanel.SetActive(false);
            gameOver.SetActive(true);
        }

        if (int.Parse(gameTextScore.text) >= SaveManager.Instance.state.BestScore)
        {
            newImage.SetActive(true);
            SaveManager.Instance.state.BestScore = int.Parse(gameTextScore.text);
            SaveManager.Instance.Save();
        }
    }
    private void FixedUpdate()
    {
        Score.transform.localScale = Vector3.Lerp(Score.transform.localScale, new Vector3(1, 1, 1), 0.15f);
    }
    public void Retry()
    {
        SceneManager.LoadScene(0);
    }

    public void GameStart()
    {
        menuPanel.SetActive(false);
        gamePanel.SetActive(true);
        ChangeGameState(GameState.GamePlay);
    }

    public void Pause()
    {
        if(gameState == GameState.Pause)
        {
            pausePlayButton.GetComponent<Image>().sprite = pause;
            pausePanel.SetActive(false);
            gamePanel.SetActive(true);
            ChangeGameState(GameState.GamePlay);
        }
        else
        {
            gamePanel.SetActive(false);
            pausePanel.SetActive(true);
            ChangeGameState(GameState.Pause);

        }
    }

    public void NoThanks()
    {
        ChangeGameState(GameState.GameOver);
        watchAdPanel.SetActive(false);
        gameOver.SetActive(true);
    }   
    


    public void Revive()
    {
        RandomMap.instance.Go = false;
        SecondPanel.SetActive(true);
        foreach (GameObject item in RandomMap.instance.Obstacles)
        {
            Destroy(item);
        }
        player.transform.position = new Vector3(0, player.transform.position.y, player.transform.position.z);
        watchAdPanel.SetActive(false);
        //gamePanel.SetActive(true);
        ChangeGameState(GameState.GamePlay);
        currentTime = startingTime;
        RewardcurrentTime = RewardstartingTime;
        currentCircleTime = 0;
        noThanks.SetActive(false);
        FindObjectOfType<PlayerController>().moveLeft = false;
        FindObjectOfType<PlayerController>().moveRight = false;
        FindObjectOfType<PlayerController>().Once = true;
        newImage.SetActive(false);    
    }

    public void Setting()
    {
        ChangeGameState(GameState.Setting);
        menuPanel.SetActive(false);
        SettingPanel.SetActive(true);
    }    
    
    public void Back()
    {
        ChangeGameState(GameState.MainMenu);
        menuPanel.SetActive(true);
        SettingPanel.SetActive(false);
    }

    public void Shop()
    {
        ChangeGameState(GameState.Shop);
        menuPanel.SetActive(false);
        shopPanel.SetActive(true);
    }

    public void ShopBack()
    {
        ChangeGameState(GameState.MainMenu);
        menuPanel.SetActive(true);
        shopPanel.SetActive(false);
    }

    public void Unlock()
    {


        if (int.Parse(gameTextScore.text) >= 25 && !SaveManager.Instance.state.Items[1])
        {
            SaveManager.Instance.state.Items[1] = true;
            SaveManager.Instance.state.seen[1] = true;
            SaveManager.Instance.Save();
            currentShopItems[1].transform.GetChild(3).gameObject.SetActive(true);
            dotNewImage.SetActive(true);
        }

        if (int.Parse(gameTextScore.text) >= 50 && !SaveManager.Instance.state.Items[2])
        {
            SaveManager.Instance.state.Items[2] = true;
            SaveManager.Instance.state.seen[2] = true;
            SaveManager.Instance.Save();
            currentShopItems[2].transform.GetChild(3).gameObject.SetActive(true);
            dotNewImage.SetActive(true);
        }

        if (SaveManager.Instance.state.Coin >= 25 && !SaveManager.Instance.state.Items[3])
        {
            SaveManager.Instance.state.Items[3] = true;
            SaveManager.Instance.state.seen[3] = true;
            SaveManager.Instance.Save();
            currentShopItems[3].transform.GetChild(3).gameObject.SetActive(true);
            dotNewImage.SetActive(true);
        }

        if (int.Parse(gameTextScore.text) >= 75 && !SaveManager.Instance.state.Items[4])
        {
            SaveManager.Instance.state.Items[4] = true;
            SaveManager.Instance.state.seen[4] = true;
            SaveManager.Instance.Save();
            currentShopItems[4].transform.GetChild(3).gameObject.SetActive(true);
            dotNewImage.SetActive(true);
        }

        if (SaveManager.Instance.state.Coin >= 40 && !SaveManager.Instance.state.Items[5])
        {
            SaveManager.Instance.state.Items[5] = true;
            SaveManager.Instance.state.seen[5] = true;
            SaveManager.Instance.Save();
            currentShopItems[5].transform.GetChild(3).gameObject.SetActive(true);
            dotNewImage.SetActive(true);
        }

        if (int.Parse(gameTextScore.text) >= 100 && !SaveManager.Instance.state.Items[6])
        {
            SaveManager.Instance.state.Items[6] = true;
            SaveManager.Instance.state.seen[6] = true;
            SaveManager.Instance.Save();
            currentShopItems[6].transform.GetChild(3).gameObject.SetActive(true);
            dotNewImage.SetActive(true);
        }

        for (int i = 1; i < currentShopItems.Count; i++)
        {
            if (SaveManager.Instance.state.Items[i])
            {

                currentShopItems[i].transform.GetChild(2).GetComponent<Image>().sprite = PlaySprite;
                currentShopItems[i].transform.GetChild(2).GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
            else
            {
                currentShopItems[i].transform.GetChild(2).GetComponent<Image>().sprite = null;
                currentShopItems[i].transform.GetChild(2).GetComponent<Image>().color = new Color(1, 1, 1, 0);
     
            }
        }
    }

    public void SelectPlayer(int index)
    {
        if (SaveManager.Instance.state.Items[index] && Right)
        {
            player.GetComponent<SpriteRenderer>().sprite = AllPlayers[index];
            SaveManager.Instance.state.LastIndex = index;
            SaveManager.Instance.Save();
            ChangeGameState(GameState.MainMenu);
            menuPanel.SetActive(true);
            shopPanel.SetActive(false);
            for (int i = 0; i < currentShopItems.Count; i++)
            {
                if (i != 0)
                {
                    currentShopItems[i].transform.GetChild(3).gameObject.SetActive(false);
                    SaveManager.Instance.state.seen[i] = false;
                }

                //shopNewImage[i].SetActive(false);
            }

            SaveManager.Instance.Save();
            dotNewImage.SetActive(false);
        }
    }

    public void Check(GameObject i)
    {
        print("going");
        if (swipe.instance.currentItem.name == i.name)
        {
            Right = true;
            print("true");
        }

        else
        {
            Right = false;
            print("false");

        }
    }

    public void ButtonSound()
    {
        AudioManager.Instance.Play("Button");
    }

    public void MuteSound()
    {
        AudioManager.Instance.MuteSounds();
        AudioManager.Instance.Play("Button");


    }

    public void MuteMusic()
    {
        AudioManager.Instance.MuteMusic();
        AudioManager.Instance.Play("Button");

    }

    public void CoinsReward()
    {
        int rand = Random.Range(5, 11);
        SaveManager.Instance.state.Coin += rand;
        SaveManager.Instance.Save();
        showCoinText.transform.GetChild(0).gameObject.GetComponent<Text>().text = rand + " COINS ADDED";
        showCoinText.SetActive(true);
        Invoke("TurnoffRewardText", 2f);
        RewardCoinButton.SetActive(false);
    }

    void TurnoffRewardText()
    {
        showCoinText.SetActive(false);

    }


}
