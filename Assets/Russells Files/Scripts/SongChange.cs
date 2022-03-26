using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DanielLochner.Assets.SimpleScrollSnap;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SongChange : MonoBehaviour
{
    private Song sf;
    private SongInfo songIn;
    public SimpleScrollSnap sss;
    public AudioSource audioS;
    public Saver save;
    [Space(20)]
    public SpriteRenderer bgImage;
    [Space(20)]
    public Text highscore;
    public Text eDiff;
    public Text mDiff;
    public Text hDiff;
    public Image largeImage;
    public Image largeImage2;
    [Space(20)]
    public Text mainArtist;
    public Text mainSong;
    public Text scoringText;
    [Space(20)]
    public Text speedAmount;
    public float speedMultiplier;
    [Space(20)]
    public Text leaderboardSongName;
    public Image leaderboardSongImage;
    [Space(20)]
    public Text loadingSong;
    public Image loadingImage;
    public Text loadingHighScore;
    public Text loadingSongTime;



    private void Awake()
    {


    }

    private void Start()
    {
        save.scoring = true;
        scoringText.text = "ON";
        speedMultiplier = 1;
        save.multiplier = 1;

        SongSelect();
    }

    // Update is called once per frame
    void Update()
    {
        if (speedMultiplier <= 0)
        {
            speedMultiplier = 0;
            speedAmount.text = speedMultiplier.ToString();
            save.multiplier = speedMultiplier;
        }

    }

    public void SpeedButtonUp()
    {
        if (speedMultiplier >= 0)
        {
            speedMultiplier += 0.1f;
            save.multiplier = speedMultiplier;
            float text = (float)System.Math.Round(save.multiplier, 2);
            speedAmount.text = text.ToString();
        }
    }

    public void SpeedButtonDown()
    {
        if (speedMultiplier >= 0)
        {
            speedMultiplier -= 0.1f;
            save.multiplier = speedMultiplier;
            float text = (float)System.Math.Round(save.multiplier, 2);
            speedAmount.text = text.ToString();
        }
    }

    public void StopAudio()
    {
        audioS.Stop();

    }

    public void Changesong()
    {
        songIn = null;
        sf = null;
    }

    public void ScoreOn()
    {
        scoringText.text = "ON";
        save.scoring = true;

    }

    public void ScoreOff()
    {
        scoringText.text = "OFF";
        save.scoring = false;
    }

    public void GetPlayerPref()
    {
        save.playerPrefScore = songIn.diffID;

    }

    public void SongSelect()
    {
        audioS.Stop();
        songIn = sss.Panels[sss.NearestPanel].GetComponent<SongInfo>();
        sf = songIn.songFile;

        save.mainSong = sf;

        save.mainTrack = sf.MainKoreograpy;
        save.beats = sf.beats;
        save.beatsString = sf.beatsTrackName;
        save.tick = sf.tickTrack;
        save.tickString = sf.tickTrackString;
        save.leaderboard = save.mainSong.leaderboard;
        save.takeL = sf.takeLife;
        save.addL = sf.addLife;

        eDiff.text = sf.easyLevelNum;
        mDiff.text = sf.mediumLevellNum;
        hDiff.text = sf.hardLevelNum;

        audioS.PlayOneShot(sf.songSnip);
        largeImage.sprite = sf.ssBigIcon;
        largeImage2.sprite = sf.ssBigIcon;

        GetPlayerPref();

        mainArtist.text = sf.artistName;
        mainSong.text = sf.songName;
        leaderboardSongName.text = sf.songName;
        leaderboardSongImage.sprite = sf.ssBigIcon;

        SongLoadInfo();

    }

    void SongLoadInfo()
    {
        loadingImage.sprite = sf.ssBigIcon;
        loadingSong.text = "Now loading " + (sf.songName) + " by " + (sf.artistName);
        loadingSongTime.text = "Song time: " + sf.songTime;
        loadingHighScore.text = "Highscore: " + songIn.highScore.ToString();
    }

  
    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(3.0f);
        Application.backgroundLoadingPriority = ThreadPriority.Low;
        SceneManager.LoadSceneAsync("PlayScreen");
    }

    public void StartLoad()
    {
        StartCoroutine(Waiting());
    }
}
