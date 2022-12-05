using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

public class oscilador : MonoBehaviour
{
    [SerializeField] Vector3 posInicial;
    [SerializeField] Vector3 dirDesplazamiento;
    [SerializeField] [Range(-1,1)]float desplazamiento;
    [SerializeField] float periodo=1;
    // Start is called before the first frame update
    void Start()
    {
        posInicial = transform.position;
    }

    // Update is called once per frame
    void Update()
    {   
        if(periodo >= Mathf.Epsilon){
            float ciclos = Time.time / periodo;
            float tau = Mathf.PI * 2 ;
            float funcionSeno = Mathf.Sin(tau * ciclos);
            desplazamiento = funcionSeno/2 + 0.5f;

            // desplazamiento de forma automatica entre 0 y 1
            transform.position = posInicial + (dirDesplazamiento * desplazamiento);
        }
    }

}
