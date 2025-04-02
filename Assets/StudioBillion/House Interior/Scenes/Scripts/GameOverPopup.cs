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
        popupPanel.SetActive(true);  
    }

    
    private void OnYesClicked()
    {
        popupPanel.SetActive(false);  
    }

    private void OnNoClicked()
    {
        Application.Quit();  
    }
}
