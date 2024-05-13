using UnityEngine;

public class MobileControler : MonoBehaviour
{
    [SerializeField]
    public FixedJoystick RotaionJoystick;
    [SerializeField]
    public FixedJoystick PositionJoystick;

    public GameObject GORotaionJoystick;
    public GameObject GOPositionJoystick;
    public GameObject JumpButton;


    public Vector3 InputPosition
    {
        get
        {
            if (PositionJoystick != null)
                return new Vector3(PositionJoystick.Direction.x, 0, PositionJoystick.Direction.y);
            else
                return new Vector3();
        }
    }
    public Vector3 InputRoation
    {
        get
        {
            if (RotaionJoystick != null)
                return new Vector3(RotaionJoystick.Direction.x,0, RotaionJoystick.Direction.y);
            else
                return new Vector3();
        }
    }

    public void SetActiveControls(bool state)
    {
        GORotaionJoystick.SetActive(state);
        GOPositionJoystick.SetActive(state);
        JumpButton.SetActive(state);
    }
}
