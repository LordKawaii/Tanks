using UnityEngine;
using System.Collections;

public class TerrianGeneration : MonoBehaviour {
	public Transform floorTransform;
	public GameObject hill;
	public int minHills = 2;
	public int maxHills = 5;

	public float minWidth = .3f;
	public float maxWidth = 2f;

	public float minHight = .3f;
	public float maxHight = 2f;

	void Start()
	{
		if (floorTransform != null)
		{
			float StartingX = floorTransform.transform.position.x - (floorTransform.lossyScale.x/2);
			if (hill != null)
			{
				int totalHills = Random.Range(minHills, maxHills);
				float incrementX = floorTransform.lossyScale.x / totalHills;
				float currentX = StartingX;
                for (int i = totalHills; i > 0; i--)
				{
					float hillPlacement = Random.Range(currentX, currentX + incrementX);
					float hillHight = Random.Range(minHight, maxHight);
					float hillWidth = Random.Range(minWidth, maxWidth);
					GameObject tempHill = Instantiate(hill, new Vector3(hillPlacement, hill.transform.position.y, 0), Quaternion.identity) as GameObject;
					tempHill.transform.localScale = new Vector3(hillWidth, hillHight, 0);
					currentX += incrementX;
				}
			}
        }

	}
}
