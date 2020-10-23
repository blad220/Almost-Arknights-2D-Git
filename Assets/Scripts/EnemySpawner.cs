using UnityEngine;
using PathCreation;
using System.Collections.Generic;
using System.Collections;

namespace MainController.Enemy.EnemySpawn
{
    [RequireComponent(typeof(PathCreator))]
    public class EnemySpawner : MonoBehaviour
    {
        [Space(10)]
        public Transform StartPointObject;
        public List<Transform> IntermediatePoints;
        public Transform EndPointObject;
        public float hightOfPath = 0.11f;
        public bool createSafePointsSpheres;
        public List<Transform> IntermediatePointsSave;

        [Space(10)]
        public float timeSpawnDelay = 2f;
        public Spawn[] spawns;
        public float speed = 5;

        private PathCreator pathCreator;
        private float distanceTravelled;
        private GameObject parent;
        

        void Start()
        {
            if (createSafePointsSpheres)
            {
                IntermediatePointsSave.Clear();
                GameObject parentPoint = new GameObject($"Points {gameObject.name}");
                parentPoint.transform.SetParent(transform);
                parentPoint.transform.localPosition = Vector3.zero;
                int i = 0;
                foreach (Transform IntermediatePoint in IntermediatePoints)
                {
                    if (IntermediatePoint != null)
                    {
                       
                        GameObject point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        point.name = $"Point {i}";
                        point.transform.SetParent(parentPoint.transform);
                        point.transform.localPosition = IntermediatePoint.position + new Vector3(0.5f, 0f, 0.5f);
                        point.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

                        IntermediatePointsSave.Add(point.transform);
                        i++;
                    }
                }
                parentPoint.SetActive(false);
            }
            transform.localPosition = Vector3.zero;

            pathCreator = GetComponent<PathCreator>();

            parent = new GameObject("Enemies");

            BezierPath bezierPath = CreatePath(StartPointObject, IntermediatePointsSave, EndPointObject);
            pathCreator.bezierPath = bezierPath;

            foreach (Spawn spawn in spawns) {
                StartCoroutine(TimeOfSpawn(spawn));
            }

            int colMax = MainController.EnemyMax + spawns.Length;
            MainController.SetEnemyColFieldStart(0, colMax);
        }
        private IEnumerator TimeOfSpawn(Spawn wait)
        {
            yield return new WaitForSeconds(timeSpawnDelay + wait.time);
            StartEnemies(pathCreator, wait.enemyObject);
            yield return null;
        }
        private BezierPath CreatePath(Transform startWaypoint, List<Transform> waypoints, Transform endWaypoint)
        {
            if (waypoints.Count > 0)
            {
                List<Vector3> waypointsVector = new List<Vector3>();
                foreach (Transform waypoint in waypoints)
                {
                    Vector3 tempVector = new Vector3(waypoint.localPosition.x, hightOfPath, waypoint.localPosition.z);
                    waypointsVector.Add(tempVector);
                }

                waypointsVector.Insert(0, new Vector3(startWaypoint.position.x, hightOfPath, startWaypoint.position.z));
                waypointsVector.Add(new Vector3(endWaypoint.position.x, hightOfPath, endWaypoint.position.z));

                BezierPath bezierPath = new BezierPath(waypointsVector, false, PathSpace.xyz);
                bezierPath.ControlPointMode = BezierPath.ControlMode.Automatic;
                bezierPath.AutoControlLength = 0.01f;

                return bezierPath;
            }
            return null;
        }
        private void StartEnemies(PathCreator path, GameObject enemyObject)
        {
            
            if (enemyObject != null)
            {
                CreateEnemy(enemyObject, parent, path);
            }
        }
        private void CreateEnemy(GameObject enemyObject, GameObject parent, PathCreator path)
        {

            GameObject enemy = Instantiate(enemyObject, parent.transform);

            EnemyController enemyController;

            if (enemy.TryGetComponent(out EnemyController _enemyController))
            {
                enemyController = _enemyController;
            }
            else
            {
                enemyController = enemy.AddComponent<EnemyController>();
            }
            enemyController.SetParametrs(speed, path);
            enemyController.StartMove();
        }
        private void MoveEnemy(GameObject enemyObject)
        {

        }
        [System.Serializable]
        public class Spawn {
            public GameObject enemyObject;
            public float time;
        }
    }
}
