// Boots the Blazor runtime in the visitor's language.
//
// The culture has to be known before .NET starts: the runtime loads the matching satellite
// assembly and ICU data at boot and cannot switch afterwards, which is why CultureService
// reloads the page when the visitor picks another language. The choice is: the stored
// preference, else the first browser language the site has strings for, else English.
(function () {
    'use strict';

    var supported = ['en', 'pt'];
    var storageKey = 'fili.readytemplate.culture';

    function stored() {
        try { return localStorage.getItem(storageKey); } catch (e) { return null; }
    }

    function fromBrowser() {
        var languages = navigator.languages || [navigator.language || 'en'];
        for (var i = 0; i < languages.length; i++) {
            var neutral = String(languages[i]).toLowerCase().split('-')[0];
            if (supported.indexOf(neutral) >= 0) { return neutral; }
        }
        return null;
    }

    var culture = stored();
    if (supported.indexOf(culture) < 0) { culture = fromBrowser() || 'en'; }

    document.documentElement.lang = culture;

    window.filiReadyTemplate = {
        culture: culture,

        setCulture: function (code) {
            try { localStorage.setItem(storageKey, code); } catch (e) { /* private mode: the reload still applies it for this load */ }
            document.documentElement.lang = code;
        },

        // Blazor renders the fetched page after navigation, so a #fragment in the URL has to be
        // scrolled to once the target element exists.
        scrollToHash: function () {
            var hash = location.hash;
            if (!hash || hash.length < 2) { return; }
            var target = document.getElementById(decodeURIComponent(hash.slice(1)));
            if (target) { target.scrollIntoView(); }
        }
    };

    Blazor.start({ applicationCulture: culture });
})();
