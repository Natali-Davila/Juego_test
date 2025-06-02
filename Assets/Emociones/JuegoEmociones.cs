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
    public AudioClip sonido;
}

public class JuegoEmociones : MonoBehaviour
{
    [Header("UI Elementos")]
    public Text preguntaTexto;
    public Button emocionTextoBoton;
    public Text emocionTexto;
    public List<Button> botonesImagen;
    public Text contadorAciertosTexto;
    public AudioSource audioSource;

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
        CargarSonidosEmociones();
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

        emocionTextoBoton.onClick.RemoveAllListeners();
        emocionTextoBoton.onClick.AddListener(() => ReproducirSonidoEmocion());

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
        if (emociones.Count < 2 || botonesImagen.Count < 2)
        {
            Debug.LogError("Se requieren al menos 2 emociones y 2 botones.");
            return;
        }


        emocionCorrecta = emociones[Random.Range(0, emociones.Count)];
        emocionTexto.text = emocionCorrecta.nombre;


        List<Emocion> emocionesIncorrectas = emociones.Where(e => e.nombre != emocionCorrecta.nombre).ToList();
        Emocion emocionIncorrecta = emocionesIncorrectas[Random.Range(0, emocionesIncorrectas.Count)];


        List<Emocion> opciones = new List<Emocion> { emocionCorrecta, emocionIncorrecta };
        opciones = opciones.OrderBy(x => Random.value).ToList();


        for (int i = 0; i < botonesImagen.Count; i++)
        {
            if (i < 2)
            {
                botonesImagen[i].gameObject.SetActive(true);
                int index = i;
                botonesImagen[i].GetComponent<Image>().sprite = opciones[i].imagen;
                botonesImagen[i].onClick.RemoveAllListeners();
                botonesImagen[i].onClick.AddListener(() => VerificarRespuesta(opciones[index]));
            }
            else
            {
                botonesImagen[i].gameObject.SetActive(false);
            }
        }

        preguntaTexto.text = "Selecciona la emoción correcta";
    }

    void ReproducirSonidoEmocion()
    {
        if (emocionCorrecta != null && emocionCorrecta.sonido != null)
        {
            audioSource.Stop();
            audioSource.clip = emocionCorrecta.sonido;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No hay sonido asignado a esta emoción.");
        }
    }
    void CargarSonidosEmociones()
    {
        foreach (var emocion in emociones)
        {
            string ruta = "SonidosEmociones/" + emocion.nombre.ToLower(); 
            AudioClip clip = Resources.Load<AudioClip>(ruta);
            if (clip != null)
            {
                emocion.sonido = clip;
            }
            else
            {
                Debug.LogWarning($"No se encontró el sonido para la emoción: {emocion.nombre} en Resources/SonidosEmociones/");
            }
        }
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
        contadorAciertosTexto.text = respuestasCorrectas.ToString() + "/10";
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

