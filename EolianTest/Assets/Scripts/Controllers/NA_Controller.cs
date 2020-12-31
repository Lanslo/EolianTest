using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace Vodgets
{

    public class NA_Controller : Controller
    {
        
        public enum NodesToUse
        {
            Right,
            Left,
            REye,
            LEye,
            Head
        }
        [SerializeField]
        public NodesToUse Devices;

        // Get your devices 

        public InputDevice RightController;
        public InputDevice LeftController;
        public InputDevice RightEye;
        public InputDevice LeftEye;
        public InputDevice Head;

        public bool triggerState = false;
        public bool triggerState2 = false;
        public bool temp = false;
        public bool temp2 = false;

        private void Start()
        {
            RightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            LeftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
            RightEye = InputDevices.GetDeviceAtXRNode(XRNode.RightEye);
            LeftEye = InputDevices.GetDeviceAtXRNode(XRNode.LeftEye);
            Head = InputDevices.GetDeviceAtXRNode(XRNode.Head);
        }

        private void Update()
        {
            if (Devices == NodesToUse.Right)
            {
                
                RightController.TryGetFeatureValue(CommonUsages.devicePosition, out var pos);
                RightController.TryGetFeatureValue(CommonUsages.deviceRotation, out var rot);
                RightController.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerState);
                RightController.TryGetFeatureValue(CommonUsages.gripButton, out bool triggerState2);

                this.gameObject.transform.localPosition = pos;
                this.gameObject.transform.localRotation = rot;

                if (triggerState != temp)
                {
                    temp = triggerState;
                    selector.SendButtonEvent(Selector.ButtonType.Trigger, triggerState);
                    Debug.Log(triggerState);
                }
                if (triggerState2 != temp2)
                {
                    temp2 = triggerState2;
                    selector.SendButtonEvent(Selector.ButtonType.Grip, triggerState2);
                    Debug.Log(triggerState2);
                }

            }
            if (Devices == NodesToUse.Left)
            {

                LeftController.TryGetFeatureValue(CommonUsages.devicePosition, out var pos);
                LeftController.TryGetFeatureValue(CommonUsages.deviceRotation, out var rot);
                LeftController.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerState);
                LeftController.TryGetFeatureValue(CommonUsages.gripButton, out bool triggerState2);

                this.gameObject.transform.localPosition = pos;
                this.gameObject.transform.localRotation = rot;

                if (triggerState != temp)
                {
                    temp = triggerState;
                    selector.SendButtonEvent(Selector.ButtonType.Trigger, triggerState);
                    Debug.Log(triggerState);
                }
                if (triggerState2 != temp2)
                {
                    temp2 = triggerState2;
                    selector.SendButtonEvent(Selector.ButtonType.Grip, triggerState2);
                    Debug.Log(triggerState2);
                }


            }
            if (Devices == NodesToUse.Head)
            {

                Head.TryGetFeatureValue(CommonUsages.devicePosition, out var pos);
                Head.TryGetFeatureValue(CommonUsages.deviceRotation, out var rot);

                this.gameObject.transform.localPosition = pos;
                this.gameObject.transform.localRotation = rot;
            }
            if (Devices == NodesToUse.LEye)
            {

                LeftEye.TryGetFeatureValue(CommonUsages.leftEyePosition, out var pos);
                LeftEye.TryGetFeatureValue(CommonUsages.leftEyeRotation, out var rot);

                this.gameObject.transform.localPosition = pos;
                this.gameObject.transform.localRotation = rot;
            }
            if (Devices == NodesToUse.REye)
            {

                RightEye.TryGetFeatureValue(CommonUsages.rightEyePosition, out var pos);
                RightEye.TryGetFeatureValue(CommonUsages.rightEyeRotation, out var rot);

                this.gameObject.transform.localPosition = pos;
                this.gameObject.transform.localRotation = rot;
            }
        }
    }
}