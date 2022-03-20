using System.Collections.Generic;
using UnityEngine;
using SimpleBehaviour;

public class GameController : MonoBehaviour
{

    private Stack<INode> ThingsToDo;

    // Start is called before the first frame update
    public void Start()
    {
        ThingsToDo = new Stack<INode>(10);
        ThingsToDo.Push(new TweeBeurtenTodo(FindObjectsOfType<TankControl>()));
    }

    // Update is called once per frame
    public void Update()
    {
        //only the top of the todo stack gets to do something this frame:
        ThingsToDo.Peek().Tick();
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
            throw new UnityException("ScorchedUnity: illegal request to stop doing " + todo.ToString() + " while actually doing " + ThingsToDo.Peek().ToString());
        }
    }
}
