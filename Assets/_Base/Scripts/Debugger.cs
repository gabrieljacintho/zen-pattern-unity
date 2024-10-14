using FireRingStudio.Patterns;

namespace FireRingStudio
{
    public class Debugger : PersistentSingleton<Debugger> // TODO: Create debugger
    {
        
    }
    
    public static class DebuggerInitializer
    {
        /*[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private void Initialize()
        {
            if (Instance != null)
                return;
        
            if (!Debug.isDebugBuild && !Application.isEditor)
                return;
                
            GameObject prefab = (GameObject) Resources.Load("Debugger");
            GameObject instance = Instantiate(prefab);
            instance.name = nameof(CustomSceneManager);
        }*/
    }
}