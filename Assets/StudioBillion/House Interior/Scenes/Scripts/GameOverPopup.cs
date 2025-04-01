using UnityEngine;
using UnityEngine.UI;

public class GameOverPopup : MonoBehaviour
{
    public GameObject popupPanel;  
    public Text messageText;       
    public Button yesButton;      
    public Button noButton;        

    void Start()
    {
       
        popupPanel.SetActive(false);

        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);
    }

    public void ShowGameOverPopup()
    {
        messageText.text = "¡Has perdido! ¿Quieres jugar otra vez?";
        popupPanel.SetActive(true);  
    }

    
    private void OnYesClicked()
    {
        Debug.Log("Jugar otra vez.");
        
        popupPanel.SetActive(false);  
    }

    private void OnNoClicked()
    {
        Debug.Log("Salir del juego.");
        Application.Quit();  
    }
}
