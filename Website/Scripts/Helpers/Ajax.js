function afterLoadMore(loadedContent, loadMoreContainerId, loadMoreLinkId) {
    var nextPage = $(loadedContent).last('tr').data('next-page');

    if (nextPage == '') {
        $('#' + loadMoreContainerId).hide();
    } else {
        var loadMoreLink = $('#' + loadMoreLinkId);
        loadMoreLink.attr('href', loadMoreLink.attr('href').replace(/Page=\d+/, 'Page=' + nextPage));
    }
}