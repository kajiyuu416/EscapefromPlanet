using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//レーザーの移動処理
//インスペクターで指定したselectnumberの行動を行う
public class LazerController : MonoBehaviour
{
    private bool isResetFlag = false;
    private string secondtagname;
    public float movespeed;
    public float time = 0.0f;
    public Transform target1;
    public Transform target2;
    public float rotatex = 0;
    public float rotatey = 0;
    public float rotatez = 0;
    public float OT;
    public float IO;
    public float IT;

    private int Direction = 1;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation; 
    public MeshRenderer LSchild;
    private MeshRenderer meshRenderer;
    private BoxCollider boxCollider;

    public enum Selectnumber
    {
        Zero, One, Two, Three, Four, Five, Six, Seven
    }
    public Selectnumber sn;
    private void Start()
    {
        //初期座標、初期角度の取得
        _initialPosition = gameObject.transform.position;
        _initialRotation = gameObject.transform.rotation;

        secondtagname = "ResetPoint";
        if (sn == Selectnumber.Six)
        {
            boxCollider = GetComponent<BoxCollider>();
            meshRenderer = GetComponent<MeshRenderer>();

            foreach (MeshRenderer child in LSchild.GetComponentsInChildren<MeshRenderer>())
            {
                if (child.gameObject == gameObject) continue;
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.tag == secondtagname)
        {
            isResetFlag = true;
            if (isResetFlag)
            {
                Reset();
            }
        }

    }
    private void Update()
    {
  
    }
    private void FixedUpdate()
    {
        RazerMoves();
    }
    public void Reset()
    {
        gameObject.transform.position = _initialPosition;
        gameObject.transform.rotation = _initialRotation;
    }

    //レーザーオブジェクトの移動、回転処理
    //sn.Zero:動作なし sn.One:ターゲットオブジェクトに向かって移動　sn.Two:ターゲットオブジェクトのｘ軸に向かって反復移動　sn.Three:ターゲットオブジェクトのy軸に向かって反復移動
    //sn.Four:ターゲットオブジェクトのz軸に向かって反復移動 sn.Five:インスペクターで指定した量の回転をレーザーオブジェクトにかける sn.Six:一定時間後に表示、一定時間後に非表示、表示される前の予兆風表示
    //sn.Seven:ターゲットに向かって回転しながら移動
    private void RazerMoves()
    {
        if (sn == Selectnumber.Zero)
        {
            transform.position += Vector3.zero;
        }
        else if (sn == Selectnumber.One)
        {
          
            transform.position = Vector3.MoveTowards(transform.position, target1.position, movespeed * Time.deltaTime);
        }
        else if (sn == Selectnumber.Two)
        {
     
            transform.position = new Vector3(transform.position.x + movespeed * Time.fixedDeltaTime * Direction, _initialPosition.y, _initialPosition.z);
            if (transform.position.x >= target2.position.x) Direction = -1;
            if (transform.position.x <= target1.position.x) Direction = 1;
        }
        else if (sn == Selectnumber.Three)
        {
            transform.position = new Vector3(_initialPosition.x, transform.position.y + movespeed * Time.fixedDeltaTime * Direction, _initialPosition.z);
            if (transform.position.y >= target2.position.y) Direction = -1;
            if (transform.position.y <= target1.position.y) Direction = 1;

        }
        else if (sn == Selectnumber.Four)
        {
   
            transform.position = new Vector3(_initialPosition.x, _initialPosition.y, transform.position.z + movespeed * Time.fixedDeltaTime * Direction);
            if (transform.position.z >= target2.position.z) Direction = -1;
            if (transform.position.z <= target1.position.z) Direction = 1;

        }
        else if (sn == Selectnumber.Five)
        {
          
            gameObject.transform.Rotate(new Vector3(rotatex, rotatey, rotatez) * Time.deltaTime);
        }
        else if (sn == Selectnumber.Six)
        {
            time += Time.deltaTime;
            if (time > OT)
            {
               
                meshRenderer.enabled = false;
                boxCollider.enabled = false;
            }
            if(time > IO)
            {
                LSchild.enabled = true;
            }
            if(time > IT)
            {
                meshRenderer.enabled = true;
                boxCollider.enabled = true;
                LSchild.enabled = false;
                time = 0;
            }
        }
        else if (sn == Selectnumber.Seven)
        {
            transform.position = new Vector3(transform.position.x + movespeed * Time.fixedDeltaTime * Direction, _initialPosition.y, _initialPosition.z);
            if (transform.position.x >= target2.position.x) Direction = -1;
            if (transform.position.x <= target1.position.x) Direction = 1;
            gameObject.transform.Rotate(new Vector3(rotatex, rotatey, rotatez) * Time.deltaTime);
        }
    }
}
    

