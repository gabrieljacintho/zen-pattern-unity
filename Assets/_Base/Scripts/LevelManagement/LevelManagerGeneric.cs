using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;

namespace FireRingStudio.LevelManagement
{
    public abstract class LevelManagerGeneric<T> : LevelManager where T : Level
    {
        [SerializeField, PropertyOrder(-1)] protected List<T> _levels;
        
        public override List<Level> Levels => _levels.Cast<Level>().ToList();
    }
}