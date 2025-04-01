using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PopupManager : MonoBehaviour
{
    public GameObject popupInstrucciones;  
    public Button closeButton;  

    void Start()
    {
        
        if (SceneManager.GetActiveScene().name == "House Interior")
        {
            popupInstrucciones.SetActive(true);
        }

        closeButton.onClick.AddListener(CerrarPopup);
    }

    void CerrarPopup()
    {
      
        popupInstrucciones.SetActive(false);
    }
}
