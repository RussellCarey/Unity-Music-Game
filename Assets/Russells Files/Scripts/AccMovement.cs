using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        float movement = Input.acceleration.x;

        transform.position += new Vector3(Mathf.Clamp(movement, -20, 20), 0, 0);

    }
}
