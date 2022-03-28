using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrickScript : MonoBehaviour
{
    public int health = 1;
    public static int hitPoint = 1;
    public GameObject DestroyParticle;
    public GameObject bulletExplosion;
    public GameObject[] BrickDestroyReward;
    int RandomReward;


    public AudioClip BreakingSound;
    public AudioClip PlayerColliedeSound;

    GameManager GameManager;
    SpriteRenderer spriteRenderer;

    public Sprite[] brickSprites;

    public bool isUnbreakable;
    public bool isNumberBrick;
    public bool isStoneBrick;
    public Text numberText;


    public char myColor;

    void Start()
    {
        RandomReward = (int)Random.Range(0, BrickDestroyReward.Length);

        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (isNumberBrick) numberText.text = health.ToString();

        if (isStoneBrick) GetComponent<SpriteRenderer>().sprite = brickSprites[health-1];
    }

    void Update()
    {
        if(health<=0)
        {
            //animation

            if(BrickDestroyReward.Length>0)    Instantiate(BrickDestroyReward[RandomReward], transform.position, Quaternion.identity);

            GameManager.bricksDestroyedCount++;

            AudioSource.PlayClipAtPoint(BreakingSound, Camera.main.transform.position, 0.3f);

            Instantiate(DestroyParticle, transform.position, Random.rotation);

            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")  CollidedWithPlayer();
    }

    private void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.tag == "bullet")
        {
            Destroy(trig.gameObject);
            Instantiate(bulletExplosion, trig.gameObject.transform.position, Quaternion.identity);

            CollidedWithPlayer();
        }

        if (trig.tag == "Player")   CollidedWithPlayer();
    }

    public void CollidedWithPlayer()
    {
        if(isUnbreakable)
        {
            AudioSource.PlayClipAtPoint(PlayerColliedeSound, Camera.main.transform.position);
            return;
        }

        if(isNumberBrick)
        {
            AudioSource.PlayClipAtPoint(PlayerColliedeSound, Camera.main.transform.position);

            health -= hitPoint;
            if (health > 0)
            {
                AudioSource.PlayClipAtPoint(PlayerColliedeSound, Camera.main.transform.position);

                numberText.text = health.ToString();
            }
            return;
        }

        health -= hitPoint;
        if (health > 0)
        {
            AudioSource.PlayClipAtPoint(PlayerColliedeSound, Camera.main.transform.position);

            spriteRenderer.sprite = brickSprites[health - 1];
        }
    }
}
