using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TeleportationActivator : MonoBehaviour
{

    public XRRayInteractor TeleportInteractor;
    public InputActionProperty TeleportActivatorAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TeleportInteractor.gameObject.SetActive(false);

        TeleportActivatorAction.action.performed += Action_performed;
    }

    private void Action_performed(InputAction.CallbackContext obj)
    {
        TeleportInteractor.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(TeleportActivatorAction.action.WasReleasedThisFrame())
        {
            TeleportInteractor.gameObject.SetActive(false);
        }
    }
}
