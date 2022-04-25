using UnityEngine;
using SimpleBehaviour;
using System.Collections.Generic;
using System;

public interface IAdvanceTurn
{
    void AdvanceTurn();
}

public class RoundRobinTurns : INode, IAdvanceTurn
{
    private GenericPlayer[] AllPlayers;
    private List<TankControl> LiveTanks;
    private int currentTank = 0;

    public RoundRobinTurns(GenericPlayer[] players)
    {
        AllPlayers = players;
        LiveTanks = new List<TankControl>(AllPlayers.Length);
    }

    TreeStatusEnum INode.Tick()
    {
        if (currentTank == 0) StartNewTurn();

        RemoveDeadTanks();
        if (currentTank >= LiveTanks.Count) currentTank = 0;

        if (LiveTanks.Count == 1)
        {
            Debug.Log($"Last tank standing. Game ended.");
            GameController._instance.RemoveThingToDo(this);
            GameController._instance.addThingToDo(new NextGame());
            GameController._instance.addThingToDo(new DeclareWinner(LiveTanks[0].GetPlayer()));
            return TreeStatusEnum.SUCCESS;
        }
        if (LiveTanks.Count == 0)
        {
            Debug.Log("No tanks left. Game ended.");
            GameController._instance.RemoveThingToDo(this);
            GameController._instance.addThingToDo(new NextGame());
            return TreeStatusEnum.SUCCESS;
        }

        TankControl tank = LiveTanks[currentTank];
        GameController._instance.addThingToDo(new AdvanceToNextTank(this, tank));
        GameController._instance.addThingToDo(tank.GetInteraction());
        //right before the current tank, we need to couple the UI: (and we add it AFTER the turn because we're working with a stack)
        GameController._instance.addThingToDo(new ConnectUI(tank));

        return TreeStatusEnum.RUNNING;
    }

    private class AdvanceToNextTank : INode
    {
        TankControl toTest;
        IAdvanceTurn rr;
        public AdvanceToNextTank(IAdvanceTurn turn, TankControl currentTank)
        {
            toTest = currentTank;
            rr = turn;
        }

        TreeStatusEnum INode.Tick()
        {
            if (toTest == null || toTest.HP <= 0)
            {
                Debug.Log("Tank is dead => will be removed from turns");
            }
            else
            {
                rr.AdvanceTurn();
            }
            GameController._instance.RemoveThingToDo(this);
            return TreeStatusEnum.SUCCESS;
        }
    }

    private void RemoveDeadTanks()
    {
        int t = 0;
        while (t < LiveTanks.Count)
        {
            while (t < LiveTanks.Count && (LiveTanks[t] == null || LiveTanks[t].HP <= 0)) LiveTanks.RemoveAt(t);
            t++;
        }
    }

    private void StartNewTurn()
    {
        LiveTanks.Clear();
        foreach (var player in AllPlayers)
        {
            TankControl tank = player.GetTank();
            if (tank.HP > 0) LiveTanks.Add(tank);
        }
    }

    //this method will only be called by inner class AdvanceToNextTank while it's on the todo stack
    //and only if the current tank didn't die
    //(if it died, it will be removed from the list and this counter doesnt need to advance)
    void IAdvanceTurn.AdvanceTurn()
    {
        currentTank++;
    }
}
