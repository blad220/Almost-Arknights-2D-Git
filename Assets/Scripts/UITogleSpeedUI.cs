using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MainController.Ui
{

    public class UITogleSpeedUI : MonoBehaviour, IPointerClickHandler
    {
        public bool isFast;
        public Text textField;

        void Start()
        {
            textField.text = "1X";
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            isFast = !isFast;
            if (isFast)
            {
                MainController.SetCurrentTimeScale(2f);
                textField.text = "2X";
            }
            else
            {
                MainController.SetCurrentTimeScale(1f);
                textField.text = "1X";
            }
        }

    }
}