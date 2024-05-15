using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDistanceSC : MonoBehaviour
{
    [SerializeField] List<GameObject> ObjectList;
    public float DestroyDistance;

    //距離に応じてオブジェクトを非表示にする。処理負荷軽減の為。
    private void Update()
    {
        float Razerdis1 = Vector3.Distance(this.transform.position, ObjectList[0].transform.position);
        float Razerdis2 = Vector3.Distance(this.transform.position, ObjectList[1].transform.position);
        float Razerdis3 = Vector3.Distance(this.transform.position, ObjectList[2].transform.position);
        float Razerdis4 = Vector3.Distance(this.transform.position, ObjectList[3].transform.position);
        float Razerdis5 = Vector3.Distance(this.transform.position, ObjectList[4].transform.position);
        float Razerdis6 = Vector3.Distance(this.transform.position, ObjectList[5].transform.position);
        if(Razerdis1 > DestroyDistance)
        {
            ObjectList[0].SetActive(false);
        }
        else if (Razerdis1 < DestroyDistance)
        {
            ObjectList[0].SetActive(true);
        } 
        
        if(Razerdis2 > DestroyDistance)
        {
            ObjectList[1].SetActive(false);
        }
        else if (Razerdis2 < DestroyDistance)
        {
            ObjectList[1].SetActive(true);
        }  
        
        if(Razerdis3 > DestroyDistance)
        {
            ObjectList[2].SetActive(false);
        }
        else if (Razerdis3 < DestroyDistance)
        {
            ObjectList[2].SetActive(true);
        }

        if(Razerdis4 > DestroyDistance)
        {
            ObjectList[3].SetActive(false);
        }
        else if(Razerdis4 < DestroyDistance)
        {
            ObjectList[3].SetActive(true);
        }

        if(Razerdis5 > DestroyDistance)
        {
            ObjectList[4].SetActive(false);
        }
        else if(Razerdis5 < DestroyDistance)
        {
            ObjectList[4].SetActive(true);
        }

        if(Razerdis6 > DestroyDistance)
        {
            ObjectList[5].SetActive(false);
        }
        else if(Razerdis6 < DestroyDistance)
        {
            ObjectList[5].SetActive(true);
        }

    }
}