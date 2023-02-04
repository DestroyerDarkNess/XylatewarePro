Namespace Core
    Public Class JavaScripts

        Public Shared AdsBlock As String = <a><![CDATA[
const clear = (() => {
    const defined = v => v !== null && v !== undefined;
    const timeout = setInterval(() => {
        const ad = [...document.querySelectorAll('.ad-showing')][0];
        if (defined(ad)) {
            const video = document.querySelector('video');
            if (defined(video)) {
                video.currentTime = video.duration;
            }
        }
    }, 500);
    return function() {
        clearTimeout(timeout);
    }
})();
 clear();
]]></a>.Value

        Public Shared AdsBlock2 As String = <a><![CDATA[
document.cookie="VISITOR_INFO1_LIVE=oKckVSqvaGw; path=/; domain=.youtube.com";
window.location.reload();
]]></a>.Value

        Public Shared Unmute As String = <a><![CDATA[
document.querySelector('.ytp-unmute-inner').click();
]]></a>.Value

        Public Shared AdsBlock3 As String = <a><![CDATA[
// Wrapper method to execute when debug mode is on
blockAdWithDebugging = () => {
  console.log("AdFreeYouTube checking for advertisements on the page.");
  ads = findAdDiv();
  videoSkipAdButton1 = document.querySelector("div.videoAdUiSkipContainer");
  videoSkipAdButton2 = document.querySelector(
    "span.ytp-ad-skip-button-container"
  );
  if (!ads.length && !videoSkipAdButton1 && !videoSkipAdButton2) {
    return;
  }

  console.log(
    "AdFreeYouTube found one or more advertisements on the page, highlighting them and removing from page after 2 seconds. To hide the ad immediately, disable verbose mode in addon options."
  );
  ads.forEach((ad) => {
    console.log(ad);
    ad.style.border = "2px dotted red";
  });
  if (videoSkipAdButton1) {
    console.log(videoSkipAdButton1.children[0]);
    videoSkipAdButton1.children[0].style.border = "2px solid red";
  }
  if (videoSkipAdButton2) {
    console.log(videoSkipAdButton2.children[0]);
    videoSkipAdButton2.children[0].style.border = "2px solid red";
  }

  setTimeout(() => {
    blockAd();
  }, 5000);
};

// Main method to remove ads
blockAd = () => {
  ads = findAdDiv();
  videoSkipAdButton1 = document.querySelector("div.videoAdUiSkipContainer");
  videoSkipAdButton2 = document.querySelector(
    "span.ytp-ad-skip-button-container"
  );
  if (!ads.length && !videoSkipAdButton1 && !videoSkipAdButton2) {
    return;
  }

  ads.forEach((ad) => {
    ad.style.display = "none";
  });
  if (videoSkipAdButton1) {
    videoSkipAdButton1.children[0].click();
  }
  if (videoSkipAdButton2) {
    videoSkipAdButton2.children[0].click();
  }
};

// Method to find all ads on page
findAdDiv = () => {
  adDivArray = [];
  videoFrameFooterAd = document.querySelector(".ad-container");
  sidebarVideoAd = document.querySelector("ytd-iframe-companion-renderer");
  siderbarAdBanner = document.querySelector("ytd-companion-slot-renderer");
  homepageAd = document.querySelector("#ad-iframe");

  if (videoFrameFooterAd && videoFrameFooterAd.style.display !== "none") {
    adDivArray.push(videoFrameFooterAd);
  }
  if (sidebarVideoAd && sidebarVideoAd.style.display !== "none") {
    adDivArray.push(sidebarVideoAd);
  }
  if (siderbarAdBanner && siderbarAdBanner.style.display !== "none") {
    adDivArray.push(siderbarAdBanner);
  }
  if (homepageAd && homepageAd.style.display !== "none") {
    adDivArray.push(homepageAd);
  }
  return adDivArray;
};

// Initialization
let storageItem = browser.storage.local.get();
storageItem.then((result) => {
  // Setting ANNOTATIONS off
  let disabledAnnotationsFrom = null;
  document
    .querySelectorAll(
      "div.ytp-panel-menu div.ytp-menuitem[role=menuitemcheckbox]"
    )
    .forEach((d) => {
      if (
        d.innerText == "Annotations" &&
        d.getAttribute("aria-checked") == "true"
      ) {
        disabledAnnotationsFrom = d;
        d.click();
      }
    });
  // Blocking ADS
  if (result.isDebugModeOn) {
    console.log(
      "Hi from AdFreeYouTube, we are here to give you ad free experience on YouTube."
    );
    if (disabledAnnotationsFrom) {
      console.log("Disabled annotations");
      console.log(disabledAnnotationsFrom);
    }
    blockAdWithDebugging();
    setInterval(blockAdWithDebugging, 5000);
  } else {
    blockAd();
    setInterval(blockAd, 5000);
  }
});
]]></a>.Value

        Public Shared NoGoogleAds As String = <a><![CDATA[
(function removeAdvertisementAndBlockingElements () {
    $('.inRek').remove();
    $('.mgbox').remove();
    
    Array.from(document.getElementsByTagName("img")).forEach(function (e) {
        if (!e.src.includes(window.location.host)) {
            e.remove()
        }
    });    
    
    Array.from(document.getElementsByTagName("div")).forEach(function (e) {
        var currentZIndex = parseInt(document.defaultView.getComputedStyle(e, null).zIndex);
        if (currentZIndex > 999) {
            console.log(parseInt(currentZIndex));
            e.remove()
        }
    });
})();
]]></a>.Value

        Public Shared SkipYoutubeAds As String = <a><![CDATA[
setInterval(() => {
  for (const button of document.getElementsByClassName("ytp-ad-skip-button")) {
    button.click(); // "Skip Ad" or "Skip Ads" buttons
  }
}, 300);
]]></a>.Value

        Public Shared SkipYoutubeAds2 As String = <a><![CDATA[
const target = document.getElementById('movie_player');
const config = { childList: true, subtree: true };

const callback = function(mutationsList, observer) {
  for(const mutation of mutationsList) {
    if (mutation.addedNodes.length === 0) {
      return;
    }

    if (mutation.target.className !== "video-ads ytp-ad-module") {
      return;
    }

    // Look for Skip Button here :)
  }
};


const observer = new MutationObserver(callback);
observer.observe(target, config);
]]></a>.Value

        Public Shared BypassYoutubeAds As String = <a><![CDATA[
//Every 500ms, check whether the browser's URL has changed or not
let urlChangeHandler = window.setInterval(checkUrlChange, 500)
let oldUrl= ""

function checkUrlChange(){
    //Continuously grab the window's URL
    let newUrl = document.URL
    if(newUrl !== oldUrl){
        // If it's different from the previous URL, check for an ad on the page
        checkForAd()
        .then(function (check) {
            if (check) {
                // If there was an ad playing on the page, skip it..
                skipAd()
                // Then replace the oldUrl with the new one and continu
                oldUrl = newUrl
            }
        })
        .catch(function (err) {
            console.error(err)
        })
    }
}

function checkForAd () {
    return new Promise(function (resolve, reject) {
        let adCheck = document.querySelector('.videoAdUi')
        resolve(adCheck)
    })
}

// Shoutout to Wes Bos for this slick line of code...
function skipAd () {
    document.querySelector('video').currentTime = document.querySelector('video').duration
}
]]></a>.Value

        Public Shared PlayVideoHideNotifycation As String = <a><![CDATA[
 window.gwd = window.gwd || {};
    gwd.pauseVideo = function(event) {
      var idBox = document.getElementById("box1");
      idBox.style.opacity = 0;
    };
  </script>
  <script type="text/javascript" gwd-events="registration">
    // Support code for event handling in Google Web Designer
     // This script block is auto-generated. Please do not edit!
    gwd.actions.events.registerEventHandlers = function(event) {
      gwd.actions.events.addHandler('gwd-youtube_1', 'playing', gwd.pauseVideo, false);
    };
    gwd.actions.events.deregisterEventHandlers = function(event) {
      gwd.actions.events.removeHandler('gwd-youtube_1', 'playing', gwd.pauseVideo, false);
    };
    document.addEventListener("DOMContentLoaded", gwd.actions.events.registerEventHandlers);
    document.addEventListener("unload", gwd.actions.events.deregisterEventHandlers);
]]></a>.Value


        Public Shared PauseVideo As String = <a><![CDATA[
 $('.youtube-video')[0].contentWindow.postMessage('{"event":"command","func":"' + 'pauseVideo' + '","args":""}', '*');
]]></a>.Value

        Public Shared PauseVideo2 As String = <a><![CDATA[
var videos = document.querySelectorAll('iframe, video');
	Array.prototype.forEach.call(videos, function (video) {
		if (video.tagName.toLowerCase() === 'video') {
			video.pause();
		} else {
			var src = video.src;
			video.src = src;
		}
	});
]]></a>.Value

        Public Shared AcceptCookies As String = <a><![CDATA[
document.querySelector('.VfPpkd-RLmnJb').click();
]]></a>.Value

        'consent.youtube.com
        Public Shared UBlock As String = <a><![CDATA[
setCookie("OTZ", "disabled", -1);
function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays*24*60*60*1000));
    var expires = "expires="+ d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}
]]></a>.Value

        Public Shared AgeRestrictionBypass As String = <a><![CDATA[



//document.getElementById("player-container-outer").innerHTML='<iframe width="560" height="315" src="https://www.youtube.com/embed/7CaWcqiauGc' + '" frameborder="0" allow="accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>';

//document.getElementById("player-container-outer").innerHTML='<iframe width="560" height="315" src="https://www.youtube.com/embed/7CaWcqiauGc' + '" frameborder="0" allow="accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>';
//document.body.style.backgroundColor = "red";

]]></a>.Value

        Public Shared TitleExtractor As String = <a><![CDATA[
let VidTitle = document.querySelector('.yt-formatted-string')[0].innerText;
eoapi.extInvoke("TitleSender", [VidTitle]);
]]></a>.Value



    End Class

    Public Class DragonTubeWeb

        Private Shared JSLoader As String = <a><![CDATA[
let html = `

<DragonTube>
 
<style>
 
body {
  background: #0f1923;
  font-family: "Muli";
}
 
.dragon_tube {
  display: flex;
  align-items: normal;
  justify-content: normal;
  flex-flow: inline;
}
 
.dragon_tube a {
  width: 100%;
  max-width: 240px;
  height: 54px;
  padding: 8px;
  font-size: 0.8rem;
  font-weight: 900;
  color: #ff4655;
  text-align: center;
  text-transform: uppercase;
  text-decoration: none;
  box-shadow: 0 0 0 1px inset rgba(236, 232, 225, 0.3);
  position: relative;
  margin: 10px 0;
}
.dragon_tube a.white:hover > p {
  color: #ece8e1;
}
.dragon_tube a.white > p {
  background: #ece8e1;
  color: #0f1923;
}
.dragon_tube a.white > p span.base {
  border: 1px solid transparent;
}
.dragon_tube a.transparent:hover > p {
  color: #ece8e1;
}
.dragon_tube a.transparent:hover > p span.text {
  box-shadow: 0 0 0 1px #ece8e1;
}
.dragon_tube a.transparent > p {
  background: #0f1923;
  color: #ece8e1;
}
.dragon_tube a.transparent > p span.base {
  border: 1px solid #ece8e1;
}
.dragon_tube a:after, .dragon_tube a:before {
  content: "";
  width: 1px;
  position: absolute;
  height: 8px;
  background: #0f1923;
  left: 0;
  top: 50%;
  transform: translateY(-50%);
}
.dragon_tube a:before {
  right: 0;
  left: initial;
}
.dragon_tube a p {
  margin: 0;
  height: 54px;
  line-height: 54px;
  box-sizing: border-box;
  z-index: 1;
  left: 0;
  width: 100%;
  position: relative;
  overflow: hidden;
}
.dragon_tube a p span.base {
  box-sizing: border-box;
  position: absolute;
  z-index: 2;
  width: 100%;
  height: 100%;
  left: 0;
  border: 1px solid #ff4655;
}
.dragon_tube a p span.base:before {
  content: "";
  width: 2px;
  height: 2px;
  left: -1px;
  top: -1px;
  background: #0f1923;
  position: absolute;
  transition: 0.3s ease-out all;
}
.dragon_tube a p span.bg {
  left: -5%;
  position: absolute;
  background: #ff4655;
  width: 0;
  height: 100%;
  z-index: 3;
  transition: 0.3s ease-out all;
  transform: skewX(-10deg);
}
.dragon_tube a p span.text {
  z-index: 4;
  width: 100%;
  height: 100%;
  position: absolute;
  left: 0;
  top: 0;
}
.dragon_tube a p span.text:after {
  content: "";
  width: 4px;
  height: 4px;
  right: 0;
  bottom: 0;
  background: #0f1923;
  position: absolute;
  transition: 0.3s ease-out all;
  z-index: 5;
}
.dragon_tube a:hover {
  color: #ece8e1;
}
.dragon_tube a:hover span.bg {
  width: 110%;
}
.dragon_tube a:hover span.text:after {
  background: #ece8e1;
}
 
</style>
 
<div class="panel panel-default dragon_tube">
 
<a onclick="DragonTube_Download()"><p><span class="bg"></span><span class="base"></span><span class="text">Download</span></p></a>
<a class="white" onclick="DragonTube_VancedPlayer()"><p><span class="bg"></span><span class="base"></span><span class="text">Vanced Player</span></p></a>
<a class="white" onclick="DragonTube_PlayerMini()"><p><span class="bg"></span><span class="base"></span><span class="text">Player Mini</span></p></a>
<a class="transparent" onclick="DragonTube_GIFMaker()"><p><span class="bg"></span><span class="base"></span><span class="text">Open GIF Maker</span></p></a>
 
</div>
 
</DragonTube>

`;

function DragonTube_Download() {
        eoapi.extInvoke("Download", [document.URL]);
}

function DragonTube_VancedPlayer() {
        eoapi.extInvoke("MediaPlayer", [document.URL]);
}

function DragonTube_PlayerMini() {
        eoapi.extInvoke("PlayerMini", [document.URL]);
}

function DragonTube_GIFMaker() {
        eoapi.extInvoke("GIFMaker", [document.URL]);
}

let tmp = document.createElement('div');
tmp.innerHTML = html;
document.getElementById('info').append(tmp)

]]></a>.Value

        Public Shared Function GetJSFull() As String

            Return JSLoader

        End Function

    End Class


End Namespace

