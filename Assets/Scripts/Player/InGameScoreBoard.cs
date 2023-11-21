using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;

public class InGameScoreBoard : MonoBehaviour
{
    [SerializeField] private Transform scoreBoard;
    [SerializeField] private OwnerGetter ownerGetter;
    [SerializeField] private TMP_Text scoreText;

    private PlayerRef owner;

    private void Start()
    {
        InvokeRepeating(nameof(UpdateBoard), 1f, 1f);
    }
    // Update is called once per frame
    public void SetOwner(PlayerRef owner)
    {
        this.owner = owner;
    }
    void Update()
    {
        UpdateActivationBoard();
        UpdateBoard();
    }

    private void UpdateActivationBoard()
    {
        if (Input.GetKey(KeyCode.Tab))
            scoreBoard.gameObject.SetActive(true);
        else
            scoreBoard.gameObject.SetActive(false);
    }

    private void UpdateBoard()
    {
        ClearBoard();
        PopulateBoard();
        /* if (ownerGetter.GetOwner.IsValid)
            Debug.Log(ownerGetter.GetOwner); */
    }
    private void PopulateBoard()
    {
        if (ownerGetter.GetOwner.IsValid && ScoreManager.Instance.IsReady)
        {
            Debug.Log(ScoreManager.Instance.GetScore(ownerGetter.GetOwner));
            foreach (var item in ScoreManager.Instance.playerScores)
            {
                Debug.Log(item.Key);
            }
        }

    }
    private void ClearBoard()
    {
        foreach (Transform scoreText in scoreBoard)
        {
            Destroy(scoreText.gameObject);
        }

    }
}
