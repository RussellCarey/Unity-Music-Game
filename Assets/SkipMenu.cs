using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipMenu : MonoBehaviour
{
    public ZUIManager zManager;
    public Menu zMenu;
    private MenuSwitch ms;

    // Start is called before the first frame update

    private void Awake()
    {
        ms = GameObject.FindGameObjectWithTag("MenuSaver").GetComponent<MenuSwitch>();
    }

    public void Start()
    {
        MenuSkip();
    }

    public void MenuSkip()
    {
        ms = GameObject.FindGameObjectWithTag("MenuSaver").GetComponent<MenuSwitch>();

        if (ms != null)
        {
            if (ms.hasStarted == true)
            {
                zManager.OpenMenu(zMenu);
            }

        }
    }

}
