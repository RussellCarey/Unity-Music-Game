//----------------------------------------------
//            	   Koreographer                 
//    Copyright © 2014-2019 Sonic Bloom, LLC    
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using SonicBloom.Koreo;

[AddComponentMenu("Koreographer/Demos/Rhythm Game/Lane Controller")]
public class LaneController : MonoBehaviour
{
    #region Fields

    private LifeManger lm;
    public PlayTouchManager touchManager;
    public GameObject[] parts;
    public Color color = Color.white;
    public Transform noteSpawnPoint;
    public Transform targetPosition;
    private float spawnHeight;
    public Text comboText;

    private Saver saver;
    private ScoreManager scoreManager;
    [Tooltip("A reference to the visuals for the \"target\" location.")]
    public SpriteRenderer targetVisuals;

    int samplesToTarget;

    private string laneNumber;

    [Tooltip("A list of Payload strings that Koreography Events will contain for this Lane.")]
    public List<string> matchedPayloads = new List<string>();

    // The list that will contain all events in this lane.  These are added by the Rhythm Game Controller.
    public List<KoreographyEvent> laneEvents = new List<KoreographyEvent>();

    // A Queue that contains all of the Note Objects currently active (on-screen) within this lane.  Input and
    //  lifetime validity checks are tracked with operations on this Queue.
    public List<NoteObject> trackedNotes = new List<NoteObject>();

    // A reference to the Rythm Game Controller.  Provides access to the NoteObject pool and other parameters.
    RhythmGameController gameController;

    // Lifetime boundaries.  This game goes from the top of the screen to the bottom.
    float spawnX = 0f;
    float despawnY = 0f;
    float despawnXL = 0f;
    float despawnXR = 0f;

    // Index of the next event to check for spawn timing in this lane.
    int pendingEventIdx = 0;

    // Feedback Scales used for resizing the buttons on press.
    Vector3 defaultScale;
    float scaleNormal = 0.5f;
    float scalePress = 1.2f;
    float scaleHold = 0.7f;
    public bool pressed;
    public bool held;

    #endregion
    #region Properties


    // The position at which new Note Objects should spawn.
    public Vector3 SpawnPosition
    {
        get
        {
            return new Vector3(noteSpawnPoint.position.x, noteSpawnPoint.position.y, noteSpawnPoint.position.z);
        }
    }


    // The position at which the timing target exists.
    public Vector3 TargetPosition
    {
        get
        {
            return new Vector3(targetPosition.position.x, targetPosition.position.y, targetPosition.position.z);
        }
    }


    // The position at which Note Objects should despawn and return to the pool.
    public float DespawnY
    {
        get
        {
            return despawnY;
        }
    }

    public float DespawnXR
    {
        get
        {
            return despawnXR;
        }
    }

    public float DespawnXL
    {
        get
        {
            return despawnXL;
        }
    }


    #endregion
    #region Methods


    public void Initialize(RhythmGameController controller)
    {
        gameController = controller;
    }

    // This method controls cleanup, resetting the internals to a fresh state.
    public void Restart(int newSampleTime = 0)
    {
        // Find the index of the first event at or beyond the target sample time.
        for (int i = 0; i < laneEvents.Count; ++i)
        {
            if (laneEvents[i].StartSample >= newSampleTime)
            {
                pendingEventIdx = i;
                break;
            }
        }

        // Clear out the tracked notes.
        int numToClear = trackedNotes.Count;
        for (int i = 0; i < numToClear; ++i)
        {
            trackedNotes.RemoveAt(0);
        }
    }



    void Start()
    {
        scoreManager = GameObject.FindGameObjectWithTag("Score Manager").GetComponent<ScoreManager>();
        saver = GameObject.FindGameObjectWithTag("Saver").GetComponent<Saver>();
        lm = GameObject.FindGameObjectWithTag("Life Manager").GetComponent<LifeManger>();

        // Get the vertical bounds of the camera.  Offset by a bit to allow for offscreen spawning/removal.
        float cameraOffsetZ = -Camera.main.transform.position.z;

        spawnHeight = Vector3.Distance(noteSpawnPoint.transform.position, transform.position);
        spawnX = noteSpawnPoint.transform.position.x;
        despawnY = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, cameraOffsetZ)).y - 1f;
        despawnXR = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, cameraOffsetZ)).x + 1f;
        despawnXL = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, cameraOffsetZ)).x - 1f;

        // Update our visual color.
        targetVisuals.color = color;

        // Capture the default scale set in the Inspector.
        defaultScale = targetVisuals.transform.localScale;

    }



    public void tapMessage()
    {
        if(trackedNotes[0] != null)
        {
            if (trackedNotes[0].isTap == true || trackedNotes[0].isHold == true)
            {
                CheckNoteHit();
            }
            pressed = true;
        }
      
    }

    public void heldMessage()
    {
        if(trackedNotes[0] != null)
        {
            if (trackedNotes[0].isHold == true)
            {
                CheckNoteHit();
                held = true;
            }
        }
        

    }

    public void endMessage()
    {
        held = false;
        SetScaleDefault();

    }






    #region Editing notes code
    /*
    // When the button is pressed it adds a note at the current sample
    void AddNewEvent()
    {
        int sampleTime = Koreographer.Instance.GetMusicSampleTime();
        KoreographyEvent newEvent = new KoreographyEvent();
        newEvent.StartSample = sampleTime;
        newEvent.EndSample = sampleTime;
        TextPayload payload = new TextPayload();
        payload.TextVal = laneNumber;
        newEvent.Payload = payload;
        gameController.rhythmTrack.AddEvent(newEvent);
    }

    
    // When button is pressed it gets the closest 2 exsisting samples a assigns the closest one with an event
    void FindClosestEvent()
    {
        int currentSample = Koreographer.Instance.GetMusicSampleTime();
        for(int i = 0; i < gameController.rawEvents.Count; i++)
        {
            if(currentSample >= gameController.rawEvents[i].StartSample  && currentSample <= gameController.rawEvents[i+1].StartSample)
            {
                int middleSample = Mathf.RoundToInt((gameController.rawEvents[i].StartSample + gameController.rawEvents[i + 1].StartSample) / 2);

                if(currentSample >= middleSample)
                {
                    TextPayload payload = new TextPayload();
                    payload.TextVal = laneNumber;
                    gameController.rawEvents[i + 1].Payload = payload;
                }
                else
                {
                    TextPayload payload = new TextPayload();
                    payload.TextVal = laneNumber;
                    gameController.rawEvents[i].Payload = payload;
                }
            }
        }
    }

    // Moves all notes without a lane number (the unassigned notes) to 0 start samples to be easily destroyed
    public void EraseBlankNotes()
    {
        for (int i = 0; i < gameController.rawEvents.Count; i++)
        {
            if (gameController.rawEvents[i].Payload == null)
            {
                gameController.rawEvents[i].StartSample = 0;
                gameController.rawEvents[i].EndSample = 0;
            }
        }

        for (int i = 0; i < gameController.rawEvents.Count; i++)
        {
            if (gameController.rawEvents[i].StartSample == 0)
            {
                gameController.rawEvents.Remove(gameController.rawEvents[i]);
            }
        }
    
    }
    */
    #endregion

    // Just clears out unused notes
    void Update()
    {
        // Clear out invalid entries.
        while (trackedNotes.Count > 0 && trackedNotes[0].IsNoteMissed())
        {
            trackedNotes.RemoveAt(0);
            scoreManager.totalMisses++;
            scoreManager.combo = 0;
            lm.TakeLife();
        }

        // Check for new spawns.
        CheckSpawnNext();

        // Check for input.  Note that touch controls are handled by the Event System, which is all
        //  configured within the Inspector on the buttons themselves, using the same functions as
        //  what is found here.  Touch input does not have a built-in concept of "Held", so it is not
        //  currently supported.

    }


    // Adjusts the scale with a multiplier against the default scale.
    void AdjustScale(float multiplier)
    {
        targetVisuals.transform.localScale = defaultScale * multiplier;

    }


    // Uses the Target position and the current Note Object speed to determine the audio sample
    //  "position" of the spawn location.  This value is relative to the audio sample position at // --------------------- Speeds
    //  the Target position (the "now" time).

    int GetSpawnSampleOffset()
    {
        // Given the current speed, what is the sample offset of our current.
        float spawnDistToTargety = Vector3.Distance(noteSpawnPoint.position, transform.position);

        // At the current speed, what is the time to the location?
        double spawnSecsToTargety = (double)spawnDistToTargety / (double)gameController.noteSpeed;

        // Figure out the samples to the target.
        return (int)(spawnSecsToTargety * gameController.SampleRate);

    }


    // Checks if a Note Object is hit.  If one is, it will perform the Hit and remove the object
    //  from the trackedNotes Queue.
    public void CheckNoteHit()
    {
        if (trackedNotes.Count > 0 && trackedNotes[0].IsNoteHittableHold() && trackedNotes[0].isHold == true)
        {
            if (held == true)
            {

                NoteObject hitNote = trackedNotes[0];
                scoreManager.perfect++;
                scoreManager.combo++;
                scoreManager.totalHits++;
                scoreManager.score += (int)scoreManager.perfectScore;

                hitNote.OnHit();

                Instantiate(parts[0], this.gameObject.transform.position, Quaternion.identity);

                trackedNotes.RemoveAt(0);

                hitNote.gameObject.SetActive(false);


            }
        }

        if (trackedNotes.Count > 0 && trackedNotes[0].IsNoteHittablePerfect())
        {

            if (pressed == true)
            {
                NoteObject hitNote = trackedNotes[0];
                trackedNotes.RemoveAt(0);
                pressed = false;
                hitNote.OnHit();
                Instantiate(parts[0], this.gameObject.transform.position, Quaternion.identity);

                hitNote.gameObject.SetActive(false);

                scoreManager.perfect++;
                scoreManager.combo++;
                scoreManager.totalHits++;
                scoreManager.score += (int)scoreManager.perfectScore;
                lm.AddLifePerfect();


            }

        }

        if (trackedNotes.Count > 0 && trackedNotes[0].IsNoteHittableGreat())
        {

            if (pressed == true)
            {
                NoteObject hitNote = trackedNotes[0];
                trackedNotes.RemoveAt(0);
                pressed = false;

                Instantiate(parts[0], this.gameObject.transform.position, Quaternion.identity);

                hitNote.gameObject.SetActive(false);

                hitNote.OnHit();

                scoreManager.good++;
                scoreManager.combo++;
                scoreManager.totalHits++;
                scoreManager.score += (int)scoreManager.greatScore;
                lm.AddLifeGreat();

            }
        }


        if (trackedNotes.Count > 0 && trackedNotes[0].IsNoteHittableGood())
        {

            if (pressed == true)
            {
                NoteObject hitNote = trackedNotes[0];
                trackedNotes.RemoveAt(0);
                pressed = false;
                hitNote.OnHit();
                Instantiate(parts[0], this.gameObject.transform.position, Quaternion.identity);

                hitNote.gameObject.SetActive(false);

                scoreManager.ok++;
                scoreManager.combo++;
                scoreManager.totalHits++;
                scoreManager.score += (int)scoreManager.goodScore;
                lm.AddLifeGood();

            }
        }

        if (trackedNotes.Count > 0 && trackedNotes[0].IsNoteHittableBad())
        {
            if (pressed == true)
            {
                NoteObject hitNote = trackedNotes[0];
                trackedNotes.RemoveAt(0);
                pressed = false;
                hitNote.OnHit();
                Instantiate(parts[0], this.gameObject.transform.position, Quaternion.identity);

                hitNote.gameObject.SetActive(false);

                scoreManager.bad++;
                scoreManager.combo++;
                scoreManager.totalHits++;
                scoreManager.score += (int)scoreManager.badScore;
                lm.AddLifeBad();
            }
        }
    }

    public void Hit()
    {
        NoteObject hitNote = trackedNotes[0];
        trackedNotes.RemoveAt(0);
        hitNote.OnHit();
        //Instantiate(parts[0], this.gameObject.transform.position, Quaternion.identity);
    }


    // Checks if the next Note Object should be spawned.  If so, it will spawn the Note Object and
    //  add it to the trackedNotes Queue.


    void CheckSpawnNext()
    {
        samplesToTarget = GetSpawnSampleOffset();


        int currentTime = gameController.DelayedSampleTime;

        // Spawn for all events within range.
        while (pendingEventIdx < laneEvents.Count &&
         laneEvents[pendingEventIdx].StartSample < currentTime + samplesToTarget)
        {
            KoreographyEvent evt = laneEvents[pendingEventIdx];
            string pay = evt.GetTextValue();

            // Will always add at least one NoteObject.
            NoteObject newObj = gameController.GetFreshNoteObject();

            if (pay.Length <= 1)
            {
                newObj.Initialize(evt, this, gameController, 1);
                trackedNotes.Add(newObj);
                Debug.Log("Note with one digit added to lane");
            }
            else
            {

                if (pay.Contains("u") && pay.Length <= 2) //5
                {
                    newObj.Initialize(evt, this, gameController, 5);
                }

                if (pay.Contains("d") && pay.Length <= 2) //6
                {
                    newObj.Initialize(evt, this, gameController, 6);
                }

                if (pay.Contains("l") && pay.Length <= 2) //7
                {
                    newObj.Initialize(evt, this, gameController, 7);
                }

                if (pay.Contains("t") && pay.Length <= 2) //8
                {
                    newObj.Initialize(evt, this, gameController, 8);
                }



                if (pay.Contains("h") && pay.Length <= 2)
                {
                    newObj.Initialize(evt, this, gameController, 3);
                }

                if (pay.Contains("h") && pay.Length > 3)
                {

                    newObj.Initialize(evt, this, gameController, 3);
                }

                if (pay.Length == 2 && pay.Substring(1, 1) != "h")
                {
                    newObj.Initialize(evt, this, gameController, 2);
                }


                trackedNotes.Add(newObj);


            }

            pendingEventIdx++;
        }
    }


    // Adds a KoreographyEvent to the Lane.  The KoreographyEvent contains the timing information
    //  that defines when a Note Object should appear on screen.
    public void AddEventToLane(KoreographyEvent evt)
    {
        laneEvents.Add(evt);
    }



    // Checks to see if the string value passed in matches any of the configured values specified
    //  in the matchedPayloads List.
    public bool DoesMatchPayload(string payload)
    {
        bool bMatched = false;

        for (int i = 0; i < matchedPayloads.Count; ++i)
        {
            if (payload == matchedPayloads[i])
            {
                bMatched = true;
                Debug.Log(" In LC - Has matched = true");
                break;
            }
        }
        return bMatched;
    }



    public bool DoesMatchPayloadDouble1(string payload)
    {
        bool bMatched = false;

        if (payload.Length > 1)
        {
            for (int i = 0; i < matchedPayloads.Count; ++i)
            {
                if ((payload.Substring(0, 1)) == matchedPayloads[i])
                {
                    bMatched = true;
                    break;
                }
            }
        }
        return bMatched;
    }



    public bool DoesMatchPayloadDoubleEnd(string payload)
    {
        bool bMatched = false;

        if (payload.Length > 1)
        {
            for (int i = 0; i < matchedPayloads.Count; ++i)
            {
                if ((payload.Substring(1, 1)) == matchedPayloads[i])
                {
                    bMatched = true;
                    break;
                }
            }
        }
        return bMatched;
    }



    public bool DoesMatchPayloadHold(string payload)
    {
        bool bMatched = false;

        for (int i = 0; i < matchedPayloads.Count; ++i)
        {
            if (payload.Length == 2 && (payload.Substring(0, 1)) == matchedPayloads[i] && (payload.Substring(1, 1)) == "h")
            {
                bMatched = true;
                break;
            }
        }
        return bMatched;
    }

    //------

    public bool DoesMatchPayloadUp(string payload)
    {
        bool bMatched = false;

        for (int i = 0; i < matchedPayloads.Count; ++i)
        {
            if (payload.Length == 2 && (payload.Substring(0, 1)) == matchedPayloads[i] && (payload.Substring(1, 1)) == "u")
            {
                bMatched = true;
                break;
            }
        }
        return bMatched;
    }

    public bool DoesMatchPayloadDown(string payload)
    {
        bool bMatched = false;

        for (int i = 0; i < matchedPayloads.Count; ++i)
        {
            if (payload.Length == 2 && (payload.Substring(0, 1)) == matchedPayloads[i] && (payload.Substring(1, 1)) == "d")
            {
                bMatched = true;
                break;
            }
        }
        return bMatched;
    }

    public bool DoesMatchPayloadLeft(string payload)
    {
        bool bMatched = false;

        for (int i = 0; i < matchedPayloads.Count; ++i)
        {
            if (payload.Length == 2 && (payload.Substring(0, 1)) == matchedPayloads[i] && (payload.Substring(1, 1)) == "l")
            {
                bMatched = true;
                break;
            }
        }
        return bMatched;
    }

    public bool DoesMatchPayloadRight(string payload)
    {
        bool bMatched = false;

        for (int i = 0; i < matchedPayloads.Count; ++i)
        {
            if (payload.Length == 2 && (payload.Substring(0, 1)) == matchedPayloads[i] && (payload.Substring(1, 1)) == "r")
            {
                bMatched = true;
                break;
            }
        }
        return bMatched;
    }

    //-------


    public bool DoesMatchPayloadHoldDouble1(string payload)
    {
        bool bMatched = false;

        for (int i = 0; i < matchedPayloads.Count; ++i)
        {
            if (payload.Length == 4 && (payload.Substring(0, 1)) == matchedPayloads[i] && (payload.Substring(1, 1)) == "h")
            {
                bMatched = true;
                break;
            }
        }
        return bMatched;
    }


    public bool DoesMatchPayloadHoldDouble2(string payload)
    {
        bool bMatched = false;

        for (int i = 0; i < matchedPayloads.Count; ++i)
        {
            if (payload.Length == 4 && (payload.Substring(2, 1)) == matchedPayloads[i] && (payload.Substring(3, 1)) == "h")
            {
                bMatched = true;
                break;
            }
        }
        return bMatched;
    }





    // Sets the Target scale to the original default scale.
    public void SetScaleDefault()
    {
        AdjustScale(scaleNormal);
    }


    // Sets the Target scale to the specified "initially pressed" scale.
    public void SetScalePress()
    {
        AdjustScale(scalePress);
    }


    // Sets the Target scale to the specified "continuously held" scale.
    public void SetScaleHold()
    {
        AdjustScale(scaleHold);
    }

    #endregion
}

