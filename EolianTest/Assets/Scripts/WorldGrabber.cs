using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vodgets
{
    public class WorldGrabber : MonoBehaviour
    {
        public Selector selectorLeft;
        public Selector selectorRight;

        Srt cursorSrt = new Srt();  // Cursor model in cameraRig (this.transform) space.
        Srt worldSrt = new Srt();   // World as a child of cameraRig ( world model in cameraRig space ). 
        Srt childSrt = new Srt();   // World as a child of cursorSrt ( world model in cursorSrt space ).

        float initialDistance = 0;

        bool leftGrabbing = false;
        bool rightGrabbing = false;

        public bool scaleLock = false;
        public bool dollyMode = true;

        private void Start()
        {
            // Register for button events as an external client of each selector.
            selectorLeft.ButtonEvt  += LeftGrab;
            selectorRight.ButtonEvt += RightGrab;
        }

        // Set cursorSrt to the cursor model in camaraRig space.
        private void SetLocalCursor()
        {

            if ( leftGrabbing && rightGrabbing )
            {
                
                Vector3 tempVec2 = (selectorLeft.transform.localPosition + selectorRight.transform.localPosition) * 0.5f;

                cursorSrt.localPosition = tempVec2;

                Vector3 tempVec = selectorLeft.transform.localPosition - selectorRight.transform.localPosition;
                cursorSrt.localRotation = Quaternion.LookRotation(tempVec, Vector3.up);

                if (scaleLock == false)
                {
                    float currentDistance = Vector3.Distance(selectorLeft.transform.localPosition, selectorRight.transform.localPosition);

                    float scaletemp = currentDistance / initialDistance;
                    // Vector3 tempVec3 = new Vector3(scaletemp, scaletemp, scaletemp);
                    cursorSrt.localScale = Vector3.one * scaletemp;
                }
                else
                {
                    cursorSrt.localScale = Vector3.one;
                }

            } else if ( leftGrabbing )
            {

                cursorSrt.Set(selectorLeft.transform.GetLocalSrt());

            } else if ( rightGrabbing )
            {

                cursorSrt.Set(selectorRight.transform.GetLocalSrt());
            }

            if (dollyMode == true)
            {
                Vector3 wonkylocal = cursorSrt.localRotation * Vector3.up;
                Quaternion DQ = Quaternion.FromToRotation(wonkylocal, Vector3.up);
                // Vector3 fixedVec = DQ * wonkylocal;
                cursorSrt.localRotation = Quaternion.FromToRotation(wonkylocal, Vector3.up) * cursorSrt.localRotation;

            }
        }

        private void LeftGrab( Selector.ButtonType button, bool grabState )
        {
            if (button != Selector.ButtonType.Grip)
                return;

            initialDistance = Vector3.Distance(selectorLeft.transform.localPosition, selectorRight.transform.localPosition);

            leftGrabbing = grabState;

            if ( leftGrabbing || rightGrabbing )
            {

                if (!rightGrabbing)
                {

                    worldSrt.Clear();

                }

                Srt cameraRig = new Srt(transform);

                SetLocalCursor();

                childSrt = cameraRig.Inverse() * worldSrt;
                childSrt = cursorSrt.Inverse() * childSrt;

            }
        }

        private void RightGrab(Selector.ButtonType button, bool grabState)
        {
            if (button != Selector.ButtonType.Grip)
                return;

            rightGrabbing = grabState;

            initialDistance = Vector3.Distance(selectorLeft.transform.localPosition, selectorRight.transform.localPosition);

            if (leftGrabbing || rightGrabbing)
            {

                if (!leftGrabbing)
                {

                    worldSrt.Clear();

                }

                Srt cameraRig = new Srt(transform);

                SetLocalCursor();

                childSrt = cameraRig.Inverse() * worldSrt;
                childSrt = cursorSrt.Inverse() * childSrt;

            }
        }

        private void Update()
        {
            if (!leftGrabbing && !rightGrabbing)
                return;

            SetLocalCursor();

            Srt CameraRigSrt = new Srt(transform);

            worldSrt = CameraRigSrt * (cursorSrt * childSrt);            

            // Convert childSrt through cameraRigSrt and cursorSrt to world space and save to a new local variable worldMovedSrt.
            // This is where the world would be if it actually could be moved!

            // Note: cameraRigSrt and worldMovedSrt are now siblings in world space.
            // Convert cameraRigSrt (this) frame to be a child of worldMovedSrt and save to cameraRigMovedSrt.
            // Note: This is where the cameraRig would be in worldMovedSrt space!

            Srt cameraRigMovedSrt = worldSrt.Inverse() * CameraRigSrt;

            // But we cannot actually move world space so we move cameraRig instead.
            // If we changed worldMovedSrt to identity cameraRigMovedSrt is just the new cameraRig transform in world (identity) space.
            // You don't have to actually change worldMovedSrt to identity... just set cameraRig (this) transform to cameraRigMovedSrt.

            this.transform.SetLocalSRT(cameraRigMovedSrt);

            // Note: Use the transform.SetLocalSrt( ... ) method to set cameraRig (this) frame to your result.
        }
    }
}