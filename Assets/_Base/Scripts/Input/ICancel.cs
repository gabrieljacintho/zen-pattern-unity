namespace FireRingStudio.Input
{
    public interface ICancelable
    {
        int Priority { get; }

        void Cancel();

        bool CanCancel();
    }
}