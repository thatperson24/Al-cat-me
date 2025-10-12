using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Scenario { combat, shop, rest, boss};
public class OvermapNode : MonoBehaviour
{
    public bool Selectable;
    public bool Completed;
    public Button NodeButton;

    public Scenario LinkedScenario;
    public List<OvermapNode> NextNodes;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Selectable = false;
        Completed = false;

        NodeButton.onClick.AddListener(() => Overmap.Get.LoadScenario(LinkedScenario));
    }

    public void CompleteNodeLevel()
    {
        Completed = true;
        foreach (var node in NextNodes)
        {
            node.Selectable = true;
        }
        NodeButton.enabled = false;
    }
}
