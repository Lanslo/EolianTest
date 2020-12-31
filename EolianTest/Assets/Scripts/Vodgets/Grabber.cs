using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vodgets
{
    public class Grabber : Vodget
    {
        protected bool isGrabbing = false;

        Selector currSelc = null;

        // Declare any class variables here. 
        // Hint: The child Srt must be calculated and saved when the Trigger button is pressed. 

        Srt Child = new Srt();

        // The selector notifies a vodget when the user has selected or deselected an object.
        // The technique for selection can vary from having the user "touch" a game object, shoot a selection ray at an object, or
        // in the case of this homework the selection is "fixed". See the FixedSelector component on CameraRig/Controller( right ). 
        public override void Focus(Selector cursor, bool state)
        {

            // Optional: When your grabber vodget gets focus from a selector you could provide user feedback.
            // Examples of feedback are typically highlighting and providing a haptic pulse.

            

        }

        // The selctor sends vodgets with focus all button events. 
        public override void Button(Selector selector, Selector.ButtonType button, bool state)
        {
            if (button != Selector.ButtonType.Trigger)
                return;

            if (currSelc != null && selector != currSelc)
            {
                return;
            }

            if (state == true)
            {
                // Hint: You will need to access the world cursor Srt at selector.Cursor
                // This component script is attached to the scaled cube primative.
                // Convert this.transform to a world space Srt that you then convert to be a child of the world cursor.
                // Save this child Srt as a class variable to be used during the focusUpdate.

                currSelc = selector;

                Child.localPosition = this.transform.position;
                Child.localRotation = this.transform.rotation;
                Child.localScale    = Vector3.one;

                Child = currSelc.Cursor.Inverse() * Child;

                // Don't allow the selector to find another client until the trigger is released.
                currSelc.GrabFocus(true);
                isGrabbing = true;
            }
            else
            {
                // The trigger is released so free the selector to find other clients.

                if (currSelc != null)
                {
                currSelc.GrabFocus(false);
                isGrabbing = false;
                currSelc = null;
                }
            }
        }

        // The selector calls FocusUpdate every Update that a vodget has its focus.
        public override void FocusUpdate(Selector selector)
        {
            if (!isGrabbing)
                return;

            // Convert the child Srt that you saved at the moment the Trigger button was pressed back to a world Srt through the current world cursor Srt.
            // Set this.transform.position and rotation to the result.

            Srt Offset = currSelc.Cursor * Child;

            this.transform.position = Offset.localPosition;
            this.transform.rotation = Offset.localRotation;

        }
    }
}
