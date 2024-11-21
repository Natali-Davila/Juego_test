using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class animalesSound : MonoBehaviour
{
    public Button buttonDetect;
    private GameObject player;

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            if (buttonDetect != null)
                buttonDetect.gameObject.SetActive(true);
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            if (buttonDetect != null)
                buttonDetect.gameObject.SetActive(false);
        }
    }
    private void Start()
    {
        if (buttonDetect != null)
            buttonDetect.gameObject.SetActive(false);

        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("no se encontro el GameObject");
        }

        if (buttonDetect != null)
        {
            buttonDetect.onClick.AddListener(ActivarObjeto);
        }
    }
    //public void ActivarObjeto()
    //{
    //

    //    Destroy(gameObject);

    //    if (gameObject != null)
    //    {
    //        buttonDetect.gameObject.SetActive(false);
    //    }
    //}
    public void ActivarObjeto()
    {

        SceneManager.LoadScene("arcadeAnimalesSonidos");
    }
}

