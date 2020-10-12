using Operator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class MainController: MonoBehaviour
{
    public OperatorData[] _operatorOnGame;
    public static OperatorData[] operatorOnGame;

    public List<GameObject> _deployedOperatorOnGame = new List<GameObject>();

    [System.Serializable]
    public class  DeployedOperatorOnGame
    {
        public GameObject operatorObject;
        public TileDescription operatorPlaceCube;
    }
    //public static List<DeployedOperatorOnGame> deployedOperatorOnGame = new List<DeployedOperatorOnGame>();

    public static List<KeyValuePair<TileDescription, GameObject>> deployedOperatorOnGame = new List<KeyValuePair<TileDescription, GameObject>>();

    private GameObject _mainObject;
    public static GameObject mainObject;

    public GameObject _OperatorInterfaseObject;
    public static GameObject OperatorInterfaseObject;

    public static int startDP = 11;
    
    public static UIMainInterfaceFieldsUI mainInterfaceFields;

    public static int _DPcount;
    private float timerDPSlider = 0.0f;
    public static int UnitLimit = 6;

    private static float timeScaleBefore;
    void Awake()
    {
        operatorOnGame = _operatorOnGame;

        _mainObject = gameObject;
        mainObject = _mainObject;

        OperatorInterfaseObject = _OperatorInterfaseObject;

        SetCurrentTimeScale(Time.timeScale);
    }
    void Start()
    {
        mainInterfaceFields.DPSlider.minValue = 0f;
        mainInterfaceFields.DPSlider.maxValue = 1f;

        _DPcount = startDP;
        mainInterfaceFields.DPSlider.value = startDP;
        mainInterfaceFields.DPField.text = startDP.ToString();

        mainInterfaceFields.UnitLimitField.text = UnitLimit.ToString();
        //StartCoroutine(startDPgeneration(1f));
        //StartCoroutine(startDPgenerationSlider());
    }
    
    void Update()
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
        else {
            if (timerDPSlider != 0.0f)
            {
                timerDPSlider = 0.0f;
                mainInterfaceFields.DPSlider.value = 0.0f;
            }
        }
    }
    public static void decreaseDP(int dp)
    {
        _DPcount -= dp;
        //Debug.Log($"{_DPcount} - {dp} = {_DPcount-dp}");
        mainInterfaceFields.DPField.text = _DPcount.ToString();
    }
    public static void increaseDP(int dp)
    {
        _DPcount += dp;
        //Debug.Log($"{_DPcount} - {dp} = {_DPcount-dp}");
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

            TimeScaleReset();

        }

    }
    public static bool removeDeployedOperator(GameObject operatorObject)
    {
        foreach (KeyValuePair<TileDescription, GameObject> deployedOperator in deployedOperatorOnGame)
        {
            if (deployedOperator.Value == operatorObject)
            {
                deployedOperatorOnGame.Remove(deployedOperator);

                OperatorController operatorController = operatorObject.GetComponent<OperatorController>();
                increaseDP((int)(operatorController._operatorData.DpCost / 2));
                increaseUnitLimit(1);

                deployedOperator.Key.Reset();

                mainInterfaceFields.operatorPanelCreate.checkDPCostAndUnitLimit();

                operatorController.EndOperator();

                return true;
            }
        }
        return false;
    }
    //public static int getDeployedOperatorIndex(GameObject operatorObject)
    //{
    //    return deployedOperatorOnGame.IndexOf(operatorObject);
    //}
    public static bool isDeployedOperator(GameObject operatorObject)
    {

        foreach (KeyValuePair<TileDescription, GameObject> deployedOperator in deployedOperatorOnGame)
        {
            if (GameObject.ReferenceEquals(deployedOperator.Value, operatorObject)) return true;
        }
        return false;
        //return !deployedOperatorOnGame.Contains(operatorObject);
    }
    public static TileDescription KeyOfDeployedOperator(GameObject operatorObject)
    {

        foreach (KeyValuePair<TileDescription, GameObject> deployedOperator in deployedOperatorOnGame)
        {
            if (GameObject.ReferenceEquals(deployedOperator.Value, operatorObject)) return deployedOperator.Key;
        }
        return null;
        //return !deployedOperatorOnGame.Contains(operatorObject);
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
}
