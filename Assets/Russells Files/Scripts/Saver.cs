using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;

public class Saver : MonoBehaviour
{
    public Song mainSong;
    public Koreography mainTrack;
    public KoreographyTrack difficulty;
    public string difficultyString;
    public KoreographyTrack beats;
    public string beatsString;
    public KoreographyTrack tick;
    public string tickString;
    public float speedo;
    public float baseSpeedo;
    public float multiplier;
    public bool scoring;
    public float takeL;
    public float addL;
    public string leaderboard;
    public string playerPrefScore;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        speedo = (float)System.Math.Round(speedo, 2);
    }

}
