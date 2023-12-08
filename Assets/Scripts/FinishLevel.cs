using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevel : MonoBehaviour
{
    public void FinishLvl()
    {
        // Сохранение значения currentLevel
        PlayerPrefs.SetInt("currentLevel", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.Save();

        SceneManager.LoadScene(1);
    }
}
