using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class koma : MonoBehaviour {

    public GameObject stone;

    public GameController gameController;

    private int currentPlayer = -1;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        int player = gameController.getCurrentPlayer();
        if (currentPlayer != player)
        {
            reverse(stone);
            currentPlayer *= -1;
        }
	}

    private void reverse(GameObject stone)
    {

        Vector3 localAngle = stone.transform.localEulerAngles;
        localAngle.x = 180.0f;
        stone.transform.localEulerAngles = localAngle;

    }
}
