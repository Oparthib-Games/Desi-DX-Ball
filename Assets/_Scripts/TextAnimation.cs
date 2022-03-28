using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TextAnimation : MonoBehaviour
{
    public int AnimationNumber = 1;

    Text text;

    int initialFontSize;
    bool shrink;

    public int maxFontSize = 100;

    void Start()
    {
        text = GetComponent<Text>();
        initialFontSize = text.fontSize;

        StartCoroutine("FontGrowShrink");
        StartCoroutine("FontGrowNDVanish");

        if(transform.name == "levelNameText") text.text = SceneManager.GetActiveScene().name;
    }

    private void OnEnable()
    {
        //StartCoroutine("FontGrowShrink");
    }

    IEnumerator FontGrowNDVanish()
    {
        if (AnimationNumber != 2) StopCoroutine("FontGrowNDVanish");

        float Aplha = text.color.a;

        while (text.fontSize < maxFontSize)
        {
            text.fontSize++;
            yield return new WaitForSeconds(0.0000000000000000001f);
        }
        while(text.color.a >= 0)
        {
            Aplha -= Time.deltaTime;
            text.color = new Color(250, 250, 250, Aplha);
            yield return new WaitForSeconds(0.005f);
        }
        Destroy(gameObject);
    }

    IEnumerator FontGrowShrink()
    {
        if (AnimationNumber != 1) StopCoroutine("FontGrowShrink");

        while(text.fontSize > 105)
        {
            text.fontSize--;
            yield return new WaitForSeconds(0.025f);
        }
        
        while(text.fontSize < 125)
        {
            text.fontSize++;
            yield return new WaitForSeconds(0.025f);
        }
        StartCoroutine("FontGrowShrink");
    }
}
