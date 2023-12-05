$(document).ready(function () {
    console.log(1)
    let check = $('.js-example-placeholder-multiple');

    if (check != null) {
        $('.js-example-placeholder-multiple').select2({
            placeholder: 'Select options', // Set your desired placeholder text
        })
    }
})