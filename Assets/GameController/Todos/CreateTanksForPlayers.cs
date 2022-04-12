using UnityEngine;
using SimpleBehaviour;

public class CreateTanksForPlayers : MonoBehaviour, INode
{
    [SerializeField] LeftRightBorderControl LeftBorder;
    [SerializeField] LeftRightBorderControl RightBorder;

    private float minX, maxX;

    TreeStatusEnum INode.Tick()
    {
        Debug.Log("Creating tanks ...");
        minX = LeftBorder.transform.position.x + LeftBorder.transform.localScale.x / 2f;
        maxX = RightBorder.transform.position.x + RightBorder.transform.localScale.x / 2f;

        GenericPlayer[] players = GameObject.FindObjectsOfType<GenericPlayer>();
        for (int i = 0; i < players.Length; i++)
        {
            TankControl newTank = GameObject.Instantiate<TankControl>(players[i].getPreferredTankPrefab());
            newTank.transform.position = new Vector3(Mathf.Lerp(minX, maxX, (i+1f) / (players.Length + 1f)), 20f, 0f);
            newTank.HP = 1000;
            players[i].SetTank(newTank);

        }

        GameController._instance.RemoveThingToDo(this);
        return TreeStatusEnum.SUCCESS;
    }

}
