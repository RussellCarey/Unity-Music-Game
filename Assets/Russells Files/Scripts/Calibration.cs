using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SonicBloom.Koreo;

public class Calibration : MonoBehaviour
{

    public Koreographer kore;


    void Start()
    {
        kore.EventDelayInSeconds = PlayerPrefs.GetFloat("Cali");
    }
     

}