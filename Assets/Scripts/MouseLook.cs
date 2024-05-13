using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensitivity = 100f;
    public MobileControler mobileControler;
    public Camera cam;
    public Transform playerBody;
    public Vector3 firstPerson = new Vector3(0, 1, 0), thirdPerson = new Vector3(0, 1.5f, -2);
    
    private float xRotation = 0;
    public Constants localConstants = new Constants();
    void Start()
    {
         Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        float _mouseX = 0, _mouseY=0;

        if(WorldManager.DeviceOS == (int)Devices.Windows)
        {
            _mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime * localConstants.playerTimeScale;
            _mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime * localConstants.playerTimeScale;
        }
        else if (WorldManager.DeviceOS == (int)Devices.Android)
        {
            _mouseX = mobileControler.InputRoation.x * sensitivity * Time.deltaTime * localConstants.playerTimeScale;
            _mouseY = mobileControler.InputRoation.z * sensitivity * Time.deltaTime * localConstants.playerTimeScale;
        }
        else
        {
            _mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime * localConstants.playerTimeScale;
            _mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime * localConstants.playerTimeScale;
        }

        xRotation -= _mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * _mouseX);
    }
    public bool toggle(bool b)
    {
        if (b)
            return false;
        else
            return true;
    }
}
