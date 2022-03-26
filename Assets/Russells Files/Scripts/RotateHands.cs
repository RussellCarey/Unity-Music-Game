using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;


public class RotateHands : MonoBehaviour
{
    public AudioSource audioS;
    public Koreographer mainMusic;
    public KoreographyTrack track;
    public string trackString;
    public GameObject hand;
    public float amount;
    public float currentAmount;
    public float songTime;
    public bool stop = false;


    // Start is called before the first frame update
    void Start()
    {
        songTime = audioS.clip.length;

    }

    // Update is called once per frame
    void Update()
    {
        if (audioS.clip != null && stop == false && audioS.isPlaying)
        {
            songTime = audioS.clip.length;
            amount = 360 / songTime / 60;
            hand.transform.Rotate(0.0f, 0.0f, -amount, Space.Self);
        }

        
    }


    public void StopHand()
    {
        stop = true;
        if(stop == true)
        {
            hand.transform.Rotate(0.0f, 0.0f, 0.0f, Space.Self);
        }
    }
}
