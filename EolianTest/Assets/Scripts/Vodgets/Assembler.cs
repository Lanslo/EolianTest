using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vodgets
{
    public class Assembler : Grabber
    {
        //public Transform controller;
        //public DockPin pin;
        public float local_break_dist = 0.25f;

        // The grabbing controller.
        Selector controller = null;

        // The distance the controller moves in the local space after snapping before unsnapping.
        // The local controller position the moment after the snap. 
        // You need this to test when stress on the joint exceeds local_break_dist.
        // You also need this to rotate the pin joint after the snap.
        protected Vector3 grabpt_after_snap = Vector3.zero;

        // The dock we might be snapped to.
        protected DockPinTarget othdock = null;

        // We save the hierarchy values prior to snapping so we can restore them if the snap breaks.
        Transform parent_save = null;

        private void OnTriggerEnter(Collider other)
        {
            // Reject collisions when not being grabbed and when already snapped.
            if (controller == null || othdock != null)
                return;


            // If we collided with an object that has a DockPinTarget Snap (reparent) to the dock (other) as parent.


            // Set grabpt_after_snap to the local Cursor position. 
            // This will be used to measure ongoing stress on the dock.


        }

        void Snap(Transform othdockTransform )
        {

            // Save the wheels (this.transform.parent) current parent and reparent the wheel to othdock
            // Note: After snapping make sure the transforms localPosition and localRotation are identity.




        }

        void Unsnap()
        {

            // Unsnap the wheel from the dock by reparenting it back to the saved parent and clearing othdock.



        }

        // Test the difference between the local Cursor position currently and immediately after the snap.
        // If it is greater than local_break_dist (provided) returning true will break the snap.
        // Otherwise when the stress on the joint is within limits return true.
        bool TestBreakSnap(Selector selector)
        {
            bool break_snap = false;


            // Write a test to determine if the controller has moved past local_break_dist in the local frame.




            return break_snap;
        }

        void RotatePin()
        {

            // Rotate in the local frame when othdock != null (snapped). 



        }

        public override void Button(Selector selector, Selector.ButtonType button, bool state)
        {
            if (button != Selector.ButtonType.Trigger)
                return;

            // Save the grabbing selector for onTriggerEnter
            controller = (state) ? selector : null;

            // Do a normal grab if not docked.
            if ( othdock == null)
                base.Button(selector, button, state);

            selector.GrabFocus(state);
        }

        public override void FocusUpdate(Selector selector)
        {
            if (othdock == null)
            {
                // Do a normal grab if not docked
                base.FocusUpdate(selector);

            } else
            {
                // Rotate the wheel when snapped.
                // Break the snap when excessive stress on the joint is detected.
                if (TestBreakSnap(selector))
                    Unsnap();
                else
                    RotatePin();
            }
        }
    }

}
