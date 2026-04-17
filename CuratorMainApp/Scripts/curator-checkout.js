/* =============================================
   curator-checkout.js
   wwwroot/js/curator-checkout.js
   Only loaded on checkout pages via @section Scripts
   ============================================= */

/* ---- Cart: Quantity change ---- */
/* Maps: tblOrderDetail.Quantity, tblOrderDetail.CartID */
function changeCartQty(btn, delta) {
    const qtyEl = btn.parentElement.querySelector('.qty-num');
    const current = parseInt(qtyEl.dataset.qty);
    const newQty = Math.max(1, Math.min(10, current + delta));
    qtyEl.dataset.qty = newQty;
    qtyEl.textContent = newQty;
    const cartId = btn.closest('[data-cart-id]')?.dataset.cartId;
    // AJAX: $.post('/Cart/UpdateQty', { cartId, qty: newQty }, recalcTotals);
    showToast('Quantity updated');
}

/* ---- Cart: Remove item ---- */
/* Maps: tblOrderDetail.CartID */
function removeItem(btn) {
    const item = btn.closest('.cart-item');
    const cartId = item.dataset.cartId;
    item.style.opacity = '0';
    item.style.transform = 'translateX(20px)';
    item.style.transition = 'all .3s';
    setTimeout(() => item.remove(), 300);
    // AJAX: $.post('/Cart/Remove', { cartId }, refreshCart);
    showToast('Item removed', 'bi-trash3', '#ff3b30');
}

/* ---- Cart: Coupon ---- */
/* Maps: tblOrder.CouponAmount, tblOrder.PaymentAmount */
function applyCoupon() {
    const code = document.getElementById('couponInput').value.trim().toUpperCase();
    const msgEl = document.getElementById('couponMsg');
    // AJAX: $.post('/Order/ApplyCoupon', { code }, handleResult);
    if (code === 'CURATOR10') {
        msgEl.style.display = 'block';
        msgEl.style.color = 'var(--green)';
        msgEl.textContent = '✓ Coupon applied — 10% off!';
        document.getElementById('couponRow').style.removeProperty('display');
        document.getElementById('couponAmt').textContent = '−$97';
        document.getElementById('totalAmt').textContent = '$873';
        showToast('Coupon applied! You save $97 🎉');
    } else if (!code) {
        msgEl.style.display = 'block'; msgEl.style.color = 'var(--red)';
        msgEl.textContent = 'Please enter a coupon code';
    } else {
        msgEl.style.display = 'block'; msgEl.style.color = 'var(--red)';
        msgEl.textContent = 'Invalid or expired coupon';
    }
}

/* ---- Address: select saved address ---- */
/* Maps: tblAddress.AddressID → tblOrder.DeliveryLocationID */
function selectAddress(card) {
    document.querySelectorAll('.address-card').forEach(c => c.classList.remove('selected'));
    card.classList.add('selected');
    // Session: selectedAddressId = card.dataset.addressId
    showToast('Delivery address selected');
}

/* ---- Address: toggle new address form ---- */
function toggleNewAddressForm() {
    const form = document.getElementById('newAddressForm');
    const btn = document.getElementById('addAddrBtn');
    const show = form.style.display === 'none';
    form.style.display = show ? 'block' : 'none';
    btn.innerHTML = show
        ? '<i class="bi bi-x-circle" style="font-size:1.1rem"></i> Cancel'
        : '<i class="bi bi-plus-circle" style="font-size:1.1rem"></i> Add a new address';
}

/* ---- Address: save new address ---- */
/* Maps ALL tblAddress columns: Address1, Address2, City, StateID, PinCode, Location */
function saveNewAddress() {
    const fields = {
        addr1: document.getElementById('newAddr1').value.trim(),
        city: document.getElementById('newCity').value.trim(),
        pin: document.getElementById('newPin').value.trim(),
        state: document.getElementById('newState').value,
    };
    let valid = true;
    const show = id => { document.getElementById(id).style.display = 'block'; valid = false; };
    const hide = id => { document.getElementById(id).style.display = 'none'; };

    fields.addr1 ? hide('err-addr1') : show('err-addr1');
    fields.city ? hide('err-city') : show('err-city');
    fields.pin.length >= 6 ? hide('err-pin') : show('err-pin');
    fields.state ? hide('err-state') : show('err-state');

    if (!valid) return;
    // AJAX: $.post('/Address/Save', formData, onSaveSuccess);
    showToast('Address saved!');
    toggleNewAddressForm();
}

/* ---- Delivery type selection ---- */
/* Maps: tblOrderType.OrderTypeID, OrderTypeCode, OrderType */
function selectOrderType(chip) {
    document.querySelectorAll('.order-type-chip').forEach(c => c.classList.remove('active'));
    chip.classList.add('active');
    const label = chip.querySelector('.chip-label').textContent;
    const fee = chip.querySelector('.chip-sub:last-child').textContent;
    const reviewEl = document.getElementById('reviewOrderType');
    if (reviewEl) reviewEl.textContent = label + ' Delivery';
    const feeEl = document.getElementById('deliveryFee2');
    if (feeEl) feeEl.textContent = fee;
    // Session: orderTypeId = chip.dataset.typeId
}

/* ---- Proceed to payment (step 4) ---- */
function goToPayment() {
    document.querySelectorAll('.step').forEach((s, i) => {
        s.className = 'step ' + (i < 3 ? 'completed' : 'active');
    });
    showToast('Redirecting to payment...', 'bi-lock-fill', '#0071e3');
    // MVC: window.location.href = '/Order/Payment';
}