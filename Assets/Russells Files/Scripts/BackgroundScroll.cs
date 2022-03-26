using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    bool lr;
    Vector3 startPos;
    Vector3 endPos;
    public float speed;
    public GameObject bgImage;

    // Start is called before the first frame update
    void Start()
    {
        lr = true;
        startPos = bgImage.transform.position;
        endPos = new Vector3(0.83f, -3f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(lr == true)
        {
            Vector3 pos = bgImage.transform.position;
            pos.x += (Time.deltaTime * speed);
            bgImage.transform.position = pos;

            if(bgImage.transform.position.x >= endPos.x)
            {
                lr = false;
            }
        }


        if(lr == false)
        {
            Vector3 pos = bgImage.transform.position;
            pos.x -= (Time.deltaTime * speed);
            bgImage.transform.position = pos;

            if (bgImage.transform.position.x <= startPos.x)
            {
                lr = true;
            }
        }
    }
}
