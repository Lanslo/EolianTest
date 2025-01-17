﻿
using UnityEngine;
using UnityEngine.XR;

namespace Unity.XR.WindowsMR
{
    /// <summary>
    /// Input Usages, consumed by the UnityEngine.XR.InputDevice class in order to retrieve inputs.
    /// These usages are all WindowsMR specific.
    /// </summary>
    public static class WindowsMRUsages
    {
        /// <summary>
        /// A Vector3 position representing the tip of the controller pointing forward.
        /// </summary>
        public static InputFeatureUsage<Vector3> PointerPosition = new InputFeatureUsage<Vector3>("PointerPosition");
        /// <summary>
        ///  A Quaternion rotation representing the tip of the controller pointing forward.
        /// </summary>
        public static InputFeatureUsage<Quaternion> PointerRotation = new InputFeatureUsage<Quaternion>("PointerRotation");
        /// <summary>
        /// A [0,1] value that reports the risk that detection of the hand will be lost.
        /// </summary>
        public static InputFeatureUsage<float> SourceLossRisk = new InputFeatureUsage<float>("SourceLossRisk");
        /// <summary>
        /// A Vector3 direction that reports the suggested direction the user should move his hand if he is at risk of losing tracking.
        /// </summary>
        public static InputFeatureUsage<Vector3> SourceMitigationDirection = new InputFeatureUsage<Vector3>("SourceMitigationDirection");
    }
}
