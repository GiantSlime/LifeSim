using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBar : MonoBehaviour
{
    public RectTransform Bar;

    // Start is called before the first frame update
    void Start()
    {
        if (Bar == null)
            throw new System.Exception($"BAR IS MISSING FROM ENERGY BAR: {gameObject.name}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void SetEnergyLevel(float value)
	{
        
	}
}
