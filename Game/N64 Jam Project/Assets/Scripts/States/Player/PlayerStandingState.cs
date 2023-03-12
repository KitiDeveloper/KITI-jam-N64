using UnityEngine;
namespace N64Jam.States
{
    public class PlayerStandingState : PlayerBaseState
    {
        public PlayerStandingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
        }

        public override void Executue(float deltaTime)
        {
            //Handle Falling Landing 
            
            //Movement 
            Vector3 movement = stateMachine.CalculateMovementDirection();
            stateMachine.mover.MoveTo(movement);
            stateMachine.FaceMovementDirection(movement,deltaTime);
        }

        public override void Exit()
        {
        }
    }
}