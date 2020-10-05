using UnityEngine;
using UnityEngine.UI;

public class UIOperatorHp : MonoBehaviour
{
    public Text textField;
    private Slider hpBar;
    public Gradient gradient;
    public Image fill;

    private void Awake()
    {
        hpBar = gameObject.GetComponent<Slider>();
    }

    //обновление значений в апдейте плохая идея. Лучше сделать это через свойства или callback
    private void Update()
    {
        if (textField != null)
        {
            textField.text = $"{hpBar.value}/{hpBar.maxValue}";
        }

        fill.color = gradient.Evaluate(hpBar.normalizedValue);
    }

    public void SetMaxHP(int health)
    {
        hpBar.maxValue = health;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetHP(int health)
    {
        hpBar.value = health;
        fill.color = gradient.Evaluate(hpBar.normalizedValue);
    }
}