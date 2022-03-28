using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public AudioClip ButtonTapSound;

    string scene;

    public void LoadLevel(string sceneName)
    {
        scene = sceneName;

        StartCoroutine("LoadScene");
    }
    IEnumerator LoadScene()
    {
        AudioSource.PlayClipAtPoint(ButtonTapSound, Camera.main.transform.position);
        yield return new WaitForSeconds(ButtonTapSound.length);
        Application.LoadLevel(scene);
    }

    public void Exit()
    {
        StartCoroutine("ExitButtonAudio");
    }
    IEnumerator ExitButtonAudio()
    {
        AudioSource.PlayClipAtPoint(ButtonTapSound, Camera.main.transform.position);
        yield return new WaitForSeconds(ButtonTapSound.length);
        Application.Quit();
    }


    IEnumerator playButtonTap()
    {
        AudioSource.PlayClipAtPoint(ButtonTapSound, Camera.main.transform.position);
        yield return new WaitForSeconds(ButtonTapSound.length);
    }

    public void PlayButtonTap()
    {
        AudioSource.PlayClipAtPoint(ButtonTapSound, Camera.main.transform.position);
    }

    public void DirectLoadScene(string scene)
    {
        Application.LoadLevel(scene);
    }
}
