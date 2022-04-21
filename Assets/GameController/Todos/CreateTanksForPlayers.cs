using UnityEngine;
using SimpleBehaviour;

public class CreateTanksForPlayers : MonoBehaviour, INode
{
    [SerializeField] Transform TopBorder;
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
            foreach (var sp in newTank.GetComponentsInChildren<SpriteRenderer>()) sp.color = players[i].PlayerColor;
            newTank.HP = 1000;

            //put tank in player position, at top of screen
            Vector2 toppos = new Vector3(Mathf.Lerp(minX, maxX, (i + 1f) / (players.Length + 1f)), TopBorder.position.y, 0f);
            //now lower tank to terrain height:
            RaycastHit2D hit = Physics2D.BoxCast(toppos, Vector2.one, 0f, Vector2.down);
            Debug.Log("tank landed on " + hit.collider.gameObject.name);
            if (hit.collider.GetComponent<TerrainTile>() == null) Debug.Break();
            newTank.transform.position = new Vector3(toppos.x, hit.point.y, 0f);
            players[i].SetTank(newTank);

        }

        GameController._instance.RemoveThingToDo(this);
        return TreeStatusEnum.SUCCESS;
    }

}
