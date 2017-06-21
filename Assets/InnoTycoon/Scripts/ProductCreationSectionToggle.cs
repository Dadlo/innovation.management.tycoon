using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductCreationSectionToggle : MonoBehaviour {

	public delegate void OnToggled(Transform toggledTrans);

	public OnToggled onToggled;

	public void OnToggledOn(bool toggledOn) {
		if (toggledOn) {
			onToggled(transform);
		}
	}
}
