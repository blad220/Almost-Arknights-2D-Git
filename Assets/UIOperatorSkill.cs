using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOperatorSkill : MonoBehaviour
{
    public Text textField;
    private Slider skillBar;
    public Gradient gradient;
    public Image fill;

    // Start is called before the first frame update
    void Awake()
    {
        skillBar = gameObject.GetComponent<Slider>();
    }

    // Update is called once per frame
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
