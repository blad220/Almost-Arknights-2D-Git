using UnityEngine;
using UnityEngine.UI;

namespace MainController.Ui
{
    public class UIMainInterfaceFieldsUI : MonoBehaviour
    {
        public Text DPField;
        public Slider DPSlider;

        public Text EnemyColField;
        public Text ColBrokeThroughEnemyField;

        public Text UnitLimitField;
        public OperatorPanelCreate operatorPanelCreate;
        public UISelectOperatorUI selectOperatorUI;
        public UISelectRotateUI selectRotateUI;

        void Awake()
        {
            UIMainInterfaceFieldsUI link = MainController.mainInterfaceFields;
            if (link == null) MainController.mainInterfaceFields = gameObject.GetComponent<UIMainInterfaceFieldsUI>();
        }

        public void SetEnemyColField(int value, int maxValue)
        {
            EnemyColField.text = $"{value}/{maxValue}";
        }

        public void SetColBrokeThroughEnemyField(int value)
        {
            ColBrokeThroughEnemyField.text = $"{value}";
        }
    }
}