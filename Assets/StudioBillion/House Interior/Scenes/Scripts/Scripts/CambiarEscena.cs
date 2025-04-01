using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscena : MonoBehaviour 
{

    public void Cambiar() 
    {
        SceneManager.LoadScene(0);
    }
}
