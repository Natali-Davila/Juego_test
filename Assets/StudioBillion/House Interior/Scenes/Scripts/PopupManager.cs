using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public GameObject popupInstrucciones;
    public Button closeButton;
    public Button instruccionesButton;

    void Start()
    {
        popupInstrucciones.SetActive(false);

        closeButton.onClick.AddListener(CerrarPopup);
        instruccionesButton.onClick.AddListener(MostrarPopup);
    }

    void CerrarPopup()
    {
        popupInstrucciones.SetActive(false);
    }

    void MostrarPopup()
    {
        popupInstrucciones.SetActive(true);
    }
}




