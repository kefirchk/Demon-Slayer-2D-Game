using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    [SerializeField] public string type;
    [SerializeField] public AudioSource getBonusSound;
    public void Destroy()
    {
        StartCoroutine(DestroyWithSound());
    }

    private IEnumerator DestroyWithSound()
    {
        getBonusSound.Play();

        gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;


        // Подождать, пока звук не закончится
        yield return new WaitForSeconds(getBonusSound.clip.length);

        // Уничтожить объект
        Destroy(this.gameObject);
    }
}
