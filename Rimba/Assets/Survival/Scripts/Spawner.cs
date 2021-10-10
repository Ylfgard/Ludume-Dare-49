using UnityEditor;
using UnityEngine;

namespace Rimba
{
    namespace Survival
    {
        public class Spawner : MonoBehaviour
        {
            [SerializeField] private BoxCollider2D spawnArea;
            [SerializeField] private Transform bonfire;
            [SerializeField] private float bonfireMinDistance;
            [SerializeField] private GameObject prefab;
            [SerializeField] private int count;
            [SerializeField] private float minDistance;

            void Start()
            {
                Bounds bounds = spawnArea.bounds;

                Vector3[] spawned = new Vector3[count];
                for (int i=0; i < count; i++)
                {
                    for (int t=0;; t++)
                    {
                        Vector3 position = new Vector3(
                            Random.Range(bounds.min.x, bounds.max.x),
                            Random.Range(bounds.min.y, bounds.max.y),
                            Random.Range(bounds.min.z, bounds.max.z)
                        );

                        if (Vector3.Distance(position, bonfire.position) < bonfireMinDistance)
                        {
                            continue;
                        }

                        bool failed = false;
                        for (int j=0; j < i; j++)
                        {
                            if (Vector3.Distance(position, spawned[j]) < minDistance)
                            {
                                failed = true;
                                break;
                            }
                        }
                        if (failed && t < 4)
                            continue;

                        spawned[i] = position;
                        SpawnAt(position);

                        break;
                    }
                }
            }

            public virtual GameObject SpawnAt(Vector3 position)
            {
                return Instantiate(prefab, position, Quaternion.identity, transform);
            }

            void OnDrawGizmosSelected() {
                Handles.color = new Color(0f, 1f, 0f, 0.2f);
                Handles.DrawSolidDisc(transform.position, Vector3.forward, bonfireMinDistance);
            }
        }
    }
}
