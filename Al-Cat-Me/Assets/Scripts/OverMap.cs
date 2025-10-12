using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Overmap : MonoBehaviour
{
    public static Overmap Get;

    public List<OvermapNode> StartingNodes;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Get != null && Get != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Get = this;
        }

        if(StartingNodes == null || StartingNodes.Count <= 0)
        {
            Debug.LogError("Assign starting nodes");
        }
        // Must be persistent throughout game
        DontDestroyOnLoad(this);

        foreach(var node in StartingNodes)
        {
            node.NodeButton.onClick.AddListener(() => DisableOtherStartingNodes(node));
        }
    }

    public void LoadScenario(Scenario scenario)
    {
        switch (scenario)
        {
            case Scenario.combat:
                break;
            case Scenario.rest:
                break;  
            case Scenario.shop:
                break;  
            case Scenario.boss:
                break;
        }
    }
    
    public void DisableOtherStartingNodes(OvermapNode triggeringNode)
    {
        foreach (var node in StartingNodes)
        {
            if (node != triggeringNode)
            {
                node.Selectable = false;
            }
            node.NodeButton.onClick.RemoveListener(() => DisableOtherStartingNodes(node));
        }
    }
}
