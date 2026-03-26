using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerPanel : MonoBehaviour
{
    private Player _player;
    [SerializeField] TMP_Text _scoretext;

    public void Bind(Player player)
    {
        _player = player;
    }

    private void Update()
    {
        _scoretext.SetText(_player.Coins.ToString());
    }
}
