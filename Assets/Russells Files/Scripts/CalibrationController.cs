using UnityEngine;
using UnityEngine.Video;
using System.Collections;
using System.Collections.Generic;
using SonicBloom.Koreo.Players;
using SonicBloom.Koreo;
using System;

public class CalibrationController : MonoBehaviour
{

    public Saver saver;
    public SimpleMusicPlayer simple;
    public KoreographyTrackBase rhythmTrack;
    public List<KoreographyEvent> rawEvents;


    [Tooltip("The amount of time in seconds to provide before playback of the audio begins.  Changes to this value are not immediately handled during the lead-in phase while playing in the Editor.")]
    public float leadInTime;

    [Tooltip("The Audio Source through which the Koreographed audio will be played.  Be sure to disable 'Auto Play On Awake' in the Music Player.")]
    public AudioSource audioCom;

    // The amount of leadInTime left before the audio is audible.
    float leadInTimeLeft;

    // The amount of time left before we should play the audio (handles Event Delay).
    public float timeLeftToPlay;


    // Local cache of the Koreography loaded into the Koreographer component.
    Koreography playingKoreo;


    // The Sample Rate specified by the Koreography.
    public int SampleRate
    {
        get
        {
            return playingKoreo.SampleRate;
        }
    }


    // The current sample time, including any necessary delays.
    public int DelayedSampleTime
    {
        get
        {
            // Offset the time reported by Koreographer by a possible leadInTime amount.
            return playingKoreo.GetLatestSampleTime() - (int)(audioCom.pitch * leadInTimeLeft * SampleRate);
        }
    }


    public void Start()
    {
        saver = GameObject.FindGameObjectWithTag("CSaver").GetComponent<Saver>();

        playingKoreo = saver.mainTrack;
        simple.LoadSong(saver.mainTrack);

        rhythmTrack = saver.difficulty;
        rawEvents = rhythmTrack.GetAllEvents();

        for (int i = 0; i < rawEvents.Count; ++i)
        {
            KoreographyEvent evt = rawEvents[i];

        }
        InitializeLeadIn();
    }


    // Sets up the lead-in-time.  Begins audio playback immediately if the specified lead-in-time is zero.
    void InitializeLeadIn()
    {

        // Initialize the lead-in-time only if one is specified.
        if (leadInTime > 0f)
        {
            // Set us up to delay the beginning of playback.
            leadInTimeLeft = leadInTime;
            timeLeftToPlay = leadInTime - Koreographer.Instance.EventDelayInSeconds;
        }
        else
        {
            // Play immediately and handle offsetting into the song.  Negative zero is the same as
            //  zero so this is not an issue.

            audioCom.time = -leadInTime;
            audioCom.Play();

        }
    }


    void Update()
    {

        // Count down some of our lead-in-time.
        if (leadInTimeLeft > 0f)
        {
            leadInTimeLeft = Mathf.Max(leadInTimeLeft - Time.unscaledDeltaTime, 0f);
            audioCom.Stop();
        }

        // Count down the time left to play, if necessary.
        if (timeLeftToPlay > 0f)
        {
            timeLeftToPlay -= Time.unscaledDeltaTime;

            // Check if it is time to begin playback.
            if (timeLeftToPlay <= 0f)
            {
                    audioCom.time = -timeLeftToPlay;
                    audioCom.Play();
            }

        }
    }

}

