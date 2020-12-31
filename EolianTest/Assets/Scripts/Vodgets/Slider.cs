using UnityEngine;
using UnityEngine.Events;

namespace Vodgets
{
    public class Slider : Vodget
    {

        protected bool isGrabbing = false;

        [System.Serializable]
        public class SliderEvent : UnityEvent<float> { }

        [SerializeField]
        public SliderEvent slider_changed;

        public float min_y = 0.7379591f;
        public float max_y = 3.49f;

        float StoredYValue = 0;

        // Declare any class variables here. 
        // Hint: You will need to save the local y value at the moment of grab to compute the change in knob position during FocusUpdate.

        // The selector notifies a vodget when the user has selected or deselected an object.
        // The technique for selection can vary from having the user "touch" a game object, shoot a selection ray at an object, or
        // in the case of this homework the selection is "fixed". See the FixedSelector component on CameraRig/Controller( right ). 
        public override void Focus(Selector cursor, bool state)
        {

            // Optional: When your vodget gets focus from a selector you could provide user feedback.
            // Examples of feedback are typically highlighting and providing a haptic pulse. 

        }

        // The selctor sends vodgets with focus all button events. 
        public override void Button(Selector selector, Selector.ButtonType button, bool state)
        {

            if (button != Selector.ButtonType.Trigger)
                return;

            if (state == true)
            {

                Vector3 temp = this.transform.InverseTransformPoint(selector.Cursor.localPosition);

                StoredYValue = temp.y;

                // Don't allow the selector to find another client until the trigger is released.
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

        // The selector calls FocusUpdate every Update that a vodget has its focus.
        public override void FocusUpdate(Selector selector)
        {

            if (!isGrabbing)
                return;
                       
            Vector3 currCursor = transform.InverseTransformPoint(selector.Cursor.localPosition);

            float deltavalue = currCursor.y - StoredYValue;

            transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + deltavalue, this.transform.localPosition.z);
            
            if (transform.localPosition.y < min_y)
                transform.localPosition = new Vector3(this.transform.localPosition.x, min_y, this.transform.localPosition.z); ;
            if (transform.localPosition.y > max_y)
                transform.localPosition = new Vector3(this.transform.localPosition.x, max_y, this.transform.localPosition.z); ;

            float ratio = (max_y - this.transform.localPosition.y) / (max_y - min_y);

            slider_changed.Invoke(1 - ratio);

            // Convert the controllers current position into the local frame, calculate the change in position
            // along the movement axis and apply the offset to transform.localPosition to move the knob.

            // Constrain the knob stay within its track from min_y to max_y public variables (provided).

            // Compute the slider value from 0f to 1f and update the slider_changed event with slider_changed.Invoke( slider_value );
            //
            // Note: A EventValuePrinter component script is provided to show how to configure and use UnityEvents. 
            // The EventValuePrinter component has already been added to the handle node and mapped to the slider_value event.
            // You will need to understand how this is done to complete future homeworks.

        }
    }
}