using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ContinuousMovement : MonoBehaviour
{
    public float speed = 1;
    public XRNode leftControllerInputSource;
    public XRNode rightControllerInputSource;
    private XROrigin xrrig;
    private Vector2 leftInputAxis;
    private bool leftPrimaryButtonValue;
    private bool leftSecondaryButtonValue;
    private bool leftGripButtonValue;
    private bool rightPrimaryButtonValue;
    private CharacterController character;
    private int vertical = 0;

    Transform directionalLight;

    // Start is called before the first frame update
    void Start()
    {
        directionalLight = GameObject.Find("Directional Light").GetComponent<Transform>();
        character = GetComponent<CharacterController>();
        xrrig = GetComponent<XROrigin>();
    }

    // Update is called once per frame
    void Update()
    {
        //Setting the input devices to the left and right controllers, and checking the different buttons' states
        InputDevice leftDevice = InputDevices.GetDeviceAtXRNode(leftControllerInputSource);
        InputDevice rightDevice = InputDevices.GetDeviceAtXRNode(rightControllerInputSource);
        //Left joystick
        leftDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out leftInputAxis);
        //X button on left controller
        leftDevice.TryGetFeatureValue(CommonUsages.primaryButton, out leftPrimaryButtonValue);
        //Y button on left controller
        leftDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out leftSecondaryButtonValue);
        //Grip button on side of left controller
        leftDevice.TryGetFeatureValue(CommonUsages.gripButton, out leftGripButtonValue);
        //A button on right controller
        rightDevice.TryGetFeatureValue(CommonUsages.primaryButton, out rightPrimaryButtonValue);
    }

    private void FixedUpdate()
    {
        vertical = 0;
        if (leftPrimaryButtonValue)
        {
            vertical = 1;
        } else if (leftSecondaryButtonValue)
        {
            vertical = -1;
        }
        if (leftGripButtonValue)
        {
            speed = 4;
        } else
        {
            speed = 1;
        }
        if (rightPrimaryButtonValue)
        {
            directionalLight.GetComponent<Transform>().SetPositionAndRotation(transform.position, transform.rotation);
        }
        //Moves user in direction of left joystick and head rotation
        Quaternion headYaw = Quaternion.Euler(0, xrrig.Camera.transform.eulerAngles.y, 0);
        Vector3 direction = headYaw * new Vector3(leftInputAxis.x, vertical, leftInputAxis.y);

        character.Move(direction * Time.fixedDeltaTime * speed);
    }
}
