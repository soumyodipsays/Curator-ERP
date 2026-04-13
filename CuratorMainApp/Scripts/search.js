// Scripts/search.js
// Curator. — Search Overlay

$(function () {

    /* =============================================
       CONFIG — replace with AJAX calls to your
       SearchController once backend is ready
       ============================================= */
    var recentSearches = ['Wool overcoat', 'Leather tote bag', 'Silk dress'];

    var trendingItems = [
        'Minimal outerwear', 'Relaxed trousers', 'Essentials tee',
        'Summer linen', 'Platform footwear', 'Monochrome sets'
    ];

    var popularCategories = [
        { name: 'Outerwear', icon: 'bi-scissors', slug: 'outerwear' },
        { name: 'Accessories', icon: 'bi-gem', slug: 'accessories' },
        { name: 'Footwear', icon: 'bi-award', slug: 'footwear' },
        { name: 'Dresses', icon: 'bi-bag-heart', slug: 'dresses' }
    ];

    /* =============================================
       OPEN / CLOSE
       ============================================= */
    $(document).on('click', '#nav-search-btn', openSearch);
    $(document).on('click', '#search-cancel-btn, #search-backdrop', closeSearch);

    $(document).on('keydown', function (e) {
        if (e.key === 'Escape') closeSearch();
    });

    function openSearch() {
        renderRecent();
        renderTrending();
        renderCategories();
        $('#search-overlay, #search-backdrop').fadeIn(180);
        setTimeout(function () { $('#search-input').focus(); }, 60);
        $('body').css('overflow', 'hidden');
    }

    function closeSearch() {
        $('#search-overlay, #search-backdrop').fadeOut(160);
        $('body').css('overflow', '');
        clearInput();
    }

    /* =============================================
       INPUT HANDLING
       ============================================= */
    var searchTimer;

    $('#search-input').on('input', function () {
        var val = $(this).val().trim();
        $('#clear-btn').toggle(val.length > 0);

        clearTimeout(searchTimer);

        if (!val) {
            showDefault();
            return;
        }

        // Debounce — 280ms before firing AJAX
        searchTimer = setTimeout(function () {
            fetchSuggestions(val);
        }, 280);
    });

    $('#search-input').on('keydown', function (e) {
        if (e.key === 'Enter') {
            var val = $(this).val().trim();
            if (val) goToResults(val);
        }
    });

    $('#clear-btn').on('click', function () {
        clearInput();
        $('#search-input').focus();
    });

    function clearInput() {
        $('#search-input').val('');
        $('#clear-btn').hide();
        showDefault();
    }

    /* =============================================
       VIEWS: default vs results
       ============================================= */
    function showDefault() {
        $('#search-default').show();
        $('#search-results').hide();
    }

    function showResults() {
        $('#search-default').hide();
        $('#search-results').show();
    }

    /* =============================================
       RENDER — Recent
       ============================================= */
    function renderRecent() {
        var $list = $('#recent-list').empty();

        if (!recentSearches.length) {
            $list.append('<li class="text-secondary" style="font-size:0.83rem">No recent searches</li>');
            return;
        }

        $.each(recentSearches, function (i, term) {
            var $li = $('<li>')
                .addClass('d-flex align-items-center gap-2 py-2')
                .css({ cursor: 'pointer', borderBottom: '1px solid #f5f5f7' });

            $li.html(
                '<i class="bi bi-clock-history text-secondary" style="font-size:12px;flex-shrink:0"></i>' +
                '<span class="flex-grow-1" style="font-size:0.88rem;color:#1d1d1f">' + $('<span>').text(term).html() + '</span>' +
                '<i class="bi bi-x remove-recent" style="font-size:13px;color:#c7c7cc;cursor:pointer" data-term="' + $('<span>').text(term).html() + '"></i>'
            );

            $li.on('click', function (e) {
                if (!$(e.target).hasClass('remove-recent')) fillSearch(term);
            });

            $list.append($li);
        });
    }

    $(document).on('click', '.remove-recent', function (e) {
        e.stopPropagation();
        var term = $(this).data('term');
        recentSearches = recentSearches.filter(function (t) { return t !== term; });
        renderRecent();
    });

    $('#clear-recent-btn').on('click', function () {
        recentSearches = [];
        renderRecent();
    });

    /* =============================================
       RENDER — Trending pills
       ============================================= */
    function renderTrending() {
        var $wrap = $('#trending-pills').empty();
        $.each(trendingItems, function (i, t) {
            $('<span>')
                .text(t)
                .addClass('badge rounded-pill bg-light text-dark border')
                .css({ cursor: 'pointer', fontSize: '0.8rem', fontWeight: '400', padding: '6px 14px' })
                .on('click', function () { fillSearch(t); })
                .appendTo($wrap);
        });
    }

    /* =============================================
       RENDER — Popular categories
       ============================================= */
    function renderCategories() {
        var $list = $('#category-list').empty();
        $.each(popularCategories, function (i, cat) {
            var $li = $('<li>')
                .addClass('d-flex align-items-center gap-2 py-2')
                .css({ cursor: 'pointer', borderBottom: '1px solid #f5f5f7' })
                .html(
                    '<i class="bi ' + cat.icon + '" style="color:#0071e3;font-size:13px;flex-shrink:0"></i>' +
                    '<span style="font-size:0.88rem;color:#1d1d1f;flex:1">' + cat.name + '</span>' +
                    '<i class="bi bi-arrow-right" style="color:#c7c7cc;font-size:11px"></i>'
                )
                .on('click', function () {
                    window.location.href = '/Products?category=' + cat.slug;
                });
            $list.append($li);
        });
    }

    /* =============================================
       AJAX — Fetch suggestions & quick products
       ============================================= */
    function fetchSuggestions(query) {
        showResults();

        // --- SUGGESTIONS ---
        $.get('/Search/Suggestions', { q: query }, function (data) {
            // data = { suggestions: ["...","..."], products: [{id,name,price,imageUrl},...] }
            renderSuggestions(query, data.suggestions);
            renderQuickResults(data.products);
        }).fail(function () {
            // Fallback if endpoint not ready yet
            renderSuggestions(query, ['Search for "' + query + '"']);
            renderQuickResults([]);
        });
    }

    /* =============================================
       RENDER — Suggestions list
       ============================================= */
    function renderSuggestions(query, items) {
        var $list = $('#suggestions-list').empty();
        $.each(items, function (i, s) {
            // Bold the matching prefix
            var display = s;
            if (s.toLowerCase().indexOf(query.toLowerCase()) === 0) {
                display = '<strong>' + $('<span>').text(s.substring(0, query.length)).html() + '</strong>'
                    + $('<span>').text(s.substring(query.length)).html();
            } else {
                display = $('<span>').text(s).html();
            }

            var $li = $('<li>')
                .addClass('d-flex align-items-center gap-2 py-2')
                .css({ cursor: 'pointer', borderBottom: '1px solid #f5f5f7' })
                .html(
                    '<i class="bi bi-search text-secondary" style="font-size:12px;flex-shrink:0"></i>' +
                    '<span style="font-size:0.88rem;color:#1d1d1f;flex:1">' + display + '</span>' +
                    '<i class="bi bi-arrow-up-left" style="color:#c7c7cc;font-size:11px"></i>'
                )
                .on('click', function () { fillSearch(s); });

            $list.append($li);
        });
    }

    /* =============================================
       RENDER — Quick product results
       ============================================= */
    function renderQuickResults(products) {
        var $wrap = $('#quick-results').empty();

        if (!products || !products.length) {
            $wrap.html('<p class="text-secondary" style="font-size:0.83rem">No quick results</p>');
            return;
        }

        $.each(products, function (i, p) {
            var $row = $('<div>')
                .addClass('d-flex align-items-center gap-3 py-2')
                .css({ cursor: 'pointer', borderBottom: '1px solid #f5f5f7' })
                .html(
                    '<img src="' + p.imageUrl + '" class="rounded-2 flex-shrink-0"' +
                    ' style="width:52px;height:52px;object-fit:cover;background:#f5f5f7">' +
                    '<div class="flex-grow-1 overflow-hidden">' +
                    '<div class="fw-medium text-truncate" style="font-size:0.85rem">' + $('<span>').text(p.name).html() + '</div>' +
                    '<div class="text-secondary" style="font-size:0.78rem;margin-top:2px">' + $('<span>').text(p.price).html() + '</div>' +
                    '</div>' +
                    '<i class="bi bi-arrow-right text-secondary flex-shrink-0" style="font-size:12px"></i>'
                )
                .on('click', function () {
                    window.location.href = '/Products/Detail/' + p.id;
                });

            $wrap.append($row);
        });

        // "View all results" link
        var query = $('#search-input').val().trim();
        $wrap.append(
            $('<a>')
                .attr('href', '/Search/Results?q=' + encodeURIComponent(query))
                .addClass('d-inline-flex align-items-center gap-1 mt-2')
                .css({ color: '#0071e3', fontSize: '0.82rem', textDecoration: 'none' })
                .html('View all results <i class="bi bi-arrow-right" style="font-size:11px"></i>')
        );
    }

    /* =============================================
       HELPERS
       ============================================= */
    function fillSearch(term) {
        $('#search-input').val(term).trigger('input').focus();
        // Save to recent
        recentSearches = recentSearches.filter(function (t) { return t !== term; });
        recentSearches.unshift(term);
        if (recentSearches.length > 5) recentSearches.pop();
    }

    function goToResults(query) {
        // Save to recent before navigating
        recentSearches = recentSearches.filter(function (t) { return t !== query; });
        recentSearches.unshift(query);
        window.location.href = '/Search/Results?q=' + encodeURIComponent(query);
    }

});