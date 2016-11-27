using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Tutorial : MonoBehaviour
{
	public List<Sprite> tutorialImages;
	public Image CurrentTutorialImage;
	int currentTutorial;
	// Use this for initialization
	void Start()
	{
		CurrentTutorialImage.sprite = tutorialImages[currentTutorial];
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonUp(0))
		{
			if (currentTutorial == tutorialImages.Count - 1)
			{
				CurrentTutorialImage.gameObject.SetActive(false);
			}
			else
			{
				CurrentTutorialImage.sprite = tutorialImages[++currentTutorial];
			}
		}
	}
}
