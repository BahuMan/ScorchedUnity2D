using UnityEngine;
using SimpleBehaviour;
using System.Collections.Generic;
using System;

public class RoundRobinTurns : INode
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

        if (tank.HP > 0)
        {
            GameController._instance.addThingToDo(tank.GetInteraction());
            //right before the current tank, we need to couple the UI: (and we add it AFTER the turn because we're working with a stack)
            GameController._instance.addThingToDo(new ConnectUI(tank));
        }

        if (tank.HP > 0)
        {
            currentTank++;
        }
        else
        {
            Debug.Log("Tank " + tank.name + " is dead => removed from turns");
            LiveTanks.RemoveAt(currentTank);
        }

        if (currentTank >= LiveTanks.Count) currentTank = 0;
        return TreeStatusEnum.RUNNING;
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
}
