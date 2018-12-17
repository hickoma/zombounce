using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
    public class IronSourceController : MonoBehaviour
    {
        private bool m_AdsActivity = true;

        private System.Action OnRewardVideoEndedCallback;
        private System.Action OnInterstitialEndedCallback;

        public void LateStart() 
        {
            // maybe not needed
            IronSource.Agent.validateIntegration();

            // reward videos
            IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
            IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent; 
            IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
            IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
            IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;
            IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent; 
            IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;

            GameEventsController.Instance.OnShowAdvertising += ShowRewardVideo;

            // banner
            IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);

            IronSourceEvents.onBannerAdLoadedEvent += BannerAdLoadedEvent;
            IronSourceEvents.onBannerAdLoadFailedEvent += BannerAdLoadFailedEvent;        
            IronSourceEvents.onBannerAdClickedEvent += BannerAdClickedEvent; 
            IronSourceEvents.onBannerAdScreenPresentedEvent += BannerAdScreenPresentedEvent; 
            IronSourceEvents.onBannerAdScreenDismissedEvent += BannerAdScreenDismissedEvent;
            IronSourceEvents.onBannerAdLeftApplicationEvent += BannerAdLeftApplicationEvent;

            Systems.GameState.Instance.OnAdsActivityChanged += ChangeAdsActivity;
            ChangeAdsActivity(Systems.GameState.Instance.AreAdsActive);

            // interstitial
            if (GameState.Instance.IsItTimeToShowInterstitial)
            {
                IronSource.Agent.loadInterstitial();
            }

            IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
            IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;        
            IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdShowSucceededEvent; 
            IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent; 
            IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
            IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
            IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;

            GameEventsController.Instance.OnShowInterstitial += ShowInterstitial;

            // set state on start
            GameState.Instance.IsRewardVideoAvailable = IronSource.Agent.isRewardedVideoAvailable();
            GameState.Instance.IsInterstitialReady = IronSource.Agent.isInterstitialReady();
        }

        //Invoked when the RewardedVideo ad view has opened.
        //Your Activity will lose focus. Please avoid performing heavy 
        //tasks till the video ad will be closed.
        void RewardedVideoAdOpenedEvent()
        {
            
        }  

        //Invoked when the RewardedVideo ad view is about to be closed.
        //Your activity will now regain its focus.
        void RewardedVideoAdClosedEvent()
        {
            if (OnRewardVideoEndedCallback != null)
            {
                OnRewardVideoEndedCallback();
            }
        }

        //Invoked when there is a change in the ad availability status.
        //@param - available - value will change to true when rewarded videos are available. 
        //You can then show the video by calling showRewardedVideo().
        //Value will change to false when no videos are available.
        void RewardedVideoAvailabilityChangedEvent(bool available)
        {
            //Change the in-app 'Traffic Driver' state according to availability.
            GameState.Instance.IsRewardVideoAvailable = available;
        }

        //  Note: the events below are not available for all supported rewarded video 
        //   ad networks. Check which events are available per ad network you choose 
        //   to include in your build.
        //   We recommend only using events which register to ALL ad networks you 
        //   include in your build.
        //Invoked when the video ad starts playing.
        void RewardedVideoAdStartedEvent()
        {
            
        }

        //Invoked when the video ad finishes playing.
        void RewardedVideoAdEndedEvent()
        {
            
        }

        //Invoked when the user completed the video and should be rewarded. 
        //If using server-to-server callbacks you may ignore this events and wait for the callback from the  ironSource server.
        //
        //@param - placement - placement object which contains the reward data
        //
        void RewardedVideoAdRewardedEvent(IronSourcePlacement placement)
        {
            
        }

        //Invoked when the Rewarded Video failed to show
        //@param description - string - contains information about the failure.
        void RewardedVideoAdShowFailedEvent (IronSourceError error)
        {
            
        }

        void ShowRewardVideo(System.Action onRewardVideoEnded)
        {
            // set callback
            // maybe there should be more callbacks for more cases
            OnRewardVideoEndedCallback = onRewardVideoEnded;
            // start video
            IronSource.Agent.showRewardedVideo();
        }

        //Invoked once the banner has loaded
        void BannerAdLoadedEvent()
        {
            
        }

        //Invoked when the banner loading process has failed.
        //@param description - string - contains information about the failure.
        void BannerAdLoadFailedEvent (IronSourceError error)
        {
            
        }

        // Invoked when end user clicks on the banner ad
        void BannerAdClickedEvent ()
        {
            
        }

        //Notifies the presentation of a full screen content following user click
        void BannerAdScreenPresentedEvent ()
        {
            
        }

        //Notifies the presented screen has been dismissed
        void BannerAdScreenDismissedEvent()
        {
            
        }

        //Invoked when the user leaves the app
        void BannerAdLeftApplicationEvent()
        {
            
        }

        void ChangeAdsActivity(bool active)
        {
            m_AdsActivity = active;

            if (m_AdsActivity)
            {
                IronSource.Agent.displayBanner();
            }
            else
            {
                IronSource.Agent.hideBanner();
            }
        }

        //Invoked when the initialization process has failed.
        //@param description - string - contains information about the failure.
        void InterstitialAdLoadFailedEvent (IronSourceError error)
        {
            GameState.Instance.IsInterstitialReady = false;
        }

        //Invoked right before the Interstitial screen is about to open.
        void InterstitialAdShowSucceededEvent()
        {
            
        }

        //Invoked when the ad fails to show.
        //@param description - string - contains information about the failure.
        void InterstitialAdShowFailedEvent(IronSourceError error)
        {
            if (OnInterstitialEndedCallback != null)
            {
                OnInterstitialEndedCallback();
            }
        }

        // Invoked when end user clicked on the interstitial ad
        void InterstitialAdClickedEvent ()
        {
            
        }

        // Invoked when the interstitial ad closed and the user goes back to the application screen.
        void InterstitialAdClosedEvent ()
        {
            if (OnInterstitialEndedCallback != null)
            {
                OnInterstitialEndedCallback();
            }
        }

        //Invoked when the Interstitial is Ready to shown after load function is called
        void InterstitialAdReadyEvent()
        {
            GameState.Instance.IsInterstitialReady = true;
        }

        //Invoked when the Interstitial Ad Unit has opened
        void InterstitialAdOpenedEvent()
        {
            
        }

        void ShowInterstitial(System.Action onInterstitialEnded)
        {
            // set callback
            // maybe there should be more callbacks for more cases
            OnInterstitialEndedCallback = onInterstitialEnded;

            if (m_AdsActivity)
            {
                // start video
                if (GameState.Instance.IsInterstitialReady)
                {
                    IronSource.Agent.showInterstitial();
                }
                else
                {
                    if (OnInterstitialEndedCallback != null)
                    {
                        OnInterstitialEndedCallback();
                    }
                }
            }
            else
            {
                if (OnInterstitialEndedCallback != null)
                {
                    OnInterstitialEndedCallback();
                }
            }
        }
    }
}
