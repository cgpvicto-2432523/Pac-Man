using UnityEngine;
using UnityEngine.SceneManagement;

public class TestButton : MonoBehaviour
{
    public void TestClick()
    {
        Debug.Log("BOUTON CLIQUÉ!");
        SceneManager.LoadScene("Level1");
    }
}