alertType = {
    SUCCESS: "success",
    ERROR: "error",
    WARNING: "warning",
    INFO: "info",
    QUESTİON: "question"
};



toast = Swal.mixin({
    toast: true,
    position: "top-right",
    showConfirmButton: false,
    timer: 3000,

    timerProgressBar: true,
    onOpen: function (toastEvent) {
        toastEvent.addEventListener("mouseenter", Swal.stopTimer);
        toastEvent.addEventListener("mouseleave", Swal.resumeTimer);
    }
});



function ajax(url, method, data, callBack) {
    $.ajax({
        url: url,
        method: method,
        data: data,
        timeout: 15000,
        success: function (result) {
            if (callBack)
                callBack(result);
        }
    });
}

function showAlert(message, alertType) {
    if (!alertType) {
        alertType = this.alertType.SUCCESS;
    }
    this.toast.fire({
        icon: alertType,
        title: message
    });
}
// TODO : Sweetaler IE'de patlıyor. Düzelt .
function showAlertDialog(message, title, alertType) {
    if (!alertType) {
        alertType = this.alertType.SUCCESS;
    }

    Swal.fire({
        icon: alertType,
        title: title,
        text: message,
        confirmButtonText: "Tamam"
    });
}

function showAlertDialogResult(
    message,
    title,
    callback
) {
    Swal.fire({
        title: title,
        text: message,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        cancelButtonText: "Hayır",
        confirmButtonText: "Evet"
    }).then(function (result) {
        callback(result);
    });
}

function showLoader() {
    KTApp.blockPage({
        overlayColor: '#000000',
        state: 'danger',
        message: 'Lütfen Bekleyiniz...'
    });
}

function hideLoader() {
    KTApp.unblockPage();
}


