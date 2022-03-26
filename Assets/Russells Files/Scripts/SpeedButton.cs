using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedButton : MonoBehaviour
{
    public float multiplier;
    [Space(10)]
    private Saver songData;
    [Space(10)]
    public Text speedAmount;


    // Start is called before the first frame update
    void Start()
    {
        songData = GameObject.FindGameObjectWithTag("Saver").GetComponent<Saver>();
        multiplier = 1;
        songData.multiplier = 1;
        //offsetAmount.text = "Current Delay: " + PlayerPrefs.GetFloat("msDelay") + "ms";
//        speedAmount.text = multiplier.ToString();
    }


    // Update is called once per frame
    void Update()
    {
        if (multiplier <= 0)
        {
            multiplier = 0;
            speedAmount.text = multiplier.ToString();
            songData.multiplier = multiplier;
        }

    }

     public void ButtonUp()
    {
        if (multiplier >= 0)
        {
            multiplier += 0.1f;
            songData.multiplier = multiplier;
            float text = (float)System.Math.Round(songData.multiplier, 2);
            speedAmount.text = text.ToString();
        }
    }

        public void ButtonDown()
    {
        if (multiplier >= 0)
        {
            multiplier -= 0.1f;
            songData.multiplier = multiplier;
            float text = (float)System.Math.Round(songData.multiplier, 2);
            speedAmount.text = text.ToString();
        }
    }
}
