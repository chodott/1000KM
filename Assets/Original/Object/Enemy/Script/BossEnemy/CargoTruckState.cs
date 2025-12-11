
using UnityEngine;

public class DropCargoState : IState<CargoTruckController>
{
    public void Enter(CargoTruckController owner)
    {
        owner.DropProjectile();
        owner.ChangeStunState();
    }

    public void Exit()
    {
    }

    public void Update()
    {
    }

    public void HandleEvent(StateEvent stateEvent)
    {
    }
}