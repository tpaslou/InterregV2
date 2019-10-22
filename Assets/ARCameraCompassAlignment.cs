using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using UnityARInterface;
using Mapbox.Unity.Location;



public class ARCameraCompassAlignment : MonoBehaviour
{
    #region Editor Variables

    /// <summary>
    /// A reference to the Unity AR Interface ARController.
    /// </summary>
    [SerializeField]
    [Tooltip("A reference to the Unity AR Interface ARController.")]
    private ARController arController = null;

    /// <summary>
    /// Indicates whether or not to realign with the compass on every Update.
    /// </summary>
    [SerializeField]
    [Tooltip("Indicates whether or not to realign with the compass on every Update.")]
    private bool realignOnUpdate = false;

    /// <summary>
    /// Indicates whether or not to realign with the compass on the game was started.
    /// </summary>
    [SerializeField]
    [Tooltip("Indicates whether or not to realign with the compass on the game was started.")]
    private bool realignAfterStart = true;

    /// <summary>
    /// Indicates whether or not to realign with the compass after the game is unpaused. 
    /// </summary>
    [SerializeField]
    [Tooltip("Indicates whether or not to realign with the compass after the game is unpaused.")]
    private bool realignAfterPause = true;

    /// <summary>
    /// Disables the compass alignment in the Unity Editor.
    /// </summary>
    [SerializeField]
    [Tooltip("Disables the compass alignment in the Unity Editor.")]
    private bool disableInEditor = true;

    #endregion



    #region Private variables

    // The current default location provider
    private ILocationProvider locationProvider = null;

    // Stores if the initial align happened
    private bool didInitialAlign = false;

    // Stores when the application was unpaused
    private bool didUnpause = false;

    #endregion



    #region Properties

    // Returns whether or not a compass realignment is needed on the next update
    private bool ShouldRealign
    {
        get
        {
            // Can't realign is there's no location provider
            if (this.locationProvider == null)
            {
                return false;
            }

            // Don't realign if in the Unity editor
#if UNITY_EDITOR
            if (this.disableInEditor)
            {
                return false;
            }
#endif

            // Realign on every update if needed
            if (this.realignOnUpdate)
            {
                return true;
            }

            // Realign if no initial align took place yet
            if (this.realignAfterStart && !this.didInitialAlign)
            {
                this.didInitialAlign = true;
                return true;
            }

            // Realign if the application was paused
            if (this.realignAfterPause && this.didUnpause)
            {
                this.didUnpause = false;
                return true;
            }

            // Realign if the difference between the camera orientation & the compass orientation is too high,
            // we assume something went wrong with the AR rotation
            float worldY = this.arController.arCamera.transform.eulerAngles.y;
            float worldDiff = Mathf.Abs(Mathf.DeltaAngle(this.CompassOrientation, worldY));

            /*if (this.CompassAccuracy <= 25 && worldDiff > 20)
            {
                return true;
            }*/

            return false;
        }
    }

    private float CompassOrientation
    {
        get
        {
            float compassOrientation = this.locationProvider.CurrentLocation.DeviceOrientation;
            return compassOrientation;
        }
    }

    /*
    private float CompassAccuracy
    {
        get
        {
            float compassAccuracy = this.locationProvider.CurrentLocation.DeviceOrientationAccuracy;
            return compassAccuracy;
        }
    }
    */
    #endregion



    #region Unity Lifecycle

    void Start()
    {
        this.locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
    }

    private void Update()
    {
        if (this.ShouldRealign)
        {
            Realign(this.CompassOrientation);
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            this.didUnpause = true;
        }
    }

    #endregion



    #region Realign

    [ContextMenu("Realign")]
    private void Realign(float orientation)
    {
        float inverseRotation = (orientation - this.transform.localEulerAngles.y) * -1;
        this.transform.rotation = Quaternion.Euler(new Vector3(0, inverseRotation, 0));
    }

    #endregion

}