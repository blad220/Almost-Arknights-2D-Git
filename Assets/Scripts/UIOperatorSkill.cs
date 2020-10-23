using UnityEngine;
using UnityEngine.UI;

namespace MainController.Ui
{

    public class UIOperatorSkill : MonoBehaviour
    {
        public Text textField;
        public Gradient gradient;
        public Image fill;

        private Slider skillBar;

        void Awake()
        {
            skillBar = gameObject.GetComponent<Slider>();
        }

        void Update()
        {
            if (textField != null) textField.text = $"{skillBar.value}/{skillBar.maxValue}";
            fill.color = gradient.Evaluate(skillBar.normalizedValue);
        }

        public void SetMaxSkillPoint(int skillPoint)
        {
            skillBar.maxValue = skillPoint;

            fill.color = gradient.Evaluate(1f);
        }
        public void SetSkillPoint(int skillPoint)
        {
            skillBar.value = skillPoint;
            fill.color = gradient.Evaluate(skillBar.normalizedValue);
        }
    }
}