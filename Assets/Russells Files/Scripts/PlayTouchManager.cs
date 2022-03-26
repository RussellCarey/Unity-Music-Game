using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayTouchManager : MonoBehaviour
{
    public Text debug;
    private Vector2 startPos;
    private Vector2 endPos;
    public LaneController leftLC;
    public LaneController rightLC;

    void Update()
    {

        for (int i = 0; i < Input.touchCount; ++i)
        {
            Vector2 test = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
            RaycastHit2D hit = Physics2D.Raycast(test, (Input.GetTouch(i).position));
            test = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
            


            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                startPos = Input.GetTouch(i).position;
                if (hit.collider != null && hit.collider.tag == "Button")
                {
                    LaneController lc = hit.collider.GetComponent<ButtonScript>().lc;
                    lc.tapMessage();
                }

            }



            if (Input.GetTouch(i).phase == TouchPhase.Stationary)
            {
                if (hit.collider != null && hit.collider.tag == "Button")
                {
                    LaneController lc = hit.collider.GetComponent<ButtonScript>().lc;
                    lc.heldMessage();
                }
            }


            if (Input.GetTouch(i).phase == TouchPhase.Moved)
            {
                endPos = Input.GetTouch(i).position;
                LaneController lc = hit.collider.GetComponent<ButtonScript>().lc;


                endPos = Input.GetTouch(i).position;

                if (startPos.y < endPos.y)
                {
                    debug.text = ("UPP");
                    lc = hit.collider.GetComponent<ButtonScript>().lc;
                    if (lc.trackedNotes[0].isUp == true)
                    {
                        lc.CheckNoteHit();
                        lc.pressed = true;
                    }

                }
                if (startPos.y > endPos.y)
                {
                    debug.text = ("DOWN");
                    lc = hit.collider.GetComponent<ButtonScript>().lc;
                    if (lc.trackedNotes[0].isDown == true)
                    {
                        lc.CheckNoteHit();
                        lc.pressed = true;
                    }
                }
            }

            if (Input.GetTouch(i).phase == TouchPhase.Ended)
            {


            }
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            leftLC.tapMessage();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            rightLC.tapMessage();
        }

    }



}

