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
}