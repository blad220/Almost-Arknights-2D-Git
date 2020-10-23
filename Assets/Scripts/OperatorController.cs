using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;
using MainController.Map.Tile;
using MainController.Operator.Bullet;
using MainController.Enemy;
using MainController.Ui;

namespace MainController.Operator
{
    public class OperatorController : MonoBehaviour
    {
        public OperatorData _operatorData;
        public GameObject _operatorObject;
        public GameObject _underlayObject;

        public GameObject _operatorHP;
        public GameObject _operatorSkill;

        public enum SetupDirection { Right, Left, Top, Bottom }
        public SetupDirection setupDirection;

        public TileDescription tilePosition;

        public UnityArmatureComponent operatorObject;

        public UISelectAvatar selectAvatar;

        [HideInInspector] public ParticleSystem hitSelfParticle;
        [HideInInspector] public ParticleSystem healSelfParticle;

        [HideInInspector] public List<RangeTile> rangeTiles = new List<RangeTile>();

        private float velocityHP;
        private bool isDead;

        private UIOperatorHp operatorHpComponent;
        private CapsuleCollider capsuleColliderComponent;
        private Coroutine HealLoopCoroutine;

        private List<EnemyController> blockingEnemy = new List<EnemyController>();
        private List<EnemyController> enemyInRange = new List<EnemyController>();
        private List<OperatorController> operatorInRange = new List<OperatorController>();

        private bool isAttack;
        private bool isAttackLoop;

        private List<BulletController> bulletList = new List<BulletController>();

        private EnemyController wichEnemyAttack;
        private OperatorController wichOperatorHeal;

        private bool isShowRange;
        private bool isAttacking;

        private GameObject rangeTilesParent;

        private void Awake()
        {
            
            if (_operatorHP.TryGetComponent(out UIOperatorHp _componentHP))
            {
                operatorHpComponent = _componentHP;
            }
            
        }
        private void Start()
        {
            if (_operatorObject.TryGetComponent(out UnityArmatureComponent _component))
            {
                operatorObject = _component;
            }

            if (TryGetComponent(out CapsuleCollider _componentCapsuleCollider))
            {
                capsuleColliderComponent = _componentCapsuleCollider;
                capsuleColliderComponent.enabled = false;
            }

            SetHP(_operatorData.maxHP);
            _operatorData.skill.curSkillPoint = _operatorData.skill.initCost;
            _operatorData.skill.isCanActive = false;

            DrowRange();

            BulletInitialization(2);
        }
        
        private IEnumerator EnemySearch()
        {
            while (!isDead)
            {
                if (tilePosition != null)
                {
                    if (_operatorData.isHealAttack)
                    {
                        wichOperatorHeal = ChooseOperatorWithLowerHP(operatorInRange);
                    }
                    else
                    {
                        wichEnemyAttack = ChooseClosesEnemy(enemyInRange);
                    }
                    int curHP = operatorHpComponent.GetHPValue();
                    if (_operatorData.curHP != curHP && _operatorData.curHP > 0)
                    {
                        float newPosition = Mathf.SmoothDamp(curHP, (int)_operatorData.curHP, ref velocityHP, 0.03f);
                        SetHP((int)newPosition);
                    }
                }
                yield return new WaitForSeconds(0.03f);
            }
            yield return null;
        }
        public void Damage(int damage)
        {
            
            if(damage - _operatorData.Defense <= (int)(damage * 0.05f))
            {
                damage = (int)(damage * 0.05f);
            }
            else
            {
                damage -= _operatorData.Defense;
            }

            SetHP(_operatorData.curHP - damage);

            if (_operatorData.curHP <= 0)
            {
                MainController.removeDeployedOperator(this.gameObject);
                isDead = true;
            }
        }
        public bool Heal(int healValue)
        {
            SetHP(_operatorData.curHP + healValue);

            if (_operatorData.curHP > _operatorData.maxHP)
            {
                SetHP(_operatorData.maxHP);
                return false;
            }
            return true;
        }
        private OperatorController ChooseOperatorWithLowerHP(List<OperatorController> enemyControllers)
        {
            operatorInRange.Clear();

            operatorInRange.Add(this);

            foreach (RangeTile operatorList in rangeTiles)
            {
                foreach (OperatorController oper in operatorList.operatorInTile)
                {
                    if (!operatorInRange.Contains(oper))
                    {
                        operatorInRange.Add(oper);
                    }
                }
            }
            if (operatorInRange.Count != 0)
            {
                OperatorController minHPOperator = null;
                float minHP = 0f;
                for (int i = 0; i < operatorInRange.Count; i++)
                {
                    
                    if (operatorInRange[i]._operatorData.curHP < operatorInRange[i]._operatorData.maxHP) {
                        if(minHP == 0f)
                        {
                            minHP = operatorInRange[i]._operatorData.curHP;
                            minHPOperator = operatorInRange[i];
                        }
                        else
                        {
                            float curHP = operatorInRange[i]._operatorData.curHP;
                            if (curHP < minHP)
                            {
                                minHP = curHP;
                                minHPOperator = operatorInRange[i];
                            }
                        }
                    }
                }
                return minHPOperator;
            }

            return null;
        }
        private EnemyController ChooseClosesEnemy(List<EnemyController> enemyControllers)
        {
            enemyInRange.Clear();
            foreach (RangeTile enemisList in rangeTiles)
            {
                foreach(EnemyController enemy in enemisList.enemyInTile)
                {
                    if (!enemyInRange.Contains(enemy))
                    {
                        enemyInRange.Add(enemy);
                    }
                }
            }
            if (enemyInRange.Count != 0)
            {
                float minDist = 0f;
                EnemyController minDistEnemy = null;
                for (int i = 0; i < enemyInRange.Count; i++)
                {
                    if (i == 0)
                    {
                        minDist = Vector3.Distance(transform.position, enemyInRange[i].transform.position);
                        minDistEnemy = enemyInRange[i];
                    }
                    else
                    {
                        float dist = Vector3.Distance(transform.position, enemyInRange[i].transform.position);
                        if (dist < minDist)
                        {
                            minDist = dist;
                            minDistEnemy = enemyInRange[i];
                        }
                    }
                }
                return minDistEnemy;
            }
            return null;
        }

        private void HealOperator(OperatorController _operatorController, OperatorData operatorData = null)
        {
            if (HealLoopCoroutine != null)
            {
                StopCoroutine(HealLoopCoroutine);
            }
            isAttackLoop = true;
            HealLoopCoroutine = StartCoroutine(AnimationHealLoopOperator(_operatorController));
        }
        public void EnemyInRangeRemove(EnemyController enemyController)
        {
            enemyInRange.Remove(enemyController);
        }
        
        public void BlockingAdd(EnemyController enemyController)
        {
            if (!IsContains(enemyController))
            {
                blockingEnemy.Add(enemyController);
            }
        }
        public bool IsContains(EnemyController enemyController)
        {
            return blockingEnemy.Contains(enemyController);
        }
        public bool IsCanBlock()
        {
            return blockingEnemy.Count < _operatorData.Block;
        }
        public void BlockingRemove(EnemyController enemyController)
        {
            blockingEnemy.Remove(enemyController);
        }
        public bool IsRangeShow()
        {
            return isShowRange;
        }
        
        public void DrowRange(SetupDirection direction = SetupDirection.Right, bool isActive = false)
        {
            if(rangeTilesParent != null)
            {
                Destroy(rangeTilesParent);
                rangeTiles.Clear();
            }

            Range.RangeOfOperator.Range rangeOfOperator = new Range.RangeOfOperator.Range();

            if (direction == SetupDirection.Right) {
                rangeOfOperator = _operatorData.rangeOfOperator.ArrayTransformRightMain();
            }
            if (direction == SetupDirection.Top)
            {
                rangeOfOperator = _operatorData.rangeOfOperator.ArrayTransformTop();
            }
            if (direction == SetupDirection.Bottom)
            {
                rangeOfOperator = _operatorData.rangeOfOperator.ArrayTransformBottom();
            }
            if (direction == SetupDirection.Left)
            {
                rangeOfOperator = _operatorData.rangeOfOperator.ArrayTransformLeft();
            }

            bool[,] range = rangeOfOperator.array;
            int operatorPositionX = rangeOfOperator.x;
            int operatorPositionY = rangeOfOperator.y;

            GameObject rangeTilePrefab = _operatorData.rangeOfOperator.tileRangePrefab;

            rangeTilesParent = new GameObject("Range Tiles");
            rangeTilesParent.transform.SetParent(transform);
            rangeTilesParent.transform.localPosition = new Vector3(-0.5f, 0f, -0.5f);

            for (int x = 0; x < range.GetLength(0); x++)
            {
                for(int y = 0; y < range.GetLength(1); y++)
                {
                    if (range[x, y] == true || (x == operatorPositionX && y == operatorPositionY))
                    {
                        GameObject tileRange = Instantiate(rangeTilePrefab, rangeTilesParent.transform);

                        RangeTile rangeTileComponent = tileRange.GetComponent<RangeTile>();
                        rangeTileComponent.operatorController = this;
                        rangeTiles.Add(rangeTileComponent);

                        Vector3 position = Vector3.zero;
                        if (direction == SetupDirection.Bottom || direction == SetupDirection.Top)
                        {
                            position = new Vector3((operatorPositionY - y) * 1f, 0f, (operatorPositionX - x) * 1f);
                        }
                        if (direction == SetupDirection.Left || direction == SetupDirection.Right)
                        {
                            position = new Vector3((y - operatorPositionY) * 1f, 0f, (x - operatorPositionX) * 1f);
                        }
                        
                        tileRange.GetComponent<BoxCollider>().enabled = isActive;
                        tileRange.transform.localPosition = position;
                    }
                }
            }
            isShowRange = true;
        }
        public void ShowRange()
        {
            if (!isShowRange)
            {
                foreach (RangeTile rangeTile in rangeTiles)
                {
                    rangeTile.gameObject.GetComponent<MeshRenderer>().enabled = true;
                }
                isShowRange = true;
            }
        }
        public void HideRange()
        {
            if (isShowRange)
            {
                foreach (RangeTile rangeTile in rangeTiles)
                {
                    rangeTile.gameObject.GetComponent<MeshRenderer>().enabled = false;
                }
                isShowRange = false;
            }
        }
        public void TogleRange()
        {
            foreach (RangeTile rangeTile in rangeTiles)
            {
                rangeTile.gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
        }
        public void SetMaxHP(int hp)
        {
            operatorHpComponent.SetMaxHP(hp);
        }
        public void SetHP(int hp)
        {
            //Set in Operator Data HP
            _operatorData.SetCurHP(hp);

            //Set in HPbar of Operator HP
            operatorHpComponent.SetHP(hp);

            //Update Info of selecting Operator(Only HP Field)
            if(MainController.mainInterfaceFields.selectOperatorUI.displaySelectedPanel.activeSelf && MainController.mainInterfaceFields.selectOperatorUI.operatorController == this)
            {
                MainController.mainInterfaceFields.selectOperatorUI.UpdateSelectInfoOnlyHP();
            }
            
        }
        public void SetMaxSkillPoint(int maxSkillPoint)
        {
            _operatorSkill.GetComponent<UIOperatorSkill>().SetMaxSkillPoint(maxSkillPoint);
        }
        public void SetSkillPoint(int skillPoint)
        {
            _operatorSkill.GetComponent<UIOperatorSkill>().SetSkillPoint(skillPoint);
        }

        public void SetStartHP(int hp)
        {
            SetMaxHP(hp);
            SetHP(hp);
        }
        public void SetStartSkillPoint(int skillPoint, int maxSkillPoint)
        {
            SetMaxSkillPoint(maxSkillPoint);
            SetSkillPoint(skillPoint);
        }

        public static void ChangeArmatureData(UnityArmatureComponent _armatureComponent, string armatureName, string dragonBonesName)
        {
            bool isUGUI = _armatureComponent.isUGUI;
            UnityDragonBonesData unityData = null;
            Slot slot = null;
            if (_armatureComponent.armature != null)
            {
                unityData = _armatureComponent.unityData;
                slot = _armatureComponent.armature.parent;
                _armatureComponent.Dispose(false);

                UnityFactory.factory._dragonBones.AdvanceTime(0.0f);

                _armatureComponent.unityData = unityData;
            }

            _armatureComponent.armatureName = armatureName;
            _armatureComponent.isUGUI = isUGUI;

            _armatureComponent = UnityFactory.factory.BuildArmatureComponent(_armatureComponent.armatureName, dragonBonesName, null, _armatureComponent.unityData.dataName, _armatureComponent.gameObject, _armatureComponent.isUGUI);
            if (slot != null)
            {
                slot.childArmature = _armatureComponent.armature;
            }

            _armatureComponent.sortingLayerName = _armatureComponent.sortingLayerName;
            _armatureComponent.sortingOrder = _armatureComponent.sortingOrder;
        }
        public void CreateRangeTiles(Range.RangeOfOperator _rangeOfOperator)
        {
            int x = _rangeOfOperator.OperatorPositionX;
            int y = _rangeOfOperator.OperatorPositionY;
            List<Range.RangeOfOperator.Wrapper> rangeData = _rangeOfOperator.RangeData;
        }
        public void StartOperator(SetupDirection direction)
        {
            blockingEnemy.Clear();
            setupDirection = direction;
            string lastAnimation = operatorObject._armature.animation.lastAnimationName;
            DragonBones.AnimationState lastAnimationState = operatorObject._armature.animation.lastAnimationState;
            if (direction == SetupDirection.Bottom)
            {
                operatorObject.armature.flipX = false;

                _underlayObject.transform.localEulerAngles = new Vector3(_underlayObject.transform.localEulerAngles.x, 270f, _underlayObject.transform.localEulerAngles.z);

                DrowRange(SetupDirection.Bottom);
            }
            if (direction == SetupDirection.Top)
            {
                string armatureName = operatorObject.armatureName;

                ChangeArmatureData(operatorObject, armatureName.Replace("_Front", "_Back"), operatorObject.unityData.dataName);

                operatorObject._armature.animation.Play("Idle", 0);

                operatorObject.armature.flipX = false;
                operatorObject.DBUpdate();
                _underlayObject.transform.localEulerAngles = new Vector3(_underlayObject.transform.localEulerAngles.x, 90f, _underlayObject.transform.localEulerAngles.z);

                DrowRange(SetupDirection.Top);
            }
            if (direction == SetupDirection.Left)
            {
                operatorObject.armature.flipX = true;

                _underlayObject.transform.localEulerAngles = new Vector3(_underlayObject.transform.localEulerAngles.x, 0f, _underlayObject.transform.localEulerAngles.z);

                DrowRange(SetupDirection.Left);
            }
            if (direction == SetupDirection.Right)
            {
                operatorObject.armature.flipX = false;

                _underlayObject.transform.localEulerAngles = new Vector3(_underlayObject.transform.localEulerAngles.x, 180f, _underlayObject.transform.localEulerAngles.z);

                DrowRange(SetupDirection.Right);
            }

            StartCoroutine(AnimationStartOperator());
            StartCoroutine(EnemySearch());
            StartCoroutine(AnimationAttack());

            StartCoroutine(_operatorData.skill.SkillPointGenerate(this, _operatorData.skill.initCost));
        }
        public void EndOperator()
        {
            StopAllCoroutines();
            tilePosition = null;

            foreach (EnemyController blockEnemy in blockingEnemy)
            {
                blockEnemy.isMove = true;
                blockEnemy.blockByOperator = null;
            }

            transform.position = new Vector3(9999f, 9999f, 9999f);

            SetStartHP(_operatorData.maxHP);

            blockingEnemy.Clear();
            enemyInRange.Clear();

            capsuleColliderComponent.enabled = false;
            //UnityEditor.EditorApplication.delayCall += () =>
            //{
                ChangeArmatureData(operatorObject, operatorObject.armatureName.Replace("_Back", "_Front"), operatorObject.unityData.dataName);
            //};
            operatorObject.armature.flipX = false;

            operatorObject.DBUpdate();

            _underlayObject.transform.localEulerAngles = new Vector3(_underlayObject.transform.localEulerAngles.x, 180f, _underlayObject.transform.localEulerAngles.z);

            DrowRange(SetupDirection.Right);

            foreach (RangeTile tileRange in rangeTiles)
            {
                tileRange.GetComponent<BoxCollider>().enabled = false;
            }

            StartCoroutine(CooldawnRedeploy(_operatorData.RedeployTyme));
        }
        
        private void BulletInitialization(int count)
        {
            if (_operatorData.bulletController != null)
            {
                for (int i = 0; i < count; i++)
                {

                    GameObject bullet = Instantiate(_operatorData.bulletController.gameObject, transform);
                    bullet.transform.localPosition = new Vector3(9999f, 9999f, 9999f);
                    bulletList.Add(bullet.GetComponent<BulletController>());
                }
            }
        }
        private void BulletFire()
        {
            for(int i = 0; i<bulletList.Count; i++)
            {
                if(bulletList[i].target == null)
                {
                    bulletList[i].Attack = _operatorData.Attack;
                    bulletList[i].MaxSpeed = _operatorData.ASPD * 10f;
                    bulletList[i].SetTarget(wichEnemyAttack);
                    wichEnemyAttack.MakeTargetBy(bulletList[i]);
                }
            }
        }
        private void PlayAttakAnimation()
        {
            if (operatorObject._armature.animation.HasAnimation("Attack"))
            {
                operatorObject._armature.animation.Play("Attack", 1);
            }
            else
            {
                if (operatorObject._armature.animation.HasAnimation("Attack_Loop"))
                {
                    operatorObject._armature.animation.Play("Attack_Loop", 1);
                }
                else operatorObject._armature.animation.Play("Idle", 1);
            }
        }
        IEnumerator CooldawnRedeploy(int time)
        {
            int curTime = time;
            
            selectAvatar.RedeployShow(); 
            while(curTime > 0)
            {
                selectAvatar.RedeployField.text = curTime.ToString();
                curTime--;
                yield return new WaitForSeconds(1f);
            }
            selectAvatar.RedeployHide();
            yield return null;
        }
        IEnumerator AnimationStartOperator()
        {
            operatorObject._armature.animation.Play("Start", 1);

            //while (!operatorObject._armature.animation.isCompleted)
            //{
            //    yield return new WaitForSeconds(0.3f);
            //}

            operatorObject._armature.animation.Play("Idle", 0);
            yield return null;
        }

        public IEnumerator AnimationAttack()
        {
            while (_operatorData.curHP > 0)
            {
                if(wichEnemyAttack != null)
                {
                    isAttacking = true;
                    PlayAttakAnimation();

                    if (_operatorData.bulletController != null)
                    {
                        BulletFire();
                    }
                    else
                    {
           
                            if (!wichEnemyAttack.Damage(_operatorData.Attack))
                            {
                                isAttackLoop = false;
                                blockingEnemy.Remove(wichEnemyAttack);
                            }
                        
                    }

                    yield return new WaitForSeconds(_operatorData.ASPD);

                    isAttacking = false;
                }
                if (wichOperatorHeal != null)
                {
                    isAttacking = true;
                    PlayAttakAnimation();
                    if (!wichOperatorHeal.Heal(_operatorData.Attack))
                    {
                        isAttackLoop = false;
                    }
                    if (wichOperatorHeal.healSelfParticle != null)
                    {
                        wichOperatorHeal.healSelfParticle.gameObject.SetActive(false);
                        wichOperatorHeal.healSelfParticle.gameObject.SetActive(true);
                    }
                    yield return new WaitForSeconds(_operatorData.ASPD);

                    isAttacking = false;
                }
                
                    if (operatorObject._armature.animation.lastAnimationName != "Idle")
                    {
                        operatorObject._armature.animation.Play("Idle", 0);
                    }
                
                yield return new WaitForSeconds(0.03f);

            }
            
            yield return null;
        }
        IEnumerator AnimationHealLoopOperator(OperatorController _operatorController = null)
        {
            isAttacking = true;
            while (isAttackLoop)
            {
                if (operatorObject._armature.animation.HasAnimation("Attack"))
                {
                    operatorObject._armature.animation.Play("Attack", 1);
                }
                else
                {
                    if (operatorObject._armature.animation.HasAnimation("Attack_Loop"))
                    {
                        operatorObject._armature.animation.Play("Attack_Loop", 1);
                    }
                    else operatorObject._armature.animation.Play("Idle", 1);
                }
                yield return new WaitForSeconds(_operatorData.ASPD);

                if (_operatorController != null)
                {
                    if (!_operatorController.Heal(_operatorData.Attack))
                    {
                        isAttackLoop = false;
                    }
                    if (wichOperatorHeal.healSelfParticle != null)
                    {
                        wichOperatorHeal.healSelfParticle.gameObject.SetActive(false);
                        wichOperatorHeal.healSelfParticle.gameObject.SetActive(true);
                    }

                }
                else
                {
                    isAttackLoop = false;
                }
            }
            isAttacking = false;

            if (!isAttackLoop)
                operatorObject._armature.animation.Play("Idle", 0);
            yield return null;
        }
    }
}