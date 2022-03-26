using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TM : MonoBehaviour
{
    Vector2[] movementStart;



    void Start()
    {
    }


    void Update()
    {
        // Mobile Controls to use for testing on the computer
        for (int i = 0; i < Input.touchCount; ++i)
        {
            Vector2 test = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
            RaycastHit2D hit = Physics2D.Raycast(test, (Input.GetTouch(i).position));
            test = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);

            if (Input.GetTouch(i).phase == TouchPhase.Began || Input.GetMouseButtonDown(0))
            {
                //movementStart[i] = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);

                if (hit.collider != null && hit.collider.tag == "t1")
                {
                    SceneManager.LoadScene("Song Select");
                }

                if (hit.collider != null && hit.collider.tag == "t2")
                {
                    // use the facebook page ID rather than the name and it should open in the app? Page ID
                    Application.OpenURL("www.twitter.com/OucoGames");
                }

            }

        }




        // Mouse Controls to use for testing on the computer
        /*if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider != null && hit.collider.tag == "t1")
                {
                    SceneManager.LoadScene("Song Select");
                }

                if (hit.collider != null && hit.collider.tag == "t2")
                {
                    // use the facebook page ID rather than the name and it should open in the app? Page ID
                    Application.OpenURL("www.twitter.com/OucoGames");
                }
            }
        }*/
    }
}

