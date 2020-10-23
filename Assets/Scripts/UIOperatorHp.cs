using UnityEngine;
using UnityEngine.UI;

namespace MainController.Ui
{

    public class UIOperatorHp : MonoBehaviour
    {
        public Text textField;
        private Slider hpBar;
        public Gradient gradient;
        public Image fill;

        void Awake()
        {
            hpBar = gameObject.GetComponent<Slider>();
        }

        public int GetHPValue()
        {
            return (int)hpBar.value;
        }
        public void SetMaxHP(int health)
        {
            hpBar.maxValue = health;
            fill.color = gradient.Evaluate(1f);
            if (textField != null) textField.text = $"{hpBar.value}/{hpBar.maxValue}";
        }
        public void SetHP(int health)
        {
            hpBar.value = health;
            if (textField != null) textField.text = $"{hpBar.value}/{hpBar.maxValue}";
            fill.color = gradient.Evaluate(hpBar.normalizedValue);
        }
        public void SetHPCurMax(int health, int maxHealth)
        {
            hpBar.value = health;
            fill.color = gradient.Evaluate(1f);
            hpBar.maxValue = maxHealth;
            if (textField != null) textField.text = $"{hpBar.value}/{hpBar.maxValue}";
            fill.color = gradient.Evaluate(hpBar.normalizedValue);
        }
    }
}