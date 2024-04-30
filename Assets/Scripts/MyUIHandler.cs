using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MyUIHandler : MonoBehaviour
{
    public static MyUIHandler Instance { get; private set; }
    private VisualElement m_Healthbar;

    private void Awake() {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        m_Healthbar = uiDocument.rootVisualElement.Q<VisualElement>("HealthBar");
        SetHealthValue(1.0f);
    }

    public void SetHealthValue(float percentage) {
        m_Healthbar.style.width = Length.Percent(100 * percentage);
    }
}