using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//リスポーン位置更新オブジェクトのエフェクト再生、表示非表示
public class ReSpawnPoint : MonoBehaviour
{
    [SerializeField] BoxCollider boxcol;
    [SerializeField] TextMeshProUGUI actionpop;
    private float spawnEffectTime = 2;
    private float pause = 1;
    public AnimationCurve fadeIn;
    private float timer = 0;
    private int shaderProperty;
    private ParticleSystem ps;
    private PlayerController playerController;
    private new Renderer renderer;

    private void Start()
    {
        shaderProperty = Shader.PropertyToID("_cutoff");
        renderer = GetComponent<Renderer>();
        ps = GetComponentInChildren<ParticleSystem>();
        playerController = FindObjectOfType<PlayerController>();
        var main = ps.main;
        main.duration = spawnEffectTime;
        ps.Play();
    }

    private void Update()
    {
        if(playerController.Duplicate_isDead && playerController != null)
        {
            boxcol.enabled = false;
        }
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
            actionpop.text = "チェックポイントの更新を行いました";
            StartCoroutine("SetText");
        }
    }
    IEnumerator SetText()
    {
        yield return new WaitForSeconds(2.0f);
        actionpop.text = "";
        gameObject.SetActive(false);
    }
}
