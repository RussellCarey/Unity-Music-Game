using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoading : MonoBehaviour
{
    public GameObject loadingWheel;
    public Text information;
    private bool run = false;

    public void Start()
    {
        loadingWheel.SetActive(false);
        information.gameObject.SetActive(false);
    }

    public void LoadingStart()
    {
        loadingWheel.SetActive(true);
        information.gameObject.SetActive(true);
        information.text = "Connecting..";
    
    }

    private void Update()
    {
 
    }

    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Song Select");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            information.text = "Loading Game..";
            yield return null;
        }
    }

}
