namespace N64Jam.States
{
    public abstract class State
    {
        public abstract void Enter();
        public abstract void Executue(float deltaTime);
        public abstract void Exit();
    }
}