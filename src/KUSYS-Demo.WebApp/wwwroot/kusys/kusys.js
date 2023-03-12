var kusys = kusys || {};
(function () {

    /* Application paths *****************************************/

    //Current application root path (including virtual directory if exists).
    kusys.appPath = kusys.appPath || '/';

    kusys.pageLoadTime = new Date();

    //Converts given path to absolute path using kusys.appPath variable.
    kusys.toAbsAppPath = function (path) {
        if (path.indexOf('/') == 0) {
            path = path.substring(1);
        }

        return kusys.appPath + path;
    };

    /* LOGGING ***************************************************/
    //Implements Logging API that provides secure & controlled usage of console.log

    kusys.log = kusys.log || {};

    kusys.log.levels = {
        DEBUG: 1,
        INFO: 2,
        WARN: 3,
        ERROR: 4,
        FATAL: 5
    };

    kusys.log.level = kusys.log.levels.DEBUG;

    kusys.log.log = function (logObject, logLevel) {
        if (!window.console || !window.console.log) {
            return;
        }

        if (logLevel != undefined && logLevel < kusys.log.level) {
            return;
        }

        console.log(logObject);
    };

    kusys.log.debug = function (logObject) {
        kusys.log.log("DEBUG: ", kusys.log.levels.DEBUG);
        kusys.log.log(logObject, kusys.log.levels.DEBUG);
    };

    kusys.log.info = function (logObject) {
        kusys.log.log("INFO: ", kusys.log.levels.INFO);
        kusys.log.log(logObject, kusys.log.levels.INFO);
    };

    kusys.log.warn = function (logObject) {
        kusys.log.log("WARN: ", kusys.log.levels.WARN);
        kusys.log.log(logObject, kusys.log.levels.WARN);
    };

    kusys.log.error = function (logObject) {
        kusys.log.log("ERROR: ", kusys.log.levels.ERROR);
        kusys.log.log(logObject, kusys.log.levels.ERROR);
    };

    kusys.log.fatal = function (logObject) {
        kusys.log.log("FATAL: ", kusys.log.levels.FATAL);
        kusys.log.log(logObject, kusys.log.levels.FATAL);
    };

    /* LOCALIZATION ***********************************************/

    kusys.localization = kusys.localization || {};

    kusys.localization.values = {};

    kusys.localization.localize = function (key, sourceName) {
        if (sourceName === '_') { //A convention to suppress the localization
            return key;
        }

        sourceName = sourceName || kusys.localization.defaultResourceName;
        if (!sourceName) {
            kusys.log.warn('Localization source name is not specified and the defaultResourceName was not defined!');
            return key;
        }

        var source = kusys.localization.values[sourceName];
        if (!source) {
            kusys.log.warn('Could not find localization source: ' + sourceName);
            return key;
        }

        var value = source[key];
        if (value == undefined) {
            return key;
        }

        var copiedArguments = Array.prototype.slice.call(arguments, 0);
        copiedArguments.splice(1, 1);
        copiedArguments[0] = value;

        return kusys.utils.formatString.apply(this, copiedArguments);
    };

    kusys.localization.isLocalized = function (key, sourceName) {
        if (sourceName === '_') { //A convention to suppress the localization
            return true;
        }

        sourceName = sourceName || kusys.localization.defaultResourceName;
        if (!sourceName) {
            return false;
        }

        var source = kusys.localization.values[sourceName];
        if (!source) {
            return false;
        }

        var value = source[key];
        if (value === undefined) {
            return false;
        }

        return true;
    };

    kusys.localization.getResource = function (name) {
        return function () {
            var copiedArguments = Array.prototype.slice.call(arguments, 0);
            copiedArguments.splice(1, 0, name);
            return kusys.localization.localize.apply(this, copiedArguments);
        };
    };

    kusys.localization.defaultResourceName = undefined;
    kusys.localization.currentCulture = {
        cultureName: undefined
    };

    var getMapValue = function (packageMaps, packageName, language) {
        language = language || kusys.localization.currentCulture.name;
        if (!packageMaps || !packageName || !language) {
            return language;
        }

        var packageMap = packageMaps[packageName];
        if (!packageMap) {
            return language;
        }

        for (var i = 0; i < packageMap.length; i++) {
            var map = packageMap[i];
            if (map.name === language){
                return map.value;
            }
        }

        return language;
    };

    kusys.localization.getLanguagesMap = function (packageName, language) {
        return getMapValue(kusys.localization.languagesMap, packageName, language);
    };

    kusys.localization.getLanguageFilesMap = function (packageName, language) {
        return getMapValue(kusys.localization.languageFilesMap, packageName, language);
    };

    /* AUTHORIZATION **********************************************/


    /* NOTIFICATION *********************************************/
    //Defines Notification API, not implements it

    kusys.notify = kusys.notify || {};

    kusys.notify.success = function (message, title, options) {
        kusys.log.warn('kusys.notify.success is not implemented!');
    };

    kusys.notify.info = function (message, title, options) {
        kusys.log.warn('kusys.notify.info is not implemented!');
    };

    kusys.notify.warn = function (message, title, options) {
        kusys.log.warn('kusys.notify.warn is not implemented!');
    };

    kusys.notify.error = function (message, title, options) {
        kusys.log.warn('kusys.notify.error is not implemented!');
    };

    /* MESSAGE **************************************************/
    //Defines Message API, not implements it

    kusys.message = kusys.message || {};

    kusys.message._showMessage = function (message, title) {
        alert((title || '') + ' ' + message);
    };

    kusys.message.info = function (message, title) {
        kusys.log.warn('kusys.message.info is not implemented!');
        return kusys.message._showMessage(message, title);
    };

    kusys.message.success = function (message, title) {
        kusys.log.warn('kusys.message.success is not implemented!');
        return kusys.message._showMessage(message, title);
    };

    kusys.message.warn = function (message, title) {
        kusys.log.warn('kusys.message.warn is not implemented!');
        return kusys.message._showMessage(message, title);
    };

    kusys.message.error = function (message, title) {
        kusys.log.warn(message + ' ' + title);
        return kusys.message._showMessage(message, title);
    };

    kusys.message.confirm = function (message, titleOrCallback, callback) {
        kusys.log.warn('kusys.message.confirm is not properly implemented!');

        if (titleOrCallback && !(typeof titleOrCallback == 'string')) {
            callback = titleOrCallback;
        }

        var result = confirm(message);
        callback && callback(result);
    };

    /* UI *******************************************************/

    kusys.ui = kusys.ui || {};

    /* UI BLOCK */
    //Defines UI Block API and implements basically

    var $kusysBlockArea = document.createElement('div');
    $kusysBlockArea.classList.add('kusys-block-area');

    /* opts: { //Can be an object with options or a string for query a selector
     *  elm: a query selector (optional - default: document.body)
     *  busy: boolean (optional - default: false)
     *  promise: A promise with always or finally handler (optional - auto unblocks the ui if provided)
     * }
     */
    kusys.ui.block = function (opts) {
        if (!opts) {
            opts = {};
        } else if (typeof opts == 'string') {
            opts = {
                elm: opts
            };
        }

        var $elm = document.querySelector(opts.elm) || document.body;

        if (opts.busy) {
            $kusysBlockArea.classList.add('kusys-block-area-busy');
        } else {
            $kusysBlockArea.classList.remove('kusys-block-area-busy');
        }

        if (document.querySelector(opts.elm)) {
            $kusysBlockArea.style.position = 'absolute';
        } else {
            $kusysBlockArea.style.position = 'fixed';
        }

        $elm.appendChild($kusysBlockArea);

        if (opts.promise) {
            if (opts.promise.always) { //jQuery.Deferred style
                opts.promise.always(function () {
                    kusys.ui.unblock({
                        $elm: opts.elm
                    });
                });
            } else if (opts.promise['finally']) { //Q style
                opts.promise['finally'](function () {
                    kusys.ui.unblock({
                        $elm: opts.elm
                    });
                });
            }
        }
    };

    /* opts: {
     *
     * }
     */
    kusys.ui.unblock = function (opts) {
        var element = document.querySelector('.kusys-block-area');
        if (element) {
            element.classList.add('kusys-block-area-disappearing');
            setTimeout(function () {
                if (element) {
                    element.classList.remove('kusys-block-area-disappearing');
                    element.parentElement.removeChild(element);
                }
            }, 250);
        }
    };

    /* UI BUSY */
    //Defines UI Busy API, not implements it

    kusys.ui.setBusy = function (opts) {
        if (!opts) {
            opts = {
                busy: true
            };
        } else if (typeof opts == 'string') {
            opts = {
                elm: opts,
                busy: true
            };
        }

        kusys.ui.block(opts);
    };

    kusys.ui.clearBusy = function (opts) {
        kusys.ui.unblock(opts);
    };

    /* SIMPLE EVENT BUS *****************************************/

    kusys.event = (function () {

        var _callbacks = {};

        var on = function (eventName, callback) {
            if (!_callbacks[eventName]) {
                _callbacks[eventName] = [];
            }

            _callbacks[eventName].push(callback);
        };

        var off = function (eventName, callback) {
            var callbacks = _callbacks[eventName];
            if (!callbacks) {
                return;
            }

            var index = -1;
            for (var i = 0; i < callbacks.length; i++) {
                if (callbacks[i] === callback) {
                    index = i;
                    break;
                }
            }

            if (index < 0) {
                return;
            }

            _callbacks[eventName].splice(index, 1);
        };

        var trigger = function (eventName) {
            var callbacks = _callbacks[eventName];
            if (!callbacks || !callbacks.length) {
                return;
            }

            var args = Array.prototype.slice.call(arguments, 1);
            for (var i = 0; i < callbacks.length; i++) {
                callbacks[i].apply(this, args);
            }
        };

        // Public interface ///////////////////////////////////////////////////

        return {
            on: on,
            off: off,
            trigger: trigger
        };
    })();


    /* UTILS ***************************************************/

    kusys.utils = kusys.utils || {};

    /* Creates a name namespace.
    *  Example:
    *  var taskService = kusys.utils.createNamespace(kusys, 'services.task');
    *  taskService will be equal to kusys.services.task
    *  first argument (root) must be defined first
    ************************************************************/
    kusys.utils.createNamespace = function (root, ns) {
        var parts = ns.split('.');
        for (var i = 0; i < parts.length; i++) {
            if (typeof root[parts[i]] == 'undefined') {
                root[parts[i]] = {};
            }

            root = root[parts[i]];
        }

        return root;
    };

    /* Find and replaces a string (search) to another string (replacement) in
    *  given string (str).
    *  Example:
    *  kusys.utils.replaceAll('This is a test string', 'is', 'X') = 'ThX X a test string'
    ************************************************************/
    kusys.utils.replaceAll = function (str, search, replacement) {
        var fix = search.replace(/[.*+?^${}()|[\]\\]/g, "\\$&");
        return str.replace(new RegExp(fix, 'g'), replacement);
    };

    /* Formats a string just like string.format in C#.
    *  Example:
    *  kusys.utils.formatString('Hello {0}','Tuana') = 'Hello Tuana'
    ************************************************************/
    kusys.utils.formatString = function () {
        if (arguments.length < 1) {
            return null;
        }

        var str = arguments[0];

        for (var i = 1; i < arguments.length; i++) {
            var placeHolder = '{' + (i - 1) + '}';
            str = kusys.utils.replaceAll(str, placeHolder, arguments[i]);
        }

        return str;
    };

    kusys.utils.toPascalCase = function (str) {
        if (!str || !str.length) {
            return str;
        }

        if (str.length === 1) {
            return str.charAt(0).toUpperCase();
        }

        return str.charAt(0).toUpperCase() + str.substr(1);
    }

    kusys.utils.toCamelCase = function (str) {
        if (!str || !str.length) {
            return str;
        }

        if (str.length === 1) {
            return str.charAt(0).toLowerCase();
        }

        return str.charAt(0).toLowerCase() + str.substr(1);
    }

    kusys.utils.truncateString = function (str, maxLength) {
        if (!str || !str.length || str.length <= maxLength) {
            return str;
        }

        return str.substr(0, maxLength);
    };

    kusys.utils.truncateStringWithPostfix = function (str, maxLength, postfix) {
        postfix = postfix || '...';

        if (!str || !str.length || str.length <= maxLength) {
            return str;
        }

        if (maxLength <= postfix.length) {
            return postfix.substr(0, maxLength);
        }

        return str.substr(0, maxLength - postfix.length) + postfix;
    };

    kusys.utils.isFunction = function (obj) {
        return !!(obj && obj.constructor && obj.call && obj.apply);
    };

    /**
     * parameterInfos should be an array of { name, value } objects
     * where name is query string parameter name and value is it's value.
     * includeQuestionMark is true by default.
     */
    kusys.utils.buildQueryString = function (parameterInfos, includeQuestionMark) {
        if (includeQuestionMark === undefined) {
            includeQuestionMark = true;
        }

        var qs = '';

        function addSeperator() {
            if (!qs.length) {
                if (includeQuestionMark) {
                    qs = qs + '?';
                }
            } else {
                qs = qs + '&';
            }
        }

        for (var i = 0; i < parameterInfos.length; ++i) {
            var parameterInfo = parameterInfos[i];
            if (parameterInfo.value === undefined) {
                continue;
            }

            if (parameterInfo.value === null) {
                parameterInfo.value = '';
            }

            addSeperator();

            if (parameterInfo.value.toJSON && typeof parameterInfo.value.toJSON === "function") {
                qs = qs + parameterInfo.name + '=' + encodeURIComponent(parameterInfo.value.toJSON());
            } else if (Array.isArray(parameterInfo.value) && parameterInfo.value.length) {
                for (var j = 0; j < parameterInfo.value.length; j++) {
                    if (j > 0) {
                        addSeperator();
                    }

                    qs = qs + parameterInfo.name + '[' + j + ']=' + encodeURIComponent(parameterInfo.value[j]);
                }
            } else {
                qs = qs + parameterInfo.name + '=' + encodeURIComponent(parameterInfo.value);
            }
        }

        return qs;
    }

    /**
     * Sets a cookie value for given key.
     * This is a simple implementation created to be used by kusys.
     * Please use a complete cookie library if you need.
     * @param {string} key
     * @param {string} value
     * @param {Date} expireDate (optional). If not specified the cookie will expire at the end of session.
     * @param {string} path (optional)
     */
    kusys.utils.setCookieValue = function (key, value, expireDate, path) {
        var cookieValue = encodeURIComponent(key) + '=';

        if (value) {
            cookieValue = cookieValue + encodeURIComponent(value);
        }

        if (expireDate) {
            cookieValue = cookieValue + "; expires=" + expireDate.toUTCString();
        }

        if (path) {
            cookieValue = cookieValue + "; path=" + path;
        }

        document.cookie = cookieValue;
    };

    /**
     * Gets a cookie with given key.
     * This is a simple implementation created to be used by kusys.
     * Please use a complete cookie library if you need.
     * @param {string} key
     * @returns {string} Cookie value or null
     */
    kusys.utils.getCookieValue = function (key) {
        var equalities = document.cookie.split('; ');
        for (var i = 0; i < equalities.length; i++) {
            if (!equalities[i]) {
                continue;
            }

            var splitted = equalities[i].split('=');
            if (splitted.length != 2) {
                continue;
            }

            if (decodeURIComponent(splitted[0]) === key) {
                return decodeURIComponent(splitted[1] || '');
            }
        }

        return null;
    };

    /**
     * Deletes cookie for given key.
     * This is a simple implementation created to be used by kusys.
     * Please use a complete cookie library if you need.
     * @param {string} key
     * @param {string} path (optional)
     */
    kusys.utils.deleteCookie = function (key, path) {
        var cookieValue = encodeURIComponent(key) + '=';

        cookieValue = cookieValue + "; expires=" + (new Date(new Date().getTime() - 86400000)).toUTCString();

        if (path) {
            cookieValue = cookieValue + "; path=" + path;
        }

        document.cookie = cookieValue;
    }

    /**
     * Escape HTML to help prevent XSS attacks. 
     */
    kusys.utils.htmlEscape = function (html) {
        return typeof html === 'string' ? html.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;') : html;
    }

    /* SECURITY ***************************************/
    //kusys.security = kusys.security || {};
    //kusys.security.antiForgery = kusys.security.antiForgery || {};

    //kusys.security.antiForgery.tokenCookieName = 'XSRF-TOKEN';
    //kusys.security.antiForgery.tokenHeaderName = 'RequestVerificationToken';

    //kusys.security.antiForgery.getToken = function () {
    //    return kusys.utils.getCookieValue(kusys.security.antiForgery.tokenCookieName);
    //};

    /* CLOCK *****************************************/
    kusys.clock = kusys.clock || {};

    kusys.clock.kind = 'Unspecified';

    kusys.clock.supportsMultipleTimezone = function () {
        return kusys.clock.kind === 'Utc';
    };

    var toLocal = function (date) {
        return new Date(
            date.getFullYear(),
            date.getMonth(),
            date.getDate(),
            date.getHours(),
            date.getMinutes(),
            date.getSeconds(),
            date.getMilliseconds()
        );
    };

    var toUtc = function (date) {
        return Date.UTC(
            date.getUTCFullYear(),
            date.getUTCMonth(),
            date.getUTCDate(),
            date.getUTCHours(),
            date.getUTCMinutes(),
            date.getUTCSeconds(),
            date.getUTCMilliseconds()
        );
    };

    kusys.clock.now = function () {
        if (kusys.clock.kind === 'Utc') {
            return toUtc(new Date());
        }
        return new Date();
    };

    kusys.clock.normalize = function (date) {
        var kind = kusys.clock.kind;

        if (kind === 'Unspecified') {
            return date;
        }

        if (kind === 'Local') {
            return toLocal(date);
        }

        if (kind === 'Utc') {
            return toUtc(date);
        }
    };
    
    /* FEATURES *************************************************/

    kusys.features = kusys.features || {};

    kusys.features.values = kusys.features.values || {};

    kusys.features.isEnabled = function(name){
        var value = kusys.features.get(name);
        return value == 'true' || value == 'True';
    }

    kusys.features.get = function (name) {
        return kusys.features.values[name];
    };
    
})();
