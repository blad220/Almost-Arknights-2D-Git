using UnityEngine;
using PathCreation;
using System.Collections;
using System.Collections.Generic;
using MainController.Operator.Bullet;
using MainController.Operator;
using MainController.Ui;

namespace MainController.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        public PathCreator pathCreator;
        public bool isMove;
        public float speed = 5;

        public float HP = 2000;
        public int Defense = 100;
        private float HPdamage = 0;
        public GameObject _enemyHP;

        public float distanceTravelled;

        public OperatorController blockByOperator;
        public ParticleSystem hitSelfParticles;

        public List<BulletController> targetByBullets = new List<BulletController>();

        private Coroutine damageHealth;
        private float velocityHP;
        public bool isDead;
        private UIOperatorHp uiOperatorHp;

        private void Start()
        {
            SetStartHP((int)HP);
            uiOperatorHp = _enemyHP.GetComponent<UIOperatorHp>();

            StartCoroutine(EnemyMove());
            StartCoroutine(HealthChange());
        }
        private void Awake()
        {
            
        }
        
        public void ClearTarget()
        {
            foreach(BulletController target in targetByBullets)
            {
                target.KillBullet();
            }
            targetByBullets.Clear();
        }
        public EnemyController MakeTargetBy(BulletController bulletController)
        {
            targetByBullets.Add(bulletController);
            return this;
        }
        private IEnumerator HealthChange()
        {
            while (!isDead)
            {
                if (HPdamage > 0 && HP > 0)
                {
                    int curHP = uiOperatorHp.GetHPValue();
                    float newPosition = Mathf.SmoothDamp(curHP, HP, ref velocityHP, 0.3f);
                    uiOperatorHp.SetHP((int)newPosition);
                    HPdamage -= (curHP - newPosition);
                }
                yield return new WaitForSeconds(0.03f);
            }
            yield return null;
        }
        private IEnumerator EnemyMove()
        {
            isMove = true;
            while (!isDead)
            {
                if (isMove)
                {
                    distanceTravelled += speed * 0.01f * Time.timeScale;
                    transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, EndOfPathInstruction.Stop);
                }
                if (distanceTravelled > pathCreator.path.length)
                {
                    int BrokeThroughEnemy = MainController.curEnemyBrokeThrough - 1;
                    MainController.SetBrokeThroughEnemy(BrokeThroughEnemy);
                    
                    if(blockByOperator != null)
                    {
                        blockByOperator.BlockingRemove(this);

                    }
                    transform.position = new Vector3(9999f, 9999f, 9999f);
                    isMove = false;
                    isDead = true;
                }
                yield return new WaitForSeconds(0.03f);
            }
            yield return null;
        }
        void OnTriggerEnter(Collider collider)
        {

        }
        public int AttackPower = 100;
        public float AttackSpeed = 1.3f;
        private float AttackSpeedTimer = 0f;
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

                if (blockByOperator != null)
                {
                    if (AttackSpeedTimer >= AttackSpeed)
                    {
                        AttackSpeedTimer = 0f;
                        blockByOperator.Damage(AttackPower);
                        if (blockByOperator != null)
                        {
                            blockByOperator.hitSelfParticle.gameObject.SetActive(false);
                            blockByOperator.hitSelfParticle.gameObject.SetActive(true);
                        }
                    }
                    AttackSpeedTimer += Time.deltaTime;
                }
                else
                {
                    AttackSpeedTimer = 0f;
                }
            }

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
            if (collider.gameObject.TryGetComponent(out RangeTile rangeTile))
            {
                if (HP > 0)
                {
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
            if (damage - Defense <= 0)
            {
                damage = (int)(damage * 0.05f);
            }
            else
            {
                damage -= Defense;
            }
            HPdamage += damage;
            HP -= damage; 
            
        }
        public bool Damage(int damage)
        {
            if (hitSelfParticles != null)
            {
                hitSelfParticles.gameObject.SetActive(false);
                hitSelfParticles.gameObject.SetActive(true);
            }
            DamageHP(damage);

            if (!isDead) {
                if (HP <= 0)
                {
                    isDead = true;
                    isMove = false;
                    if (blockByOperator != null)
                    {
                        blockByOperator.BlockingRemove(this);
                        blockByOperator = null;
                    }
                    ClearTarget();
                    transform.position = new Vector3(-9999f, -9999f, -9999f);
                    int EnemyCol = MainController.EnemyCol + 1;
                    MainController.SetEnemyColField(EnemyCol);
                    return false;
                }
            }
            else
            {
                ClearTarget();
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
            Debug.Log("DamageHealth Start");
            int curHP = _enemyHP.GetComponent<UIOperatorHp>().GetHPValue();
            while (HP <= curHP)
            {
                curHP -= damage / 100;
                _enemyHP.GetComponent<UIOperatorHp>().SetHP(curHP);
                yield return new WaitForSeconds(.03f);
            }
            Debug.Log("DamageHealth End");
            yield return null;
        }
    }
}
