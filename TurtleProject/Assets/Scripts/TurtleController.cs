using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleController : MonoBehaviour
{
    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxRotationSpeed;
    [SerializeField] private float maxTopHeight;
    [SerializeField] private UIManager UImanager;
    private Animator turtleanim;

    private Rigidbody rb;
    private float speed, verticalRotationSpeed, lateralRotationSpeed;
    private Vector3 eulerRotationSpeed;
    private float h, v, j;

    private GameObject posizioni_cageKey;

    // Start is called before the first frame update
    void Awake()
    {
        this.rb = GetComponent<Rigidbody>();
        this.eulerRotationSpeed = new Vector3(0, this.maxRotationSpeed, 0);
        this.turtleanim = GetComponentInChildren<Animator>();
        maxTopHeight = 40;

        this.posizioni_cageKey = GameObject.Find("Map/Posizioni_CageKey");
        posizioni_cageKey.SetActive(true);
        GameDirector.Instance.LoadGame();
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

        if (rb.position.y >= maxTopHeight && j>0)
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
        
    }
    */
    // -------------------------------------------------------------------- //
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Target" && GameDirector.Instance.getGameState() == GameDirector.GameState.ObstacleCourse)
        {
            other.GetComponentInParent<TargetHandler>().TargetCollision(other.name);
        }
        else if(GameDirector.Instance.getGameState() == GameDirector.GameState.FreeRoaming)
        {
            switch (other.name)
            {
                case "Anguilla_collider":
                    UImanager.promptInterface.setPromptText("Premi E per parlare con l'anguilla");
                    break;

                case "PesceRosso_collider":
                    UImanager.promptInterface.setPromptText("Premi E per parlare con Peppe");
                    break;
                case "pesceColorato":
                    UImanager.promptInterface.setPromptText("Premi E per parlare con Dory");
                    break;
                case "SpecialTarget":
                    UImanager.promptInterface.setPromptText("Premi E per cavalcare l'onda");
                    break;
            }
            UImanager.promptInterface.togglePrompt(true);
        }

    }
    public void OnTriggerStay(Collider other)
    {
        if(Input.GetKey(KeyCode.E) && other.tag != "CoralSpot")
        {
            if (GameDirector.Instance.getGameState() == GameDirector.GameState.FreeRoaming)
            {
                UImanager.promptInterface.togglePrompt(false);
                switch (other.name)
                {
                    case "Anguilla_collider":
                        UImanager.dialogueInterface.setNPCName("Anguilla");
                        UImanager.dialogueInterface.setDialogueText("Hey, tu! Sembri una tipa molto in forma. Ti andrebbe di aiutarmi con una faccenda?\n" +
                                     "Le alghe in questo canyon sono in acqua stagnante... Svegliale attraversando tutti gli anelli rocciosi!\n" +
                                     "Sono sicura che il livello di ossigeno aumentera'... E se vai abbastanza veloce, ti daro' anche qualche perla in piu'. Ci stai?");
                        UImanager.dialogueInterface.setCurrentNPC("Anguilla");
                        //TODO: lascia solo setCurrentNPC, e sposta setNPCName e setDialogueText in DialogueInterface;
                        //In questo modo standardizziamo i testi e li rimuoviamo da turtleController.
                        break;

                    case "PesceRosso_collider":
                        UImanager.dialogueInterface.setNPCName("Peppe il pesce");
                        UImanager.dialogueInterface.setDialogueText("Hey sembrerebbe che ci siano dei sacchetti della spazzatura\n " +
                                    "ad inquinare l'oceano! Ti va di aiutarmi a metterli tutti nella rete? \n" +
                                    "Attenta! hai solo un minuto di tempo prima che risalga la rete");
                        UImanager.dialogueInterface.setCurrentNPC("Peppe");
                        break;

                    case "pesceColorato":
                        UImanager.dialogueInterface.setNPCName("Dory");
                        UImanager.dialogueInterface.setDialogueText("Hey Shellie! Ci sono dei granchi che hanno bisogno di essere liberati! \n" +
                                    "Ti va di aiutarmi?" + " Nel labirinto troverai delle chiavi con cui poter aprire le gabbie \n" +
                                    "Attenta! Puoi prendere solo una chiave alla volta ed hai 3 minuti di tempo per liberarli tutti!");
                        UImanager.dialogueInterface.setCurrentNPC("Dory");
                        break;

                    case "SpecialTarget":
                        other.GetComponentInParent<TargetHandler>().summonSpecialTarget();
                        UImanager.promptInterface.togglePrompt(false);
                        break;
                }
                if(other.name != "SpecialTarget")
                    UImanager.dialogueInterface.toggleDialoguePanel(true);

            } else if(other.tag == "Chiave" || other.tag == "Gabbia")
            {
                posizioni_cageKey.GetComponent<OpenCagesHandler>().TriggerMethod(other);
            }
        }

    }
    public void OnTriggerExit(Collider other)
    {
        UImanager.promptInterface.togglePrompt(false);
        UImanager.dialogueInterface.toggleDialoguePanel(false);
    }
    // -------------------------------------------------------------------- //

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("spazzatura"))
        { 
            float intensity = 2f * (acceleration);
            Vector3 directionToPlayer = (transform.position - collision.transform.position).normalized;
            float pushAngle = 3f;  //angolo di spinta
            Quaternion rotation = Quaternion.Euler(0, pushAngle, 0);
            //Vector3 force = rotation * directionToPlayer;
            Vector3 force = directionToPlayer + new Vector3(0, pushAngle, 0);
            force.Normalize();
            collision.gameObject.GetComponent<Rigidbody>().AddForce((force * intensity), ForceMode.Force);

            Debug.Log("spinta");
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