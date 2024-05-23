using UnityEngine;

//プレイヤーの子オブジェクト（各パーツ）に使用
//ゲームオーバー時にプレイヤー本体は非表示にし
//子オブジェクト（各パーツ）を表示、レーザーに降れば部分はランダムな方向に力を加え飛ばされるような表現を実装
public class Child : MonoBehaviour
{
    private float Speed = 20.0f;
    private float time;
    private float vecX;
    private float vecY;
    private float vecZ;
    private Vector3 initialPosition;
    public GameObject particleObject;
    public PlayerController playerController;

    private MeshRenderer meshRenderer;
    private new Rigidbody rigidbody;
    private BoxCollider boxCollider;

    private void Start()
    {
        time = 1.0f;
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void Update()
    {
        time -= Time.deltaTime;
        initialPosition = gameObject.transform.position;
    }
    public void On()
    {
        meshRenderer.enabled = true;
    }
    private void OnTriggerEnter(Collider triger)
    {
        if(triger.CompareTag("laser"))
        {
            boxCollider.isTrigger = false;
            Instantiate(particleObject,this.transform.position,Quaternion.identity);
            rigidbody.constraints = RigidbodyConstraints.None;
            this.gameObject.transform.parent = null;
            playerController.PlayerDeath();
            if (time <= 0.0f)
            {
                vecX = Random.Range(-5, 5);
                vecY = Random.Range(2, 5);
                vecZ = Random.Range(-5, 5);
                transform.position = new Vector3(vecX, vecY, vecZ);
                transform.position = initialPosition;
                rigidbody.velocity = transform.forward.normalized * Speed;
                time = Const.CO.Const_Float_List[3];
            }
        }
     
    }

}

