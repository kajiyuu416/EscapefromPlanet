using UnityEngine;

public class UnlockDoorSC : MonoBehaviour
{
    [SerializeField] GameObject lockDoor;
    [SerializeField] GameObject UnlockDoor;
    private bool GenerateFlag = false;
    private void Update()
    {
        Generate_Door();
    }

    private void Generate_Door()
    {
        if(actionEvent.actionFlag && !GenerateFlag)
        {
            GenerateFlag = true;
            Instantiate(UnlockDoor, transform.position, Quaternion.identity);
            lockDoor.SetActive(false);
 
        }
    }

}
