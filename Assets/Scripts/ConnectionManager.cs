using System;
using Thirdweb;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionManager : MonoBehaviour 
{
    [SerializeField] private TMP_Text _statusText;
    [SerializeField] private Button _connectButton;

    private string activeChain;

    private void Awake() 
    {
        _connectButton.onClick.AddListener(ConnectMetaMask);
    }

    private void Start() 
    {
        activeChain = ThirdwebManager.Instance.activeChain;
        ThirdwebManager.Instance.Initialize(activeChain);
    }

    private async void ConnectMetaMask() 
    {
        try 
        {
            _statusText.text = "Connecting to Metamask...";
            _connectButton.interactable = false;
            var chainData = ThirdwebManager.Instance.supportedChains.Find(c => c.identifier == activeChain);
            var walletConnection = new WalletConnection(WalletProvider.Metamask, int.Parse(chainData.chainId));
        
            var walletAddress = await ThirdwebManager.Instance.SDK.wallet.Connect(walletConnection);
            _statusText.text = $"Connected to wallet {walletAddress}";
        }
        catch (Exception e) 
        {
            _connectButton.interactable = true;
            _statusText.text = "Connection to Metamask failed... try again";
            Debug.LogException(e);
        }
    }
    
}
