using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleController : MonoBehaviour
{
    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxRotationSpeed;
    private Animator turtleanim;
    private GameObject oggettoscriptTrash;

    private Rigidbody rb;
    private float speed, verticalRotationSpeed, lateralRotationSpeed;
    private Vector3 eulerRotationSpeed;
    private float h, v, j;

    private GameObject posizioni_cageKey;
    private AudioManager audioManager;

    // Start is called before the first frame update
    void Awake()
    {
        this.rb = GetComponent<Rigidbody>();
        this.eulerRotationSpeed = new Vector3(0, this.maxRotationSpeed, 0);
        this.turtleanim = GetComponentInChildren<Animator>();

        this.posizioni_cageKey = GameObject.Find("Map/Posizioni_CageKey");
        posizioni_cageKey.SetActive(true);
        GameDirector.Instance.LoadGame();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }


    private void FixedUpdate()      //In questa funzione vanno i calcoli delle velocità, che verranno passati ad Update
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        j = Input.GetAxis("Jump");

        // -------------------------------------------------------------------- //
        //calcolo della velocità frontale
        if (speed >= 0 && speed <= maxSpeed)
            speed += acceleration * v * Time.deltaTime;
        if (v == 0 && speed >= 0) //se non è premuta W, la tartaruga rallenta
            speed -= acceleration * Time.deltaTime * 0.5f;
        if (speed < 0)
            speed = 0;
        if (speed > maxSpeed)
            speed = maxSpeed;

        // -------------------------------------------------------------------- //
        //animazione movimento tartaruga
        turtleanim.SetFloat("speed", speed);

        // -------------------------------------------------------------------- //
        //calcolo della rotazione laterale
        lateralRotationSpeed = 0;
        if (Mathf.Abs(lateralRotationSpeed) <= maxRotationSpeed && h != 0)
        {
            lateralRotationSpeed += acceleration * h * Time.deltaTime;
            //TODO: forse è meglio mettere un'accelerazione di rotazione separata
        }

        //Se non sono premuti tasti laterali, la rotazione rallenta fino a tornare dritti
        else if (h == 0 && lateralRotationSpeed != 0)
        {
            if (lateralRotationSpeed > 0)
                lateralRotationSpeed -= acceleration * Time.deltaTime * 0.2f;
            else
                lateralRotationSpeed += acceleration * Time.deltaTime * 0.2f * (maxSpeed / speed);
        }

        //Fattore di rotazione relativo alla velocità (più la tartaruga è lenta, più può ruotare veloce
        float lateralRotationFactor = 7f - (speed / (maxSpeed)) * 3f;
        // -------------------------------------------------------------------- //
        //calcolo della rotazione verticale
        verticalRotationSpeed = 0;

        //Limita l'input del giocatore una volta raggiunta l'altezza massima
        if (rb.position.y >= 40 && j>0)
        {
            j = 0;
        }
        if (Mathf.Abs(verticalRotationSpeed) <= maxRotationSpeed && j != 0)
        {
            verticalRotationSpeed += acceleration * Time.deltaTime * j; //TODO: forse è meglio mettere un'accelerazione di rotazione separata
        }


        //Se non sono premuti tasti verticali, la rotazione rallenta fino a tornare dritti
        else if (j == 0 && verticalRotationSpeed != 0)
        {
            if (verticalRotationSpeed > 0)
                verticalRotationSpeed -= acceleration * Time.deltaTime * 0.2f;
            else
                verticalRotationSpeed += acceleration * Time.deltaTime * 0.2f;
        }

        // -------------------------------------------------------------------- //
        //rotazione laterale
        this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y + lateralRotationSpeed * lateralRotationFactor, -lateralRotationSpeed * 30 * lateralRotationFactor);

        //rotazione verticale
        this.transform.eulerAngles = new Vector3(-verticalRotationSpeed * 100, this.transform.eulerAngles.y, this.transform.eulerAngles.z);

        //movimento frontale
        if (speed > 0)
            this.rb.MovePosition(this.rb.position + this.transform.forward * (speed * Time.deltaTime));
    }
    /*
    public void Update()        //In questo metodo vanno le funzioni dedicate allo spostamento
    {
        
    }*/


    // -------------------------------------------------------------------- //
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Target")
        {
            other.GetComponentInParent<TargetHandler>().TargetCollision(other.name);
        }
        else
        {
            switch (other.name)
            {
                case "Anguilla_collider":
                    PromptInterface.setPromptText("Premi E per parlare con l'anguilla");
                    break;

                case "PesceRosso_collider":
                    PromptInterface.setPromptText("Premi E per parlare con Peppe");
                    break;
                case "pesceColorato":
                    PromptInterface.setPromptText("Premi E per parlare con Dory");
                    break;
            }
            PromptInterface.togglePromptOn();
        }

    }
    public void OnTriggerStay(Collider other)
    {
        if(Input.GetKey(KeyCode.E))
        {
            if (other.tag != "Chiave" && other.tag != "Gabbia")
            {
                PromptInterface.togglePromptOff();
                switch (other.name)
                {
                    case "Anguilla_collider":
                        DialogueInterface.setNPCName("Anguilla");
                        DialogueInterface.setDialogueText("Hey, tu! Sembri una tipa molto in forma. Ti andrebbe di aiutarmi con una faccenda?\n" +
                                     "Le alghe in questo canyon sono in acqua stagnante... Svegliale attraversando tutti gli anelli rocciosi!\n" +
                                     "Sono sicura che il livello di ossigeno aumentera'... E se vai abbastanza veloce, ti daro' anche qualche perla in piu'. Ci stai?");
                        DialogueInterface.setCurrentNPC("Anguilla");
                        //TODO: lascia solo setCurrentNPC, e sposta setNPCName e setDialogueText in DialogueInterface;
                        //In questo modo standardizziamo i testi e li rimuoviamo da turtleController.
                        break;

                    case "PesceRosso_collider":
                        DialogueInterface.setNPCName("Peppe il pesce");
                        DialogueInterface.setDialogueText("Hey sembrerebbe che ci siano dei sacchetti della spazzatura\n " +
                                    "ad inquinare l'oceano! Ti va di aiutarmi a metterli tutti nella rete? \n" +
                                    "Attenta! hai solo un minuto di tempo prima che risalga la rete");
                        DialogueInterface.setCurrentNPC("Peppe");
                        break;

                    case "pesceColorato":
                        DialogueInterface.setNPCName("Dory");
                        DialogueInterface.setDialogueText("Hey Shellie! Ci sono dei granchi che hanno bisogno di essere liberati! \n" +
                                    "Ti va di aiutarmi?" + " Nel labirinto troverai delle chiavi con cui poter aprire le gabbie \n" +
                                    "Attenta! Puoi prendere solo una chiave alla volta ed hai 3 minuti di tempo per liberarli tutti!");
                        DialogueInterface.setCurrentNPC("Dory");
                        break;

                    case "SpecialTarget":
                        //other.GetComponentInParent<TargetHandler>().summonSpecialTarget();
                        break;
                }
                DialogueInterface.toggleDialoguePanelOn();
            } else
            {
                posizioni_cageKey.GetComponent<OpenCagesHandler>().TriggerMethod(other);
            }
        }

    }
    public void OnTriggerExit(Collider other)
    {
        PromptInterface.togglePromptOff();
        DialogueInterface.toggleDialoguePanelOff();
    }
    // -------------------------------------------------------------------- //

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("spazzatura"))
        {
            float intensità = 1.5f * (speed / 2);
            Vector3 push = new Vector3(0, 10, 0);
            //Vector3 angle = new Vector3(10, 0, 0);
            Vector3 directionToPlayer = (transform.position - collision.transform.position).normalized;
            //Vector3 force = directionToPlayer + push;
            Quaternion rotation = Quaternion.Euler(0, 45, 0);//rotazione 30 gradi attoeno ad y
            Vector3 rotatedDirection = rotation * directionToPlayer; //ruota player 30 gradi
            Vector3 force = rotatedDirection + push; //spinta diagonale
            force.Normalize();
            Debug.Log("spinta");
            audioManager.PlayTrash();

            collision.gameObject.GetComponent<Rigidbody>().AddForce((force * intensità), ForceMode.Force);
        }
    }
    //Funzione per rallentare in caso di collisione, principalmente per prevenire movimenti fuori controllo e passaggi attraverso il terreno
    public void OnCollisionStay(Collision collision)
    {
        if (speed >= maxSpeed / 6)
            speed = maxSpeed / 6;
    }

    //Funzione per eliminare la velocità generata da collisioni
    public void OnCollisionExit(Collision collision)
    {
        StartCoroutine(stopForces()); //Questa coroutine aspetta un secondo dopo che si esce da una collisione, e poi resetta le velocità causate dalla spinta

    }
    IEnumerator stopForces()
    {
        yield return new WaitForSeconds(1);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        //TODO: con Vector3.zero la velocità si taglia di colpo. Vedi se riesci a trovare un modo di rallentare in maniera graduale.
        //Intanto, per ora funziona.

    }

    // -------------------------------------------------------------------- //
}