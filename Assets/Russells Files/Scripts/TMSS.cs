using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TMSS : MonoBehaviour
{
    Vector2[] movementStart;
    Vector2 mouseStart;
    Vector2 mouseEnd;

    public GameObject settings;

    public DifficultyButton diff;
    

    void Start()
    {
    }

    public void DiffUp()
    {
        diff.difficultyNo ++;
    }

    public void DiffDown()
    {
        diff.difficultyNo -= 1;
    }

    public void EasySelect()
    {
        diff.difficultyNo = 0;
    }

    public void MedSelect()
    {
        diff.difficultyNo = 1;
    }

    public void HardSelect()
    {
        diff.difficultyNo = 2;
    }

    public void OpenSettings()
    {
        settings.SetActive(true);
    }

    void Update()
    {
        // Mobile Controls to use for testing on the computer
        for (int i = 0; i < Input.touchCount; ++i)
        {
            Vector2 test = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
            RaycastHit2D hit = Physics2D.Raycast(test, (Input.GetTouch(i).position));
            test = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);

            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                //movementStart[i] = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);

                if (hit.collider != null && hit.collider.tag == "easy")
                {
                    diff.difficultyNo = 0;
                }

                if (hit.collider != null && hit.collider.tag == "medium")
                {
                    diff.difficultyNo = 1;
                }

                if (hit.collider != null && hit.collider.tag == "hard")
                {
                    diff.difficultyNo = 2;
                }


            }
            if (Input.GetTouch(i).phase == TouchPhase.Ended)
            {
           
            }
        }




        // Mouse Controls to use for testing on the computer
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            mouseStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("pressed the mouse");
            if (Physics.Raycast(ray, out hit, 100))
            {
                

                if (hit.collider != null && hit.collider.tag == "easy")
                {
                    diff.difficultyNo = 0;
                }

                if (hit.collider != null && hit.collider.tag == "medium")
                {
                    diff.difficultyNo = 1;
                }

                if (hit.collider != null && hit.collider.tag == "hard")
                {
                    diff.difficultyNo = 2;
                }

                if (hit.collider != null && hit.collider.tag == "settings")
                {
                    diff.difficultyNo = 2;
                }

                if (hit.collider != null && hit.collider.tag == "diffup")
                {
                    diff.difficultyNo++;
                }

                if (hit.collider != null && hit.collider.tag == "diffdown")
                {
                    diff.difficultyNo -= 1;
                }

                if (hit.collider != null && hit.collider.tag == "settings")
                {
                    settings.SetActive(true);
                    
                }

                if (hit.collider != null && hit.collider.tag == "back")
                {
                    settings.SetActive(false);
                }
            }
        }
    }
}

