using UnityEngine;
using PathCreation;
using Operator;
using System.Collections;

namespace Enemy.EnemyControll
{
    public class EnemyController : MonoBehaviour
    {
        public PathCreator pathCreator;
        public bool isMove;
        public float speed = 5;

        public int HP = 2000;
        public GameObject _enemyHP;

        public float distanceTravelled;

        public OperatorController blockByOperator;

        private void Start()
        {
            SetStartHP(HP);
        }
        private void Update()
        {
            if (isMove)
            {
                distanceTravelled += speed * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, EndOfPathInstruction.Stop);
                //transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, EndOfPathInstruction.Stop);

                if (distanceTravelled > pathCreator.path.length)
                {
                    //Дошел
                    //Изменить счетчик
                    //"Удалить" врага(себя)
                    isMove = false;
                }
            }
        }
        void OnTriggerEnter(Collider collider)
        {
            //if (collider.gameObject.TryGetComponent(out OperatorController operatorController))
            //{
            //    if(operatorController.IsCanBlock())
            //    {
            //        isMove = false;
            //        blockByOperator = operatorController;
            //        operatorController.BlockingAdd(this);
            //    }
                
            //}
        }
        void OnTriggerStay(Collider collider)
        {
            if (collider.gameObject.TryGetComponent(out OperatorController operatorController))
            {
                if (operatorController.IsCanBlock())
                {
                    isMove = false;
                    blockByOperator = operatorController;
                    
                    operatorController.BlockingAdd(this);

                }

            }
            //if (collider.gameObject.TryGetComponent(out OperatorController operatorController))
            //{
            //    if (HP > 0)
            //    {
            //        isMove = true;
            //        blockByOperator = null;
            //    }
            //}
        }
        void OnTriggerExit(Collider collider)
        {
            if (collider.gameObject.TryGetComponent(out OperatorController operatorController))
            {
                if (HP > 0)
                {
                    isMove = true;
                    blockByOperator = null;
                }
            }
        }
        public void SetStartHP(int hp)
        {
            SetMaxHP(hp);
            SetHP(hp);
        }
        public void SetMaxHP(int hp)
        {
            _enemyHP.GetComponent<UIOperatorHp>().SetMaxHP(hp);
        }
        public void SetHP(int hp)
        {
            _enemyHP.GetComponent<UIOperatorHp>().SetHP(hp);
        }
        public void DamageHP(int damage)
        {

            //while(HP > HP - damage)
            //{
            //    _enemyHP.GetComponent<UIOperatorHp>().SetHP(HP - (int)Time.deltaTime);
            //}
            HP -= damage;
            StopCoroutine("DamageHealth");
            StartCoroutine(DamageHealth(damage));
            
        }
        public bool Damage(int damage)
        {
            //HP -= damage;
            DamageHP(damage);
            if (HP <= 0)
            {
                //if (blockByOperator != null)
                //{
                //    blockByOperator.BlockingRemove(gameObject.GetComponent<EnemyController>());
                //}
                isMove = false;
                transform.position = new Vector3(-9999f, -9999f, -9999f);
                return false;
                //Destroy(gameObject);
            }
            return true;
        }

        public void SetParametrs(float _speed, PathCreator path)
        {
            speed = _speed;
            pathCreator = path;
        }

        public void StartMove(bool _isMove = true)
        {
            isMove = _isMove;
        }
        IEnumerator DamageHealth(int damage)
        {
            int curHP = _enemyHP.GetComponent<UIOperatorHp>().GetHPValue();
            while (HP <= curHP)
            {
                curHP -= damage / 10;
                _enemyHP.GetComponent<UIOperatorHp>().SetHP(curHP);
                yield return new WaitForSeconds(.1f);
            }
            yield return null;
        }
    }
}
