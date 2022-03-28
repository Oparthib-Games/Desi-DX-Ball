using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class starSystem : MonoBehaviour
{
    public GameObject[] stars;

    public IEnumerator Active1Star()
    {
        stars[0].active = true;
        stars[1].active = false;
        stars[2].active = false;
        yield return new WaitForSeconds(1f);
    }    
    public IEnumerator Active2Star()
    {
        stars[0].active = true;
        yield return new WaitForSeconds(1f);
        stars[1].active = true;
        stars[2].active = false;
    }    
    public IEnumerator Active3Star()
    {
        stars[0].active = true;
        yield return new WaitForSeconds(1f);
        stars[1].active = true;
        yield return new WaitForSeconds(1f);
        stars[2].active = true;
    }

}
