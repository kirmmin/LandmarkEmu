namespace LandmarkEmulator.Shared.Game.Events
{
    public interface IEvent
    {
        bool CanExecute();
        void Execute();
    }
}
