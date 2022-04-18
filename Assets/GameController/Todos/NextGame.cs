using UnityEngine.SceneManagement;
using SimpleBehaviour;

public class NextGame : INode
{
    public TreeStatusEnum Tick()
    {
        GameController._instance.RemoveThingToDo(this);
        SceneManager.LoadScene("SampleScene");
        return TreeStatusEnum.SUCCESS;
    }
}
