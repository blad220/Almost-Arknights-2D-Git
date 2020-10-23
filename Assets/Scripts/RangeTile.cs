using MainController.Enemy;
using System.Collections.Generic;
using UnityEngine;

namespace MainController.Operator
{

    public class RangeTile : MonoBehaviour
    {
        public OperatorController operatorController;
        public List<EnemyController> enemyInTile = new List<EnemyController>();
        public List<OperatorController> operatorInTile = new List<OperatorController>();

        void OnTriggerEnter(Collider collider)
        {
            if (operatorController._operatorData.isHealAttack)
            {
                if (collider.gameObject.TryGetComponent(out OperatorController _operatorController))
                {
                    operatorInTile.Add(_operatorController);
                }
            }
            else
            {
                if (collider.gameObject.TryGetComponent(out EnemyController _enemyController))
                {
                    enemyInTile.Add(_enemyController);
                }
            }
        }
        void OnTriggerStay(Collider collider)
        {
            if (collider.gameObject.TryGetComponent(out EnemyController enemyController))
            {


            }

        }
        void OnTriggerExit(Collider collider)
        {
            if (operatorController._operatorData.isHealAttack)
            {
                if (collider.gameObject.TryGetComponent(out OperatorController _operatorController))
                {

                    operatorInTile.Remove(_operatorController);
                }
            }
            else
            {
                if (collider.gameObject.TryGetComponent(out EnemyController _enemyController))
                {
                    enemyInTile.Remove(_enemyController);
                }
            }
        }
    }
}