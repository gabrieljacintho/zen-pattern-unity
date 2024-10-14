namespace FireRingStudio.Interaction
{
    public interface IInteractor
    {
        bool IsInteracting { get; }
        
        bool TryInteract();

        bool CanInteract();
    }
}