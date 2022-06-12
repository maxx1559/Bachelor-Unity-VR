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
    private bool rightSecondaryButtonValue;
    private CharacterController character;
    private int vertical = 0;

    Transform directionalLight;
    Vector3 initialPos;
    Vector3 initialRot;
    Vector3 initialLightPos;
    Vector3 initialLightRot;
    DataBase db = DataBase.getInstance();

    // Start is called before the first frame update
    void Start()
    {
        directionalLight = GameObject.Find("Directional Light").GetComponent<Transform>();
        character = GetComponent<CharacterController>();
        xrrig = GetComponent<XROrigin>();
        initialLightPos = directionalLight.position;
        initialLightRot = directionalLight.eulerAngles;
        initialPos = transform.position;
        initialRot = transform.eulerAngles;

        Vector3 a = new Vector3(0.0f, 0.0f, 1.0f);
        Vector3 b = db.getCenterPoint();
        b[1] = 0;

        float abDot = a[0] * b[0] + a[1] + b[1] + a[2] + b[2];
        float aMag = Mathf.Sqrt(Mathf.Pow(a[0], 2) + Mathf.Pow(a[1], 2) + Mathf.Pow(a[2], 2));
        float bMag = Mathf.Sqrt(Mathf.Pow(b[0], 2) + Mathf.Pow(b[1], 2) + Mathf.Pow(b[2], 2));

        float angle1 = Mathf.Atan2(b[2] * a[0] - b[0] * a[2], b[0] * a[0] + b[2] * a[2]);
        float angle2 = Vector3.Angle(a, b);

        initialRot = (new Vector3(0, -angle1 * 180 / Mathf.PI, 0));
        transform.rotation = Quaternion.Euler(new Vector3(0, -angle1 * 180 / Mathf.PI, 0));

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
        //B button on right controller
        rightDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out rightSecondaryButtonValue);
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
        if (rightSecondaryButtonValue)
        {
            //Stop character controller from overriding teleport
            character.enabled = false;
            transform.position = initialPos;
            character.enabled = true;
            transform.eulerAngles = initialRot;
            directionalLight.position = initialLightPos;
            directionalLight.eulerAngles = initialLightRot;
        }
        //Moves user in direction of left joystick and head rotation
        Quaternion headYaw = Quaternion.Euler(0, xrrig.Camera.transform.eulerAngles.y, 0);
        Vector3 direction = headYaw * new Vector3(leftInputAxis.x, vertical, leftInputAxis.y);

        character.Move(direction * Time.fixedDeltaTime * speed);
    }
}
