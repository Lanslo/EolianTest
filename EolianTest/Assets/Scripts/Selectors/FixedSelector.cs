using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vodgets
{
    // This is a very trivial example of a selector that only selects a vodget set in advance by the user.
    // Because selection is known it immediately focus's on that vodget and sends a Trigger button down event.
    // The FixedSelector is provided to show how vodgets might be tested when no VR gear is present.
    // It is also provided to demonstrate bare bones responsibilities of a Selector.
    public class FixedSelector : Selector
    {
        public Vodget focus_vodget = null;

        protected override void SetCursor()
        {
            Cursor.Set(transform.position, transform.rotation, Vector3.one);
        }

        // Use this for initialization
        void Start()
        {
            focusVodget = focus_vodget;
            SetCursor();
            focus_vodget.Focus(this, true);
            focus_vodget.Button(this, ButtonType.Trigger, true);
        }

        // Update is called once per frame
        void Update()
        {
            SetCursor();
            focus_vodget.FocusUpdate(this);
        }
    }
}
