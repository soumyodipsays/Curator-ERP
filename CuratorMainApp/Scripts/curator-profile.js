/* =============================================
   curator-profile.js
   Cleaned + Optimized Version
   ============================================= */

"use strict";

/* =========================================================
   GLOBALS
========================================================= */

let addressModal = null;

/* =========================================================
   INIT
========================================================= */

document.addEventListener("DOMContentLoaded", () => {

    /* Bootstrap Modal */
    const modalEl = document.getElementById("addressModal");

    if (modalEl) {
        addressModal = new bootstrap.Modal(modalEl);
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
});

/* =========================================================
   INLINE PROFILE FIELD EDITING
========================================================= */

function startEdit(field) {

    /* Hide all edit forms */
    document.querySelectorAll(".field-edit-form")
        .forEach(form => {
            form.style.display = "none";
        });

    /* Show current form */
    const form = document.getElementById(`edit-${field}`);

    if (form) {
        form.style.display = "block";
    }

    /* Clear error */
    const err = document.getElementById(`err-${field}`);

    if (err) {
        err.textContent = "";
    }

    /* Focus input */
    const input = document.getElementById(`input-${field}`);

    if (input) {
        input.focus();
    }
}

function cancelEdit(field) {

    const form = document.getElementById(`edit-${field}`);

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

    const reader = new FileReader();

    reader.onload = e => {

        const img =
            document.querySelector(".avatar-img");

        if (img) {
            img.src = e.target.result;
        }

        showToast("Profile photo updated ✓");

        /* TODO:
           Upload using FormData API
        */
    };

    reader.readAsDataURL(file);
}

/* =========================================================
   ADDRESS MODAL
========================================================= */

function openAddressModal() {

    setModalMode("Add New Address", "Save Address");

    $("#modalAddressId").val(0);

    resetAddressForm();

    clearModalErrors();

    addressModal?.show();
}

function openEditModal(
    id,
    addr1,
    addr2,
    city,
    stateId,
    stateName,
    pin,
    location,
    isDefault
) {

    setModalMode("Edit Address", "Update Address");

    $("#modalAddressId").val(id);

    $("#m-addr1").val(addr1 || "");
    $("#m-addr2").val(addr2 || "");
    $("#m-city").val(city || "");
    $("#m-pin").val(pin || "");
    $("#m-state").val(stateId || "");
    $("#m-location").val(location || "Home");

    $("#m-default")
        .prop("checked", !!isDefault);

    clearModalErrors();

    addressModal?.show();
}

function setModalMode(title, buttonText) {

    $(".addr-modal-title").text(title);

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

    $("#m-location").val("Home");

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

    const address = getAddressFormData();

    if (!validateAddress(address)) {
        return;
    }

    const isEdit =
        address.AddressID > 0;

    const endpoint =
        isEdit
            ? "/Account/UpdateAddress"
            : "/Account/SaveAddress";

    setAddressButtonLoading(true);

    $.ajax({

        url: endpoint,
        type: "POST",
        data: address,

        success: function (response) {

            if (!response.success) {
                showToast("Failed to save address");
                return;
            }

            addressModal?.hide();

            if (isEdit) {
                updateAddressCard(response.address);
            }
            else {
                appendAddressCard(response.address);
            }

            showToast(
                isEdit
                    ? "Address updated successfully 📍"
                    : "Address added successfully 📍"
            );
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

        AddressID:
            parseInt($("#modalAddressId").val()) || 0,

        Address1:
            $("#m-addr1").val().trim(),

        Address2:
            $("#m-addr2").val().trim(),

        City:
            $("#m-city").val().trim(),

        PinCode:
            $("#m-pin").val().trim(),

        StateID:
            $("#m-state").val(),

        StateName:
            $("#m-state option:selected").text(),

        Location:
            $("#m-location").val(),

        IsDefault:
            $("#m-default").is(":checked")
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

    const $btn = $("#modalSaveBtn");

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
        parseInt($("#modalAddressId").val()) > 0;

    $btn.html(`
        <i class="bi bi-check-lg"></i>
        <span>
            ${isEdit ? "Update Address" : "Save Address"}
        </span>
    `);
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

function generateAddressCard(address) {

    const icon =
        address.Location === "Home"
            ? "🏠"
            : address.Location === "Office"
                ? "🏢"
                : "📍";

    return `
        <div class="addr-profile-card ${address.IsDefault ? "default" : ""}"
             id="addrCard-${address.AddressID}">

            <div class="d-flex justify-content-between align-items-start mb-3">

                <div class="d-flex align-items-center gap-2">

                    <span class="addr-type-icon">
                        ${icon}
                    </span>

                    <div>

                        <span class="addr-type-label">
                            ${address.Location}
                        </span>

                        ${address.IsDefault
            ? `<span class="default-badge">Default</span>`
            : ""
        }

                    </div>

                </div>

                <div class="addr-action-btns">

                    <button class="addr-action-btn"
                            onclick="openEditModal(
                                ${address.AddressID},
                                '${escapeJs(address.Address1)}',
                                '${escapeJs(address.Address2)}',
                                '${escapeJs(address.City)}',
                                '${escapeJs(address.StateID)}',
                                '${escapeJs(address.StateName)}',
                                '${escapeJs(address.PinCode)}',
                                '${escapeJs(address.Location)}',
                                ${address.IsDefault}
                            )"
                            title="Edit">

                        <i class="bi bi-pencil"></i>

                    </button>

                    <button class="addr-action-btn delete-btn"
                            onclick="deleteAddress(${address.AddressID})"
                            title="Delete">

                        <i class="bi bi-trash3"></i>

                    </button>

                </div>

            </div>

            <div class="addr-full-line">
                ${address.Address1}
            </div>

            ${address.Address2
            ? `
                <div class="addr-full-line secondary">
                    ${address.Address2}
                </div>
            `
            : ""
        }

            <div class="addr-city-line">
                ${address.City},
                ${address.StateName}
                — ${address.PinCode}
            </div>

        </div>
    `;
}

/* =========================================================
   DELETE ADDRESS
========================================================= */

function deleteAddress(addressId) {

    if (!confirm("Remove this address?")) {
        return;
    }

    $.ajax({

        url: "/Account/DeleteAddress",
        type: "POST",

        data: {
            addressId
        },

        success: function (response) {

            if (!response.success) {
                showToast("Failed to remove address");
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

            showToast("Failed to remove address");
        }
    });
}

/* =========================================================
   DEFAULT ADDRESS
========================================================= */

function setDefault(addressId) {

    $.ajax({

        url: "/Account/SetDefaultAddress",
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

            showToast("Default address updated ⭐");
        }
    });
}

/* =========================================================
   SIDE NAV ACTIVE SECTION
========================================================= */

function initSectionObserver() {

    const sections =
        document.querySelectorAll("[id$='Section']");

    const navItems =
        document.querySelectorAll(".sidenav-item");

    if (!sections.length || !navItems.length) {
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