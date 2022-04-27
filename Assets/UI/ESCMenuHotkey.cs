using UnityEngine;
using SimpleBehaviour;

public class ESCMenuHotkey : MonoBehaviour, INode
{

    [SerializeField] GameObject panel;
    private float previousTimeScale = 1f;
    private bool MenuHidden = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && MenuHidden)
        {
            Debug.Log("Menu activated");
            panel.SetActive(true);
            GameController._instance.addThingToDo(this);
        }

        //the "MenuHidden" flag is set with 1 frame delay so
        //ESC doesn't immediately re-activate the menu
        if (MenuHidden == false && panel.activeSelf == false)
        {
            MenuHidden = true;
        }
    }
    TreeStatusEnum INode.Tick()
    {
        //in the first frame, don't listen to ESC key or we will quit menu
        //before it even activates :)
        if (MenuHidden == true && panel.activeSelf == true)
        {
            MenuHidden = false;
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            return TreeStatusEnum.RUNNING; //next frame, we'll listen to the UI
        }


        if (Input.GetKeyDown(KeyCode.F)) ESC_FastForward_Clicked();
        if (Input.GetKeyDown(KeyCode.K)) ESC_KillAll_Clicked();
        if (Input.GetKeyDown(KeyCode.Q)) ESC_Quit_Clicked();

        if (Input.GetKeyDown(KeyCode.Escape)
            || Input.GetKeyDown(KeyCode.R)
            || panel.activeSelf == false)
        {
            Time.timeScale = previousTimeScale;
            panel.SetActive(false);
            GameController._instance.RemoveThingToDo(this);
            return TreeStatusEnum.SUCCESS;
        }

        return TreeStatusEnum.RUNNING;
    }

    public void ESC_Resume_Clicked()
    {
        MenuHidden = true;
        panel.SetActive(false); //tick() will take care of the rest
    }

    public void ESC_FastForward_Clicked()
    {
        previousTimeScale = 4f; //when we resume game, speed will be faster
    }

    public void ESC_KillAll_Clicked()
    {
        foreach (TankControl t in FindObjectsOfType<TankControl>())
        {
            t.HP = 0;
        }
    }

    public void ESC_Quit_Clicked()
    {
        Debug.Log("Bye");
        UnityEngine.Application.Quit();
    }

    private void OnDestroy()
    {
        Time.timeScale = 0.8f; //level ended -> normal speed again
    }
}
