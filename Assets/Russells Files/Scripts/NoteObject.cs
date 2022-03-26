using UnityEngine;
using SonicBloom.Koreo;

[AddComponentMenu("Koreographer/Demos/Rhythm Game/Note Object")]
public class NoteObject : MonoBehaviour
{
    #region Fields

    [Tooltip("The visual to use for this Note Object.")]
    public SpriteRenderer visuals;
    public Sprite noteObject;
    public Sprite holdObject;
    public Sprite swipeCenter;
    public bool isTap;
    public bool isHold;
    public bool isUp;
    public bool isDown;
    public bool isLeft;
    public bool isRight;
    private Saver saver;
    public SpriteRenderer center;
    public int noteType;

    public Color leftPressColor;
    public Color rightPressColor;
    public Color holdColor;
    public Color doubleColor;
    public Color swipeColor;

    // If active, the KoreographyEvent that this Note Object wraps.  Contains the relevant timing information.
    KoreographyEvent trackedEvent;

    // If active, the Lane Controller that this Note Object is contained by.
    public LaneController laneController;

    // If active, the Rhythm Game Controller that controls the game this Note Object is found within.
    RhythmGameController gameController;

    #endregion
    #region Static Methods

    // Unclamped Lerp.  Same as Vector3.Lerp without the [0.0-1.0] clamping.
    static Vector3 Lerp(Vector3 from, Vector3 to, float t)
    {
        return new Vector3(from.x + (to.x - from.x) * t, from.y + (to.y - from.y) * t, from.z + (to.z - from.z) * t);
    }

    #endregion
    #region Methods


    // Prepares the Note Object for use.
    public void Initialize(KoreographyEvent evt, LaneController laneCont, RhythmGameController gameCont, int type)
    {
        trackedEvent = evt;
        laneController = laneCont;
        gameController = gameCont;
        saver = GameObject.FindGameObjectWithTag("Saver").GetComponent<Saver>();


        if (type == 1)
        {
            isTap = true;
            if(laneController.name == "Left")
            {
                center.color = leftPressColor;
            }
            if (laneController.name == "Right")
            {
                center.color = rightPressColor;
            }

            isHold = false;
        }

        if (type == 2)
        {
            center.color = doubleColor;
            isHold = false;
        }

        if (type == 3)
        {
            isHold = true;
            center.color = holdColor;
        }

        if (type == 4)
        {
            visuals.sprite = holdObject;
            center.color = holdColor;
            isHold = true;
            center = null;

        }



        if (type == 5)
        {
            isUp = true;
            center.color = swipeColor;
            center.sprite = swipeCenter;
            isHold = false;
            noteType = 5;
        }

        if (type == 6)
        {
            isDown = true;
            center.color = doubleColor;
            isHold = false;
            noteType = 6;
        }

        if (type == 7)
        {
            isLeft = true;
            center.color = doubleColor;
            isHold = false;
            noteType = 7;
        }

        if (type == 8)
        {
            isRight = true;
            center.color = doubleColor;
            isHold = false;
            noteType = 8;
        }


        UpdatePosition();
    }

    // Resets the Note Object to its default state.
    void Reset()
    {
        trackedEvent = null;
        laneController = null;
        gameController = null;
    }

    void Start()
    {
        saver = GameObject.FindGameObjectWithTag("Saver").GetComponent<Saver>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<RhythmGameController>();

    }


    void Update()
    {
        UpdatePosition();

        if (transform.name.Contains("L") && transform.position.x <= laneController.DespawnXL)
        {
            gameController.ReturnNoteObjectToPool(this);
            Reset();
        }

        if (transform.name.Contains("R") && transform.position.x >= laneController.DespawnXR)
        {
            gameController.ReturnNoteObjectToPool(this);
            Reset();
        }

    }



    // Updates the position of the Note Object along the Lane based on current audio position.
    void UpdatePosition()
    {
        // Calculate the rate
        float samplesPerUnit = gameController.SampleRate / gameController.noteSpeed;

        // Calculate a direction vector from end position to start position
        Vector3 dir = laneController.TargetPosition - laneController.SpawnPosition;

        // Normalize the direction vector
        dir.Normalize();

        // Calculate the distance the NoteObject has to travel
        float distanceLeft = (gameController.DelayedSampleTime - trackedEvent.StartSample) / samplesPerUnit;

        // Scale the direction vector by the distance the NoteObject has left
        //  to travel
        Vector3 offset = dir * distanceLeft;

        // Position this NoteObject to a location indicated by the offset
        //  from the target location
        transform.position = laneController.TargetPosition + offset;
    }






    // Checks to see if the Note Object is currently hittable or not based on current audio sample
    //  position and the configured hit window width in samples (this window used during checks for both
    //  before/after the specific sample time of the Note Object).

    public bool IsNoteHittableHold()
    {
        int noteTime = trackedEvent.StartSample;
        int curTime = gameController.DelayedSampleTime;
        int hitWindow = gameController.holdWindowSampleWidth;

        return (Mathf.Abs(noteTime - curTime) <= hitWindow);
    }

    public bool IsNoteHittablePerfect()
    {
        int noteTime = trackedEvent.StartSample;
        int curTime = gameController.DelayedSampleTime;
        int hitWindow = gameController.perfectHitWindowSampleWidth;

        return (Mathf.Abs(noteTime - curTime) <= hitWindow);
    }

    public bool IsNoteHittableGreat()
    {
        int noteTime = trackedEvent.StartSample;
        int curTime = gameController.DelayedSampleTime;
        int hitWindow = gameController.greatHitWindowSampleWidth;

        return (Mathf.Abs(noteTime - curTime) <= hitWindow);
    }

    public bool IsNoteHittableGood()
    {
        int noteTime = trackedEvent.StartSample;
        int curTime = gameController.DelayedSampleTime;
        int hitWindow = gameController.goodHitWindowSampleWidth;

        return (Mathf.Abs(noteTime - curTime) <= hitWindow);
    }

    public bool IsNoteHittableBad()
    {
        int noteTime = trackedEvent.StartSample;
        int curTime = gameController.DelayedSampleTime;
        int hitWindow = gameController.badHitWindowSampleWidth;

        return (Mathf.Abs(noteTime - curTime) <= hitWindow);
    }


    public bool IsNoteMissed()
    {
        bool bMissed = true;

        if (enabled)
        {
            int noteTime = trackedEvent.StartSample;
            int curTime = gameController.DelayedSampleTime;
            int hitWindow = gameController.badHitWindowSampleWidth;

            bMissed = (curTime - noteTime > hitWindow);
        }

        return bMissed;
    }





    // Returns this Note Object to the pool which is controlled by the Rhythm Game Controller.  This
    //  helps reduce runtime allocations.
    void ReturnToPool()
    {
        gameController.ReturnNoteObjectToPool(this);
        Reset();
    }


    // Performs actions when the Note Object is hit.
    public void OnHit()
    {
        ReturnToPool();

    }


    // Performs actions when the Note Object is cleared.
    public void OnClear()
    {
        ReturnToPool();
    }

    #endregion
}

