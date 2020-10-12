using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using DragonBones;
using Enemy.EnemyControll;
//#endif

namespace Operator
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
        //public Range.RangeOfOperator rangeOfOperator;

        public bool isAttack;
        public bool isAttackLoop;

        private bool isShowRange;
        private List<RangeTile> rangeTiles= new List<RangeTile>();
        private bool isAttacking;
        private GameObject rangeTilesParent;
        private List<Enemy.EnemyControll.EnemyController> blockingEnemy = new List<Enemy.EnemyControll.EnemyController>();
        DragonBones.UnityArmatureComponent operatorObject;
        public GameObject tempObject;
        // Start is called before the first frame update
        void Start()
        {
            if (_operatorObject.TryGetComponent(out DragonBones.UnityArmatureComponent component))
            {
                operatorObject = component;
                //StartOperator();
            }
            //operatorObject = _operatorObject.GetComponent<DragonBones.UnityArmatureComponent>();
            DrowRange();
        }

        // Update is called once per frame
        void Update()
        {
            if (blockingEnemy.Count > 0)
            {
                tempObject = blockingEnemy[0].gameObject;
            }
        }
        void OnTriggerEnter(Collider collider)
        {

        }
        void OnTriggerStay(Collider collider)
        {
            if (collider.gameObject.TryGetComponent(out Enemy.EnemyControll.EnemyController enemyController))
            {

                if (!isAttackLoop)
                {

                    if (blockingEnemy.Count > 0)
                    {
                        AttackEnemy(blockingEnemy[0]);
                    }

                    //if (AtackEnemiesList.Count > 0)
                    //{
                    //    AttackEnemy(ChooseEnemy(AtackEnemiesList));
                    //}

                }

            }
        }
        void OnTriggerExit(Collider collider)
        {
            

        }
        private EnemyController ChooseEnemy(List<EnemyController> enemyControllers)
        {
            //float minDist = 0f;
            //EnemyController minDistEnemy;
            //for(int i=0;) { 
            //if(i == 0) {minDist = Vector3.distance(gameObject, enemyControllers[i]); minDistEnemy = enemyControllers[i];}
            //float dist = Vector3.distance(gameObject, enemyControllers[i])
            // if(dist<minDist) {minDist = dist; minDistEnemy = enemyControllers[i];}
            //return minDistEnemy;
            return null;
        }
        private void AttackEnemy(EnemyController enemyController)
        {
            isAttackLoop = true;
            StartCoroutine(AnimationAttackLoopOperator(enemyController));
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
            //Debug.Log($"blockingEnemy Count {blockingEnemy.Count}");
            return blockingEnemy.Count < _operatorData.Block;
        }
        public void BlockingRemove(EnemyController enemyController)
        {
            blockingEnemy.Remove(enemyController);
        }
        public void ShowRange(bool isShow)
        {
            if(isShow)
            {
                rangeTilesParent.SetActive(true);
            }
            else
            {
                rangeTilesParent.SetActive(false);
            }
        }
        public void DrowRange(SetupDirection direction = SetupDirection.Right)
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
            //Material rangeTileMaterial = _operatorData.rangeOfOperator.tileRangeMaterial;

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

                        rangeTiles.Add(tileRange.GetComponent<RangeTile>());

                        Vector3 position = Vector3.zero;
                        if (direction == SetupDirection.Bottom || direction == SetupDirection.Top)
                        {
                            position = new Vector3((operatorPositionY - y) * 1f, 0f, (operatorPositionX - x) * 1f);
                        }
                        if (direction == SetupDirection.Left || direction == SetupDirection.Right)
                        {
                            position = new Vector3((y - operatorPositionY) * 1f, 0f, (x - operatorPositionX) * 1f);
                        }
                        //tileRange.GetComponent<MeshRenderer>().materials = new Material[2] { rangeTileMaterial , rangeTileMaterial };

                        tileRange.transform.localPosition = position;
                        //Debug.Log(MainController.KeyOfDeployedOperator(gameObject).gameObject.name);
                        //Debug.Log(MainController.deployedOperatorOnGame.Count);
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
            _operatorHP.GetComponent<UIOperatorHp>().SetMaxHP(hp);
        }
        public void SetHP(int hp)
        {
            _operatorHP.GetComponent<UIOperatorHp>().SetHP(hp);
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
      
            //_rangeOfOperator.getRangeDataElementbyIndex(curX - x);
            //x + 
        }
        public void StartOperator(SetupDirection direction)
        {
            //transform.position;
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
                //Transform transformOperator = operatorObject.transform;
                //operatorObject.armature.Dispose();
                string armatureName = operatorObject.armatureName;

                ChangeArmatureData(operatorObject, armatureName.Replace("_Front", "_Back"), operatorObject.unityData.dataName);
                //DragonBones.UnityFactory.factory.BuildArmatureComponent("Default_Skin_Back", "Tiger_Front", null, null, gameObject, false);


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
            //if (!string.IsNullOrEmpty(operatorObject.animationName))
            //{

            StartCoroutine(AnimationStartOperator());
            //}
        }
        public void EndOperator()
        {
            transform.position = new Vector3(9999f, 9999f, 9999f);

            ChangeArmatureData(operatorObject, operatorObject.armatureName.Replace("_Back", "_Front"), operatorObject.unityData.dataName);

            operatorObject.armature.flipX = false;

            _underlayObject.transform.localEulerAngles = new Vector3(_underlayObject.transform.localEulerAngles.x, 180f, _underlayObject.transform.localEulerAngles.z);

            DrowRange(SetupDirection.Right);

            StopCoroutine(AnimationStartOperator());
        }
        IEnumerator AnimationStartOperator()
        {
            operatorObject._armature.animation.Play("Start", 1);
            //Debug.Log("Animation Start");
            //Debug.Log(operatorObject._armature.animation.isCompleted);
            //yield return new WaitForSeconds(2f);

            while (!operatorObject._armature.animation.isCompleted)
            {
                yield return new WaitForSeconds(.1f);
            }
            //Debug.Log(operatorObject._armature.animation.isCompleted);

            operatorObject._armature.animation.Play("Idle", 0);
            //Debug.Log("Animation Idle");
            yield return null;
        }
        IEnumerator AnimationAttackOperator(Enemy.EnemyControll.EnemyController enemyController = null)
        {
            Debug.Log("Animation AnimationAtackOperator");
            isAttacking = true;
            operatorObject._armature.animation.Play("Attack", 1);

            while (!operatorObject._armature.animation.isCompleted)
            {
                yield return new WaitForSeconds(.1f);
            }

            //AtackDamage()
            if (enemyController != null) enemyController.Damage(_operatorData.Attack);
            isAttacking = false;
            isAttack = false;

            operatorObject._armature.animation.Play("Idle", 0);
            yield return null;
        }
        IEnumerator AnimationAttackLoopOperator(Enemy.EnemyControll.EnemyController enemyController = null)
        {

            isAttacking = true;
            while (isAttackLoop)
            {
                operatorObject._armature.animation.Play("Attack", 1);

                while (!operatorObject._armature.animation.isCompleted)
                {
                    yield return new WaitForSeconds(.1f);
                }

                //AtackDamage()
                if (enemyController != null) {
                    if(!enemyController.Damage(_operatorData.Attack))
                    {
                        isAttackLoop = false;
                        blockingEnemy.Remove(enemyController);
                        //enemyController.transform.position = new Vector3(-9999f, -9999f, -9999f);
                    }
                }
            }
            isAttacking = false;


            operatorObject._armature.animation.Play("Idle", 0);
            yield return null;
        }
    }
}