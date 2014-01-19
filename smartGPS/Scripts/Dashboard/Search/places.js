$(document).ready(function () {

    var i = 1;
    $("#placesTable tbody td num").each(function () {
        $(this).val(i);
        i++;
    });

    //var numberOfPages = Math.ceil(data.Results.length / 10);
    //var paginatorOptions = {
    //    currentPage: 1,
    //    totalPages: numberOfPages,
    //    onPageChanged: function (e, oldPage, newPage) {
    //        refreshList(newPage);
    //    }
    //}

    //$('#paginatorPlaces').bootstrapPaginator(paginatorOptions);


    //function refreshList(currentPage)
    //{
    //    var start, end;
    //    if (currentPage == 1) {
    //        start = 0;
    //    }
    //    else {
    //        start = 10 * (currentPage - 1);
    //    }

    //    end = start + 10;
    //}
});