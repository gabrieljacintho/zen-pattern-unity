namespace FireRingStudio.Helpers
{
    public static class SymbolsHelper
    {
        public static bool IsInDevelopment()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD || DEVELOPMENT
            return true;
#else
            return false;
#endif
        }
    }
}