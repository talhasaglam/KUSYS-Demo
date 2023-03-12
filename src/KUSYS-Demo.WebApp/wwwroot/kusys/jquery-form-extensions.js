(function ($) {
    if (!$ || !$.fn.ajaxForm) {
        return;
    }

    $.fn.kusysAjaxForm = function (userOptions) {
        var $form = $(this);
        userOptions = $.extend({
            formSubmitting: {
                disableSubmitButtonBusy: false,
                disableOtherButtonsBusy: false,
                $excludedBusyButtons: {} /*Prevents busy or disabled effect on these buttons. Must be jQuery instances*/
            }
        }, userOptions || {});

        var options = $.extend({}, $.fn.kusysAjaxForm.defaults, userOptions);

        options.beforeSubmit = function (arr, $form) {
            if ((userOptions.beforeSubmit && userOptions.beforeSubmit.apply(this, arguments)) === false) {
                return false;
            }

            if (!$form.valid()) {
                return false;
            }

            if (!userOptions.formSubmitting.disableSubmitButtonBusy) {
                var submitButtons = $form.find("button[type=submit]").not(userOptions.formSubmitting.$excludedBusyButtons);
                if (submitButtons.length === 1) {
                    submitButtons.prop('disabled', true);
                } else if (submitButtons.length > 1) {
                    $(document.activeElement).prop('disabled', true);
                }
            }

            if (!userOptions.formSubmitting.disableOtherButtonsBusy) {
                var otherButtons = $form.find("button[type!=submit]").not(userOptions.formSubmitting.$excludedBusyButtons);
                otherButtons.prop('disabled', true);
            }

            return true;
        };

        options.success = function (responseText, statusText, xhr, $form) {
            userOptions.success && userOptions.success.apply(this, arguments);
            $form.trigger('kusys-ajax-success',
                {
                    responseText: responseText,
                    statusText: statusText,
                    xhr: xhr,
                    $form: $form
                });
        };

        options.error = function (jqXhr) {
            if ((userOptions.error && userOptions.error.apply(this, arguments)) === false) {
                return;
            }

            //<talha saglam: general Exception>
            $form.trigger("kusys-ajax-post-error");

            //<orhan ak: start validator change>
            if (jqXhr.status === 444) {
                $form.trigger('kusys-ajax-validation-error', { jqXhr: jqXhr });
                return;
            }
            //<end validator change>

            if (jqXhr.getResponseHeader('_kusysErrorFormat') === 'true') {
                kusys.ajax.logError(jqXhr.responseJSON.error);
                var messagePromise = kusys.ajax.showError(jqXhr.responseJSON.error);
                if (jqXhr.status === 401) {
                    kusys.ajax.handleUnAuthorizedRequest(messagePromise);
                }
            } else {
                kusys.ajax.handleErrorStatusCode(jqXhr.status);
            }
        };

        options.complete = function (jqXhr, status, $form) {
            if ($.contains(document, $form[0])) {
                $form.find("button[type='submit']").prop('disabled', false);
                if (!userOptions.formSubmitting.disableOtherButtonsBusy) {
                    var otherButtons = $form.find("button[type!=submit]").not(userOptions.formSubmitting.$excludedBusyButtons);
                    otherButtons.prop('disabled', false);
                }
            }

            $form.trigger('kusys-ajax-complete',
                {
                    status: status,
                    jqXhr: jqXhr,
                    $form: $form
                });

            userOptions.complete && userOptions.complete.apply(this, arguments);
        };

        return $form.ajaxForm(options);
    };

    $.fn.kusysAjaxForm.defaults = {
        method: 'POST'
    };

})(jQuery);