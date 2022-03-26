using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyButton : MonoBehaviour
{
    public Image mainSong;
    public Sprite yellow;
    public Sprite blue;
    public Sprite pink;
    public Color yellowC;
    public Color blueC;
    public Color pinkC;

    public int difficultyNo;
    private Saver songData;
    private SpeedButton sb;

    public Text songDiff;

    public Image capsuleTop;
    public Image capsuleBot;
    public Text diffAmountText;
    public Text diffText;

    //public Text difficultyText;
    //private Text highScore;
    //private Text highCombo;

    //private Text offset;



    // Start is called before the first frame update
    void Start()
    {
        songData = GameObject.FindGameObjectWithTag("Saver").GetComponent<Saver>();
        //highScore = GameObject.FindGameObjectWithTag("SS Highscore").GetComponent<Text>();
        //highCombo = GameObject.FindGameObjectWithTag("SS Combo").GetComponent<Text>();
        difficultyNo = 0;
    }

    public void Easy()
    {
        difficultyNo = 0;
    }

    public void Medium()
    {
        difficultyNo = 1;
    }

    public void Hard()
    {
        difficultyNo = 2;
    }

    public void PlusDifficulty()
    {
        difficultyNo++;
    }

    public void MinusDifficulty()
    {
        difficultyNo--;
    }


    // Update is called once per frame
    void Update()
    {

    

        if (difficultyNo >= 2)
        {
            difficultyNo = 2;
        }

        if (difficultyNo <= 0)
        {
            difficultyNo = 0;
        }

        if (difficultyNo == 0)
        {
            songData.difficulty = songData.mainSong.easyTrack;
            songData.difficultyString = songData.mainSong.easyTrackString;
            mainSong.sprite = blue;
            capsuleBot.color = blueC;
            capsuleTop.color = blueC;
            diffAmountText.color = blueC;
            diffText.color = blueC;
            songDiff.text = "EASY";
            
            //difficultyText.text = "EASY";
            songData.baseSpeedo = songData.mainSong.easySpeedBase;
            songData.speedo = songData.mainSong.easySpeedBase * songData.multiplier;
            //highScore.text = PlayerPrefs.GetInt(songData.mainSong.playerPrefsE).ToString();
            //highCombo.text = PlayerPrefs.GetInt(songData.mainSong.playerPrefsEC).ToString();

        }

        if (difficultyNo == 1)
        {
            songData.difficulty = songData.mainSong.mediumTrack;
            songData.difficultyString = songData.mainSong.mediumTrackString;
            mainSong.sprite = yellow;
            songDiff.text = "MED";
            capsuleBot.color = yellowC;
            capsuleTop.color = yellowC;
            diffAmountText.color = yellowC;
            diffText.color = yellowC;

            //difficultyText.text = "MED";
            songData.baseSpeedo = songData.mainSong.mediumSpeedBase;
            songData.speedo = songData.mainSong.mediumSpeedBase * songData.multiplier;
            //highScore.text = PlayerPrefs.GetInt(songData.mainSong.playerPrefsM).ToString();
            //highCombo.text = PlayerPrefs.GetInt(songData.mainSong.playerPrefsMC).ToString();
        }

        if (difficultyNo == 2)
        {
            songData.difficulty = songData.mainSong.hardTrack;
            songData.difficultyString = songData.mainSong.hardTrackString;
            mainSong.sprite = pink;
            songDiff.text = "HARD";
            capsuleBot.color = pinkC;
            capsuleTop.color = pinkC;
            diffAmountText.color = pinkC;
            diffText.color = pinkC;

            //difficultyText.text = "HARD";
            songData.baseSpeedo = songData.mainSong.hardSpeedBase;
            songData.speedo = songData.mainSong.hardSpeedBase * songData.multiplier;
            //highScore.text = PlayerPrefs.GetInt(songData.mainSong.playerPrefsH).ToString();
            //highCombo.text = PlayerPrefs.GetInt(songData.mainSong.playerPrefsHC).ToString();
        }

    }
}

