using UnityEngine;
public class ObjctRotate : MonoBehaviour
{
    // 中心点centerの周りを、軸axisで、period周期で円運動
    [SerializeField] Vector3 center = Vector3.zero;
    [SerializeField]Vector3 axis = Vector3.up;
    [SerializeField] float period = 2;
    private void Update()
    {
        transform.RotateAround( center,axis, 360 / period * Time.deltaTime);
    }
}
