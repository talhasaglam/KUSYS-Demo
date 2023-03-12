var kusys = kusys || {};
(function($) {

    if (!$) {
        throw "kusys/jquery library requires the jquery library included to the page!";
    }

    // kusys CORE OVERRIDES /////////////////////////////////////////////////////

    kusys.message._showMessage = function (message, title) {
        alert((title || '') + ' ' + message);

        return $.Deferred(function ($dfd) {
            $dfd.resolve();
        });
    };

    kusys.message.confirm = function (message, titleOrCallback, callback) {
        if (titleOrCallback && !(typeof titleOrCallback == 'string')) {
            callback = titleOrCallback;
        }

        var result = confirm(message);
        callback && callback(result);

        return $.Deferred(function ($dfd) {
            $dfd.resolve(result);
        });
    };

    kusys.utils.isFunction = function (obj) {
        return $.isFunction(obj);
    };

    // JQUERY EXTENSIONS //////////////////////////////////////////////////////

    $.fn.findWithSelf = function (selector) {
        return this.filter(selector).add(this.find(selector));
    };

    // DOM ////////////////////////////////////////////////////////////////////

    kusys.dom = kusys.dom || {};

    kusys.dom.onNodeAdded = function (callback) {
        kusys.event.on('kusys.dom.nodeAdded', callback);
    };

    kusys.dom.onNodeRemoved = function (callback) {
        kusys.event.on('kusys.dom.nodeRemoved', callback);
    };

    var mutationObserverCallback = function (mutationsList) {
        for (var i = 0; i < mutationsList.length; i++) {
            var mutation = mutationsList[i];
            if (mutation.type === 'childList') {
                if (mutation.addedNodes && mutation.removedNodes.length) {
                    for (var k = 0; k < mutation.removedNodes.length; k++) {
                        kusys.event.trigger(
                            'kusys.dom.nodeRemoved',
                            {
                                $el: $(mutation.removedNodes[k])
                            }
                        );
                    }
                }

                if (mutation.addedNodes && mutation.addedNodes.length) {
                    for (var j = 0; j < mutation.addedNodes.length; j++) {
                        kusys.event.trigger(
                            'kusys.dom.nodeAdded',
                            {
                                $el: $(mutation.addedNodes[j])
                            }
                        );
                    }
                }
            }
        }
    };

    $(function(){
        new MutationObserver(mutationObserverCallback).observe(
            $('body')[0],
            {
                subtree: true,
                childList: true
            }
        );
    });    

    // AJAX ///////////////////////////////////////////////////////////////////

    kusys.ajax = function (userOptions) {
        userOptions = userOptions || {};

        var options = $.extend(true, {}, kusys.ajax.defaultOpts, userOptions);

        options.success = undefined;
        options.error = undefined;

        var xhr = null;
        var promise = $.Deferred(function ($dfd) {
            xhr = $.ajax(options)
                .done(function (data, textStatus, jqXHR) {
                    $dfd.resolve(data);
                    userOptions.success && userOptions.success(data);
                }).fail(function (jqXHR) {
                    if(jqXHR.statusText === 'abort') {
                        //ajax request is abort, ignore error handle.
                        return;
                    }
                    if (jqXHR.getResponseHeader('_kusysErrorFormat') === 'true') {
                        kusys.ajax.handlekusysErrorResponse(jqXHR, userOptions, $dfd);
                    } else {
                        kusys.ajax.handleNonkusysErrorResponse(jqXHR, userOptions, $dfd);
                    }
                });
        }).promise();

        promise['jqXHR'] = xhr;

        return promise;
    };

    $.extend(kusys.ajax, {
        defaultOpts: {
            dataType: 'json',
            type: 'POST',
            contentType: 'application/json',
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        },

        defaultError: {
            message: 'An error has occurred!',
            details: 'Error detail not sent by server.'
        },

        defaultError401: {
            message: 'You are not authenticated!',
            details: 'You should be authenticated (sign in) in order to perform this operation.'
        },

        defaultError403: {
            message: 'You are not authorized!',
            details: 'You are not allowed to perform this operation.'
        },

        defaultError404: {
            message: 'Resource not found!',
            details: 'The resource requested could not found on the server.'
        },

        logError: function (error) {
            kusys.log.error(error);
        },

        showError: function (error) {
            if (error.details) {
                return kusys.message.error(error.details, error.message);
            } else {
                return kusys.message.error(error.message || kusys.ajax.defaultError.message);
            }
        },

        handleTargetUrl: function (targetUrl) {
            if (!targetUrl) {
                location.href = kusys.appPath;
            } else {
                location.href = targetUrl;
            }
        },

        handleErrorStatusCode: function (status) {
            switch (status) {
                case 401:
                    kusys.ajax.handleUnAuthorizedRequest(
                        kusys.ajax.showError(kusys.ajax.defaultError401),
                        kusys.appPath
                    );
                    break;
                case 403:
                    kusys.ajax.showError(kusys.ajax.defaultError403);
                    break;
                case 404:
                    kusys.ajax.showError(kusys.ajax.defaultError404);
                    break;
                default:
                    kusys.ajax.showError(kusys.ajax.defaultError);
                    break;
            }
        },

        handleNonkusysErrorResponse: function (jqXHR, userOptions, $dfd) {
            if (userOptions.kusysHandleError !== false) {
                kusys.ajax.handleErrorStatusCode(jqXHR.status);
            }

            $dfd.reject.apply(this, arguments);
            userOptions.error && userOptions.error.apply(this, arguments);
        },

        handlekusysErrorResponse: function (jqXHR, userOptions, $dfd) {
            var messagePromise = null;

            var responseJSON = jqXHR.responseJSON ? jqXHR.responseJSON : JSON.parse(jqXHR.responseText);

            if (userOptions.kusysHandleError !== false) {
                messagePromise = kusys.ajax.showError(responseJSON.error);
            }

            kusys.ajax.logError(responseJSON.error);

            $dfd && $dfd.reject(responseJSON.error, jqXHR);
            userOptions.error && userOptions.error(responseJSON.error, jqXHR);

            if (jqXHR.status === 401 && userOptions.kusysHandleError !== false) {
                kusys.ajax.handleUnAuthorizedRequest(messagePromise);
            }
        },

        handleUnAuthorizedRequest: function (messagePromise, targetUrl) {
            if (messagePromise) {
                messagePromise.done(function () {
                    kusys.ajax.handleTargetUrl(targetUrl);
                });
            } else {
                kusys.ajax.handleTargetUrl(targetUrl);
            }
        },

        blockUI: function (options) {
            if (options.blockUI) {
                if (options.blockUI === true) { //block whole page
                    kusys.ui.setBusy();
                } else { //block an element
                    kusys.ui.setBusy(options.blockUI);
                }
            }
        },

        unblockUI: function (options) {
            if (options.blockUI) {
                if (options.blockUI === true) { //unblock whole page
                    kusys.ui.clearBusy();
                } else { //unblock an element
                    kusys.ui.clearBusy(options.blockUI);
                }
            }
        },

        //ajaxSendHandler: function (event, request, settings) {
        //    var token = kusys.security.antiForgery.getToken();
        //    if (!token) {
        //        return;
        //    }

        //    if (!settings.headers || settings.headers[kusys.security.antiForgery.tokenHeaderName] === undefined) {
        //        request.setRequestHeader(kusys.security.antiForgery.tokenHeaderName, token);
        //    }
        //}
    });

    //$(document).ajaxSend(function (event, request, settings) {
    //    return kusys.ajax.ajaxSendHandler(event, request, settings);
    //});

    kusys.event.on('kusys.configurationInitialized', function () {
        var l = kusys.localization.getResource('kusysUi');

        kusys.ajax.defaultError.message = l('DefaultErrorMessage');
        kusys.ajax.defaultError.details = l('DefaultErrorMessageDetail');
        kusys.ajax.defaultError401.message = l('DefaultErrorMessage401');
        kusys.ajax.defaultError401.details = l('DefaultErrorMessage401Detail');
        kusys.ajax.defaultError403.message = l('DefaultErrorMessage403');
        kusys.ajax.defaultError403.details = l('DefaultErrorMessage403Detail');
        kusys.ajax.defaultError404.message = l('DefaultErrorMessage404');
        kusys.ajax.defaultError404.details = l('DefaultErrorMessage404Detail');
    });

    // RESOURCE LOADER ////////////////////////////////////////////////////////

    /* UrlStates enum */
    var UrlStates = {
        LOADING: 'LOADING',
        LOADED: 'LOADED',
        FAILED: 'FAILED'
    };

    /* UrlInfo class */
    function UrlInfo(url) {
        this.url = url;
        this.state = UrlStates.LOADING;
        this.loadCallbacks = [];
        this.failCallbacks = [];
    }

    UrlInfo.prototype.succeed = function () {
        this.state = UrlStates.LOADED;
        for (var i = 0; i < this.loadCallbacks.length; i++) {
            this.loadCallbacks[i]();
        }
    };

    UrlInfo.prototype.failed = function () {
        this.state = UrlStates.FAILED;
        for (var i = 0; i < this.failCallbacks.length; i++) {
            this.failCallbacks[i]();
        }
    };

    UrlInfo.prototype.handleCallbacks = function (loadCallback, failCallback) {
        switch (this.state) {
            case UrlStates.LOADED:
                loadCallback && loadCallback();
                break;
            case UrlStates.FAILED:
                failCallback && failCallback();
                break;
            case UrlStates.LOADING:
                this.addCallbacks(loadCallback, failCallback);
                break;
        }
    };

    UrlInfo.prototype.addCallbacks = function (loadCallback, failCallback) {
        loadCallback && this.loadCallbacks.push(loadCallback);
        failCallback && this.failCallbacks.push(failCallback);
    };

    /* ResourceLoader API */

    kusys.ResourceLoader = (function () {

        var _urlInfos = {};

        function getCacheKey(url) {
            return url;
        }

        function appendTimeToUrl(url) {

            if (url.indexOf('?') < 0) {
                url += '?';
            } else {
                url += '&';
            }

            url += '_=' + new Date().getTime();

            return url;
        }

        var _loadFromUrl = function (url, loadCallback, failCallback, serverLoader) {

            var cacheKey = getCacheKey(url);

            var urlInfo = _urlInfos[cacheKey];

            if (urlInfo) {
                urlInfo.handleCallbacks(loadCallback, failCallback);
                return;
            }

            _urlInfos[cacheKey] = urlInfo = new UrlInfo(url);
            urlInfo.addCallbacks(loadCallback, failCallback);

            serverLoader(urlInfo);
        };

        var _loadScript = function (url, loadCallback, failCallback) {
            _loadFromUrl(url, loadCallback, failCallback, function (urlInfo) {
                $.get({
                    url: url,
                    dataType: 'text'
                })
                .done(function (script) {
                    $.globalEval(script);
                    urlInfo.succeed();
                })
                .fail(function () {
                    urlInfo.failed();
                });
            });
        };

        var _loadStyle = function (url) {
            _loadFromUrl(url, undefined, undefined, function (urlInfo) {

                $('<link/>', {
                    rel: 'stylesheet',
                    type: 'text/css',
                    href: appendTimeToUrl(url)
                }).appendTo('head');
            });
        };

        return {
            loadScript: _loadScript,
            loadStyle: _loadStyle
        }
    })();

})(jQuery);