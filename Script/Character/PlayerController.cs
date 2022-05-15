using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Speed
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    private float applySpeed;  //Total Speed

    //Jump
    [SerializeField]
    private float jumpForce;

    //bool
    private bool isRun = false;
    private bool isGround = true;


    //Sensitivity
    [SerializeField]
    private float lookSensitivity;

    //Camera
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0;
    
    //Component
    [SerializeField]
    private Camera theCamera;
    private Rigidbody myRigid;
    private CapsuleCollider capsuleCollider;
    Animator animator;

    void Awake() 
    {
       animator = GetComponentInChildren<Animator>();   
    }
    
    void Start()
    {
      capsuleCollider = GetComponent<CapsuleCollider>();
      myRigid = GetComponent<Rigidbody>();
      applySpeed = walkSpeed;
    }
    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        Move();
        CameraRotation();
        CharacterRotation();
    }

    void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }

    void TryJump()
    {
       if(Input.GetKeyDown(KeyCode.Space) && isGround == true)
       {
          Jump();
       }
    }

    void Jump()
    {
        myRigid.velocity = transform.up * jumpForce;
    }

    void TryRun()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }
     
    void Running()
    {
        isRun = true;
        applySpeed = runSpeed;
        
        animator.SetBool("isRun", true);
    }

    void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;

        animator.SetBool("isRun",false);
    }

    void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);

        animator.SetBool("isWalk", _velocity != Vector3.zero);
    }

    void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }
}
