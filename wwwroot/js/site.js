// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// IziToast Configuration
$(document).ready(function () {
    // Configure default options for all iziToasts
    iziToast.settings({
        timeout: 5000,
        resetOnHover: true,
        transitionIn: 'flipInX',
        transitionOut: 'flipOutX',
        position: 'topRight',
    });

    // Custom CSS for more prominent dialogs
    $("<style>")
        .prop("type", "text/css")
        .html(`
            .iziToast-overlay {
                background: rgba(0, 0, 0, 0.6) !important;
            }
            .custom-question {
                width: 450px !important;
                padding: 30px !important;
                border-radius: 8px !important;
                box-shadow: 0 10px 25px rgba(0, 0, 0, 0.3) !important;
                justify-content: center;
                align-items: center;
                text-align: center !important;
            }
            .custom-question .iziToast-header {
                margin-bottom: 20px !important;
                justify-content: center;
                align-items: center;
                text-align: center !important;
            }
            .custom-question .iziToast-title {
                font-size: 24px !important;
                font-weight: 600 !important;
                margin-bottom: 10px !important;
                color: #333 !important;
                justify-content: center;
                align-items: center;
                text-align: center !important;
            }
            .custom-question .iziToast-message {
                font-size: 18px !important;
                line-height: 1.5 !important;
                color: #555 !important;
                margin-bottom: 20px !important;
                justify-content: center;
                align-items: center;
                text-align: center !important;
            }
            .custom-question .iziToast-buttons {
                display: flex !important;
                justify-content: center !important;
                gap: 15px !important;
                justify-content: center;
                align-items: center;
                flex-wrap: wrap;
                width: 100%;

            }
            .custom-question .iziToast-buttons button {
                min-width: 120px !important;
                padding: 12px 20px !important;
                font-size: 16px !important;
                border-radius: 6px !important;
                cursor: pointer !important;
                transition: all 0.2s ease !important;
                border: none !important;
                outline: none !important;
            }
            .custom-question .iziToast-buttons button:first-child {
                background-color:rgb(214, 113, 123) !important;
                color: white !important;
            }
            .custom-question .iziToast-buttons button:first-child:hover {
                background-color: #c82333 !important;
            }
            .custom-question .iziToast-buttons button:last-child {
                background-color: #6c757d !important;
                color: white !important;
            }
            .custom-question .iziToast-buttons button:last-child:hover {
                background-color: #5a6268 !important;
            }

            title{
                text-align: center;
            }
        `)
        .appendTo("head");

    // DELETE EVENT CONFIRMATION
    $(document).on('click', '.delete-event', function (e) {
        e.preventDefault();
        var deleteUrl = $(this).attr('href');
        var eventTitle = $(this).data('title');

        iziToast.question({
            timeout: false,
            close: false,
            overlay: true,
            displayMode: 'once',
            id: 'question',
            zindex: 999,
            icon: '',
            title: 'Confirm Deletion',
            message: 'Are you sure you want to delete the event "' + eventTitle + '"?',
            position: 'center',
            class: 'custom-question',
            backgroundColor: '#F5E8C7',
            buttons: [
                ['<button><b>Yes, Delete</b></button>', function (instance, toast) {
                    instance.hide({ transitionOut: 'fadeOut' }, toast, 'button');
                    
                    // Use AJAX to delete the event
                    $.ajax({
                        url: deleteUrl,
                        type: 'GET',
                        headers: {
                            'X-Requested-With': 'XMLHttpRequest'
                        },
                        success: function(response) {
                            if (response.success) {
                                window.showSuccessToast('Event deleted successfully');
                                // Reload the page or remove the event element from DOM
                                setTimeout(function() {
                                    window.location.reload();
                                }, 1000);
                            } else {
                                window.showErrorToast('Failed to delete event');
                            }
                        },
                        error: function() {
                            window.showErrorToast('Error deleting event');
                        }
                    });
                }, true],
                ['<button>Cancel</button>', function (instance, toast) {
                    instance.hide({ transitionOut: 'fadeOut' }, toast, 'button');
                }]
            ]
        });
    });

    // EDIT EVENT CONFIRMATION
    $('#edit-event-form').on('submit', function (e) {
        e.preventDefault();
        var form = $(this);

        iziToast.question({
            timeout: false,
            close: false,
            overlay: true,
            displayMode: 'once',
            id: 'question',
            zindex: 999,
            icon: '',
            title: 'Confirm Update',
            message: 'Are you sure you want to update this event?',
            position: 'center',
            class: 'custom-question',
            buttons: [
                ['<button><b>Yes, Update</b></button>', function (instance, toast) {
                    instance.hide({ transitionOut: 'fadeOut' }, toast, 'button');
                    form.off('submit').submit();
                }, true],
                ['<button>Cancel</button>', function (instance, toast) {
                    instance.hide({ transitionOut: 'fadeOut' }, toast, 'button');
                }]
            ]
        });
    });

    // CREATE EVENT CONFIRMATION
    $('#create-event-form').on('submit', function (e) {
        e.preventDefault();
        var form = $(this);

        iziToast.question({
            timeout: false,
            close: false,
            overlay: true,
            displayMode: 'once',
            id: 'question',
            zindex: 999,
            icon: '',
            title: 'Confirm Creation',
            message: 'Are you sure you want to create this event?',
            position: 'center',
            class: 'custom-question',
            buttons: [
                ['<button><b>Yes, Create</b></button>', function (instance, toast) {
                    instance.hide({ transitionOut: 'fadeOut' }, toast, 'button');
                    form.off('submit').submit();
                }, true],
                ['<button>Cancel</button>', function (instance, toast) {
                    instance.hide({ transitionOut: 'fadeOut' }, toast, 'button');
                }]
            ]
        });
    });

    // Success and Error notifications (for future use)
    window.showSuccessToast = function(message) {
        iziToast.success({
            title: 'Success',
            message: message
        });
    };

    window.showErrorToast = function(message) {
        iziToast.error({
            title: 'Error',
            message: message
        });
    };
});
