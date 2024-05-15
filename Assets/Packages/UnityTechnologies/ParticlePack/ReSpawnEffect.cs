using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSpawnEffect : MonoBehaviour
{

    public float spawnEffectTime = 2;
    ParticleSystem ps;
    Renderer _renderer;

    int shaderProperty;

    void Start()
    {
        shaderProperty = Shader.PropertyToID("_cutoff");
        _renderer = GetComponent<Renderer>();
        ps = GetComponentInChildren<ParticleSystem>();

        var main = ps.main;
        main.duration = spawnEffectTime;

        ps.Play();

    }

}
