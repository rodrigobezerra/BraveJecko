using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject PainelCompleto;
    public bool estaPausado;

    // Start is called before the first frame update
    void Start()
    {
        estaPausado = false;
    }

    // Update is called once per frame
    void Update()
    {
        Pause();
    }

    public void Pause()
    {

        if (Input.GetKeyDown(KeyCode.Joystick1Button7) || Input.GetKeyDown(KeyCode.Return))
        {
            if (estaPausado == false)
            {
                PainelCompleto.SetActive(true);
                estaPausado = true;
            }
            else
            {
                PainelCompleto.SetActive(false);
                estaPausado = false;
            }

        }

    }

}
