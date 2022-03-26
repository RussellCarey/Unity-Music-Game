using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeManger : MonoBehaviour
{
    private Saver s;
    public ZUIManager menuManager;
    public Menu deathMenu;
    public float totalLife = 100;
    public ScoreManager scoreM;
    public Text scoreFailed;
    private float add;
    private float take;


    // Need some sort of life bar or indicator..


    private void Start()
    {
        s = GameObject.FindGameObjectWithTag("Saver").GetComponent<Saver>();
        add = s.addL;
        take = s.takeL;
    }


    private void Update()
    {
        if(s.scoring == true)
        {
            if (totalLife < 0)
            {
                Die();
            }
        }

    }


    public void AddLifePerfect()
    {
        // On hit a note add life back--
        totalLife += add;

    }

    public void AddLifeGreat()
    {
        // On hit a note add life back--
        totalLife += add/2;

    }

    public void AddLifeGood()
    {
        // On hit a note add life back--
        totalLife += add/3;

    }

    public void AddLifeBad()
    {
        // On hit a note add life back--
        totalLife += add/4;

    }

    public void TakeLife()
    {
        // On miss a note take life-- Dificulty changes amount taken
        totalLife -= take;
    }

    public void Die()
    {
        scoreM.failed = true;
    }

}
