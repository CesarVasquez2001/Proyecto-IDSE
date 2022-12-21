using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class controldenave : MonoBehaviour
{
    Rigidbody rigidbody;
    Transform transform;
    AudioSource audiosource;

    enum EstadoNave{Viva,Muerta,NivelCompleto};
    private EstadoNave estadoNave = EstadoNave.Viva;
    private bool colisionesActivas =true;
    //timer
    public float timer = 0;
    public Text textoTimer;

    public int CantidadCargaCombustible = 1;
    

    [SerializeField] float timerMuerte = 1.0f;
    [SerializeField] float timerNivelCompleto = 1.0f; 

    [Header("Sonidos")]
    [SerializeField] AudioClip sonidoPropulsion; 
    [SerializeField] AudioClip sonidoNivelCompletado; 
    [SerializeField] AudioClip sonidoMuerte; 
    [Header("Particulas")]
    [SerializeField] ParticleSystem partMuerte;
    [SerializeField] ParticleSystem partNivelCompletado;
    [SerializeField] ParticleSystem partPropulsion;
    

    // Start is called before the first frame update
    void Start(){
        rigidbody = GetComponent<Rigidbody>();
        transform = GetComponent<Transform>();
        audiosource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update(){
        //print("Hi");
        //Debug.Log(Time.deltaTime + " seg. " + (1.0f / Time.deltaTime) + " FPS ");
        ProcesarInput();
        Tiempo();


    }
    private void Tiempo()
    {
        timer -= Time.deltaTime;
        textoTimer.text= ""+timer.ToString("f2");
        if(textoTimer.text == "0.00")
        {
            ProcesarMuerte();
            timer =10;
        }
    }
    private void OnCollisionEnter(Collision collision){
        if(estadoNave != EstadoNave.Viva || colisionesActivas==false){
            
            return;
        }
        switch(collision.gameObject.tag)
        {
            case "colisionSegura":
                print("Colision segura");
                break;
            case "combustible":
                print("Combustible...");
                ProcesarCombustible();
                break;
            case "terreno":
                print("Terreno...");
                break;
            case "aterrizaje":
                ProcesarNivelCompleto();
                break;
            default:  
                ProcesarMuerte();
                break;
        }  
    }
    private void ProcesarCombustible()
    {
        if(CantidadCargaCombustible == 1)
        {
            timer=timer+20;
            CantidadCargaCombustible =0;
        }
        
    }
    private void ProcesarMuerte(){
        audiosource.Stop();
        partPropulsion.Stop();
        audiosource.PlayOneShot(sonidoMuerte);
        partMuerte.Play();
        estadoNave = EstadoNave.Muerta;
        Invoke("Muerte",timerMuerte);
    }
    private void ProcesarNivelCompleto(){
        audiosource.Stop();
        partPropulsion.Stop();
        audiosource.PlayOneShot(sonidoNivelCompletado);
        partNivelCompletado.Play();
        estadoNave = EstadoNave.NivelCompleto;
        Invoke("PasarNivel",timerNivelCompleto);
    }
    private void Muerte(){
        print("Muerto...!!!");
        SceneManager.LoadScene("Nivel1");        
    }
    private void PasarNivel(){
        print("Aterrizaje...");
        int cantidadEscenas = SceneManager.sceneCountInBuildSettings;
        int escenaActual = SceneManager.GetActiveScene().buildIndex;

        int siguienteEscena = escenaActual+1;
        if(siguienteEscena == cantidadEscenas){
            siguienteEscena=0;
        }
        SceneManager.LoadScene(siguienteEscena);    

    }
    private void ProcesarInput(){
        if(estadoNave == EstadoNave.Viva){
            ProcesarPropulsion();
            ProcesarRotacion();         
        }
        //Solo desarrollador
        if(Debug.isDebugBuild){
            ProcesarInputDesarrollador();
        }
    }
    private void ProcesarInputDesarrollador(){
        if(Input.GetKeyDown(KeyCode.C)){
            colisionesActivas = !colisionesActivas;
        }
        else if(Input.GetKeyDown(KeyCode.N)){
            PasarNivel();
        }
    }
    private void ProcesarPropulsion(){
        if (Input.GetKey(KeyCode.Space))
        { 
            Propulsion();
        }else{
            audiosource.Stop();
            partPropulsion.Stop();
        }   
        rigidbody.freezeRotation = false;
    }
    private void Propulsion(){
        rigidbody.freezeRotation = true;
        //print("Propulsor ...");
        rigidbody.AddRelativeForce(Vector3.up);
        if(!partPropulsion.isPlaying){
            partPropulsion.Play();
        }
        if (!audiosource.isPlaying){
            audiosource.PlayOneShot(sonidoPropulsion);
        }
    }
    private void ProcesarRotacion(){
        if (Input.GetKey(KeyCode.D))
        {
            //print("Rotar Derecha ...");
            //transform.Rotate(Vector3.back);
            var rotarDerecha = transform.rotation;
            rotarDerecha.z += Time.deltaTime * 0.5f;
            transform.rotation = rotarDerecha;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            //print("Rotar Izquierda ...");
            //transform.Rotate(Vector3.forward);
            var rotarIzquierda = transform.rotation;
            rotarIzquierda.z -= Time.deltaTime * 0.5f;
            transform.rotation = rotarIzquierda;
        }
    }
}
