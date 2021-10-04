using UnityEngine;
using UnityEngine.Rendering;

namespace Rimba
{
    namespace Survival
    {
        public class WolfSpawner : Spawner
        {
            [SerializeField] private Collider2D roamArea;

            public override GameObject SpawnAt(Vector3 position)
            {
                GameObject go = base.SpawnAt(position);
                Wolf wolf = go.GetComponent<Wolf>();
                wolf.roamArea = roamArea;
                return go;
            }
        }
    }
}
