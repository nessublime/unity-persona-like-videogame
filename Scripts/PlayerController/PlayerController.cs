using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour , IAnimable
{
    //*******************************************************************************************//
    //***********************************      ATTRIBUTES      **********************************//
    //*******************************************************************************************//
    //**********************************    MAIN ATTRIBUTES    **********************************//
    [SerializeField] private CharacterController cc;
    public float movementSpeed;
    private Vector3 refVelocity0;
    public float smoothDampTime;
    private Vector3 movementVelocity;

    [SerializeField] private Animator animator;
    

    //*******************************************************************************************//
    //**************************************      MAIN      *************************************//
    //*******************************************************************************************//
    //***********************************    AWAKE & START    ***********************************//
    protected  void Awake(){
        CameraMaster.SetCameraInfo(this.transform.position);
    }

    private void SetInputActions(){

    }
    //**************************************    UPDATE    ***************************************//
    private void FixedUpdate(){
        
    }

    private void Update(){
        CameraMaster.SetCameraInfo(this.transform.position);
        CameraMaster.SmoothUpdateDollyCamInfo();
        Move(); 
    }

    //*******************************************************************************************//
    //*************************************      METHODS      ***********************************//
    //*******************************************************************************************//
    private void Move(){
        Vector2 joyInput = GameMaster.DefaultInputActions.Player.Move.ReadValue<Vector2>();
        Vector3 movement = CameraMaster.FixMovementDirection(new Vector3(joyInput.x,0,joyInput.y));
        movementVelocity = Vector3.SmoothDamp(movementVelocity,movement*movementSpeed,ref refVelocity0, smoothDampTime);
        cc.Move(movementVelocity * Time.deltaTime);
        if(movement != Vector3.zero){transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movement), 2 * movementSpeed * Time.deltaTime);}

        //Vector3 movementInput = new Vector3(joyInput.x,0,joyInput.y);
        //cc.Move(CameraMaster.FixMovementDirection(movementInput * movementSpeed));
        //if(movementInput != Vector3.zero){transform.rotation = Quaternion.LookRotation(movementInput);}
        //if(movementInput != Vector3.zero){transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(CameraMaster.FixMovementDirection(movementInput * movementSpeed)), 2*movementSpeed);}
    }
    //**************************************    ANIMATIONS    ***********************************//
    public const string anim_walk = "walk", anim_phone = "phone";

    public void AnimationStateMachine(string animType){
        switch(animType){
            case anim_walk:
                if(animator.GetBool(Animator.StringToHash("isWalking"))){animator.SetBool(Animator.StringToHash("isWalking"),false);}
                else{animator.SetBool(Animator.StringToHash("isWalking"),true);}
                break;
            case anim_phone:
                break;
            default:
                break;        
        }
    }
}
