﻿using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class GroundButtonController : MonoBehaviour {

    public float PositionOffsetYOnPressed = -.06f;
    public GroundButtonModel.Settings Settings;
    
    public GroundButtonModel Model { get; private set; }

    protected virtual void Start () {
        Model = new GroundButtonModel(Settings, Game.Instance.PrincessCake.Settings);
    }

    protected virtual void OnCollisionEnter(Collision collision) {
        foreach (ContactPoint contact in collision.contacts) {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }

        IWeightableController controller = collision.gameObject.GetComponent<IWeightableController>();

        if (controller != null) {
            HopedOn(controller);
        }
    }

    protected virtual void OnCollisionExit(Collision collision) {
        IWeightableController controller = collision.gameObject.GetComponent<IWeightableController>();

        if (controller != null) {
            HopedOff(controller);
        }
    }

    protected virtual void Update() {
        TryDepress();

#if UNITY_EDITOR
        if (Input.GetKeyUp(KeyCode.E)) {
            HopedOn(Game.Instance.PrincessCake);
        }

        if (Input.GetKeyUp(KeyCode.L)) {
            HopedOff(Game.Instance.PrincessCake);
        }
#endif
    }

    // To be overriden by elevator.
    protected virtual void OnHopedOnBy(IWeightableController controller) {
        transform.SetPosY(transform.position.y + PositionOffsetYOnPressed);
    }

    protected virtual void OnHopedOffBy(IWeightableController controller) {

    }

    protected virtual void OnDepressed() {
        transform.SetPosY(transform.position.y - PositionOffsetYOnPressed);
    }


    private void HopedOn(IWeightableController controller) {
        if (Model.HopedOn(controller.Model)) {
            OnHopedOnBy(controller);
        }
    }

    private void HopedOff(IWeightableController controller) {
        if (Model.HopedOff(controller.Model, Time.time)) {
            OnHopedOffBy(controller);
        }
    }

    private void TryDepress() {
        if (Model.Depressed(Time.time)) {
            OnDepressed();
        }
    }
}
