using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDistanceSC : MonoBehaviour
{
    [SerializeField] List<GameObject> laserobjectList;
    private const float destroyDistance = 100.0f;
    //Razerオブジェクトをリストに登録し
    //距離に応じてオブジェクトを非表示にする。処理負荷軽減の為。
    private void Update()
    {
        Check_Distance();
    }
    private void Check_Distance()
    {
        foreach(var obj in laserobjectList)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);

            if(distance > destroyDistance && obj.activeSelf)
            {
                obj.gameObject.SetActive(false);
            }
            else if(distance < destroyDistance && !obj.activeSelf)
            {
                obj.gameObject.SetActive(true);
            }

        }
    }
}