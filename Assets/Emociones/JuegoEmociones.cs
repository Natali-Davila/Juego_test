
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;

//[System.Serializable]
//public class Emocion
//{
//    public string nombre;
//    public Sprite imagen;
//}

//public class JuegoEmociones : MonoBehaviour
//{
//    [Header("UI Elementos")]
//    public Text preguntaTexto;
//    public Button emocionTextoBoton;
//    public Text emocionTexto;
//    public List<Button> botonesImagen;
//    public Text contadorAciertosTexto; // 👈 NUEVO

//    [Header("Emociones")]
//    public List<Emocion> emociones;

//    [Header("Vidas")]
//    public GameObject[] corazones;
//    private int vidas;

//    [Header("Paneles")]
//    public GameObject panelGameOver;
//    public GameObject panelVictoria;
//    public Text textoVictoria;

//    [Header("Botones de Navegación")]
//    public Button botonSalir;
//    public Button botonInstrucciones;
//    public GameObject panelInstrucciones;
//    public Button botonCerrarInstrucciones;
//    public GameObject panelConfirmarSalida;
//    public Button botonConfirmarSalida;
//    public Button botonCancelarSalida;
//    public Button botonReiniciar;
//    public Button botonMenu;

//    [Header("Botón de Victoria")]
//    public Button botonSalirVictoria; // 👈 NUEVO (Botón de salir en panel de victoria)

//    private Emocion emocionCorrecta;
//    private int respuestasCorrectas;
//    private const int maxRespuestasCorrectas = 10;

//    void Start()
//    {
//        vidas = corazones.Length;
//        ActualizarCorazones();
//        ConfigurarBotonesUI();
//        ActualizarContadorAciertos(); // 👈 NUEVO
//        IniciarJuego();
//    }

//    void ConfigurarBotonesUI()
//    {
//        botonSalir.onClick.AddListener(() => panelConfirmarSalida.SetActive(true));
//        botonConfirmarSalida.onClick.AddListener(() => SceneManager.LoadScene(1)); // Escena principal
//        botonCancelarSalida.onClick.AddListener(() => panelConfirmarSalida.SetActive(false));
//        botonReiniciar.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
//        botonMenu.onClick.AddListener(() => SceneManager.LoadScene(1)); // Escena principal
//        botonInstrucciones.onClick.AddListener(() => panelInstrucciones.SetActive(true));
//        botonCerrarInstrucciones.onClick.AddListener(() => panelInstrucciones.SetActive(false));

//        // Configuración del botón de victoria
//        botonSalirVictoria.onClick.AddListener(() => SceneManager.LoadScene(1)); // Regresa a la escena principal

//        panelGameOver.SetActive(false);
//        panelVictoria.SetActive(false);
//        panelInstrucciones.SetActive(false);
//        panelConfirmarSalida.SetActive(false);
//    }

//    void IniciarJuego()
//    {
//        if (emociones.Count < 8 || botonesImagen.Count < 8)
//        {
//            Debug.LogError("Se requieren al menos 8 emociones y 8 botones.");
//            return;
//        }

//        SeleccionarNuevaPregunta();
//    }

//    void SeleccionarNuevaPregunta()
//    {
//        List<Emocion> emocionesAleatorias = emociones.OrderBy(x => Random.value).ToList();
//        emocionCorrecta = emocionesAleatorias[Random.Range(0, emocionesAleatorias.Count)];
//        emocionTexto.text = emocionCorrecta.nombre;

//        for (int i = 0; i < botonesImagen.Count; i++)
//        {
//            int index = i;
//            botonesImagen[i].GetComponent<Image>().sprite = emocionesAleatorias[i].imagen;
//            botonesImagen[i].onClick.RemoveAllListeners();
//            botonesImagen[i].onClick.AddListener(() => VerificarRespuesta(emocionesAleatorias[index]));
//        }

//        preguntaTexto.text = "¿Cuál es la emoción correspondiente al texto?";
//    }

//    void VerificarRespuesta(Emocion seleccion)
//    {
//        if (seleccion.nombre == emocionCorrecta.nombre)
//        {
//            respuestasCorrectas++;
//            ActualizarContadorAciertos(); // 👈 NUEVO

//            if (respuestasCorrectas >= maxRespuestasCorrectas)
//            {
//                MostrarVictoria();
//            }
//            else
//            {
//                SeleccionarNuevaPregunta(); // Nuevas emociones aleatorias para continuar
//            }
//        }
//        else
//        {
//            vidas--; // Solo se pierde una vida por error
//            ActualizarCorazones();

//            if (vidas <= 0)
//            {
//                MostrarGameOver();
//            }
//        }
//    }

//    void ActualizarContadorAciertos() // 👈 NUEVO
//    {
//        contadorAciertosTexto.text = "Aciertos: " + respuestasCorrectas.ToString();
//    }

//    void ActualizarCorazones()
//    {
//        for (int i = 0; i < corazones.Length; i++)
//        {
//            corazones[i].SetActive(i < vidas);
//        }
//    }

//    void MostrarGameOver()
//    {
//        panelGameOver.SetActive(true);
//    }

//    void MostrarVictoria()
//    {
//        panelVictoria.SetActive(true);
//        textoVictoria.text = "¡Felicidades! Has identificado todas las emociones.";
//    }
//}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Emocion
{
    public string nombre;
    public Sprite imagen;
}

public class JuegoEmociones : MonoBehaviour
{
    [Header("UI Elementos")]
    public Text preguntaTexto;
    public Button emocionTextoBoton;
    public Text emocionTexto;
    public List<Button> botonesImagen;
    public Text contadorAciertosTexto; 

    [Header("Emociones")]
    public List<Emocion> emociones;

    [Header("Vidas")]
    public GameObject[] corazones;
    private int vidas;

    [Header("Paneles")]
    public GameObject panelGameOver;
    public GameObject panelVictoria;
    public Text textoVictoria;

    [Header("Botones de Navegación")]
    public Button botonSalir;
    public Button botonInstrucciones;
    public GameObject panelInstrucciones;
    public Button botonCerrarInstrucciones;
    public GameObject panelConfirmarSalida;
    public Button botonConfirmarSalida;
    public Button botonCancelarSalida;
    public Button botonReiniciar;
    public Button botonMenu;

    [Header("Botón de Victoria")]
    public Button botonSalirVictoria;
    public Button botonVolverAJugar; 

    private Emocion emocionCorrecta;
    private int respuestasCorrectas;
    private const int maxRespuestasCorrectas = 10;

    void Start()
    {
        vidas = corazones.Length;
        ActualizarCorazones();
        ConfigurarBotonesUI();
        ActualizarContadorAciertos();
        IniciarJuego();
    }

    void ConfigurarBotonesUI()
    {
        botonSalir.onClick.AddListener(() => panelConfirmarSalida.SetActive(true));
        botonConfirmarSalida.onClick.AddListener(() => SceneManager.LoadScene(1)); 
        botonCancelarSalida.onClick.AddListener(() => panelConfirmarSalida.SetActive(false));
        botonReiniciar.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
        botonMenu.onClick.AddListener(() => SceneManager.LoadScene(1)); 
        botonInstrucciones.onClick.AddListener(() => panelInstrucciones.SetActive(true));
        botonCerrarInstrucciones.onClick.AddListener(() => panelInstrucciones.SetActive(false));

        
        botonSalirVictoria.onClick.AddListener(() => SceneManager.LoadScene(1)); 
        botonVolverAJugar.onClick.AddListener(() => ReiniciarJuego()); 

        panelGameOver.SetActive(false);
        panelVictoria.SetActive(false);
        panelInstrucciones.SetActive(false);
        panelConfirmarSalida.SetActive(false);
    }

    void IniciarJuego()
    {
        if (emociones.Count < 8 || botonesImagen.Count < 8)
        {
            Debug.LogError("Se requieren al menos 8 emociones y 8 botones.");
            return;
        }

        SeleccionarNuevaPregunta();
    }

    void SeleccionarNuevaPregunta()
    {
        List<Emocion> emocionesAleatorias = emociones.OrderBy(x => Random.value).ToList();
        emocionCorrecta = emocionesAleatorias[Random.Range(0, emocionesAleatorias.Count)];
        emocionTexto.text = emocionCorrecta.nombre;

        for (int i = 0; i < botonesImagen.Count; i++)
        {
            int index = i;
            botonesImagen[i].GetComponent<Image>().sprite = emocionesAleatorias[i].imagen;
            botonesImagen[i].onClick.RemoveAllListeners();
            botonesImagen[i].onClick.AddListener(() => VerificarRespuesta(emocionesAleatorias[index]));
        }

        preguntaTexto.text = "Selecciona la emocion correcta";
    }

    void VerificarRespuesta(Emocion seleccion)
    {
        if (seleccion.nombre == emocionCorrecta.nombre)
        {
            respuestasCorrectas++;
            ActualizarContadorAciertos(); 

            if (respuestasCorrectas >= maxRespuestasCorrectas)
            {
                MostrarVictoria();
            }
            else
            {
                SeleccionarNuevaPregunta(); 
            }
        }
        else
        {
            vidas--; 
            ActualizarCorazones();

            if (vidas <= 0)
            {
                MostrarGameOver();
            }
        }
    }

    void ActualizarContadorAciertos() 
    {
        contadorAciertosTexto.text = respuestasCorrectas.ToString()+ "/10";
    }

    void ActualizarCorazones()
    {
        for (int i = 0; i < corazones.Length; i++)
        {
            corazones[i].SetActive(i < vidas);
        }
    }

    void MostrarGameOver()
    {
        panelGameOver.SetActive(true);
    }

    void MostrarVictoria()
    {
        panelVictoria.SetActive(true);
        textoVictoria.text = "¡Felicidades! Has identificado todas las emociones.";
    }

    void ReiniciarJuego()
    {
        respuestasCorrectas = 0;
        vidas = corazones.Length;
        ActualizarCorazones();
        ActualizarContadorAciertos();
        SeleccionarNuevaPregunta();
        panelVictoria.SetActive(false); 
    }
}


