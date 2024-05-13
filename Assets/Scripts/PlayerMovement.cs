using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController cc;
    public MobileControler mobileControler;
    public float speed, jumpHieght = 3f;
    Vector3 velocity;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;
    public string GameMode = "Creative";

    public Constants localConstants = new Constants();

    private void Update()
    {

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float _x = 0, _z= 0;
        if (WorldManager.DeviceOS == (int)Devices.Windows)
        {
            _x = Input.GetAxis("Horizontal") * localConstants.playerTimeScale;
            _z = Input.GetAxis("Vertical") * localConstants.playerTimeScale;
        }
        else if(WorldManager.DeviceOS == (int)Devices.Android)
        {
            _x = mobileControler.InputPosition.x * localConstants.playerTimeScale;
            _z = mobileControler.InputPosition.z * localConstants.playerTimeScale;
        }
        else
        {
            _x = Input.GetAxis("Horizontal") * localConstants.playerTimeScale;
            _z = Input.GetAxis("Vertical") * localConstants.playerTimeScale;
        }
         

        Vector3 _move = transform.right * _x + transform.forward * _z;
        cc.Move(_move * speed * Time.deltaTime * localConstants.playerSpeedScale);

        if (GameMode == "Creative")
        { 
            velocity.y = Input.GetAxis("Jump") * jumpHieght * localConstants.playerTimeScale;
            localConstants.gravity = 0f;
        }

        Jump();

        velocity.y += localConstants.gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);

    }

    public void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded && Input.GetAxis("Jump") > 0)
            velocity.y = Mathf.Sqrt(-2 * localConstants.gravity * jumpHieght);
    }
}

public class Constants
{
    public float gravity = -9.81f;
    public float playerTimeScale = 1 , playerSpeedScale = 1f;
}