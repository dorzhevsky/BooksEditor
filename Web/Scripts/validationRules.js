(function ($) {

    var isValidIsbn10 = function(isbn) {

        var sum = 0;
        var val;

        for (var i = 0; i < 9; i++) {
            var c = isbn[i];
            val = parseInt(c);
            if (val != NaN) {
                sum += (i + 1) * val;
            }
            else {
                return false;
            }
        }

        var remainder = sum % 11;
        var lastCharacter = isbn[isbn.length - 1];

        if (lastCharacter == 'X') {
            return remainder == 10;
        }

        val = parseInt(lastCharacter);
        if (val != NaN) {
            return remainder == val;
        }

        return false;
    }

    var isValidIsbn13 = function (isbn) {

        var sum = 0;
        var val;

        for (var i = 0; i < 12; i++) {
            var c = isbn[i];
            val = parseInt(c);
            if (val != NaN) {
                sum += (i % 2 == 1 ? 3 : 1) * val;
            }
            else {
                return false;
            }
        }

        var remainder = sum % 10;
        var checkDigit = 10 - remainder;
        if (checkDigit == 10) checkDigit = 0;

        var lastCharacter = isbn[isbn.length - 1];

        val = parseInt(lastCharacter);
        if (val != NaN) {
            return checkDigit == val;
        }

        return false;
    }

    $.validator.addMethod("isbn", function (value) {

        if (!value) {
            return true;
        }

        var normalizedValue = value.replace(/-/g, "");
        if (normalizedValue.length == 10) {
            return isValidIsbn10(normalizedValue);
        }
        if (normalizedValue.length == 13) {
            return isValidIsbn13(normalizedValue);
        }
        return false;
    });

}(jQuery));