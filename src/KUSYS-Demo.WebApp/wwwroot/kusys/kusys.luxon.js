var kusys = kusys || {};
(function () {

    if (!luxon) {
        throw "kusys/luxon library requires the luxon library included to the page!";
    }

    /* TIMING *************************************************/

    kusys.timing = kusys.timing || {};

    var setObjectValue = function (obj, property, value) {
        if (typeof property === "string") {
            property = property.split('.');
        }

        if (property.length > 1) {
            var p = property.shift();
            setObjectValue(obj[p], property, value);
        } else {
            obj[property[0]] = value;
        }
    }

    var getObjectValue = function (obj, property) {
        return property.split('.').reduce((a, v) => a[v], obj)
    }

    kusys.timing.convertFieldsToIsoDate = function (form, fields) {
        for (var field of fields) {
            var dateTime = luxon.DateTime
                .fromFormat(
                    getObjectValue(form, field),
                    kusys.localization.currentCulture.dateTimeFormat.shortDatePattern,
                    {locale: kusys.localization.currentCulture.cultureName}
                );

            if (!dateTime.invalid) {
                setObjectValue(form, field, dateTime.toFormat("yyyy-MM-dd HH:mm:ss"))
            }
        }

        return form;
    }

})(jQuery);
