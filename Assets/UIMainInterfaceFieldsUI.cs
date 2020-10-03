using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainInterfaceFieldsUI : MonoBehaviour
{
    public Text DPField;
    public Slider DPSlider;

    public Text UnitLimitField;
    public OperatorPanelCreate operatorPanelCreate;
    public UISelectOperatorUI selectOperatorUI;
    public UISelectRotateUI selectRotateUI;

    void Awake()
    {
        UIMainInterfaceFieldsUI link = MainController.mainInterfaceFields;
        if (link == null) MainController.mainInterfaceFields = gameObject.GetComponent<UIMainInterfaceFieldsUI>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
