﻿using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TerrainButtonController : MonoBehaviour, IController {

    [SerializeField]
    private bool _triggeredByPlayerOnly = false;

    [SerializeField]
    private string _firstTimeHopedOnButtonNotFatText = "You need to to maximize your weight to press buttons.";
    private string _firstTimeHopedOnButtonNotFatGrabBoxesText = "Then, you could also grab & move boxes !!!";
    private bool _firstTimeHopedOnButtonNotFat = false;

    [SerializeField]
    private string _firstTimeHopedOnButtonFatText = "Good job. Did you know that you can use 'E' to move boxes on buttons ?";
    private bool _firstTimeHopedOnFatButton = false;

    [SerializeField]
    private string _firstTimeHopedOffButtonText = "Run Forest!! Or use a box. The button is 'E'";
    private static bool _firstTimeHopedOffButton = false;

    [SerializeField]
    private AudioClip _onPressed;
    [SerializeField]
    private AudioClip _onDepressed;

    public List<TweenController> TweensOnAtPress = new List<TweenController>();
    public List<TweenController> TweensOffAtDepress = new List<TweenController>();
    public List<IController> DisableAtPress = new List<IController>();
    public List<IController> DisableAtDepress = new List<IController>();

    public TerrainButtonModel.Settings Settings;

    public TerrainButtonModel Model { get; private set; }
    public string Name { get { return name; } }

    private Logger _logger;
    private AudioSource _audio;

    protected virtual void Start () {
        _logger = Game.Instance.LoggerFactory(name + "::TerrainButtonController");

        Model = new TerrainButtonModel(name, Settings, Game.Instance.PrincessCake.Settings);

        _audio = this.GetOrAddComponent<AudioSource>();
    }

    protected virtual void OnTriggerEnter(Collider collider) {
        IWeightableController controller = collider.gameObject.GetComponent<IWeightableController>();

        _logger.Info("OnTriggerEnter", collider.gameObject.name + " entered");

        if (controller != null) {

            if (_triggeredByPlayerOnly && controller != Game.Instance.PrincessCake) {
                return;
            }

            HopedOn(controller);
        }
    }

    protected virtual void OnTriggerExit(Collider collider) {
        IWeightableController controller = collider.gameObject.GetComponent<IWeightableController>();

        _logger.Info("OnTriggerExit", collider.gameObject.name + " left");

        if (controller != null) {
            HopedOff(controller);
        }
    }

    protected virtual void Update() {
        TryDepress();

#if UNITY_EDITOR
        if (Input.GetKeyUp(KeyCode.I)) {
            HopedOn(Game.Instance.PrincessCake);
        }

        if (Input.GetKeyUp(KeyCode.J)) {
            HopedOff(Game.Instance.PrincessCake);
        }
#endif
    }

    // To be overriden by elevator.
    protected virtual void OnHopedOnBy(IWeightableController controller) {
        _logger.Info("OnHopedOnBy", controller.Name + " hoped on");

        foreach (TweenController tweensToOn in TweensOnAtPress) {
            tweensToOn.TryTweenToOn(true);
        }

        foreach (IController ctrl in DisableAtPress) {
            Game.Instance.Disable(ctrl);
        }

        _audio.TryPlaySFX(_onPressed);

        if (!_firstTimeHopedOnFatButton && controller == Game.Instance.PrincessCake) {
            UserInterfaceController.Instance_._PopUpDisplay.Display(_firstTimeHopedOnButtonFatText);
            _firstTimeHopedOnFatButton = true;
        }
    }

    protected virtual void OnHopedOffBy(IWeightableController controller) {
        _logger.Info("OnHopedOnBy", controller.Name + " hoped off");

        if (!_firstTimeHopedOffButton) {
            UserInterfaceController.Instance_._PopUpDisplay.Display(_firstTimeHopedOffButtonText);
            _firstTimeHopedOffButton = true;
        }
    }

    protected virtual void OnDepressed() {
        _logger.Info("OnDepressed");

        foreach (TweenController tweensToOff in TweensOffAtDepress) {
            tweensToOff.TryTweenToOff(true);
        }

        foreach (IController ctrl in DisableAtDepress) {
            Game.Instance.Disable(ctrl);
        }

        _audio.TryPlaySFX(_onDepressed);
    }

    private void HopedOn(IWeightableController controller) {
        if (Model.HopedOn(controller.Model())) {
            OnHopedOnBy(controller);
        } else {
            if (!_firstTimeHopedOnButtonNotFat && controller == Game.Instance.PrincessCake) {
                UserInterfaceController.Instance_._PopUpDisplay.Display(_firstTimeHopedOnButtonNotFatText, () => {

                    UserInterfaceController.Instance_._PopUpDisplay.Display(_firstTimeHopedOnButtonNotFatGrabBoxesText);

                });

                _firstTimeHopedOnButtonNotFat = true;
            }
        }
    }

    private void HopedOff(IWeightableController controller) {
        if (Model.HopedOff(controller.Model(), Time.time)) {
            OnHopedOffBy(controller);
        }
    }

    private void TryDepress() {
        if (Model.Depressed(Time.time)) {
            OnDepressed();
        }
    }

    public void OnResetEvent() {
        gameObject.SetActive(true);
    }

    public void OnDisableEvent() {
        gameObject.SetActive(false);
    }
}
