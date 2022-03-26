using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;

public class PauseGame : MonoBehaviour
{
    public RotateHands hands;
    public GameObject darken;
    public Text countdownText;
    public Text currentSpeedMultiplier;
    public GameObject speedUp;
    public GameObject speedDown;
    public Text songSelect;
    public Text reset;
    public GameObject UI;
    public GameObject pauseButton;
    
    private SimpleMusicPlayer smp;
    private AudioSource ass;
    public bool paused;
    private RhythmGameController rgc;
    private Saver saver;
    private ScoreManager sm;

    private float baseSpeed;

    private float countDown;
    private bool counting;


    // Start is called before the first frame update
    void Start()
    {
        ass = GameObject.FindGameObjectWithTag("audiocontroller").GetComponent<AudioSource>();
        smp = GameObject.FindGameObjectWithTag("audiocontroller").GetComponent<SimpleMusicPlayer>();
        rgc = GameObject.FindGameObjectWithTag("GameController").GetComponent<RhythmGameController>();
        saver = GameObject.FindGameObjectWithTag("Saver").GetComponent<Saver>();
        sm = GameObject.FindGameObjectWithTag("Score Manager").GetComponent<ScoreManager>();
        
        paused = false;
        countDown = 5.0f;
        baseSpeed = saver.baseSpeedo;
        pauseButton.SetActive(false);
    }


   public void PressPause()
    {
        paused = true;
        ass.Pause();
        smp.Pause();
        hands.stop = true;
        hands.StopHand();
        countdownText.text = "Paused";
        darken.SetActive(true);
        UI.SetActive(true);
        countdownText.gameObject.SetActive(true);
        rgc.paused = true;
        reset.gameObject.SetActive(true);
        songSelect.gameObject.SetActive(true);
        currentSpeedMultiplier.gameObject.SetActive(true);
        speedUp.SetActive(true);
        speedDown.SetActive(true);
        currentSpeedMultiplier.text = "Current Speed: x" + saver.multiplier.ToString();
        


    }


    public void Unpause()
    {
        if(paused == true)
        {
            counting = true;
    
        }
    }

    public void Reset()
    {
        if(paused == true)
        {
            sm.Reset();
            Initiate.Fade("PlayScreen", Color.black, 4f);
        }
    }

    public void SongSelectPause()
    {
        Destroy(saver.gameObject);
        if(paused == true)
        {
            Initiate.Fade("Song Select", Color.black, 4f);
        }
    }

    public void SongSelect()
    {
        Destroy(saver.gameObject);
        Initiate.Fade("Song Select", Color.black, 4f);
    }


    public void SpeedDown()
    {
        if (saver.multiplier >= 0.2f && paused == true)
        {
            saver.multiplier -= 0.1f;
            rgc.noteSpeed = baseSpeed * saver.multiplier;
            saver.multiplier = (float)System.Math.Round(saver.multiplier, 2);
            currentSpeedMultiplier.text = "Current Speed: x" + saver.multiplier.ToString();

        }
    }

    public void SpeedUp()
    {
        if (paused == true)
        {
            saver.multiplier += 0.1f;
            rgc.noteSpeed = baseSpeed * saver.multiplier;
            saver.multiplier = (float)System.Math.Round(saver.multiplier, 2);
            currentSpeedMultiplier.text = "Current Speed: x" + saver.multiplier.ToString();
        }
    }

    // Update is called once per frame
    public void Update()
    {
        if(rgc.timeLeftToPlay <= 0)
        {
            pauseButton.SetActive(true);
        }

        if (counting == true)
        {
            countDown -= Time.deltaTime;
            countdownText.text = Mathf.Round(countDown).ToString();
        }

        if (countDown <= 0)
        {
            counting = false;
            paused = false;
            countdownText.text = null;
            countdownText.gameObject.SetActive(false);
            currentSpeedMultiplier.gameObject.SetActive(false);
            speedUp.SetActive(false);
            speedDown.SetActive(false);
            darken.SetActive(false);
            ass.UnPause();
            smp.Play();

            
            countDown = 5.0f;
            rgc.paused = false;
            UI.SetActive(true);
            reset.gameObject.SetActive(false);
            songSelect.gameObject.SetActive(false);
            hands.stop = false;
        }

    }
}
