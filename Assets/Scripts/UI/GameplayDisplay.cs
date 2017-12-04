﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameplayDisplay : Display, IController
{
	[SerializeField] private Text _weightTextField;
	[SerializeField] private Text _cakesTextField;
	[SerializeField] private Text _teaTextField;
	[SerializeField] private Text _timerTextField;

	public string Name
	{
		get
		{
			return "Gameplay Display";
		}
	}

	private void Start()
	{
		this.DeactivateTimer();

		this.UpdateWeightDisplay();
		this.UpdateCakesDisplay();
		this.UpdateTeaDisplay();

		Game.Instance.RegisterOnResetEvent(this);

		Game.Instance.PrincessCake.Model.OnConsumeCake += this.UpdateWeightDisplay;
		Game.Instance.PrincessCake.Model.OnConsumeCake += this.UpdateCakesDisplay;

		Game.Instance.PrincessCake.Model.OnConsumeTea += this.UpdateWeightDisplay;
		Game.Instance.PrincessCake.Model.OnConsumeTea += this.UpdateTeaDisplay;
	}

	public void UpdateWeightDisplay()
	{
		this._weightTextField.text = "Weight - " + Game.Instance.PrincessCake.Model.Weight.ToString();
	}

	public void UpdateCakesDisplay()
	{
		this._cakesTextField.text = "Cakes - " + Game.Instance.PrincessCake.Model.CakesEaten.ToString();
	}

	public void UpdateTeaDisplay()
	{
		this._teaTextField.text = "Tea - " + Game.Instance.PrincessCake.Model.TeasDrunk.ToString();
	}

	public void ActivateTimer()
	{
		this._timerTextField.gameObject.SetActive(true);
	}

	public void DisplayTimer(float time)
	{
		this._timerTextField.text = "Time left - " + time.ToString("{0.00}");
	}

	public void DeactivateTimer()
	{
		this._timerTextField.gameObject.SetActive(false);
	}

#if UNITY_EDITOR
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
	}

	public void OnResetEvent()
	{
		this.DeactivateTimer();

		this.UpdateWeightDisplay();
		this.UpdateCakesDisplay();
		this.UpdateTeaDisplay();
	}

	public void OnDisableEvent()
	{
		
	}
#endif
}

namespace New.UTILITY
{
#if UNITY_EDITOR
	[CustomEditor(typeof(GameplayDisplay))]
	[CanEditMultipleObjects]
	public class GameplayDisplayEditor : Editor
	{
		private void OnEnable()
		{

		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

#pragma warning disable 0219
			GameplayDisplay sGameplayDisplay = target as GameplayDisplay;
#pragma warning restore 0219
		}
	}
#endif
}