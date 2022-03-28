using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource audioSource;

    public AudioClip[] musics;

    static bool audioAlreadySelected;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(2);

        audioSource = GetComponent<AudioSource>();

        while(!audioAlreadySelected)
        {
            audioSource.clip = musics[Random.Range(0, musics.Length)];
            audioSource.Play();
            Debug.LogError("Now playing =>> " + audioSource.clip.name);
            audioAlreadySelected = true;

            if (FindObjectsOfType(GetType()).Length > 1)    Destroy(gameObject);
            else                                            DontDestroyOnLoad(gameObject);

            yield return new WaitForSeconds(audioSource.clip.length);

            audioAlreadySelected = false;
        }



    }
}
