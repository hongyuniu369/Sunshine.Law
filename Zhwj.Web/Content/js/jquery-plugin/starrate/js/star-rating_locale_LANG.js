/*!
 * Star Rating <LANG> Translations
 *
 * This file must be loaded after 'star-rating.js'. Patterns in braces '{}', or
 * any HTML markup tags in the messages must not be converted or translated.
 *
 * @see http://github.com/kartik-v/bootstrap-star-rating
 * @author Kartik Visweswaran <kartikv2@gmail.com>
 *
 * NOTE: this file must be saved in UTF-8 encoding.
 */
(function ($) {
    "use strict";
    $.fn.ratingLocales['<LANG>'] = {
        defaultCaption: '{rating} 星（{rating}分）',
        starCaptions: {
            0.5: '半星（0.5分）',
            1: '一星（1分）',
            1.5: '一星半（1.5分）',
            2: '两星（2分）',
            2.5: '两星半（2.5分）',
            3: '三星（三分）',
            3.5: '三星半（3.5分）',
            4: '四星（4分）',
            4.5: '四星半（4.5分）',
            5: '五星（5分）'
        },
        clearButtonTitle: 'Clear',
        clearCaption: 'Not Rated'
    };
})(window.jQuery);
