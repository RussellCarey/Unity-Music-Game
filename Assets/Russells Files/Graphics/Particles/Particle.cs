using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{

    private ParticleSystem particle;

    private void Start()
    {
        particle = GetComponent<ParticleSystem>();
        //particle.
    }
    void Finished()
    {
        if(particle.isStopped == true)
        {
            Destroy(this.gameObject);
        }
    }
}
