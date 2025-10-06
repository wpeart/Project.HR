window.bootstrapInterop = {
    showModal: function (selector) {
        console.log("Opening modal:", selector);
        const modalEl = document.querySelector(selector);
        if (!modalEl) return;

        const modal = new bootstrap.Modal(modalEl);
        modal.show();
    },
    hideModal: function (selector) {
        const modalEl = document.querySelector(selector);
        if (!modalEl) return;

        const modal = bootstrap.Modal.getInstance(modalEl);
        if (modal) modal.hide();
    }
};