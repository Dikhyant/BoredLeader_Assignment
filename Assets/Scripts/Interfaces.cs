using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public interface ICard {

}

public interface ICardActuator {
    public void UseCard(ICard card);
}

public interface IPathProvider {
    public Path GetChosenPath();
}

public interface IPawnMover {
    public Task MovePawnBySteps(Pawn pawn, int steps);
    public Task MoveForward_By_Steps(Pawn pawn, int steps);
    public Task MoveBackward_By_Steps(Pawn pawn, int steps);
}

public interface IPawnProvider {
    public Pawn CurrentPawn {get;}
}

public interface IDice {
    public int DiceNumber{get;}
}

public interface ICommand {
    public void execute();
}

public interface INextPlayerDecisionMaker {
    public Pawn GetNextPlayerToPlay();
    public void DontLetPlayerHaveATurn(Pawn pawn);
}

public interface IWhichPlayerWillStartTheRoundDecisionMaker {
    public Pawn GetFirstPlayer();
}

public interface ICardCollector {
    public List<ICard> Cards {get;}
}

public interface IPawnData {
    public bool WillGoBackwards {get; set;}
    public bool IsImprisioned {get; set;}
    public int NumberOfRoundsImprisionedFor {get; set;}
}

public interface IPawnTargetModel {
    public Pawn TargetPawn {get;}
}