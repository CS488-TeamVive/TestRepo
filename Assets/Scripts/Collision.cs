using UnityEngine;
using System.Collections;

public class Collision : MonoBehaviour {

    private Renderer rend;
    public Material defaultMat, highlightedMat;

    void OnEnable()
    {
        EventHandlerRightController.OnTriggerClick += EventHandlerRightController_OnTriggerClick;
    }

    private void EventHandlerRightController_OnTriggerClick(object sender, VRTK.ControllerInteractionEventArgs e)
    {
        this.gameObject.SetActive(false);
    }

    // Use this for initialization
    void Start ()
    {
        rend = GetComponent<Renderer>();
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("Trigger Enter");
        rend.sharedMaterial = highlightedMat;
    }

    void OnTriggerExit(Collider col)
    {
        Debug.Log("Trigger Exit");
        rend.sharedMaterial = defaultMat;
    }
}
