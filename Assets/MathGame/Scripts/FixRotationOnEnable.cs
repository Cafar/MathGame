using UnityEngine;
using System.Collections;

public class FixRotationOnEnable : MonoBehaviour
{
	void OnEnable()
	{
		transform.rotation = Quaternion.identity;
	}

	void OnDisable()
	{
		transform.rotation = Quaternion.identity;
	}
}
