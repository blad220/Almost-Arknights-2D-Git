using MainController.Map.Tile;
using MainController.Operator;
using MainController.Operator.Range;
using MainController.Ui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainController
{
    public class MainController : MonoBehaviour
    {
        public OperatorData[] _operatorOnGame;
        public static OperatorData[] operatorOnGame;

        public List<GameObject> _deployedOperatorOnGame = new List<GameObject>();

        [System.Serializable]
        public class DeployedOperatorOnGame
        {
            public GameObject operatorObject;
            public TileDescription operatorPlaceCube;
        }

        public static List<KeyValuePair<TileDescription, GameObject>> deployedOperatorOnGame = new List<KeyValuePair<TileDescription, GameObject>>();

        private GameObject _mainObject;
        public static GameObject mainObject;

        public GameObject _OperatorInterfaseObject;
        public static GameObject OperatorInterfaseObject;

        public int startDP = 99;
        public static int _startDP;

        public static UIMainInterfaceFieldsUI mainInterfaceFields;

        public static int _DPcount;
        private static float timerDPSlider = 0.0f;
        public static int UnitLimitStart = 6;
        public static int UnitLimit = 6;

        public static int EnemyCol = 0;
        public static int EnemyMax = 0;
        public static int EnemyBrokeThrough = 0;
        public static int curEnemyBrokeThrough = 0;

        private static float timeScaleBefore = 0f;

        void Awake()
        {
            operatorOnGame = _operatorOnGame;

            _mainObject = gameObject;
            mainObject = _mainObject;

            OperatorInterfaseObject = _OperatorInterfaseObject;

            SetCurrentTimeScale(1f);

            EnemyMax = 0;

            _startDP = startDP;
            _DPcount = startDP;

            UnitLimit = UnitLimitStart;
        }
        void Start()
        {
            mainInterfaceFields.DPSlider.minValue = 0f;
            mainInterfaceFields.DPSlider.maxValue = 1f;


            mainInterfaceFields.DPSlider.value = _startDP;
            mainInterfaceFields.DPField.text = _startDP.ToString();

            mainInterfaceFields.UnitLimitField.text = UnitLimit.ToString();

            StartCoroutine(DpCounterStart());
        }

        private static IEnumerator DpCounterStart()
        {
            while (true)
            {
                if (_DPcount < 99)
                {
                    timerDPSlider += Time.deltaTime;

                    if (timerDPSlider >= 1f)
                    {
                        _DPcount++;

                        mainInterfaceFields.operatorPanelCreate.checkDPCostAndUnitLimit();

                        mainInterfaceFields.DPField.text = _DPcount.ToString();

                        timerDPSlider = timerDPSlider % 1;

                    }
                    mainInterfaceFields.DPSlider.value = timerDPSlider;
                }
                else
                {
                    if (timerDPSlider != 0.0f)
                    {
                        timerDPSlider = 0.0f;
                        mainInterfaceFields.DPSlider.value = 0.0f;
                    }
                }
                yield return new WaitForSeconds(0.03f);
            }
            yield return null;
        }
        public static void decreaseDP(int dp)
        {
            _DPcount -= dp;
            mainInterfaceFields.DPField.text = _DPcount.ToString();
        }
        public static void increaseDP(int dp)
        {
            _DPcount += dp;
            mainInterfaceFields.DPField.text = _DPcount.ToString();
        }
        public static void decreaseUnitLimit(int limit)
        {
            UnitLimit -= limit;
            mainInterfaceFields.UnitLimitField.text = UnitLimit.ToString();
        }
        public static void increaseUnitLimit(int limit)
        {
            UnitLimit += limit;
            mainInterfaceFields.UnitLimitField.text = UnitLimit.ToString();
        }
        public static OperatorData[] GetOperatorOnGame()
        {
            return operatorOnGame;
        }
        public static void ClearOperatorFromAllTiles(OperatorController _operatorController)
        {
            foreach (KeyValuePair<TileDescription, GameObject> deployedOperator in deployedOperatorOnGame)
            {
                foreach (RangeTile rangeTile in deployedOperator.Value.GetComponent<OperatorController>().rangeTiles)
                {
                    rangeTile.operatorInTile.Remove(_operatorController);
                }
            }
        }
        public static void DeployOperator(GameObject operatorObject, TileDescription targetCube)
        {
            if (!isDeployedOperator(operatorObject))
            {
                DeployedOperatorOnGame newDeploy = new DeployedOperatorOnGame();
                newDeploy.operatorObject = operatorObject;
                newDeploy.operatorPlaceCube = targetCube;

                deployedOperatorOnGame.Add(new KeyValuePair<TileDescription, GameObject>(targetCube, operatorObject));

                OperatorController operatorController = operatorObject.GetComponent<OperatorController>();
                decreaseDP(operatorController._operatorData.DpCost);
                decreaseUnitLimit(1);

                targetCube.typeTile = TileDescription.TypeTile.NoneStandableTyle;

                mainInterfaceFields.operatorPanelCreate.checkDPCostAndUnitLimit();

                operatorController.HideRange();
                mainInterfaceFields.selectOperatorUI.displaySelectedPanel.SetActive(false);

                operatorObject.GetComponent<CapsuleCollider>().enabled = true;

                foreach (RangeTile tileRange in operatorController.rangeTiles)
                {
                    tileRange.GetComponent<BoxCollider>().enabled = true;
                }

                operatorController.SetStartHP(operatorController._operatorData.maxHP);
                operatorController._operatorHP.GetComponent<UIOperatorHp>().SetHPCurMax(operatorController._operatorData.maxHP, operatorController._operatorData.maxHP);

                TimeScaleReset();

            }

        }
        public static bool removeDeployedOperator(GameObject operatorObject)
        {
            foreach (KeyValuePair<TileDescription, GameObject> deployedOperator in deployedOperatorOnGame)
            {
                if (deployedOperator.Value == operatorObject)
                {
                    OperatorController operatorController = operatorObject.GetComponent<OperatorController>();

                    deployedOperatorOnGame.Remove(deployedOperator);


                    increaseDP((int)(operatorController._operatorData.DpCost / 2));
                    increaseUnitLimit(1);

                    deployedOperator.Key.Reset();

                    mainInterfaceFields.operatorPanelCreate.checkDPCostAndUnitLimit();

                    operatorController.EndOperator();

                    mainInterfaceFields.selectOperatorUI.displaySelectedPanel.SetActive(false);

                    operatorController._operatorData.skill.curSkillPoint = operatorController._operatorData.skill.initCost;
                    operatorController._operatorData.skill.isCanActive = false;

                    return true;
                }
            }
            return false;
        }

        public static bool isDeployedOperator(GameObject operatorObject)
        {
            foreach (KeyValuePair<TileDescription, GameObject> deployedOperator in deployedOperatorOnGame)
            {
                if (GameObject.ReferenceEquals(deployedOperator.Value, operatorObject)) return true;
            }
            return false;
        }
        public static TileDescription KeyOfDeployedOperator(GameObject operatorObject)
        {
            foreach (KeyValuePair<TileDescription, GameObject> deployedOperator in deployedOperatorOnGame)
            {
                if (GameObject.ReferenceEquals(deployedOperator.Value, operatorObject)) return deployedOperator.Key;
            }
            return null;
        }
        public static void SetCurrentTimeScale(float _time, bool isReseting = true)
        {
            if (isReseting)
            {
                timeScaleBefore = Time.timeScale;
            }
            Time.timeScale = _time;
        }
        public static void TimeScaleReset()
        {
            Time.timeScale = timeScaleBefore;
        }
        public static void SetEnemyColFieldStart(int value, int maxValue)
        {
            EnemyCol = value;
            EnemyMax = maxValue;
            mainInterfaceFields.SetEnemyColField(value, maxValue);
            CheckWin();
        }
        public static void SetEnemyColField(int value)
        {
            EnemyCol = value;
            mainInterfaceFields.SetEnemyColField(value, EnemyMax);
            CheckWin();
        }
        private static void CheckWin()
        {
            if (EnemyCol >= EnemyMax - (EnemyBrokeThrough - curEnemyBrokeThrough))
            {
                WinGame();
            }
        }
        private static void CheckLoose()
        {
            if (curEnemyBrokeThrough <= 0)
            {
                LooseGame();
            }
        }
        public static void SetBrokeThroughEnemyStart(int value)
        {
            EnemyBrokeThrough = value;
            curEnemyBrokeThrough = value;
            mainInterfaceFields.SetColBrokeThroughEnemyField(value);
        }
        public static void SetBrokeThroughEnemy(int value)
        {
            curEnemyBrokeThrough = value;
            mainInterfaceFields.SetColBrokeThroughEnemyField(curEnemyBrokeThrough);
            CheckLoose();
            CheckWin();
        }
        public static void WinGame()
        {
            SceneManager.LoadScene("Win");
        }
        public static void LooseGame()
        {
            SceneManager.LoadScene("Loose");
        }
        public static void LoadMainGame()
        {
            SceneManager.LoadScene("GamePlayScene");
        }
        public static void LoadMainMenu()
        {
            SceneManager.LoadScene("Menu");
        }
    }
}