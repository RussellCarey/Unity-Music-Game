using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;

[CreateAssetMenu(fileName = "Song", menuName = "Song File")]

public class Song : ScriptableObject
{
    public bool isVideo;
    public string videoName;
    public string artistName;
    public string songName;
    public string songTime;
    [Space(20)]
    public string playerPrefsE;
    public string playerPrefsM;
    public string playerPrefsH;
    public string playerPrefsEC;
    public string playerPrefsMC;
    public string playerPrefsHC;
    [Space(20)]
    public float addLife;
    public float takeLife;
    [Space(20)]
    public string leaderboard;
    [Space(20)]
    public AudioClip songSnip;
    public int bpm;
    [Space(20)]
    public Koreography MainKoreograpy;
    public KoreographyTrack easyTrack;
    public string easyTrackString;
    public int easySpeedBase;
    public KoreographyTrack mediumTrack;
    public string mediumTrackString;
    public int mediumSpeedBase;
    public KoreographyTrack hardTrack;
    public string hardTrackString;
    public int hardSpeedBase;
    public KoreographyTrack tickTrack;
    public string tickTrackString;
    [Space(20)]
    public KoreographyTrack beats;
    public string beatsTrackName;
    [Space(30)]
    public string easyLevelNum;
    public string mediumLevellNum;
    public string hardLevelNum;
    [Space(20)]
    public Color noteColor;
    public Color doubleColor;
    public Color holdColor;
    [Space(10)]
    public Color mainColor;
    public Color lightColor;
    public Color darkColor;
    [Space(20)]
    public Sprite ssSmallIcon;
    public Sprite ssBigIcon;
    public Sprite ssBackground;
    [Space(20)]
    public Sprite songBG;
    [Space(20)]
    public Sprite comBGImage;
    public Sprite comLogo;

    public void AddingScore()
    {

    }

}
