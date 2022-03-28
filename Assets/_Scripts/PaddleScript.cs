using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PaddleScript : MonoBehaviour
{
    public float speed;

    GameManager gameManager;
    BallScript ballScript;

    public GameObject behindBarier;
    public Sprite ShooterPaddleSprite;

    public GameObject bullet;
    public float bulletForce;

    GameObject Ball;

    public GameObject[] rewardTexts;

    public GameObject sideKick;

    public bool isButtonMovement = false;
    public bool isAutoMove = false;

    public AudioClip gunfireSound;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Ball = GameObject.Find("Ball");
        ballScript = Ball.GetComponent<BallScript>();

        if (!isButtonMovement)
        {
            GameObject.Find("Left").active = false;
            GameObject.Find("Right").active = false;
        }
    }

    void Update()
    {
        if (isAutoMove)
        {
            AutoMove();
            return;
        }

        if(isButtonMovement)    ButtonMovement();
        else                    TouchMovement();

        transform.Translate(Input.GetAxis("Horizontal") * Vector2.right * speed * Time.deltaTime);
    }

    void AutoMove()
    {
        Vector3 ballXPos = new Vector3(Ball.transform.position.x, transform.position.y, transform.position.z);
        transform.position = ballXPos;
    }

    void ButtonMovement()
    {
        Vector2 paddlePos = new Vector2(transform.position.x + CrossPlatformInputManager.GetAxis("Horizontal") * speed * Time.deltaTime, transform.position.y);
        paddlePos.x = Mathf.Clamp(paddlePos.x, -2.35f, 2.35f);
        transform.position = paddlePos;
    }

    void TouchMovement()
    {
        if (!ballScript.hasLaunched) return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 paddlePos = new Vector3(mousePos.x, transform.position.y, transform.position.z);
        paddlePos.x = Mathf.Clamp(paddlePos.x, -2.35f, 2.35f);
        transform.position = Vector3.MoveTowards(transform.position, paddlePos, speed);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        print("Collided with " + col.gameObject.name);
        switch (col.tag)
        {
            case "paddle big":
                Destroy(col.gameObject);
                GameObject rewardTextsPaddle = (GameObject)Instantiate(rewardTexts[4], transform.position, Quaternion.identity);
                Destroy(rewardTextsPaddle, 2);
                StartCoroutine("PaddleBig");
                break;
            case "paddle shrink":
                Destroy(col.gameObject);
                GameObject rewardTextsPaddle2 = (GameObject)Instantiate(rewardTexts[5], transform.position, Quaternion.identity);
                Destroy(rewardTextsPaddle2, 2);
                StartCoroutine("PaddleShrink");
                break;
            case "time bonus":
                Destroy(col.gameObject);
                GameObject rewardTextsTime = (GameObject)Instantiate(rewardTexts[0], transform.position, Quaternion.identity);
                Destroy(rewardTextsTime, 2);
                gameManager.gameTime += 15;
                break;
            case "active barier":
                Destroy(col.gameObject);
                GameObject rewardTextsBarrier = (GameObject)Instantiate(rewardTexts[6], transform.position, Quaternion.identity);
                Destroy(rewardTextsBarrier, 2);
                ActiveBarier();
                break;
            case "bomb":
                Destroy(col.gameObject);
                GameObject bombExplosion = (GameObject)Instantiate(rewardTexts[7], transform.position, Quaternion.identity);
                GameObject flare = (GameObject)Instantiate(rewardTexts[8], transform.position + new Vector3(0, 0.25f, 0), Quaternion.identity);
                Destroy(bombExplosion, 2);
                Destroy(flare, 2);
                ballScript.StartCoroutine("PerformGameOver");
                break;
            case "bullet item":
                Destroy(col.gameObject);
                StartCoroutine("ActiveFiring");
                break;
            case "like":
                Destroy(col.gameObject);
                PlayerPrefs.SetInt("like", PlayerPrefs.GetInt("like",0) + 1);
                print("Like : " + PlayerPrefs.GetInt("like"));
                break;
            case "love":
                Destroy(col.gameObject);
                PlayerPrefs.SetInt("love", PlayerPrefs.GetInt("love",0) + 1);
                print("Love : " + PlayerPrefs.GetInt("love"));
                GameObject rewardTextsLife = (GameObject)Instantiate(rewardTexts[1], transform.position, Quaternion.identity);
                Destroy(rewardTextsLife, 2);
                gameManager.LifeIncrease();
                break;
            case "wow":
                Destroy(col.gameObject);
                PlayerPrefs.SetInt("wow", PlayerPrefs.GetInt("wow",0) + 1);
                print("Wow : " + PlayerPrefs.GetInt("wow"));
                break;
            case "haha":
                Destroy(col.gameObject);
                PlayerPrefs.SetInt("haha", PlayerPrefs.GetInt("haha",0) + 1);
                print("Haha : " + PlayerPrefs.GetInt("haha"));
                break;
            case "sad":
                Destroy(col.gameObject);
                PlayerPrefs.SetInt("sad", PlayerPrefs.GetInt("sad",0) + 1);
                print("Sad : " + PlayerPrefs.GetInt("sad"));
                break;
            case "angry":
                Destroy(col.gameObject);
                PlayerPrefs.SetInt("angry", PlayerPrefs.GetInt("angry",0) + 1);
                print("Angry : " + PlayerPrefs.GetInt("angry"));
                break;
            case "big ball":
                Destroy(col.gameObject);
                GameObject rewardTextsBall = (GameObject)Instantiate(rewardTexts[2], transform.position, Quaternion.identity);
                Destroy(rewardTextsBall, 2);
                StartCoroutine("BigBall");
                break;
            case "small ball":
                Destroy(col.gameObject);
                GameObject rewardTextsBall2 = (GameObject)Instantiate(rewardTexts[3], transform.position, Quaternion.identity);
                Destroy(rewardTextsBall2, 2);
                StartCoroutine("SmallBall");
                break;
            case "fireball":
                Destroy(col.gameObject);
                StartCoroutine("FireBallWrath");
                break;
            case "multiple ball":
                Destroy(col.gameObject);
                MultipleBall();
                break;
        }
    }

    void MultipleBall()
    {
        Instantiate(sideKick, Ball.transform.position, Quaternion.identity);
        Instantiate(sideKick, Ball.transform.position, Quaternion.identity);
    }

    IEnumerator FireBallWrath()
    {
        BrickScript.hitPoint = 3;

        ballScript.fireBallParticle.active = true;

        GameObject[] bricksAry;

        bricksAry = GameObject.FindGameObjectsWithTag("bricks");

        foreach(GameObject theBricks in bricksAry)
        {
            //theBricks.GetComponent<BoxCollider2D>().isTrigger = true;
            if(theBricks.GetComponent<PolygonCollider2D>()) 
                theBricks.GetComponent<PolygonCollider2D>().isTrigger = true;
        }

        yield return new WaitForSeconds(5f);

        BrickScript.hitPoint = 1;

        ballScript.fireBallParticle.active = false;

        bricksAry = GameObject.FindGameObjectsWithTag("bricks");

        foreach (GameObject theBricks in bricksAry)
        {
            //theBricks.GetComponent<BoxCollider2D>().isTrigger = false;
            if (theBricks.GetComponent<PolygonCollider2D>()) 
                theBricks.GetComponent<PolygonCollider2D>().isTrigger = false;
        }
    }


    IEnumerator BigBall()
    {
        BrickScript.hitPoint = 3;
        while(Ball.transform.localScale.x < 1.5f)
        {
            float scaleXY = Ball.transform.localScale.x;
            scaleXY += 0.05f;
            Ball.transform.localScale = new Vector2(scaleXY, scaleXY);

            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(5f);

        BrickScript.hitPoint = 1;

        while (Ball.transform.localScale.x >= 1)
        {
            float scaleXY = Ball.transform.localScale.x;
            scaleXY -= 0.05f;
            Ball.transform.localScale = new Vector2(scaleXY, scaleXY);

            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator SmallBall()
    {
        while (Ball.transform.localScale.x > 0.5f)
        {
            float scaleXY = Ball.transform.localScale.x;
            scaleXY -= 0.05f;
            Ball.transform.localScale = new Vector2(scaleXY, scaleXY);

            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(7f);

        while (Ball.transform.localScale.x <= 1)
        {
            float scaleXY = Ball.transform.localScale.x;
            scaleXY += 0.05f;
            Ball.transform.localScale = new Vector2(scaleXY, scaleXY);

            yield return new WaitForSeconds(0.01f);
        };
    }

    void ActiveBarier()
    {
        GameObject behindBarierGO = (GameObject)Instantiate(behindBarier, new Vector2(0, transform.position.y - 0.208f), Quaternion.identity);
        Destroy(behindBarierGO, 10f);
    }

    IEnumerator ActiveFiring()
    {
        Sprite originalSprite = GetComponent<SpriteRenderer>().sprite;

        GetComponent<SpriteRenderer>().sprite = ShooterPaddleSprite;

        for(int i=0; i<10; i++)
        {
            if (ballScript.isGameTempOver)
            {
                GetComponent<SpriteRenderer>().sprite = originalSprite;
                StopCoroutine("ActiveFiring");
            }

            AudioSource.PlayClipAtPoint(gunfireSound, Camera.main.transform.position, 0.4f);
            GameObject bulletGO = (GameObject)Instantiate(bullet, transform.position, Quaternion.identity);
            bulletGO.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bulletForce * Time.deltaTime);
            Destroy(bulletGO, 2);
            yield return new WaitForSeconds(0.5f);
        }

        GetComponent<SpriteRenderer>().sprite = originalSprite;
    }

    IEnumerator PaddleBig()
    {
        while(transform.localScale.x < 1.5f)
        {
            float scaleX = transform.localScale.x;
            scaleX += 0.05f;
            transform.localScale = new Vector2(scaleX, transform.localScale.y);

            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(5f);

        while (transform.localScale.x >= 1)
        {
            float scaleX = transform.localScale.x;
            scaleX -= 0.05f;
            transform.localScale = new Vector2(scaleX, transform.localScale.y);

            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator PaddleShrink()
    {
        while (transform.localScale.x > 0.7f)
        {
            float scaleX = transform.localScale.x;
            scaleX -= 0.05f;
            transform.localScale = new Vector2(scaleX, transform.localScale.y);

            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(5f);

        while (transform.localScale.x <= 1)
        {
            float scaleX = transform.localScale.x;
            scaleX += 0.05f;
            transform.localScale = new Vector2(scaleX, transform.localScale.y);

            yield return new WaitForSeconds(0.01f);
        };
    }
}
