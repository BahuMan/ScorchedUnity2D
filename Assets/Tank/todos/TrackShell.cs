using UnityEngine;
using SimpleBehaviour;

public class TrackShell : INode
{
    public delegate void UpdateShellPositionDelegate(Vector3 position, bool dead);
    public UpdateShellPositionDelegate UpdateShellPosition;
    private Rigidbody2D shell;

    public TrackShell(Rigidbody2D s, UpdateShellPositionDelegate tracker)
    {
        shell = s;
        UpdateShellPosition = tracker;
    }

    TreeStatusEnum INode.Tick()
    {
        if (shell == null)
        {
            UpdateShellPosition(Vector3.zero, true);
            GameController._instance.RemoveThingToDo(this);
            return TreeStatusEnum.SUCCESS;
        }
        else
        {
            UpdateShellPosition(shell.transform.position, false);
            return TreeStatusEnum.RUNNING;
        }
    }
}
