using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
    public class IronSourceController : MonoBehaviour
    {
        private System.Action OnRewardVideoEndedCallback;

        public void LateStart() 
        {
            // maybe not needed
            IronSource.Agent.validateIntegration();

            IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
            IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent; 
            IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
            IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
            IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;
            IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent; 
            IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;

            GameEventsController.Instance.OnShowAdvertising += ShowRewardVideo;

            // set state on start
            GameState.Instance.IsRewardedVideoAvailable = IronSource.Agent.isRewardedVideoAvailable();
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
            GameState.Instance.IsRewardedVideoAvailable = available;
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
    }
}
