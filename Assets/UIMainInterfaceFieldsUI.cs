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

    private void Awake()
    {
        UIMainInterfaceFieldsUI link = MainController.mainInterfaceFields;
        if (link == null)
        {
            MainController.mainInterfaceFields = gameObject.GetComponent<UIMainInterfaceFieldsUI>();
        }
    }
}