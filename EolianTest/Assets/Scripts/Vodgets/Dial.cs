using UnityEngine;
using UnityEngine.Events;

namespace Vodgets
{
    public class Dial : Vodget
    {
        // Declare any class variables here. 

        protected bool isGrabbing = false;

        Vector3 StoredVector = new Vector3();
        Vector3 ParentVector = new Vector3();

        [System.Serializable]
        public class DialEvent : UnityEvent<float> { }

        [SerializeField]
        public DialEvent dial_changed;

        // Hint: You will need to create a public UnityEvent and wire it up to an EventValuePrinter component (See Slider homework)
        // Hint: You will need to save the local Cursor vector at the moment of grab to compute the change in knob position during FocusUpdate.

        // The selector notifies a vodget when the user has selected or deselected an object.
        // The technique for selection can vary from having the user "touch" a game object, shoot a selection ray at an object, or
        // in the case of this homework the selection is "fixed". See the FixedSelector component on CameraRig/Controller( right ). 
        public override void Focus(Selector cursor, bool state)
        {

            // Optional: When your vodget gets focus from a selector you could provide user feedback.
            // Examples of feedback are typically highlighting and providgin a haptic pulse. 


        }

        // The selctor sends vodgets with focus all button events. 
        public override void Button(Selector selector, Selector.ButtonType button, bool state)
        {

            // When the trigger button is pressed:
            // Convert the controllers position into the local frame.

            {

                if (button != Selector.ButtonType.Trigger)
                    return;


                if (state == true)
                {

                    StoredVector = this.transform.InverseTransformPoint(selector.Cursor.localPosition);
                    ParentVector = this.transform.parent.transform.up;

                    StoredVector.z = 0;

                    selector.GrabFocus(true);
                    isGrabbing = true;

                }
                else
                {
                    // The trigger is released so free the selector to find other clients.
                    selector.GrabFocus(false);
                    isGrabbing = false;
                }
            }
        }

        // The selector calls FocusUpdate every Update that a vodget has its focus.
        public override void FocusUpdate(Selector selector)
        {

            if (!isGrabbing)
                return;

            Vector3 currCursor = transform.InverseTransformPoint(selector.Cursor.localPosition);

            currCursor.z = 0;

            // degrees = radians * 180 / pi; // for later if I need it.

            Quaternion deltaRot = Quaternion.FromToRotation(StoredVector, currCursor); // sin() of theta over 2           

            transform.localRotation = transform.localRotation * deltaRot;

            // Vector3 tempVec3ParentSpace = transform.localRotation * gameObject.GetComponentInParent<Transform>().up;
            
            float Radians = Mathf.Acos(Vector3.Dot(ParentVector, transform.up));
            float degrees = Radians * 180 / Mathf.PI;

            if (Vector3.Dot(this.transform.parent.transform.right, transform.up) > 0)
            {
                degrees = 360 - degrees;
            }

            float ratio = degrees / 360.0f;

            dial_changed.Invoke(degrees);

            // Convert the controllers current position into the local frame, calculate the change in rotation
            // about the knobs spin axis and apply the offset to transform.localRotatoin to rotate the dial.

            // Compute the dial "notch" value visible at the top of the knob from 0f to 360f as it rotates 
            // away from the zero mark visible at the top of the backplate.

            // Invoke the dial changed event with the dial value that you created as a class variable. 
            // Note: If you don't see your value printed in the console you need to make certain that you have 
            // Added and properly configured an EventValuePrinter component as demonstrated with the Slider homework.

        }
    }
}