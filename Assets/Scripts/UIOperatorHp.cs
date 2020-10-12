using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOperatorHp : MonoBehaviour
{
    public Text textField;
    private Slider hpBar;
    public Gradient gradient;
    public Image fill;

    // Start is called before the first frame update
    void Awake()
    {
        hpBar = gameObject.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(textField != null) textField.text = $"{hpBar.value}/{hpBar.maxValue}";
        fill.color = gradient.Evaluate(hpBar.normalizedValue);

    }
    public int GetHPValue()
    {
        return (int)hpBar.value;
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
