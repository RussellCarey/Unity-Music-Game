using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongScroll : MonoBehaviour
{
    public int songNumber = 1;
    public GameObject songs;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow) && songNumber > 1)
        {
            songs.transform.position = songs.transform.position + new Vector3(3, 0, 0);
            songNumber -=1;
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow) && songNumber <= 5)
        {
            songs.transform.position = songs.transform.position + new Vector3(-3, 0, 0);
            songNumber += 1;
        }

        if(songNumber < 1)
        {
            songNumber = 1;
        }
    }
}
