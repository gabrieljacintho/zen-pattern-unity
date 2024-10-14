using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace FireRingStudio.BehaviourTree
{
    [TaskCategory("FireRing Studio")]
    [TaskDescription("Compare game state to current game state. Returns success if equals.")]
    public class CompareCurrentGameState : Conditional
    {
        [SerializeField] private GameState _gameState;


        public override TaskStatus OnUpdate()
        {
            return GameManager.CurrentGameState == _gameState ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}