using UnityEngine;
using UnityEngine.Video;
using System.Collections;
using System.Collections.Generic;
using SonicBloom.Koreo.Players;
using SonicBloom.Koreo;
using System;
using Random = UnityEngine.Random;

public class RhythmGameController : MonoBehaviour
{
    #region Fields
    [Space(20)]
    public bool editingMode;
     [Space(20)]
    private Saver saver;
    public SimpleMusicPlayer simple;
    private ScoreManager sm;
    private VideoPlayer video;
    [Space(20)]
    public float timeRemaining;
    public float songLength;
    public bool paused;
    public float pitch;
    [Space(20)]
    public KoreographyTrackBase rhythmTrack;
    public List<KoreographyEvent> rawEvents;


    public LaneController lc;
    [Space(20)]
    [Tooltip("The number of milliseconds (both early and late) within which input will be detected as a Hit.")]
    [Range(8f, 150)]
    public float perfectHitWindowRangeInMS;
    [Range(8f, 150)]
    public float greatHitWindowRangeInMS;
    [Range(8f, 150)]
    public float goodHitWindowRangeInMS;
    [Range(8f, 200)]
    public float badHitWindowRangeInMS;
    [Range(8f, 200)]
    public float holdHitWindowRangeInMS;
    [Space(20)]
    [Tooltip("The number of units traversed per second by Note Objects.")]
    public float noteSpeed = 1f;

    [Tooltip("The archetype (blueprints) to use for generating notes.  Can be a prefab.")]
    public NoteObject noteObjectArchetype;

    [Tooltip("The list of Lane Controller objects that represent a lane for an event to travel down.")]
    public List<LaneController> noteLanes = new List<LaneController>();

    private KoreographyEvent addedDouble;
    private KoreographyEvent holdNote;
    private int addedSample = 1;
    [Space(20)]
    [Tooltip("The amount of time in seconds to provide before playback of the audio begins.  Changes to this value are not immediately handled during the lead-in phase while playing in the Editor.")]
    public float leadInTime;

    [Tooltip("The Audio Source through which the Koreographed audio will be played.  Be sure to disable 'Auto Play On Awake' in the Music Player.")]
    public AudioSource audioCom;

    // The amount of leadInTime left before the audio is audible.
    float leadInTimeLeft;

    // The amount of time left before we should play the audio (handles Event Delay).
    public float timeLeftToPlay;
    [Space(20)]

    // Local cache of the Koreography loaded into the Koreographer component.
    Koreography playingKoreo;

    // Koreographer works in samples.  Convert the user-facing values into sample-time.  This will simplify
    //  calculations throughout.
    int perfectHitWindowRangeInSamples; // The sample range within which a viable event may be hit.
    int greatHitWindowRangeInSamples;
    int goodHitWindowRangeInSamples;
    int badHitWindowRangeInSamples;
    int holdWindowRangeInSamples;

    // The pool for containing note objects to reduce unnecessary Instantiation/Destruction.
    Stack<NoteObject> noteObjectPool = new Stack<NoteObject>();
    [Space(20)]
    public ZUIManager endOfSong;
    public Menu endMenu;
    private bool ended = true;

    #endregion
    #region Properties

    public void OpenEndSongMenu()
    {
        endOfSong.OpenMenu(endMenu);
    }

    // Public access to the hit window.
    public int perfectHitWindowSampleWidth
    {
        get
        {
            return perfectHitWindowRangeInSamples;
        }
    }

    public int greatHitWindowSampleWidth
    {
        get
        {
            return greatHitWindowRangeInSamples;
        }
    }

    public int goodHitWindowSampleWidth
    {
        get
        {
            return goodHitWindowRangeInSamples;
        }
    }

    public int badHitWindowSampleWidth
    {
        get
        {
            return badHitWindowRangeInSamples;
        }
    }
    public int holdWindowSampleWidth
    {
        get
        {
            return holdWindowRangeInSamples;
        }
    }



    // Access to the current hit window size in Unity units.
    public float PerfectWindowSizeInUnits
    {
        get
        {
            return noteSpeed * (perfectHitWindowRangeInMS * 0.001f);
        }
    }

    public float GreatWindowSizeInUnits
    {
        get
        {
            return noteSpeed * (greatHitWindowRangeInMS * 0.001f);
        }
    }

    public float GoodWindowSizeInUnits
    {
        get
        {
            return noteSpeed * (goodHitWindowRangeInMS * 0.001f);
        }
    }

    public float BadWindowSizeInUnits
    {
        get
        {
            return noteSpeed * (badHitWindowRangeInMS * 0.001f);
        }
    }

    public float HoldWindowInUnits
    {
        get
        {
            return noteSpeed * (holdHitWindowRangeInMS * 0.001f);
        }
    }



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

    #endregion
    #region Methods


    // Gets a cooy of an event
    KoreographyEvent GetEventCopy(KoreographyEvent evt2)
    {
        addedSample = 1;
        addedDouble = new KoreographyEvent();
        addedDouble.StartSample = evt2.StartSample + addedSample;
        addedDouble.EndSample = evt2.EndSample + addedSample;
        addedDouble.Payload = evt2.Payload;
        return addedDouble;
    }


    void Start()
    {
        saver = GameObject.FindGameObjectWithTag("Saver").GetComponent<Saver>();
        sm = GameObject.FindGameObjectWithTag("Score Manager").GetComponent<ScoreManager>();
        lc = GameObject.FindGameObjectWithTag("Lane").GetComponent<LaneController>();
        noteSpeed = saver.speedo;


        // Initialize all the Lanes.
        for (int i = 0; i < noteLanes.Count; ++i)
        {
            noteLanes[i].Initialize(this);
        }


        // Initialize events.
        playingKoreo = saver.mainTrack;
        simple.LoadSong(saver.mainTrack);

        // Grab all the events out of the Koreography.
        rhythmTrack = saver.difficulty;
        rawEvents = rhythmTrack.GetAllEvents();

        // For loop edits all the notes in the loop.
        for (int i = 0; i < rawEvents.Count; ++i)
        {
            KoreographyEvent evt = rawEvents[i];
            string payload = evt.GetTextValue();

            // If payload is 0 then it makes it a random lane.
            if (payload == "0")
            {
                payload = Random.Range(1, noteLanes.Count + 1).ToString();
                Debug.Log("Assigned random lane" + payload );
            }

            // Find the right lane.
            for (int j = 0; j < noteLanes.Count; ++j)
            {
                LaneController lane = noteLanes[j];

                if (lane.DoesMatchPayloadDouble1(payload))
                {
                    // Add the object for input tracking.
                    lane.AddEventToLane(evt);
                    sm.totalNotes++;
                }

                if (lane.DoesMatchPayloadDoubleEnd(payload))
                {
                    // Add the object for input tracking.
                    GetEventCopy(evt);
                    lane.AddEventToLane(addedDouble);
                    sm.totalNotes++;
                }

                if (lane.DoesMatchPayloadHold(payload))
                {
                    lane.AddEventToLane(evt);
                    sm.totalNotes++;
                }

                if (lane.DoesMatchPayloadHoldDouble1(payload))
                {
                    lane.AddEventToLane(evt);
                    sm.totalNotes++;
                }

                if (lane.DoesMatchPayloadHoldDouble2(payload))
                {
                    GetEventCopy(evt);
                    lane.AddEventToLane(addedDouble);
                    sm.totalNotes++;
                }


                if (lane.DoesMatchPayload(payload))
                {
                    // Add the object for input tracking.
                    lane.AddEventToLane(evt);
                    sm.totalNotes++;
                    break;
                }
            }

        }

        InitializeLeadIn();
        sm.perfectScore = sm.maxScore / sm.totalNotes;
        sm.greatScore = sm.perfectScore / 1.3f;
        sm.goodScore = sm.perfectScore / 2f;
        sm.badScore = sm.perfectScore / 3f;
        timeRemaining = Mathf.RoundToInt(songLength = audioCom.clip.length);

    }


  

    /*// To do something on each beats in the beats track - eg. move some animation etc.
    IEnumerator Beat()
    {

        yield return new WaitForSeconds(0.1f);

    }

    // Void of the new beat track
    public void AnimationBeat(KoreographyEvent beatevent)
    {
        StartCoroutine(Beat());

    }*/


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
        // This should be done in Start().  We do it here to allow for testing with Inspector modifications.
        UpdateInternalValues();

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

        // if audio is playing - countdown time.
        if (audioCom.isPlaying == true)
        {
                timeRemaining = Mathf.RoundToInt(songLength -= Time.deltaTime);
            ended = false;
        }

        if(audioCom.isPlaying == false && ended == false)
        {
            //endOfSong.OpenMenu(endMenu);
            ended = true;
        }


    }


    // Update any internal values that depend on externally accessible fields (public or Inspector-driven).
    void UpdateInternalValues()
    {
        perfectHitWindowRangeInSamples = (int)(0.001f * perfectHitWindowRangeInMS * SampleRate);
        greatHitWindowRangeInSamples = (int)(0.001f * greatHitWindowRangeInMS * SampleRate);
        goodHitWindowRangeInSamples = (int)(0.001f * goodHitWindowRangeInMS * SampleRate);
        badHitWindowRangeInSamples = (int)(0.001f * badHitWindowRangeInMS * SampleRate);
        holdWindowRangeInSamples = (int)(0.001f * holdHitWindowRangeInMS * SampleRate);
    }


    // Retrieves a frehsly activated Note Object from the pool.
    public NoteObject GetFreshNoteObject()
    {
        NoteObject retObj;

        if (noteObjectPool.Count > 0)
        {
            retObj = noteObjectPool.Pop();
        }
        else
        {
            retObj = Instantiate(noteObjectArchetype);
        }

        retObj.gameObject.SetActive(true);
        retObj.enabled = true;

        return retObj;
    }


    // Deactivates and returns a Note Object to the pool.
    public void ReturnNoteObjectToPool(NoteObject obj)
    {
        if (obj != null)
        {
            obj.enabled = false;
            obj.gameObject.SetActive(false);

            noteObjectPool.Push(obj);
        }
    }


    // Restarts the game, causing all Lanes and any active Note Objects to reset or otherwise clear.
    public void Restart()
    {
        // Reset the audio.
        audioCom.Stop();
        audioCom.time = 0f;

        // Flush the queue of delayed event updates.  This effectively resets the Koreography and ensures that
        //  delayed events that haven't been sent yet do not continue to be sent.
        Koreographer.Instance.FlushDelayQueue(playingKoreo);

        // Reset the Koreography time.  This is usually handled by loading the Koreography.  As we're simply
        //  restarting, we need to handle this ourselves.
        playingKoreo.ResetTimings();

        // Reset all the lanes so that tracking starts over.
        for (int i = 0; i < noteLanes.Count; ++i)
        {
            noteLanes[i].Restart();
        }

        // Reinitialize the lead-in-timing.
        InitializeLeadIn();
    }
    #endregion
}

