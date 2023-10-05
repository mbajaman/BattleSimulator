using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    public List<TabBtn> tabButtons;
   
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

    }

    public void OnTabExit(TabBtn button)
    {

    }

    public void OnTabSelected(TabBtn button)
    {

    }
}
