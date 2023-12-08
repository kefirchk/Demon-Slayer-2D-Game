using UnityEngine;
using UnityEngine.UI;

public class PowerBar : MonoBehaviour
{
    Image powerBar;
    public float curPower;
    public float maxPower = 100f;
    // Start is called before the first frame update
    void Start()
    {
        powerBar = GetComponent<Image>();
        curPower = maxPower;
    }

    // Update is called once per frame
    void Update()
    {
        curPower = Hero.Instance.power;
        powerBar.fillAmount = curPower / maxPower;
    }
}
