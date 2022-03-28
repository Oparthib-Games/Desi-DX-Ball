using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject ButtonPanel;
    public GameObject TapToPlayPanel;
    public GameObject PauseButton;
    public GameObject PausePanel;
    public GameObject GameOverPanel;
    public GameObject Timer;
    public GameObject starPanel;
    public GameObject lifePanel;
    GameObject Ball;
    GameObject Paddle;
    GameObject Explosives;

    public AudioClip GameOverSound;
    public AudioClip WinSound;
    public AudioClip ButtonTapSound;

    public GameObject[] Life;

    public GameObject WinParticle;

    Text timerText;

    public int LifeCount;
    public int gameTime;
    int number_0f_bricks;
    public int bricksDestroyedCount;

    public int oneStarTime;
    public int twoStarTime;
    public int threeStarTime;
    int starAchived = 0;

    public string levelName;

    bool alreadyWon;

    void Start()
    {
        levelName = SceneManager.GetActiveScene().name;

        Ball = FindObjectOfType<BallScript>().transform.gameObject;
        Paddle = FindObjectOfType<PaddleScript>().transform.gameObject;
        Explosives = GameObject.Find("Explosives");

        if (Timer) timerText = Timer.GetComponent<Text>();
        LifeCount = Life.Length;
        number_0f_bricks = GameObject.FindGameObjectsWithTag("bricks").Length;

        
    }

    void Update()
    {
        if (timerText)  timerText.text = ("0" + gameTime / 60 + ":" + gameTime % 60).ToString();

        if(bricksDestroyedCount >= number_0f_bricks && !alreadyWon)
        {
            PerformWinning();
        }

        if(gameTime <= 0 && bricksDestroyedCount < number_0f_bricks || LifeCount <= 0)
        {
            performGameOver();
        }

        if(gameTime <= 15)
        {
            Timer.GetComponent<Text>().color = Color.red;
        }

        if (Input.GetKeyDown(KeyCode.Delete))               PlayerPrefs.DeleteAll();
    }

    void performGameOver()
    {
        //Particle
        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject thePlayer in Players)    Destroy(thePlayer);

        GameOverPanel.active = true;
    }

    void PerformWinning()
    {
        Instantiate(WinParticle, new Vector3(0, 6f, 0), Quaternion.Euler(new Vector3(-180, 0, 0)));
        AudioSource.PlayClipAtPoint(WinSound, Camera.main.transform.position, 0.3f);
        
        Destroy(Ball);
        Destroy(Paddle);
        Destroy(Explosives);


        StopCoroutine("CountdownStart");

        starPanel.active = true;
        PauseButton.active = false;
        lifePanel.active = false;
        
        if(gameTime >= threeStarTime && LifeCount == 3)
        {
            starAchived = 3;
            starPanel.GetComponent<starSystem>().StartCoroutine("Active3Star");
        }
        else if(gameTime >= twoStarTime)
        {
            starAchived = 2;
            starPanel.GetComponent<starSystem>().StartCoroutine("Active2Star");
        }
        else if(gameTime >= oneStarTime)
        {
            starAchived = 1;
            starPanel.GetComponent<starSystem>().StartCoroutine("Active1Star");
        }

        if(PlayerPrefs.GetInt(levelName, 0) < starAchived)
        {
            PlayerPrefs.SetInt(levelName, starAchived);
        }

        alreadyWon = true;
    }


    public void DestroyLife()
    {
        LifeCount--;
        StopCoroutine("CountdownStart");
        Life[LifeCount].active = false;
    }
    public void LifeIncrease()
    {
        if(LifeCount < 3)
        {
            LifeCount++;
            Life[LifeCount - 1].active = true;
        }
    }


    public IEnumerator CountdownStart()
    {
        gameTime--;

        yield return new WaitForSeconds(1);

        StartCoroutine("CountdownStart");
    }

    public void Pause()
    {
        StartCoroutine("playButtonTap");
        Time.timeScale = 0;
    }
    public void Resume()
    {
        Time.timeScale = 1;
        StartCoroutine("playButtonTap");
    }
    public void Retry()
    {
        BrickScript.hitPoint = 1;

        StartCoroutine("playButtonTap");
        Resume();
        Application.LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadLevel(string sceneName)
    {
        BrickScript.hitPoint = 1;
        
        Resume();
        StartCoroutine("playButtonTap");
        Application.LoadLevel(sceneName);
    }
    public void Quit()
    {
        Application.Quit();
    }
    IEnumerator playButtonTap()
    {
        AudioSource.PlayClipAtPoint(ButtonTapSound, Camera.main.transform.position);
        yield return new WaitForSeconds(ButtonTapSound.length);
    }



    public void NextLevel()
    {
        BrickScript.hitPoint = 1;

        StartCoroutine("NextLevelButtonAudio");
        Resume();
    }
    IEnumerator NextLevelButtonAudio()
    {
        AudioSource.PlayClipAtPoint(ButtonTapSound, Camera.main.transform.position);
        yield return new WaitForSeconds(ButtonTapSound.length);
        Application.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
