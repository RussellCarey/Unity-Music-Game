using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongInfo : MonoBehaviour
{
    public Song songFile;
    public bool isSelected;
    public Image largeImage;
    public Text artistName;
    public Text songName;
    public Text highscoreText;
    public DifficultyButton db;
    public string diffID;
    public int highScore;




    // Start is called before the first frame update
    void Start()
    {
        Image thisObject = this.GetComponent<Image>();
        largeImage.sprite = songFile.ssBigIcon;
        artistName.text = songFile.artistName;
        songName.text = songFile.songName;
       
    }

    void Update()
    {
        db = GameObject.FindGameObjectWithTag("DM").GetComponent<DifficultyButton>();

        if (db.difficultyNo == 0)
        {
            diffID = songFile.playerPrefsE;
            highScore = PlayerPrefs.GetInt(diffID);
            highscoreText.text = "Highscore: " + highScore.ToString();
        }

        if (db.difficultyNo == 1)
        {
            diffID = songFile.playerPrefsM;
            highScore = PlayerPrefs.GetInt(diffID);
            highscoreText.text = "Highscore: " + highScore.ToString();
        }

        if (db.difficultyNo == 2)
        {
            diffID = songFile.playerPrefsH;
            highScore = PlayerPrefs.GetInt(diffID);
            highscoreText.text = "Highscore: " + highScore.ToString();
        }
    }

}
