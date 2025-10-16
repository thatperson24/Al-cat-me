using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Scenario { combat, shop, rest, boss};
public class OvermapNode : MonoBehaviour
{
    public bool Selectable;
    public bool Completed;
    public Button NodeButton;
    public LineRenderer ConnectionVisualTemplate;
    private List<LineRenderer> lineVisuals = new();

    public Scenario LinkedScenario;
    public List<OvermapNode> NextNodes;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Selectable = false;
        Completed = false;

        NodeButton.onClick.AddListener(() => Overmap.Get.LoadScenario(LinkedScenario));
        foreach (var node in NextNodes)
        {
            var lineObject = Instantiate(ConnectionVisualTemplate.gameObject, transform);
            var lineMesh = lineObject.GetComponent<LineRenderer>();
            lineVisuals.Add(lineMesh);
            var linePoints = new List<Vector3>();
            linePoints.Add(node.transform.position);
            linePoints.Add(transform.position);
            lineMesh.SetPositions(linePoints.ToArray());
            lineObject.SetActive(true);
        }
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
