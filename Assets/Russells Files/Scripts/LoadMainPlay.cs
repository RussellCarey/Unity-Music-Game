using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainPlay : MonoBehaviour
{

    public float time;


    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(time);
        Application.backgroundLoadingPriority = ThreadPriority.Low;
        SceneManager.LoadSceneAsync("PlayScreen");
    }

    public void StartLoad()
    {
        StartCoroutine(Waiting());
    }
}
