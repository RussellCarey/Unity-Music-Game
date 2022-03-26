using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MobCali : MonoBehaviour
{
   
public ZUIManager uiManager;
    public Menu uiMenu;

    public int pressNo = 0;
    public List<float> sampleOffset = new List<float>();
    public float averageTiming;
    public float averageTimingSeconds;
    public bool pressReady;
    public int currentSample;

    public GameObject hit1;
    public GameObject hit2;
    public GameObject hit3;
    public GameObject hit4;
    public Text averageSeconds;

    public Button goButton;

    private CalibrationController gameController;
    public float timing;


    public void Start()
    {
        timing = PlayerPrefs.GetFloat("offset");
        if (timing != 0)
        {
            uiManager.OpenMenu(uiMenu);
        }

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<CalibrationController>();
        pressReady = true;
        goButton.gameObject.SetActive(false);


    }

    // Key presses.
    public void Update()
    {
        currentSample = Koreographer.Instance.GetMusicSampleTime();

        for (int i = 0; i < Input.touchCount; ++i)
        {
            Debug.Log("pressed touched super start");

            Vector2 test = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
            RaycastHit2D hit = Physics2D.Raycast(test, (Input.GetTouch(i).position));



            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                Debug.Log("Touch Began began");
                if (hit.collider != null && hit.collider.tag == "hit")
                {
                    Debug.Log("Hit Hit button");
                    FindEvent();
                }
            }

            if (Input.GetTouch(i).phase == TouchPhase.Ended)
            {
                Debug.Log("Touch Began began");
                if (hit.collider != null && hit.collider.tag == "hit")
                {
                    Debug.Log("Touch Ended");
                    pressNo = pressNo + 1;
                    Debug.Log("Press number increased");
                }
            }


        }
    }

 

    public void FindEvent()
    {
        FindClosestEvent();
    }



    // Find asmples offet.
    public void FindClosestEvent()
    {

        if (currentSample <= gameController.rawEvents[1].StartSample)
        {
            if (pressNo == 0)
            {
                sampleOffset.Add((float)currentSample - (float)gameController.rawEvents[0].StartSample);
                hit1.SetActive(true);
                Debug.Log("Sample added to thing" + pressNo);
            }


            if (pressNo == 1)
            {
                sampleOffset.Add((float)currentSample - (float)gameController.rawEvents[0].StartSample);
                hit2.SetActive(true);
                Debug.Log("Sample added to thing" + pressNo);
            }

            if (pressNo == 2)
            {
                sampleOffset.Add((float)currentSample - (float)gameController.rawEvents[0].StartSample);
                hit3.SetActive(true);
                Debug.Log("Sample added to thing" + pressNo);
            }


            if (pressNo == 3)
            {
                sampleOffset.Add((float)currentSample - (float)gameController.rawEvents[0].StartSample);
                hit4.SetActive(true);
                Debug.Log("Sample added to thing" + pressNo);
                Average();
            }
            
        }
    }



    // Finds average samples to sync and time.
    public void Average()
    {
        gameController.audioCom.Stop();
        averageTiming = (sampleOffset[0] + sampleOffset[1] + sampleOffset[2] + sampleOffset[3]) / 4;
        averageTimingSeconds = averageTiming / 44100.0f;
        //System.Math.Round(averageTimingSeconds, 3);
        averageSeconds.text = "Average Offset: " + averageTimingSeconds.ToString();
        averageSeconds.gameObject.SetActive(true);
        PlayerPrefs.SetFloat("offset", averageTimingSeconds);
        PlayerPrefs.Save();
        goButton.gameObject.SetActive(true);
        timing = averageTimingSeconds;
    }



    // Moves to the next scene.
    public void StartGame()
    {
        gameController.saver.gameObject.SetActive(false);
    }

    // Resets the calibrations.
    public void Reset()
    {
        gameController.audioCom.Stop();
        goButton.gameObject.SetActive(false);
        averageSeconds.gameObject.SetActive(false);
        hit1.SetActive(false);
        hit2.SetActive(false);
        hit3.SetActive(false);
        hit4.SetActive(false);


        gameController.audioCom.Play();
        sampleOffset.Clear();
        pressNo = 0;
        pressReady = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
