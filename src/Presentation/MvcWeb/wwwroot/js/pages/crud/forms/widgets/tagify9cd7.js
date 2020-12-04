jQuery(document).ready(function () {
    var input = document.getElementsByClassName('tagify-field');
    var tagify = new Tagify(input[0], {
        originalInputValueFormat: valuesArr => valuesArr.map(item => item.value).join(';'),
        pattern: /^.{0,20}$/,
        delimiters: ";",
        transformTag: transformTag,
    });

    function transformTag(tagData) {
        var states = [
            'success',
            'primary',
            'danger',
            'success',
            'warning',
            'dark',
            'primary',
            'info'];

        tagData.class = 'tagify__tag tagify__tag-light--' + states[KTUtil.getRandomInt(0, 7)];
    }
});
