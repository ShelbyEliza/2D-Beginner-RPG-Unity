using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MyUIHandler : MonoBehaviour {
    public float displayTime = 4.0f;
    private VisualElement m_NonPlayerDialogue;
    private float m_TimerDisplay;

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

        m_NonPlayerDialogue = uiDocument.rootVisualElement.Q<VisualElement>("NPCDialogue");
        m_NonPlayerDialogue.style.display = DisplayStyle.None;
        m_TimerDisplay = -1.0f;
    }

    public void SetHealthValue(float percentage) {
        m_Healthbar.style.width = Length.Percent(100 * percentage);
    }

    void Update() {
        if (m_TimerDisplay > 0) {
            m_TimerDisplay -= Time.deltaTime;

            if (m_TimerDisplay < 0) {
                m_NonPlayerDialogue.style.display = DisplayStyle.None;
            }
        } 
    }

    public void DisplayDialogue(string dialog) {
        m_TimerDisplay = displayTime;
        m_NonPlayerDialogue.style.display = DisplayStyle.Flex;

        foreach (VisualElement child in m_NonPlayerDialogue[0].Children()) {
            if (child.viewDataKey == dialog) {
                child.style.display = DisplayStyle.Flex;
            } else {
                child.style.display = DisplayStyle.None;     
            }
        }
    }
}