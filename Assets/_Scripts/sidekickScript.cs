using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sidekickScript : MonoBehaviour
{
    public GameObject sideKickParticle;
    public float lauch_force = 2f;
    void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-2, 2), Random.Range(1f, 1.8f)) * lauch_force, ForceMode2D.Impulse);

        Invoke("PerforemDestroy", 5);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Danger")
        {
            PerforemDestroy();
        }
    }

    public void PerforemDestroy()
    {
        Instantiate(sideKickParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }


}
