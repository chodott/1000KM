
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

    public void OnCollisionEnter(Collision collision)
    {
    }

    public void OnTriggerEnter(Collider other)
    {
    }


    public void OnParried(Vector3 contactPoint, float damage, float moveLaneSpeed)
    {
    }

    public void Update()
    {
    }
}