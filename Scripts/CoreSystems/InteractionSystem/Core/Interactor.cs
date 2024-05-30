using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    //*******************************************************************************************//
    //***********************************      ATTRIBUTES      **********************************//
    //*******************************************************************************************//
    //**********************************    MAIN ATTRIBUTES    **********************************//
    //[SerializeField] private Transform origin; // MIRAR A VER
    //[SerializeField] private Vector3 originOffset; // MIRAR A VER
    [SerializeField] private float radius; // MIRAR A VER
    [SerializeField] private Vector2 offset;
    public static Coroutine interactCoroutine = null;

    Collider[] colArray = new Collider[3];

    [SerializeField] private LayerMask layer;
    private bool interacting = false;
    //SphereCollider sphereCol;
    private bool pressed;

    //*******************************************************************************************//
    //**************************************      MAIN      *************************************//
    //*******************************************************************************************//
    private void Awake(){
        
    }
    private void Start(){
        //sphereCol = GetComponent<SphereCollider>();
        //Debug.Log(sphereCol);
    }
    protected void OnEnable(){
        GameMaster.DefaultInputActions.Player.Interact.started += ctx => pressed = true;
    }

    //***************************************    UPDATE    **************************************//
    private void Update(){
        PlayerInteract();
        
    }
    private void LateUpdate(){
        pressed = false;
    }
    private async void PlayerInteract(){
        if(!interacting){
            int numFound = Physics.OverlapSphereNonAlloc(transform.position+transform.forward*offset.x+Vector3.up*offset.y, radius, colArray, layer);
            if(numFound > 0){
                var interactuable = colArray[0].GetComponent<IInteractuable>();
                if(interactuable != null){
                    if(pressed){
                        interacting = true;
                        //PlayerController.disablePlayerMovement();
                        var finished = await interactuable.InteractTask();
                        //PlayerController.enablePlayerMovement();
                        interacting = false;
                    }
                }
            }
        }
    }

    //*******************************************************************************************//
    //*************************************      METHODS      ***********************************//
    //*******************************************************************************************//
    private void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position+transform.forward*offset.x+Vector3.up*offset.y, radius);
    } 
}
