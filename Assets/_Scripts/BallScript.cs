using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    Rigidbody2D RB;
    PolygonCollider2D PolyCol;
    CircleCollider2D circleCol;

    public float[] randX;
    public float minY = 0.8f;
    public float maxY = 1f;
    public float initGravityScale;


    public float lauch_force;

    public bool isGameTempOver = true;
    public bool hasLaunched = false;

    Vector2 force_dir;
    public Vector3 paddle_start_pos;
    public Vector3 ball_start_pos;



    public GameObject gameOverParticle;
    public GameObject fireBallParticle;

    public AudioClip simpleCollideSound;
    public AudioClip GameOverSound;
    public AudioClip launchSound;

    GameManager GameManager;
    GameObject GameOverPanel;
    GameObject TapToPlayPanel;
    GameObject Paddle;


    public Sprite[] heads;

    void Start()
    {
        foreach(Sprite theSprite in heads)
        {
            if(theSprite.name == PlayerPrefs.GetString("selected character"))
            {
                this.GetComponent<SpriteRenderer>().sprite = theSprite;
            }
        }

        Paddle = FindObjectOfType<PaddleScript>().transform.gameObject;

        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        GameOverPanel = GameManager.GameOverPanel;
        TapToPlayPanel = GameManager.TapToPlayPanel;

        RB = GetComponent<Rigidbody2D>();
        PolyCol = GetComponent<PolygonCollider2D>();
        circleCol = GetComponent<CircleCollider2D>();

        initGravityScale = RB.gravityScale;


        paddle_start_pos = GameObject.Find("Paddle").transform.position;
        ball_start_pos = transform.position;
    }

    void Update()
    {
        //print("=>>> " + RB.velocity);

        if(GameManager.LifeCount <= 0)
        {
            Invoke("ActiveGameOverPanel", 1f);
        }

        if (!hasLaunched) transform.position = ball_start_pos;
    }

    void ActiveGameOverPanel()
    {
        Time.timeScale = 0;
        GameOverPanel.active = true;
    }


    public void LaunchBall()
    {
        AudioSource.PlayClipAtPoint(launchSound, Camera.main.transform.position, 0.6f);

        force_dir = new Vector2(randX[Random.RandomRange(0, randX.Length)], Random.Range(minY, maxY));

        Debug.LogError("Force Dir =>>>> " + force_dir);

        RB.AddForce(force_dir /** Time.deltaTime*/ * lauch_force, ForceMode2D.Impulse);

        hasLaunched = true;

        GameManager.StartCoroutine("CountdownStart");

        TapToPlayPanel.active = false;
    }

    private void OnTriggerEnter2D(Collider2D trig)
    {
        if(trig.gameObject.tag == "Danger")
        {
            StartCoroutine("PerformGameOver");
        }
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "edge" || col.gameObject.tag == "paddle")
        {
            AudioSource.PlayClipAtPoint(simpleCollideSound, Camera.main.transform.position, 0.6f);
        }
    }

    IEnumerator PerformGameOver()
    {
        sidekickScript[] sideKicks = FindObjectsOfType<sidekickScript>();
        foreach (sidekickScript theSideKick in sideKicks) theSideKick.PerforemDestroy();

        GameObject[] bombs = GameObject.FindGameObjectsWithTag("bomb");
        foreach (GameObject theBomb in bombs) Destroy(theBomb);

        isGameTempOver = true;

        GameManager.DestroyLife();

        if(GameManager.LifeCount > 0) AudioSource.PlayClipAtPoint(GameOverSound, Camera.main.transform.position);
        Instantiate(gameOverParticle, transform.position, Random.rotation);
        RB.AddForce(new Vector2(Random.Range(-2, 2), Random.Range(1.5f, 3f)) * Time.deltaTime * lauch_force, ForceMode2D.Impulse);

        //if(PolyCol)     PolyCol.enabled = false;
        if(circleCol)   circleCol.enabled = false;

        yield return new WaitForSeconds(0.5f);


        RB.gravityScale = -19.8f;

        yield return new WaitForSeconds(0.3f);

        transform.position = ball_start_pos;
        Paddle.transform.position = paddle_start_pos;

        hasLaunched = false;

        RB.gravityScale = initGravityScale;

        RB.constraints = RigidbodyConstraints2D.FreezePosition;

        //if (PolyCol) PolyCol.enabled = true;
        if (circleCol) circleCol.enabled = true;

        TapToPlayPanel.active = true;

        yield return new WaitForSeconds(0.1f);

        RB.constraints = RigidbodyConstraints2D.None;

        isGameTempOver = false;
    }
}
