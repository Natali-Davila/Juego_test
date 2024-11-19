using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class refrigerador : MonoBehaviour
{
private bool jugadorColisionando = false;
    public GameObject TextDetect; // Reference to the GameObject to activate/deactivate
    private GameObject player; // Reference to the player GameObject

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            jugadorColisionando = true;
            if (TextDetect != null)
                TextDetect.SetActive(true);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            jugadorColisionando = false;
            if (TextDetect != null)
                TextDetect.SetActive(false);
        }
    }

    private void Start()
    {
        if (TextDetect != null)
            TextDetect.SetActive(false); // Deactivate the text at the start

        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player GameObject not found. Ensure there is a GameObject tagged as 'Player'.");
        }
    }

    private void Update()
    {
        // Check if the "L" key is pressed and the player is colliding
        if (jugadorColisionando && Input.GetKeyDown(KeyCode.S))
        {
            ActivarObjeto();
        }
    }

    public void ActivarObjeto()
    {
        // Desactiva el objeto TextDetect si existe
        if (TextDetect != null)
        {
            TextDetect.SetActive(false);
        }

        // Destruye el objeto asociado a este script
        Destroy(gameObject);
    }
}
