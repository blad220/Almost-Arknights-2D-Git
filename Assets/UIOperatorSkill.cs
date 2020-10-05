using UnityEngine;
using UnityEngine.UI;

public class UIOperatorSkill : MonoBehaviour
{
    public Image fill;
    public Gradient gradient;
    public Text textField;
    private Slider skillBar;

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

    private void Awake()
    {
        skillBar = gameObject.GetComponent<Slider>();
    }

    private void Update()
    {
        if (textField != null)
        {
            textField.text = $"{skillBar.value}/{skillBar.maxValue}";
        }
        fill.color = gradient.Evaluate(skillBar.normalizedValue);
    }
}