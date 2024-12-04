using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public List<Button> btns = new List<Button> ();
    private void Awake()
    {
        
    }
    void GetButtons() 
    {
        GameObject objects = GameObject.Find("name");
    }
}
