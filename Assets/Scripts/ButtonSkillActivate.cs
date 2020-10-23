using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MainController
{
    public class ButtonSkillActivate : MonoBehaviour, IPointerDownHandler
    {
        public Slider slider;

        private void Start()
        {
            slider = GetComponent<Slider>();
        }
        public void SkillActivate()
        {
            MainController.mainInterfaceFields.selectOperatorUI.operatorDataSelect.SkillActivate(MainController.mainInterfaceFields.selectOperatorUI.operatorController);
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if (MainController.mainInterfaceFields.selectOperatorUI.operatorDataSelect.skill.isCanActive &&
                !MainController.mainInterfaceFields.selectOperatorUI.operatorDataSelect.skill.isActive)
            {
                SkillActivate();
                MainController.mainInterfaceFields.selectOperatorUI.displaySelectedPanel.SetActive(true);
            }
        }
    }
}