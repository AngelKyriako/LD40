﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PickUpController : MonoBehaviour
{
	private BoxController _pickedUpBox;

	private void OnTriggerStay(Collider other)
	{
		Debug.Log("Collision detected");
		if (Input.GetKeyDown(KeyCode.E) && this._pickedUpBox == null)
		{
			this._pickedUpBox = other.GetComponent<BoxController>();

			Debug.Log(this._pickedUpBox);

			if (this._pickedUpBox != null && this._pickedUpBox.Model.CanMoveBy(Game.Instance.PrincessCake.Model))
			{
				this._pickedUpBox.GetComponent<Rigidbody>().isKinematic = true;
				this._pickedUpBox.transform.position = this.transform.position;
				this._pickedUpBox.transform.SetParent(this.transform);
				Debug.Log("nice");
				this.StopAllCoroutines();
				this.StartCoroutine(this.HandleInput());
			}
		}
	}

	private IEnumerator HandleInput()
	{
		yield return new WaitForEndOfFrame();
		while (true)
		{
			if (Input.GetKeyDown(KeyCode.E) && this._pickedUpBox != null)
			{
				this._pickedUpBox.GetComponent<Rigidbody>().isKinematic = false;
				this._pickedUpBox.transform.SetParent(null);
				this._pickedUpBox = null;
			}

			yield return null;
		}
	} 

#if UNITY_EDITOR
	protected virtual void OnDrawGizmos()
	{

	}
#endif
}

namespace New.UTILITY
{
#if UNITY_EDITOR
	[CustomEditor(typeof(PickUpController))]
	[CanEditMultipleObjects]
	public class PickUpControllerEditor : Editor
	{
		private void OnEnable()
		{

		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

#pragma warning disable 0219
			PickUpController sPickUpController = target as PickUpController;
#pragma warning restore 0219
		}
	}
#endif
}