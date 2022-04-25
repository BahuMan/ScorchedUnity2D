using UnityEngine.SceneManagement;
using SimpleBehaviour;

public class NextGame : INode
{
    private static int roundsPlayed = 0;
    public TreeStatusEnum Tick()
    {
        roundsPlayed++;
        GameController._instance.RemoveThingToDo(this);
        if (roundsPlayed < Preferences._instance.NrRounds)
        {
            SceneManager.LoadScene("BuyMenu");
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
        return TreeStatusEnum.SUCCESS;
    }
}
