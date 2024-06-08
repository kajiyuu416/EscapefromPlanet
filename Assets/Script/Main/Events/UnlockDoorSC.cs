using UnityEngine;

public class UnlockDoorSC : MonoBehaviour
{
    //フラグが返ると、開錠済みのドアを生成
    [SerializeField] GameObject lockDoor;
    [SerializeField] GameObject unlockDoor;
    private bool generateFlag = false;
    private void Update()
    {
        Generate_Door();
    }

    private void Generate_Door()
    {
        if(actionEvent.actionFlag && !generateFlag)
        {
            generateFlag = true;
            Instantiate(unlockDoor, transform.position, Quaternion.identity);
            lockDoor.SetActive(false);
 
        }
    }

}
