using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<TabBtn> tabButtons;

    public Color tabIdle;
    public Color tabActive;

    public void Subscribe(TabBtn button)
    {
        if(tabButtons == null)
        {
            tabButtons = new List<TabBtn>();
        }

        tabButtons.Add(button);
    }

    public void OnTabEnter(TabBtn button)
    {
        ResetTabs();
    }

    public void OnTabExit(TabBtn button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabBtn button)
    {
        ResetTabs();
        button.background.color = tabActive;
    }

    public void ResetTabs()
    {
        foreach(TabBtn button in tabButtons)
        {
            button.background.color = tabIdle;
        }
    }
}
