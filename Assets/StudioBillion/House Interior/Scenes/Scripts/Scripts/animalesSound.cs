using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class animalesSound : MonoBehaviour
{
    public Button buttonDetect;
    private GameObject player;

    private void Start()
    {
        if (buttonDetect != null)
            buttonDetect.gameObject.SetActive(false);

        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("No se encontró el GameObject con la etiqueta 'Player'");
        }

        if (buttonDetect != null)
        {
            buttonDetect.onClick.AddListener(ActivarObjeto);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (buttonDetect != null)
                buttonDetect.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (buttonDetect != null)
                buttonDetect.gameObject.SetActive(false);
        }
    }
    public void ActivarObjeto()
    {

        SceneManager.LoadScene("AnimalSonidos");
    }
}

