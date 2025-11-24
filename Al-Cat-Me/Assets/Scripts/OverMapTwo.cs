using UnityEngine;
using UnityEngine.SceneManagement;

public class OverMapTwo : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnButtonClick()
    {
        SceneManager.LoadScene("CastSpells");
    }

    public void combat()
    {
        SceneManager.LoadScene("CastSpells");
    }

    public void shop()
    {
        SceneManager.LoadScene("ShopScene");
    }

    public void quitGame()
    {
        Debug.Log("quit button was pushed");
        Application.Quit();
    }
}
