////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System;
using UnityEngine;
using SA.iOS.StoreKit;
using SA.iOS.Examples;


    public class SX_InApp_iOS : MonoBehaviour 
    {

        private static SX_BillingClient_iOS s_paymentManager;


        private void Start()
        {


#if UNITY_ANDROID
        Destroy(GetComponent<SX_InApp_iOS>());
#endif


        if (s_paymentManager == null)
            {
                s_paymentManager = new SX_BillingClient_iOS();
            }


            s_paymentManager.init();
        }


        public void Purchase(string ProductId)
        {
            if (ProductId == "words_us_p1")
                ProductId = "wordup_1";
            if (ProductId == "words_us_p2")
                ProductId = "wordup_2";
            if (ProductId == "words_us_p3")
                ProductId = "wordup_3";
            if (ProductId == "words_us_p4")
                ProductId = "wordup_4";
            if (ProductId == "words_us_p5")
                ProductId = "wordup_5";
        ISN_SKPaymentQueue.AddPayment(ProductId);
        }

    }
