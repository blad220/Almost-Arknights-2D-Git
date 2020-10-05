using System.Collections.Generic;
using UnityEngine;

//using UnityEngine.UI;

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

    //public static List<DeployedOperatorOnGame> deployedOperatorOnGame = new List<DeployedOperatorOnGame>();

    public static List<KeyValuePair<TileDescription, GameObject>> deployedOperatorOnGame = new List<KeyValuePair<TileDescription, GameObject>>();

    private GameObject _mainObject;
    public static GameObject mainObject;

    public GameObject _OperatorInterfaseObject;
    public static GameObject OperatorInterfaseObject;

    public static int startDP = 0;

    public static UIMainInterfaceFieldsUI mainInterfaceFields;

    public static int _DPcount;
    private float timerDPSlider = 0.0f;
    public static int UnitLimit = 6;

    private void Awake()
    {
        operatorOnGame = _operatorOnGame;

        _mainObject = gameObject;
        mainObject = _mainObject;

        OperatorInterfaseObject = _OperatorInterfaseObject;
    }

    private void Start()
    {
        //"магические числа" переменные у которых в коде объявлены значения внутри функции
        mainInterfaceFields.DPSlider.minValue = 0f;
        mainInterfaceFields.DPSlider.maxValue = 1f;

        _DPcount = startDP;
        mainInterfaceFields.DPSlider.value = startDP;
        mainInterfaceFields.DPField.text = startDP.ToString();

        mainInterfaceFields.UnitLimitField.text = UnitLimit.ToString();
        //StartCoroutine(startDPgeneration(1f));
        //StartCoroutine(startDPgenerationSlider());
    }

    private void Update()
    {
        //снова "магические числа"
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
            //странная конструкция,
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

    public static void decreaseUnitLimit(int limit)
    {
        UnitLimit -= limit;
        mainInterfaceFields.UnitLimitField.text = UnitLimit.ToString();
    }

    //IEnumerator startDPgenerationSlider()
    //{
    //    _DPslider = 0f;
    //    while (true)
    //    {
    //        Debug.Log("Started Coroutine at timestamp : " + Time.deltaTime);
    //        //Debug.Log("Started Coroutine at timestamp : " + Time.time % 1);
    //        //if (_DPslider >= 1f) {
    //        //    _DPcount++;
    //        //    mainInterfaceFields.DPField.text = (Time.time%60).ToString();
    //        //    _DPslider = 0f;
    //        //    mainInterfaceFields.DPSlider.value = 0f;
    //        //}
    //        //mainInterfaceFields.DPSlider.value = _DPslider;

    //        //if (_DPcount < 99)
    //        //{
    //        //    _DPcount = (int)(Time.time % (99 + 1));
    //        //    mainInterfaceFields.DPField.text = _DPcount.ToString();
    //        //    mainInterfaceFields.DPSlider.value = _DPslider;
    //        //    _DPslider = Time.time % 1;
    //        //}
    //        //else
    //        //{
    //        //    mainInterfaceFields.DPField.text = _DPcount.ToString();
    //        //    mainInterfaceFields.DPSlider.value = 0f;
    //        //    _DPcount = 99;
    //        //}
    //        if (_DPslider < 1f)
    //        {
    //            _DPslider += Time.deltaTime ;
    //            mainInterfaceFields.DPSlider.value = _DPslider;
    //        }
    //        else _DPslider = 0f;
    //        yield return new WaitForSeconds(0.1f);
    //    }
    //}
    //IEnumerator startDPgeneration(float speed)
    //{
    //    _DPcount = 0;
    //    while (_DPcount <= 99) {
    //        //StopCoroutine("startDPgenerationSlider");
    //        //StartCoroutine(startDPgenerationSlider());

    //        //_DPslider = 0f;
    //        //while (true)
    //        //{
    //        //    _DPslider = Time.time % 1;
    //        //    mainInterfaceFields.DPSlider.value = _DPslider;
    //        //}

    //        __text.text = _DPcount.ToString();
    //        _DPcount++;

    //        yield return new WaitForSeconds(speed);
    //    }
    //    yield return null;
    //}
    public static OperatorData[] GetOperatorOnGame()
    {
        return operatorOnGame;
    }

    public static void DeployOperator(GameObject operatorObject, TileDescription targetCube)
    {
        if (!isDeployedOperator(operatorObject))
        {
            DeployedOperatorOnGame newDeploy = new DeployedOperatorOnGame
            {
                operatorObject = operatorObject,
                operatorPlaceCube = targetCube
            };

            deployedOperatorOnGame.Add(new KeyValuePair<TileDescription, GameObject>(targetCube, operatorObject));

            decreaseDP(operatorObject.GetComponent<OperatorController>()._operatorData.DpCost);
            decreaseUnitLimit(1);

            targetCube.typeTile = TileDescription.TypeTile.NoneStandableTyle;

            mainInterfaceFields.operatorPanelCreate.checkDPCostAndUnitLimit();
        }
    }

    //public static int getDeployedOperatorIndex(GameObject operatorObject)
    //{
    //    return deployedOperatorOnGame.IndexOf(operatorObject);
    //}
    public static bool isDeployedOperator(GameObject operatorObject)
    {
        foreach (KeyValuePair<TileDescription, GameObject> deployedOperator in deployedOperatorOnGame)
        {
            if (GameObject.ReferenceEquals(deployedOperator.Value, operatorObject))
            {
                return true;
            }
        }
        return false;
        //return !deployedOperatorOnGame.Contains(operatorObject);
    }

    public static bool removeDeployedOperator(GameObject operatorObject)
    {
        foreach (KeyValuePair<TileDescription, GameObject> deployedOperator in deployedOperatorOnGame)
        {
            if (deployedOperator.Value == operatorObject)
            {
                deployedOperatorOnGame.Remove(deployedOperator);
                return true;
            }
        }
        return false;
        //deployedOperatorOnGame.Remove(operatorObject);
        //UnitLimit - deployedOperatorOnGame.Count;

        //if(UnitLimit>0) set all avatar Enabled
    }

    //void addDP {DP++; Text; //for if(AvatarUnitCost<UnitCost) set AvatarUnitCost avatar Disabled}
    //void delDP {DP--; Text; //for if(AvatarUnitCost<UnitCost) set AvatarUnitCost avatar Disabled}
    //void changeSpeedDP {stopCor; start}
}