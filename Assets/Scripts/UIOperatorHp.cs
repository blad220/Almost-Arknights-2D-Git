using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOperatorHp : MonoBehaviour
{
    public Text textField;
    private Slider hpBar;
    // Start is called before the first frame update
    void Start()
    {
        hpBar = gameObject.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(textField != null) textField.text = $"{hpBar.value}/{hpBar.maxValue}";
    }
}
