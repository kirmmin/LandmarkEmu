namespace LandmarkEmulator.Shared.Network.Message
{
    public interface IBuildable<out T>
    {
        T Build();
    }
}
