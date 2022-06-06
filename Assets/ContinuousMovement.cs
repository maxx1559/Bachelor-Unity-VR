using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ContinuousMovement : MonoBehaviour
{
    public float speed = 1;
    public XRNode inputSource;
    private XROrigin xrrig;
    private Vector2 inputAxis;
    private bool primaryButtonValue;
    private bool secondaryButtonValue;
    private bool gripButtonValue;
    private CharacterController character;
    private int vertical = 0;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        xrrig = GetComponent<XROrigin>();
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
        device.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonValue);
        device.TryGetFeatureValue(CommonUsages.secondaryButton, out secondaryButtonValue);
        device.TryGetFeatureValue(CommonUsages.gripButton, out gripButtonValue);
    }

    private void FixedUpdate()
    {
        vertical = 0;
        if (primaryButtonValue)
        {
            vertical = 1;
        } else if (secondaryButtonValue)
        {
            vertical = -1;
        }
        if (gripButtonValue)
        {
            speed = 4;
        } else
        {
            speed = 1;
        }
        Quaternion headYaw = Quaternion.Euler(0, xrrig.Camera.transform.eulerAngles.y, 0);
        Vector3 direction = headYaw * new Vector3(inputAxis.x, vertical, inputAxis.y);

        character.Move(direction * Time.fixedDeltaTime * speed);
    }
}
