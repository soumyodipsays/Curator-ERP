/* =========================================================
   GLOBALS
========================================================= */

let addressModal = null;

/* =========================================================
   INIT
========================================================= */

document.addEventListener("DOMContentLoaded", () => {

    /* Bootstrap Modal */
    const modalEl =
        document.getElementById("addressModal");

    if (modalEl) {
        addressModal =
            new bootstrap.Modal(modalEl);
    }

    /* Fade-up animations */
    setTimeout(() => {

        document.querySelectorAll(".fade-up")
            .forEach((el, i) => {

                setTimeout(() => {
                    el.classList.add("visible");
                }, i * 80);

            });

    }, 50);

    /* Field keyboard handlers */
    bindFieldEvents();

    /* Side nav observer */
    initSectionObserver();

    /* Load states */
    loadStates();

    /* Dynamic edit handler */
    bindAddressEvents();
});

function showToast(msg, icon = 'bi-check-circle-fill', color = '#30b94d') {
    const toastEl = document.getElementById('appToast');
    if (!toastEl) return;

    document.getElementById('toastMsg').textContent = msg;
    const iconEl = document.getElementById('toastIcon');

    if (iconEl) {
        iconEl.className = 'bi ' + icon;
        iconEl.style.color = color;
    }

    // Ensure bootstrap is loaded before calling Toast
    if (typeof bootstrap !== 'undefined') {
        new bootstrap.Toast(toastEl, { delay: 2500 }).show();
    } else {
        console.warn("Bootstrap JS is not loaded. Toast cannot be shown.");
    }
};

/* =========================================================
   APP CONFIG
========================================================= */

function getAuthBaseUrl() {

    const config =
        window.AppConfig || {};

    return config.authBaseUrl || "";
}

/* =========================================================
   STATES
========================================================= */

function loadStates(selectedStateId) {

    const stateURL =
        getAuthBaseUrl() + "/User/GetStates";

    $.ajax({

        url: stateURL,
        type: "GET",

        success: function (response) {

            if (!response.success) {
                return;
            }

            const $state =
                $("#m-state");

            $state.empty();

            $state.append(`
                <option value="">
                    Select State
                </option>
            `);

            response.data.forEach(state => {

                $state.append(`
                    <option value="${state.StateID}">
                        ${state.State}
                    </option>
                `);

            });

            console.log("selectedStateId: ", selectedStateId)

            if (selectedStateId) {
                $state.val(String(selectedStateId));
            }
        },

        error: function (response) {

            console.log("State Error:", response);

            showToast("Failed to load states");
        }
    });
}

/* =========================================================
   INLINE PROFILE FIELD EDITING
========================================================= */

function startEdit(field) {

    document.querySelectorAll(".field-edit-form")
        .forEach(form => {
            form.style.display = "none";
        });

    const form =
        document.getElementById(`edit-${field}`);

    if (form) {
        form.style.display = "block";
    }

    const err =
        document.getElementById(`err-${field}`);

    if (err) {
        err.textContent = "";
    }

    const input =
        document.getElementById(`input-${field}`);

    if (input) {
        input.focus();
    }
}

function cancelEdit(field) {

    const form =
        document.getElementById(`edit-${field}`);

    if (form) {
        form.style.display = "none";
    }
}

/* =========================================================
   FIELD KEYBOARD EVENTS
========================================================= */

function bindFieldEvents() {

    document.querySelectorAll(".field-input")
        .forEach(input => {

            input.addEventListener("keydown", e => {

                const field =
                    input.id.replace("input-", "");

                if (e.key === "Enter") {
                    saveField(field);
                }

                if (e.key === "Escape") {
                    cancelEdit(field);
                }

            });

        });
}

/* =========================================================
   PROFILE IMAGE
========================================================= */

function triggerAvatarUpload() {

    document.getElementById("avatarInput")?.click();
}

function uploadAvatar(input) {

    if (!input.files || !input.files[0]) {
        return;
    }

    const file = input.files[0];

    const reader =
        new FileReader();

    reader.onload = e => {

        const img =
            document.querySelector(".avatar-img");

        if (img) {
            img.src = e.target.result;
        }

        showToast("Profile photo updated ✓");
    };

    reader.readAsDataURL(file);
}

/* =========================================================
   ADDRESS MODAL
========================================================= */

function openAddressModal() {

    setModalMode(
        "Add New Address",
        "Save Address"
    );

    resetAddressForm();

    $("#modalAddressId").val(0);

    clearModalErrors();

    addressModal?.show();
}

function openEditModal(address) {

    if (!address) {
        return;
    }

    setModalMode(
        "Edit Address",
        "Update Address"
    );

    console.log("Address: ", address);

    loadStates(address.StateID);

    $("#modalAddressId")
        .val(address.AddressID || 0);

    $("#m-addr1")
        .val(address.Address1 || "");

    $("#m-addr2")
        .val(address.Address2 || "");

    $("#m-city")
        .val(address.City || "");

    $("#m-pin")
        .val(address.PinCode || "");

    $("#m-location")
        .val(address.Location || "Home");

    $("#m-default")
        .prop("checked", !!address.IsDefault);

    clearModalErrors();

    addressModal?.show();
}

function setModalMode(title, buttonText) {

    $(".addr-modal-title")
        .text(title);

    $("#modalSaveBtn")
        .html(`
            <i class="bi bi-check-lg"></i>
            <span>${buttonText}</span>
        `);
}

function resetAddressForm() {

    $("#m-addr1").val("");
    $("#m-addr2").val("");
    $("#m-city").val("");
    $("#m-pin").val("");

    $("#m-state").val("");

    $("#m-location")
        .val("Home");

    $("#m-default")
        .prop("checked", false);
}

function clearModalErrors() {

    $(".form-error").hide();
}

/* =========================================================
   SAVE ADDRESS
========================================================= */

function submitAddressModal() {

    const address =
        getAddressFormData();

    console.log("Address Payload:", address);

    if (!validateAddress(address)) {
        return;
    }

    const saveAddressURL =
        getAuthBaseUrl() + "/User/AddOrEditUserAddress";

    const updateAddressURL =
        getAuthBaseUrl() + "/User/AddOrEditUserAddress";

    const isEdit =
        address.AddressID > 0;

    const endpoint =
        isEdit
            ? updateAddressURL
            : saveAddressURL;

    setAddressButtonLoading(true);

    $.ajax({

        url: endpoint,
        type: "POST",
        data: address,

        success: function (response) {

            if (!response.success) {

                showToast(
                    "Failed to save address"
                );

                return;
            }

            addressModal?.hide();

            const savedAddress =
                address;

            if (isEdit) {

                updateAddressCard(savedAddress);

            } else {

                appendAddressCard(savedAddress);
            }

            showToast(
                isEdit
                    ? "Address updated successfully 📍"
                    : "Address added successfully 📍"
            );

            // window.location.reload();
        },

        error: function () {

            showToast("Something went wrong");
        },

        complete: function () {

            setAddressButtonLoading(false);
        }
    });
}

function getAddressFormData() {

    return {

        UserID:
            Auth.currentUser()?.UserID ||
            localStorage.getItem("UserID"),

        AddressID:
            parseInt(
                $("#modalAddressId").val()
            ) || 0,

        Address1:
            $("#m-addr1")
                .val()
                .trim(),

        Address2:
            $("#m-addr2")
                .val()
                .trim(),

        City:
            $("#m-city")
                .val()
                .trim(),

        PinCode:
            $("#m-pin")
                .val()
                .trim(),

        StateID:
            $("#m-state").val(),

        StateName:
            $("#m-state option:selected")
                .text(),

        Location:
            $("#m-location").val(),

        IsDefault:
            $("#m-default")
                .is(":checked")
    };
}

function validateAddress(address) {

    let valid = true;

    clearModalErrors();

    if (!address.Address1) {

        $("#merr-addr1").show();

        valid = false;
    }

    if (!address.City) {

        $("#merr-city").show();

        valid = false;
    }

    if (address.PinCode.length < 6) {

        $("#merr-pin").show();

        valid = false;
    }

    if (!address.StateID) {

        $("#merr-state").show();

        valid = false;
    }

    return valid;
}

function setAddressButtonLoading(loading) {

    const $btn =
        $("#modalSaveBtn");

    if (loading) {

        $btn.prop("disabled", true);

        $btn.html(`
            <i class="bi bi-hourglass-split"></i>
            Saving...
        `);

        return;
    }

    $btn.prop("disabled", false);

    const isEdit =
        parseInt(
            $("#modalAddressId").val()
        ) > 0;

    $btn.html(`
        <i class="bi bi-check-lg"></i>
        <span>
            ${isEdit
            ? "Update Address"
            : "Save Address"}
        </span>
    `);
}

/* =========================================================
   ADDRESS EVENTS
========================================================= */

function bindAddressEvents() {

    $(document).on(
        "click",
        ".edit-address-btn",
        function () {

            const raw =
                $(this).attr("data-address");

            if (!raw) {
                return;
            }

            const address =
                JSON.parse(decodeURIComponent(raw));

            console.log("Edit Address:", address);

            openEditModal(address);
        }
    );
}

/* =========================================================
   ADDRESS CARDS
========================================================= */

function appendAddressCard(address) {

    $("#addressList")
        .append(generateAddressCard(address));

    $("#noAddressMsg").hide();
}

function updateAddressCard(address) {

    const $card =
        $(`#addrCard-${address.AddressID}`);

    if (!$card.length) {
        return;
    }

    const newCard =
        $(generateAddressCard(address));

    $card.replaceWith(newCard);
}

function generateAddressCard(addr) {

    const encodedAddress =
        encodeURIComponent(
            JSON.stringify(addr)
        );

    let address1 = addr.Address1 || "";
    let address2 = addr.Address2 || "";
    let city = addr.City || "";
    let state = addr.StateName || addr.State || "";
    let pin = addr.PinCode || addr.Pin || "";

    const html = `
            <div class="modern-address-card ${addr.IsDefault ? "default-card" : ""}"
                 id="addrCard-${addr.AddressID}">

                <div class="card-top">

                    <div class="d-flex align-items-center gap-2">

                        <div class="location-icon">
                            <i class="bi bi-geo-alt-fill"></i>
                        </div>

                        ${addr.IsDefault
            ? `
                                <span class="default-badge">
                                    Default Address
                                </span>
                            `
            : `
                                <span class="address-label">
                                    Saved Address
                                </span>
                            `
        }

                    </div>

                    <div class="action-group">

                        <button class="icon-action-btn edit-address-btn"
                                title="Edit Address"
                                data-address='${JSON.stringify(addr)}'>

                            <i class="bi bi-pencil-square"></i>

                        </button>

                        <button class="icon-action-btn delete-btn"
                                title="Delete Address"
                                onclick="deleteAddress(${addr.AddressID})">

                            <i class="bi bi-trash3"></i>

                        </button>

                    </div>

                </div>

                <div class="address-content">

                    <div class="primary-address">
                        ${address1}
                    </div>

                    ${address2
            ? `
                            <div class="secondary-address">
                                ${address2}
                            </div>
                        `
            : ""
        }

                    <div class="city-state-row">
                        <i class="bi bi-building"></i>

                        <span>
                            ${city ? city + "," : ""}
                            ${state || ""}
                            ${pin ? " - " + pin : ""}
                        </span>
                    </div>

                    ${addr.Location
            ? `
                            <div class="location-row">
                                <i class="bi bi-pin-map"></i>

                                <span>
                                    ${addr.Location}
                                </span>
                            </div>
                        `
            : ""
        }

                </div>

            </div>
        `;

    return html;
}

/* =========================================================
   DELETE ADDRESS
========================================================= */

function deleteAddress(addressId) {

    const userID = Auth.currentUser()?.UserID ||
                    localStorage.getItem("UserID")

    if (!confirm("Remove this address?")) {
        return;
    }

    $.ajax({

        url:
            getAuthBaseUrl() +
            "/User/RemoveUserAddress",

        type: "POST",

        data: {
            addressId,
            userID
        },

        success: function (response) {

            if (!response.success) {

                showToast(
                    "Failed to remove address"
                );

                return;
            }

            const $card =
                $(`#addrCard-${addressId}`);

            $card.fadeOut(300, function () {

                $(this).remove();

                if (
                    $("#addressList")
                        .children()
                        .length === 0
                ) {
                    $("#noAddressMsg").show();
                }
            });

            showToast("Address removed");
        },

        error: function () {

            showToast(
                "Failed to remove address"
            );
        }
    });
}

/* =========================================================
   DEFAULT ADDRESS
========================================================= */

function setDefault(addressId) {

    $.ajax({

        url:
            getAuthBaseUrl() +
            "/User/SetDefaultAddress",

        type: "POST",

        data: {
            addressId
        },

        success: function (response) {

            if (!response.success) {
                return;
            }

            $(".addr-profile-card")
                .removeClass("default");

            $(".default-badge")
                .remove();

            const $card =
                $(`#addrCard-${addressId}`);

            $card.addClass("default");

            $card.find(".addr-type-label")
                .after(`
                    <span class="default-badge">
                        Default
                    </span>
                `);

            showToast(
                "Default address updated ⭐"
            );
        }
    });
}

/* =========================================================
   SIDE NAV ACTIVE SECTION
========================================================= */

function initSectionObserver() {

    const sections =
        document.querySelectorAll(
            "[id$='Section']"
        );

    const navItems =
        document.querySelectorAll(
            ".sidenav-item"
        );

    if (
        !sections.length ||
        !navItems.length
    ) {
        return;
    }

    const observer =
        new IntersectionObserver(entries => {

            entries.forEach(entry => {

                if (!entry.isIntersecting) {
                    return;
                }

                navItems.forEach(item => {
                    item.classList.remove("active");
                });

                const active =
                    document.querySelector(
                        `.sidenav-item[href="#${entry.target.id}"]`
                    );

                active?.classList.add("active");
            });

        }, {
            threshold: 0.45
        });

    sections.forEach(section => {
        observer.observe(section);
    });
}

/* =========================================================
   HELPERS
========================================================= */

function escapeJs(value) {

    if (!value) {
        return "";
    }

    return String(value)
        .replace(/\\/g, "\\\\")
        .replace(/'/g, "\\'")
        .replace(/"/g, "&quot;");
}