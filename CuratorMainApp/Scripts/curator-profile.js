/* =============================================
   curator-profile.js
   wwwroot/js/curator-profile.js
   Loaded only on /Account/Profile via @section Scripts
   ============================================= */

/* ── INLINE FIELD EDITING ──────────────────────────────────────────────── */

// Open edit mode for a field
// field: "FirstName" | "LastName" | "Username" | "Email" | "Phone"
function startEdit(field) {
    // Close any other open edits first
    document.querySelectorAll('.field-edit-form').forEach(f => {
        f.style.display = 'none';
    });
    document.getElementById(`edit-${field}`).style.display = 'block';
    document.getElementById(`input-${field}`).focus();
    document.getElementById(`err-${field}`).textContent = '';
}

// Cancel and restore display
function cancelEdit(field) {
    document.getElementById(`edit-${field}`).style.display = 'none';
}

// Save field via AJAX
// POST /Account/UpdateField { field, value }
function saveField(field) {
    const inputEl = document.getElementById(`input-${field}`);
    const errEl = document.getElementById(`err-${field}`);
    const value = inputEl.value.trim();

    errEl.textContent = '';

    // Client-side validation
    if (!value) { errEl.textContent = 'This field cannot be empty'; return; }
    if (field === 'Email' && !value.includes('@')) {
        errEl.textContent = 'Enter a valid email address'; return;
    }
    if (field === 'Username' && value.length < 3) {
        errEl.textContent = 'Username must be at least 3 characters'; return;
    }
    if (field === 'Phone' && value.replace(/\D/g, '').length < 10) {
        errEl.textContent = 'Enter a valid phone number'; return;
    }

    // Show saving state
    const saveBtn = document.querySelector(`#edit-${field} .field-save-btn`);
    saveBtn.innerHTML = '<i class="bi bi-hourglass-split"></i> Saving...';
    saveBtn.disabled = true;

    // AJAX call
    // REPLACE with real endpoint in production
    fetch('/Account/UpdateField', {
        method: 'POST',
        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
        body: new URLSearchParams({ field, value })
    })
        .then(r => r.json())
        .then(data => {
            if (data.success) {
                // Update display value
                const display = document.getElementById(`val-${field}`);
                display.textContent = field === 'Username' ? '@' + value : value;

                // Animate success
                display.style.color = 'var(--green)';
                setTimeout(() => display.style.color = '', 1500);

                cancelEdit(field);
                showToast(`${field.replace(/([A-Z])/g, ' $1').trim()} updated ✓`);
            } else {
                errEl.textContent = data.message || 'Update failed';
            }
        })
        .catch(() => {
            // Demo fallback (no real backend)
            const display = document.getElementById(`val-${field}`);
            display.textContent = field === 'Username' ? '@' + value : value;
            display.style.color = 'var(--green)';
            setTimeout(() => display.style.color = '', 1500);
            cancelEdit(field);
            showToast(`${field.replace(/([A-Z])/g, ' $1').trim()} updated ✓`);
        })
        .finally(() => {
            saveBtn.innerHTML = '<i class="bi bi-check-lg"></i> Save';
            saveBtn.disabled = false;
        });
}

// Allow Enter key to save
document.querySelectorAll('.field-input').forEach(input => {
    input.addEventListener('keydown', e => {
        if (e.key === 'Enter') {
            const field = input.id.replace('input-', '');
            saveField(field);
        }
        if (e.key === 'Escape') {
            const field = input.id.replace('input-', '');
            cancelEdit(field);
        }
    });
});

/* ── AVATAR UPLOAD ─────────────────────────────────────────────────────── */
function triggerAvatarUpload() {
    document.getElementById('avatarInput').click();
}
function uploadAvatar(input) {
    if (!input.files || !input.files[0]) return;
    const file = input.files[0];
    const reader = new FileReader();
    reader.onload = e => {
        document.querySelector('.avatar-img').src = e.target.result;
        showToast('Profile photo updated ✓');
        // REPLACE with: FormData upload to /Account/UploadAvatar
    };
    reader.readAsDataURL(file);
}

/* ── ADDRESS MODAL ─────────────────────────────────────────────────────── */
let addressModal = null;

document.addEventListener('DOMContentLoaded', () => {
    const modalEl = document.getElementById('addressModal');
    if (modalEl) {
        addressModal = new bootstrap.Modal(modalEl);
    }
    // Trigger fade-ups
    setTimeout(() => {
        document.querySelectorAll('.fade-up').forEach((el, i) => {
            setTimeout(() => el.classList.add('visible'), i * 80);
        });
    }, 50);
});

// Open modal for ADD
function openAddressModal() {
    document.getElementById('modalAddrTitle').textContent = 'Add New Address';
    document.getElementById('modalSaveBtnText').textContent = 'Save Address';
    document.getElementById('modalAddressId').value = '0';
    // Clear all fields
    ['m-addr1', 'm-addr2', 'm-city', 'm-pin'].forEach(id => {
        document.getElementById(id).value = '';
    });
    document.getElementById('m-state').value = '';
    document.getElementById('m-location').value = 'Home';
    document.getElementById('m-default').checked = false;
    clearModalErrors();
    addressModal?.show();
}

// Open modal for EDIT — prefill from data attributes
function openEditModal(id, addr1, addr2, city, stateId, stateName, pin, location, isDefault) {
    document.getElementById('modalAddrTitle').textContent = 'Edit Address';
    document.getElementById('modalSaveBtnText').textContent = 'Update Address';
    document.getElementById('modalAddressId').value = id;
    document.getElementById('m-addr1').value = addr1;
    document.getElementById('m-addr2').value = addr2;
    document.getElementById('m-city').value = city;
    document.getElementById('m-pin').value = pin;
    document.getElementById('m-state').value = stateId;
    document.getElementById('m-location').value = location;
    document.getElementById('m-default').checked = isDefault;
    clearModalErrors();
    addressModal?.show();
}

function clearModalErrors() {
    ['merr-addr1', 'merr-city', 'merr-pin', 'merr-state'].forEach(id => {
        document.getElementById(id).style.display = 'none';
    });
}

// Validate + submit modal (Add or Update)
function submitAddressModal() {
    const id = parseInt(document.getElementById('modalAddressId').value);
    const addr1 = document.getElementById('m-addr1').value.trim();
    const addr2 = document.getElementById('m-addr2').value.trim();
    const city = document.getElementById('m-city').value.trim();
    const pin = document.getElementById('m-pin').value.trim();
    const stateEl = document.getElementById('m-state');
    const stateId = parseInt(stateEl.value);
    const stateName = stateEl.options[stateEl.selectedIndex]?.dataset.name || '';
    const location = document.getElementById('m-location').value;
    const isDefault = document.getElementById('m-default').checked;

    // Validate
    let valid = true;
    const showErr = (id) => { document.getElementById(id).style.display = 'block'; valid = false; };
    const hideErr = (id) => { document.getElementById(id).style.display = 'none'; };
    addr1 ? hideErr('merr-addr1') : showErr('merr-addr1');
    city ? hideErr('merr-city') : showErr('merr-city');
    pin.length >= 6 ? hideErr('merr-pin') : showErr('merr-pin');
    stateId > 0 ? hideErr('merr-state') : showErr('merr-state');
    if (!valid) return;

    const btn = document.getElementById('modalSaveBtn');
    btn.innerHTML = '<i class="bi bi-hourglass-split"></i> Saving...';
    btn.disabled = true;

    const endpoint = id === 0 ? '/Account/SaveAddress' : '/Account/UpdateAddress';
    const body = new URLSearchParams({
        addressId: id, address1: addr1, address2: addr2,
        city, stateId, stateName, pinCode: pin, location,
        isDefault: isDefault.toString()
    });

    fetch(endpoint, { method: 'POST', headers: { 'Content-Type': 'application/x-www-form-urlencoded' }, body })
        .then(r => r.json())
        .then(data => {
            if (data.success) {
                addressModal?.hide();
                if (id === 0) appendAddressCard(data.address);
                else updateAddressCard(data.address);
                showToast(id === 0 ? 'Address saved! 📍' : 'Address updated! 📍');
            }
        })
        .catch(() => {
            // Demo fallback
            addressModal?.hide();
            const demoAddr = {
                addressId: Date.now(), address1: addr1, address2: addr2,
                city, stateId, stateName, pinCode: pin, location, isDefault
            };
            if (id === 0) appendAddressCard(demoAddr);
            showToast(id === 0 ? 'Address saved! 📍' : 'Address updated! 📍');
        })
        .finally(() => {
            btn.innerHTML = '<i class="bi bi-check-lg"></i> <span id="modalSaveBtnText">Save Address</span>';
            btn.disabled = false;
        });
}

// Append new card to DOM without page reload
function appendAddressCard(addr) {
    const icon = addr.location === 'Home' ? '🏠' : addr.location === 'Office' ? '🏢' : '📍';
    const html = `
      <div class="addr-profile-card ${addr.isDefault ? 'default' : ''}" id="addrCard-${addr.addressId}">
        <div class="d-flex justify-content-between align-items-start mb-2">
          <div class="d-flex align-items-center gap-2">
            <div class="addr-type-icon">${icon}</div>
            <div>
              <span class="addr-type-label">${addr.location}</span>
              ${addr.isDefault ? '<span class="default-badge">Default</span>' : ''}
            </div>
          </div>
          <div class="addr-action-btns">
            <button class="addr-action-btn" onclick="setDefault(${addr.addressId})" title="Set as default">
              <i class="bi bi-star"></i>
            </button>
            <button class="addr-action-btn" onclick="openEditModal(${addr.addressId},
              '${(addr.address1 || '').replace(/'/g, "\\'")}','${(addr.address2 || '').replace(/'/g, "\\'")}',
              '${addr.city}',${addr.stateId},'${addr.stateName}','${addr.pinCode}','${addr.location}',false)"
              title="Edit"><i class="bi bi-pencil"></i>
            </button>
            <button class="addr-action-btn delete-btn" onclick="deleteAddress(${addr.addressId})" title="Delete">
              <i class="bi bi-trash3"></i>
            </button>
          </div>
        </div>
        <div class="addr-full-line">${addr.address1}</div>
        ${addr.address2 ? `<div class="addr-full-line secondary">${addr.address2}</div>` : ''}
        <div class="addr-city-line">${addr.city}, ${addr.stateName} — ${addr.pinCode}</div>
        <div class="addr-audit">Just added</div>
      </div>`;

    const list = document.getElementById('addressList');
    list.insertAdjacentHTML('beforeend', html);
    document.getElementById('noAddressMsg')?.remove();
}

// Update existing card text in DOM
function updateAddressCard(addr) {
    const card = document.getElementById(`addrCard-${addr.addressId}`);
    if (!card) return;
    card.querySelector('.addr-full-line').textContent = addr.address1;
    card.querySelector('.addr-city-line').textContent =
        `${addr.city}, ${addr.stateName} — ${addr.pinCode}`;
    card.querySelector('.addr-audit').textContent = 'Just updated';
}

/* ── DELETE ADDRESS ──────────────────────────────────────────────────────── */
function deleteAddress(addressId) {
    if (!confirm('Remove this address?')) return;

    fetch('/Account/DeleteAddress', {
        method: 'POST',
        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
        body: new URLSearchParams({ addressId })
    })
        .then(r => r.json())
        .catch(() => ({ success: true })) // demo fallback
        .then(data => {
            if (data.success) {
                const card = document.getElementById(`addrCard-${addressId}`);
                card.style.opacity = '0';
                card.style.transform = 'scale(.95)';
                card.style.transition = 'all .3s';
                setTimeout(() => card.remove(), 300);
                showToast('Address removed');
            }
        });
}

/* ── SET DEFAULT ADDRESS ─────────────────────────────────────────────────── */
function setDefault(addressId) {
    fetch('/Account/SetDefaultAddress', {
        method: 'POST',
        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
        body: new URLSearchParams({ addressId })
    })
        .then(r => r.json())
        .catch(() => ({ success: true }))
        .then(data => {
            if (data.success) {
                // Update all cards in DOM
                document.querySelectorAll('.addr-profile-card').forEach(card => {
                    const id = parseInt(card.id.replace('addrCard-', ''));
                    card.classList.toggle('default', id === addressId);
                    // Update badge
                    const labelDiv = card.querySelector('.addr-type-label').parentElement;
                    labelDiv.querySelector('.default-badge')?.remove();
                    if (id === addressId) {
                        labelDiv.insertAdjacentHTML('beforeend', '<span class="default-badge">Default</span>');
                    }
                    // Update star button
                    const starBtn = card.querySelector('.addr-action-btn:first-child');
                    if (starBtn) {
                        if (id === addressId) {
                            starBtn.innerHTML = '<i class="bi bi-star-fill"></i>';
                            starBtn.classList.add('default-star');
                            starBtn.disabled = true;
                            starBtn.title = 'Default address';
                        } else {
                            starBtn.innerHTML = '<i class="bi bi-star"></i>';
                            starBtn.classList.remove('default-star');
                            starBtn.disabled = false;
                            starBtn.title = 'Set as default';
                        }
                    }
                });
                showToast('Default address updated ⭐');
            }
        });
}

/* ── SIDE NAV ACTIVE STATE ───────────────────────────────────────────────── */
const sections = document.querySelectorAll('[id$="Section"]');
const navItems = document.querySelectorAll('.sidenav-item');
const observer = new IntersectionObserver(entries => {
    entries.forEach(e => {
        if (e.isIntersecting) {
            navItems.forEach(n => n.classList.remove('active'));
            const active = document.querySelector(`.sidenav-item[href="#${e.target.id}"]`);
            if (active) active.classList.add('active');
        }
    });
}, { threshold: 0.5 });
sections.forEach(s => observer.observe(s));