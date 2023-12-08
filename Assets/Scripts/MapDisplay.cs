using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MapDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text mapName;
    [SerializeField] private TMP_Text mapDescription;
    [SerializeField] private UnityEngine.UI.Image mapImage;
    [SerializeField] private UnityEngine.UI.Button playButton;
    [SerializeField] private GameObject lockImage;

    public int index = 0;

    private void Awake()
    {
        playButton.onClick.AddListener(PlayButtonClicked); // Добавляем метод ButtonClicked в качестве слушателя события нажатия кнопки
    }
    public void DisplayMap(Map map)
    {
        mapName.text = map.mapName;
        mapDescription.text = map.mapDescription;
        mapImage.sprite = map.mapImage;
 
        bool mapUnlocked = (PlayerPrefs.GetInt("currentLevel", 0) - 2) >= map.mapIndex;
        Debug.Log("GetCurrrentLevel = " + PlayerPrefs.GetInt("currentLevel", 0));
        Debug.Log("MapIndex = " + map.mapIndex);

        lockImage.SetActive(!mapUnlocked);
        playButton.gameObject.SetActive(mapUnlocked);

        if (mapUnlocked)
            mapImage.color = Color.white;
        else
            mapImage.color = Color.grey;

        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(() => 
        SceneManager.LoadScene(map.mapName));//sceneToLoad.name));
    }

    private void PlayButtonClicked()
    {
        Destroy(GameObject.Find("AudioPhone"));
    }
}
