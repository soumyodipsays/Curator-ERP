// Scripts/curator.js
// Curator. — Site-wide JS

(function () {
    'use strict';

    /* =============================================
       1. COUNTDOWN TIMER
       Reads data-end-time ISO string from .countdown-pill
       ============================================= */
    var countdownPill = document.querySelector('.countdown-pill[data-end-time]');
    if (countdownPill) {
        var endTime = new Date(countdownPill.getAttribute('data-end-time'));

        function updateCountdown() {
            var diff = endTime - Date.now();
            if (diff <= 0) {
                clearInterval(timer);
                return;
            }
            document.getElementById('cd-hours').textContent =
                String(Math.floor(diff / 3600000)).padStart(2, '0');
            document.getElementById('cd-mins').textContent =
                String(Math.floor((diff % 3600000) / 60000)).padStart(2, '0');
            document.getElementById('cd-secs').textContent =
                String(Math.floor((diff % 60000) / 1000)).padStart(2, '0');
        }

        updateCountdown();
        var timer = setInterval(updateCountdown, 1000);
    }

    /* =============================================
       2. SCROLL FADE-UP ANIMATION
       ============================================= */
    var fadeEls = document.querySelectorAll('.fade-up');
    if (fadeEls.length && 'IntersectionObserver' in window) {
        var observer = new IntersectionObserver(function (entries) {
            entries.forEach(function (entry, i) {
                if (entry.isIntersecting) {
                    setTimeout(function () {
                        entry.target.classList.add('visible');
                    }, i * 60);
                    observer.unobserve(entry.target);
                }
            });
        }, { threshold: 0.12 });

        fadeEls.forEach(function (el) { observer.observe(el); });
    } else {
        // Fallback — show all immediately
        fadeEls.forEach(function (el) { el.classList.add('visible'); });
    }

    /* =============================================
       3. WISHLIST TOGGLE
       ============================================= */
    document.querySelectorAll('.card-wish-btn').forEach(function (btn) {
        btn.addEventListener('click', function (e) {
            e.stopPropagation();
            var icon = btn.querySelector('i');
            var isFilled = icon.classList.contains('bi-heart-fill');

            icon.classList.toggle('bi-heart', isFilled);
            icon.classList.toggle('bi-heart-fill', !isFilled);
            btn.style.color = !isFilled ? '#e53935' : '';

            // Optional: POST to server
            var productId = btn.getAttribute('data-product-id');
            if (productId) {
                fetch('/Wishlist/Toggle', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ productId: parseInt(productId, 10) })
                }).catch(function () { /* silent fail */ });
            }
        });
    });

    /* =============================================
       4. CATEGORY PILL ACTIVE STATE
       ============================================= */
    document.querySelectorAll('.cat-pill').forEach(function (pill) {
        pill.addEventListener('click', function () {
            document.querySelectorAll('.cat-pill').forEach(function (p) {
                p.classList.remove('active');
            });
            pill.classList.add('active');

            // Optional: filter products via AJAX
            var slug = pill.getAttribute('data-category');
            if (slug) {
                console.log('Filter by category:', slug);
                // fetch('/Products/ByCategory?slug=' + slug) ...
            }
        });
    });

    /* =============================================
       5. ADD TO BAG — quick feedback
       ============================================= */
    document.querySelectorAll('.quick-add-btn').forEach(function (btn) {
        btn.addEventListener('click', function () {
            var productId = btn.getAttribute('data-product-id');
            var original = btn.textContent;

            btn.textContent = 'Added ✓';
            btn.style.background = '#34c759';   // green flash

            setTimeout(function () {
                btn.textContent = original;
                btn.style.background = '';
            }, 1500);

            if (productId) {
                fetch('/Cart/Add', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({ productId: parseInt(productId, 10), quantity: 1 })
                }).catch(function () { /* silent fail */ });
            }
        });
    });

})();