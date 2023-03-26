using UnityEngine;

namespace N64Jam.States
{
    public abstract class StateMachine : MonoBehaviour
    {
        State currentState;

        private void FixedUpdate()
        {
            currentState?.Executue(Time.deltaTime);
        }

        public void SwitchState(State newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState?.Enter();
        }
    }
}