using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class ReSpawnPoint : MonoBehaviour
{
    public float spawnEffectTime = 2;
    public float pause = 1;
    private float timer = 0;
    public AnimationCurve fadeIn;
    private int shaderProperty;
    [SerializeField] TextMeshProUGUI AP;
    ParticleSystem ps;
    new Renderer renderer;

    private void Awake()
    {

    }
    private void Start()
    {
        shaderProperty = Shader.PropertyToID("_cutoff");
        renderer = GetComponent<Renderer>();
        ps = GetComponentInChildren<ParticleSystem>();
        var main = ps.main;
        main.duration = spawnEffectTime;
        ps.Play();
    }

    private void Update()
    {
        if (timer < spawnEffectTime + pause)
        {
            timer += Time.deltaTime;
        }
        else
        {
            ps.Play();
            timer = 0;
        }
        renderer.material.SetFloat(shaderProperty, fadeIn.Evaluate(Mathf.InverseLerp(0, spawnEffectTime, timer)));
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            AP.text = "チェックポイントの更新を行いました";
            StartCoroutine("SetText");
        }
    }
    IEnumerator SetText()
    {
        yield return new WaitForSeconds(2.0f);
        AP.text = "";
        gameObject.SetActive(false);
    }
}
