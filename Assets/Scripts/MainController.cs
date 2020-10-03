using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class MainController: MonoBehaviour
{
    public OperatorData[] _operatorOnGame;
    public static OperatorData[] operatorOnGame;

    public List<GameObject> _deployedOperatorOnGame = new List<GameObject>();
    public static List<GameObject> deployedOperatorOnGame = new List<GameObject>();

    private GameObject _mainObject;
    public static GameObject mainObject;

    public GameObject _OperatorInterfaseObject;
    public static GameObject OperatorInterfaseObject;

    public static int startDP = 0;
    public static UISelectOperatorUI selectOperatorUI;
    public static UIMainInterfaceFieldsUI mainInterfaceFields;

    public static int _DPcount;
    private float timerDPSlider = 0.0f;
    public static int UnitLimit = 6;

    void Awake()
    {
        operatorOnGame = _operatorOnGame;

        _mainObject = gameObject;
        mainObject = _mainObject;

        OperatorInterfaseObject = _OperatorInterfaseObject;
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

                mainInterfaceFields.operatorPanelCreate.checkDPCost();

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
        _DPcount =- dp;
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
    public static void DeployOperator(GameObject operatorObject)
    {
        if (!isDeployedOperator(operatorObject))
        {
            deployedOperatorOnGame.Add(operatorObject);
            //UnitLimit - deployedOperatorOnGame.Count;
            //UnitCost;
            //if(UnitLimit<=0) set all avatar Disabled
            decreaseDP(operatorObject.GetComponent<OperatorController>()._operatorData.DpCost);
            decreaseUnitLimit(1);
            mainInterfaceFields.operatorPanelCreate.checkDPCost();
            //Debug.Log(deployedOperatorOnGame.Count);
        }
    }
    public static int getDeployedOperatorIndex(GameObject operatorObject)
    {
        return deployedOperatorOnGame.IndexOf(operatorObject);
    }
    public static bool isDeployedOperator(GameObject operatorObject)
    {
        return deployedOperatorOnGame.Contains(operatorObject);
    }
    public static void removeDeployedOperator(GameObject operatorObject)
    {
        deployedOperatorOnGame.Remove(operatorObject);
        //UnitLimit - deployedOperatorOnGame.Count;

        //if(UnitLimit>0) set all avatar Enabled
    }
    //void addDP {DP++; Text; //for if(AvatarUnitCost<UnitCost) set AvatarUnitCost avatar Disabled}
    //void delDP {DP--; Text; //for if(AvatarUnitCost<UnitCost) set AvatarUnitCost avatar Disabled}
    //void changeSpeedDP {stopCor; start}
}
