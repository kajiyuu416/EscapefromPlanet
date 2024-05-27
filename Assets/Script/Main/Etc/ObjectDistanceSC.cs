using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDistanceSC : MonoBehaviour
{
    [SerializeField] List<GameObject> ObjectList;
    private const float DestroyDistance = 100.0f;
    //Razerオブジェクトをリストに登録し
    //距離に応じてオブジェクトを非表示にする。処理負荷軽減の為。
    private void Update()
    {
        Check_Distance();
    }
    private void Check_Distance()
    {
        foreach(var obj in ObjectList)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);

            if(distance > DestroyDistance && obj.activeSelf)
            {
                obj.gameObject.SetActive(false);
            }
            else if(distance < DestroyDistance && !obj.activeSelf)
            {
                obj.gameObject.SetActive(true);
            }

        }
    }
}