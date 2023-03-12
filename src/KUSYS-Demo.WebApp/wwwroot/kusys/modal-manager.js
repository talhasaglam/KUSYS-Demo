var kusys = kusys || {};

(function ($) {

    kusys.modals = kusys.modals || {};

    kusys.ModalManager = (function () {

        var CallbackList = function () {
            var _callbacks = [];

            return {
                add: function (callback) {
                    _callbacks.push(callback);
                },

                triggerAll: function (thisObj, argumentList) {
                    for (var i = 0; i < _callbacks.length; i++) {
                        _callbacks[i].apply(thisObj, argumentList);
                    }
                }

            };
        };

        return function (options) {

            if (typeof options === 'string') {
                options = {
                    viewUrl: options
                };
            }

            var _options = options;

            var _$modalContainer = null;
            var _$modal = null;
            var _$form = null;

            var _modalId = 'kusys_Modal_' + (Math.floor((Math.random() * 1000000))) + new Date().getTime();
            var _modalObject = null;

            var _publicApi = null;
            var _args = null;

            var _onOpenCallbacks = new CallbackList();
            var _onCloseCallbacks = new CallbackList();
            var _onPostErrorCallbacks = new CallbackList();
            var _onGetErrorCallbacks = new CallbackList();
            var _onResultCallbacks = new CallbackList();

            function _removeContainer() {
                _$modalContainer && _$modalContainer.remove();
            }

            function _createContainer() {
                _removeContainer();
                _$modalContainer = $('<div id="' + _modalId + 'Container' + '"></div>');
                var existsModals = $('[id^="kusys_Modal_"]');
                if (existsModals.length) {
                    existsModals.last().after(_$modalContainer);
                } else {
                    $('body').prepend(_$modalContainer);
                }
                return _$modalContainer;
            }

            function _initAndShowModal() {
                _$modal = _$modalContainer.find('.modal');
                _$form = _$modalContainer.find('form');
                if (_$form.length) {
                    if (_$form.attr('data-ajaxForm') !== 'false') {
                        _$form.kusysAjaxForm();
                    }

                    //if (_$form.attr('data-check-form-on-close') !== 'false') {
                    //    _$form.needConfirmationOnUnsavedClose(_$modal);
                    //}

                    _$form.on('kusys-ajax-success',
                        function () {
                            _publicApi.setResult.apply(_publicApi, arguments);
                            _$modal.modal('hide');
                        });

                    _$form.on("kusys-ajax-post-error",
                        function () {
                            _onPostErrorCallbacks.triggerAll(_publicApi);
                        });

                    _$form.on('kusys-ajax-validation-error',
                        function (jqEvent, response) {
                            var newContent = $(response.jqXhr.responseText).find('.modal-content');
                            var oldContent = _$modalContainer.find('.modal-content');

                            //var newToken = $(response.jqXhr.responseText).find('input[name="__RequestVerificationToken"]').val();
                            //_$modalContainer.find('input[name="__RequestVerificationToken"]').val(newToken);

                            oldContent.html(newContent.html());

                            $('.field-validation-error').each(function () {
                                var message = $(this).html();
                                if (message.search('span') < 0) {
                                    $(this).html('<span>' + message + '</span>');
                                }
                            });

                            _modalObject.initModal && _modalObject.initModal(_publicApi, _args);
                        });

                } else {
                    _$form = null;
                }

                _$modal.modal({
                    backdrop: 'static'
                });

                _$modal.on('hidden.bs.modal', function () {
                    _removeContainer();
                    _onCloseCallbacks.triggerAll(_publicApi);
                });

                _$modal.on('shown.bs.modal', function () {
                    //focuses first element if it's a typeable input.
                    var $firstVisibleInput = _$modal.find('input:not([type=hidden],[readonly]),textarea:not([type=hidden])').first();

                    _onOpenCallbacks.triggerAll(_publicApi);

                    if ($firstVisibleInput.hasClass("datepicker")) {
                        return; //don't pop-up date pickers...
                    }

                    var focusableInputs = ["text", "password", "email", "number", "search", "tel", "url", "textarea"];
                    if (!focusableInputs.includes($firstVisibleInput.prop("type"))) {
                        return;
                    }

                    $firstVisibleInput.focus();
                });

                var modalClass = kusys.modals[options.modalClass];
                if (modalClass) {
                    _modalObject = new modalClass();
                    _modalObject.init && _modalObject.init(_publicApi, _args); //TODO: Remove later
                    _modalObject.initModal && _modalObject.initModal(_publicApi, _args);
                }

                _$modal.modal('show');
            };

            var _open = function (args) {

                _args = args || {};

                _createContainer(_modalId)
                    .load(options.viewUrl, $.param(_args), function (response, status, xhr) {
                        if (status === "error") {

                            _onGetErrorCallbacks.triggerAll(_publicApi);

                            if (xhr.getResponseHeader('_kusysErrorFormat') === 'true') {

                                if (xhr.responseJSON == undefined) {
                                    xhr.responseJSON = JSON.parse(xhr.responseText);
                                }

                                var messagePromise = kusys.ajax.showError(xhr.responseJSON.error);
                                if (xhr.status === 401) {
                                    kusys.ajax.handleUnAuthorizedRequest(messagePromise);
                                }
                            } else {
                                kusys.ajax.handleErrorStatusCode(xhr.status);
                            }
                            return;
                        };

                        if (options.scriptUrl) {
                            kusys.ResourceLoader.loadScript(options.scriptUrl, function () {
                                _initAndShowModal();
                            });
                        } else {
                            _initAndShowModal();
                        }
                    });
            };

            var _close = function () {
                if (!_$modal) {
                    return;
                }

                _$modal.modal('hide');
            };

            var _onOpen = function (onOpenCallback) {
                _onOpenCallbacks.add(onOpenCallback);
            };

            var _onClose = function (onCloseCallback) {
                _onCloseCallbacks.add(onCloseCallback);
            };

            var _onPostError = function (onPostErrorCallback) {
                _onPostErrorCallbacks.add(onPostErrorCallback);
            };

            var _onGetError = function (onGetErrorCallback) {
                _onGetErrorCallbacks.add(onGetErrorCallback);
            };

            var _onResult = function (callback) {
                _onResultCallbacks.add(callback);
            };

            _publicApi = {
                open: _open,

                reopen: function () {
                    _open(_args);
                },

                close: _close,

                getModalId: function () {
                    return _modalId;
                },

                getModal: function () {
                    return _$modal;
                },

                getForm: function () {
                    return _$form;
                },

                getArgs: function () {
                    return _args;
                },

                getOptions: function () {
                    return _options;
                },

                setResult: function () {
                    _onResultCallbacks.triggerAll(_publicApi, arguments);
                },

                onOpen: _onOpen,

                onClose: _onClose,

                onPostError: _onPostError,

                onGetError: _onGetError,

                onResult: _onResult
            };

            return _publicApi;

        };
    })();
})(jQuery);