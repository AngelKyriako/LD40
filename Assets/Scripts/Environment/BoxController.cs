﻿using UnityEngine;

public class BoxController : MonoBehaviour, IWeightableController {

    public BoxModel.Settings Settings;
    
    public BoxModel Model { get; private set; }
    
    IWeightableModel IWeightableController.Model() {
        return Model;
    }

    protected virtual void Start() {
        Model = new BoxModel(Settings, Game.Instance.PrincessCake.Settings);
    }
}