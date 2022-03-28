using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBrick : MonoBehaviour
{
    public float radius = 0.5f;
    public GameObject flareExplosion;
    public GameObject BrickExplodeParticleGreen;
    public GameObject BrickExplodeParticleRed;
    public GameObject DiamondExplosion;
    public AudioClip ExplodeClip;

    GameManager gameManager;

    public int explodingId;
    public char explodingBrickColor;
    //1 -   line
    //2 -   self
    //3 -   color

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if(explodingId == 1)
            {
                LineExplode();
            }
            else if(explodingId == 2)
            {
                SelfExplode();
            }
            else if(explodingId == 3)
            {
                StartCoroutine("ColorExplode");
            }
        }
    }
    public void OnTriggerEnter2D(Collider2D trig)
    {
        if(trig.tag == "bullet")
        {
            if (explodingId == 1)
            {
                LineExplode();
            }
            else if(explodingId == 2)
            {
                SelfExplode();
            }
            else if (explodingId == 3)
            {
                StartCoroutine("ColorExplode");
            }
        }
    }

    IEnumerator ColorExplode()
    {
        //Instantiate(flareExplosion, transform.position, Quaternion.identity);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach(Collider2D theCol in colliders)
        {
            BrickScript brickScript = theCol.transform.gameObject.GetComponent<BrickScript>();

            if (brickScript && brickScript.myColor == explodingBrickColor)
            {
                gameManager.bricksDestroyedCount++;

                Destroy(theCol.gameObject);
                // theCol.transform.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            }
        }

        foreach (Collider2D theCollider in colliders)
        {
            BrickScript brickScript = theCollider.transform.gameObject.GetComponent<BrickScript>();

            AudioSource.PlayClipAtPoint(ExplodeClip, Camera.main.transform.position, 0.1f);

            if (brickScript && brickScript.myColor == explodingBrickColor)
            {
                yield return new WaitForSeconds(0.05f);

                gameManager.bricksDestroyedCount++; 

                Instantiate(DiamondExplosion, theCollider.transform.gameObject.transform.position, Quaternion.identity);
                Destroy(theCollider.gameObject);
            }
        }
        Destroy(this.gameObject);
    }

    void LineExplode()
    {
        Instantiate(flareExplosion, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(ExplodeClip, Camera.main.transform.position);


        RaycastHit2D[] hitInfo1 = Physics2D.RaycastAll(transform.position, transform.right * -1, 300);
        RaycastHit2D[] hitInfo2 = Physics2D.RaycastAll(transform.position, transform.right, 300);

        foreach(RaycastHit2D theHitInfo in hitInfo1)
        {
            if(theHitInfo.transform.gameObject.GetComponent<BrickScript>())
            {
                gameManager.bricksDestroyedCount++;

                Instantiate(BrickExplodeParticleRed, theHitInfo.transform.gameObject.transform.position, Quaternion.identity);
                Destroy(theHitInfo.transform.gameObject);
            }
        }
        foreach(RaycastHit2D theHitInfo in hitInfo2)
        {
            if(theHitInfo.transform.gameObject.GetComponent<BrickScript>())
            {
                gameManager.bricksDestroyedCount++;

                Instantiate(BrickExplodeParticleRed, theHitInfo.transform.gameObject.transform.position, Quaternion.identity);
                Destroy(theHitInfo.transform.gameObject);
            }
        }

        Destroy(this.gameObject);
    }

    void SelfExplode()
    {
        Instantiate(flareExplosion, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(ExplodeClip, Camera.main.transform.position);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (Collider2D theCollider in colliders)
        {
            BrickScript brickScript = theCollider.transform.gameObject.GetComponent<BrickScript>();

            if (brickScript)
            {
                gameManager.bricksDestroyedCount++;

                Instantiate(BrickExplodeParticleGreen, theCollider.transform.gameObject.transform.position, Quaternion.identity);
                Destroy(theCollider.gameObject);
            }
        }
        Destroy(this.gameObject);
    }
}
