using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class refrigerador : MonoBehaviour
{
private bool jugadorColisionando = false;
    public GameObject TextDetect;
    private GameObject player; 

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
            TextDetect.SetActive(false); 

        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player GameObject not found. Ensure there is a GameObject tagged as 'Player'.");
        }
    }

    private void Update()
    {
        if (jugadorColisionando && Input.GetKeyDown(KeyCode.S))
        {
            ActivarObjeto();
        }
    }

    public void ActivarObjeto()
    {
        if (TextDetect != null)
        {
            TextDetect.SetActive(false);
        }

        Destroy(gameObject);
    }
}
