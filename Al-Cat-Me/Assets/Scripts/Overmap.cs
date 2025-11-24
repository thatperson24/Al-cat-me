using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
                SceneManager.LoadScene("CastSpells");
                break;
            case Scenario.rest:
                SceneManager.LoadScene("ShopScene");
                break;  
            case Scenario.shop:
                SceneManager.LoadScene("ShopScene");
                break;  
            case Scenario.boss:
                SceneManager.LoadScene("CastSpells");
                break;
        }
    }
    
    public void OnButtonClick()
    {
        SceneManager.LoadScene("CastSpells");
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //if (scene.name == "OvermapScene")
        //{
        //    GenerateNewNodes();
        //}
    }

    private void GenerateNewNodes()
    {
        // Example: clear old nodes and spawn new ones
        foreach (var node in StartingNodes)
        {
            Destroy(node.gameObject);
        }
        StartingNodes.Clear();

        // Instantiate new nodes however you want (prefabs, procedural, etc.)
        // Example:
        // var newNode = Instantiate(nodePrefab, somePosition, Quaternion.identity);
        // StartingNodes.Add(newNode.GetComponent<OvermapNode>());
    }

}
