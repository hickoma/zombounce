using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;

public class PurchaseController : MonoBehaviour, IStoreListener
{
    #region Fields
    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

    private const string REMOVE_ADS = "com.hickoma.zombounce.remove_ads";
    #endregion

    public bool IsRemoveAdsBought { get; private set; }

    public void LateStart()
    {
        if (m_StoreController == null)
            InitializePurchasing();

		GameEventsController.Instance.OnPurchaseNoAds += BuyRemoveAds;
    }
    
    void BuyRemoveAds()
    {
        if (!IsRemoveAdsBought)
            BuyProductID(REMOVE_ADS);
    }

	#region Public
    // Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google. 
    // Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            Debug.Log("[IAPManager] RestorePurchases FAIL. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer) {

            Debug.Log("[IAPManager] RestorePurchases started ...");

            IAppleExtensions apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            // Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
            // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
            apple.RestoreTransactions((result) =>
            {
                // The first phase of restoration. If no more responses are received on ProcessPurchase then 
                // no purchases are available to be restored.
                Debug.Log("[IAPManager] RestorePurchases result = " + result);
            });
        }
    }
    #endregion

    #region Helpers
    public void InitializePurchasing()
    {
        if (IsInitialized())
            return;

        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(REMOVE_ADS, ProductType.NonConsumable);
        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);

            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("[IAPManager] Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("[IAPManager] BuyProductID FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("[IAPManager] BuyProductID FAIL. Not initialized.");
        }
    }

    private void OnRemoveAdsBought()
    {
        IsRemoveAdsBought = true;
        Debug.Log("[IAPManager] Player has RemoveAds product");
        Systems.GameState.Instance.AreAdsActive = false;
    }

    private bool ValidatePurchase(string receipt)
    {
        bool isValidPurchase = true;

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        var validator = new CrossPlatformValidator(GooglePlayTangle.Data(),
        AppleTangle.Data(), Application.identifier);

        try {
            var result = validator.Validate(receipt);
            Debug.Log("[IAPManager] Receipt is valid.");
        }
        catch (IAPSecurityException) {
            Debug.Log("[IAPManager] Invalid receipt, not unlocking content");
            isValidPurchase = false;
        }
#endif

        return isValidPurchase;
    }
    #endregion

    #region IStoreListener
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("[IAPManager] OnInitialized: PASS");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;

        if (!Systems.GameState.Instance.AreAdsActive)
        {
            Product removeAdsProduct = controller.products.WithID(REMOVE_ADS);

            if (removeAdsProduct != null && removeAdsProduct.hasReceipt && ValidatePurchase(removeAdsProduct.receipt))
                OnRemoveAdsBought();
        }

        RestorePurchases();
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("[IAPManager] OnInitializeFailed InitializationFailureReason: " + error);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        //Consider sharing this reason with the user to guide their troubleshooting actions.
        Debug.Log(string.Format("[IAPManager] OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        string productID = args.purchasedProduct.definition.id;
        Debug.Log(string.Format("[IAPManager] ProcessPurchase: Product: '{0}'", productID));

        if (ValidatePurchase(args.purchasedProduct.receipt))
        {
            if (string.Equals(productID, REMOVE_ADS, System.StringComparison.Ordinal))
                OnRemoveAdsBought();
        }

        return PurchaseProcessingResult.Complete;
    }
    #endregion
}
