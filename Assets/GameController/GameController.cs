using System.Collections.Generic;
using UnityEngine;
using SimpleBehaviour;

public class GameController : MonoBehaviour
{

    public static GameController _instance;
    public TerrainGenerator _terrain;

    private Stack<INode> ThingsToDo;

    // Start is called before the first frame update
    public void Start()
    {
        _instance = this;
        ThingsToDo = new Stack<INode>(10);
        ThingsToDo.Push(new RoundRobinTurns(FindObjectsOfType<GenericPlayer>()));
        ThingsToDo.Push(GetComponent<CreateTanksForPlayers>());
        ThingsToDo.Push(_terrain);
        ThingsToDo.Push(new DebugWaitForSeconds(.01f));
    }

    // Update is called once per frame
    public void Update()
    {

        //only the top of the todo stack gets to do something this frame:
        TreeStatusEnum result = ThingsToDo.Peek().Tick();

        //if todo finished (either way), we remove it from the top
        //WARNING: this will pop the wrong todo if the previous todo finished AND put something else on the stack
        //if (result != TreeStatusEnum.RUNNING) ThingsToDo.Pop();
    }

    //this todo will be executed until done, before all other todos:
    public void addThingToDo(INode todo)
    {
        ThingsToDo.Push(todo);
    }

    public void RemoveThingToDo(INode todo)
    {
        if (ThingsToDo.Peek().Equals(todo))
        {
            ThingsToDo.Pop();
        }
        else
        {
            Debug.LogError("ScorchedUnity: illegal request to stop doing " + todo.ToString() + " while actually doing " + ThingsToDo.Peek().ToString());
        }
    }
}
