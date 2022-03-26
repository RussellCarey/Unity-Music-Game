using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour

{
    public Saver songData;
    public Text songName;
    public Text artistName;

    public Image songPicture;

    public Text comboText;
    public Text comboAmount;
    public Text comboAmountFin;
    public Text scoreAmount;
    public Text scoreAmountFin;
    public Text gradeLetter;
    public Text totalMissesAmount;
    public Text badAmount;
    public Text okAmount;
    public Text goodAmount;
    public Text perfectAmount;
    [Space(30)]
    public string currentGrade;
    [Space(10)]
    public int maxScore;
    public int combo;
    public int maxCombo = 0;
    public long score = 0;
    [Space(10)]
    public int totalNotes;
    public int totalHits = 0;
    public int totalMisses = 0;
    [Space(10)]
    public float percentage = 0;
    public float currentPercentage = 100;
    [Space(10)]
    public int bad = 0;
    public int ok = 0;
    public int good = 0;    
    public int perfect = 0;
    [Space(30)]
    public float perfectScore;
    public float greatScore;
    public float goodScore;
    public float badScore;
    [Space(10)]
    public bool failed;
    int currentScore;


    // Start is called before the first frame update
    void Start()
    {
        songData = GameObject.FindGameObjectWithTag("Saver").GetComponent<Saver>();
        string ppName = songData.playerPrefScore;
        int id = PlayerPrefs.GetInt(ppName);

        Debug.Log("Loaded current highscore and it is: " + id);
    }

    // Update is called once per frame
    void Update()
    {
        Combo();
        //CurrentPercentage();
        Grading();
        comboAmount.text = combo.ToString();
        comboAmountFin.text = maxCombo.ToString();
        

        if(failed != true)
        {
            scoreAmountFin.text = score.ToString();
            scoreAmount.text = score.ToString();
        }

        if (failed == true)
        {
            scoreAmountFin.text = "FAILED";
            scoreAmount.text = "FAILED";
        }
    }



    void Combo()
    {
        if (combo >= maxCombo)
        {
            maxCombo = combo;
        }

    }



    void CurrentPercentage()
    {
        currentPercentage = Mathf.FloorToInt(currentPercentage = totalHits / (totalHits + totalMisses) * 100);
    }



    public void Percentage()
    {
        percentage = Mathf.RoundToInt(percentage = (totalHits / totalNotes) * 100);
    }



    void Grading()
    {
        // Sets the grapde depending on the score amount.
        if (score >= 0 && score <= 6999999)
        {
            currentGrade = "D";
        }

        if (score >= 7000000 && score <= 7999999)
        {
            currentGrade = "C";
        }

        if (score >= 8000000 && score <= 9199999)
        {
            currentGrade = "B";
        }

        if (score >= 9200000 && score <= 9499999)
        {
            currentGrade = "A";
        }

        if (score >= 9500000 && score <= 9799999)
        {
            currentGrade = "S";
        }

        if (score >= 9800000)
        {
            currentGrade = "S+";
        }
    }

    public void SongScoreFinish()
    {
        int scoring = (int)score;

        if (scoring > currentScore)
        {
            PlayerPrefs.SetInt(songData.playerPrefScore, scoring);
            PlayerPrefs.Save();
            Debug.Log("Saving new score... old score is " + currentScore + " and new score is " + scoring);
        }

        gradeLetter.text = currentGrade;
        totalMissesAmount.text = totalMisses.ToString();
        badAmount.text = bad.ToString();
        okAmount.text = ok.ToString();
        goodAmount.text = good.ToString();
        perfectAmount.text = perfect.ToString();

       


    }

    


    public void Reset()
    {
        Destroy(this.gameObject);
        Destroy(this);

    }


}
