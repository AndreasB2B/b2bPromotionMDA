//using: https://stackoverflow.com/questions/11844256/alert-for-unsaved-changes-in-form
$(document).ready(function () {
    var unsaved = false;
    var showingModal = false;
    // Another way to bind the event
    $(window).bind('beforeunload', function () {
        //disable when a modal window is opened
        
        if (unsaved && !showingModal) {
            //Custom mesages are now a security hazzard: https://stackoverflow.com/a/1119324/5744616
            return true;
        }
    });

    //disable for modals - https://getbootstrap.com/docs/4.0/components/modal/#events
    $('#products-modal').on('show.bs.modal', function (e) {
        showingModal = true;
    })

    $('#communications-modal').on('show.bs.modal', function (e) {
        showingModal = true;
    })

    // Monitor dynamic inputs
    $(document).on('change', ':input', function () { //triggers change in all input fields including text type
        unsaved = true;
    });

    $('#save').click(function () {
        unsaved = false;
    });
});