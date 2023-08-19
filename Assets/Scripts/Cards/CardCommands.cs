using System.Collections.Generic;
using UnityEngine;

class BackwardPowerCommand : ICommand
{
    private Pawn targetPawn;
    public Pawn TargetPawn {
        set {
            targetPawn = value;
        }
    }
    public void execute()
    {
        PawnMoverProvider.Instance.GetPawnMover().MoveBackward_By_Steps(targetPawn, DiceManager.Instance.GetDiceNumber());
    }
}

class ImprisonPowerCommand : ICommand
{
    public void execute()
    {
        
    }
}

class CommandInvoker {
    private static CommandInvoker instance;
    public static CommandInvoker Instance {
        get {
            if(instance == null) {
                instance = new CommandInvoker();
            }
            return instance;
        }
    }

    private List<ICommand> commands;

    public CommandInvoker() {
        commands = new List<ICommand>();
        
    }
    
}