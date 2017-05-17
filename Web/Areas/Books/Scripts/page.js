app.booksController = {

    init: function () {

        var that = this;

        var sortColumn = sessionStorage ? sessionStorage.getItem("sortColumn") || 0 : 0;
        var sortOrder = sessionStorage ? sessionStorage.getItem("sortOrder") || "asc" : "asc";

        this.booksList = $(".books-list");
        this.booksList.dataTable(
        {
            "oLanguage":
            {
                "sEmptyTable": "Нет данных для отображения",
                "sZeroRecords": "Нет данных для отображения"
            },
            "aaSorting": [[sortColumn, sortOrder]],
            "bDestroy": true,
            "bProcessing": false,
            "bInfo": false,
            "bFilter": false,
            "bPaginate": false,
            "aoColumns":
            [
                {
                    "sTitle": "Название",
                    "sWidth": "25%",
                    "bSortable": true,
                    "mData": "Title"
                },
                {
                    "sTitle": "Год публикации",
                    "bSortable": true,
                    "sWidth": "25%",
                    "mData": "PublicationYear",
                    "mRender": function (item, type, full) {
                        if ('display' === type) {
                            return item === 0 ? "" : item;
                        }
                        return item;
                    }
                },
                {
                    "sTitle": "ISBN",
                    "bSortable": false,
                    "sWidth": "25%",
                    "mData": "Isbn"
                },
                {
                    "sTitle": null,
                    "bSortable": false,
                    "sWidth": "25%",
                    "mData": null,
                    "mRender": function () {
                        return "<a href=\"javascript:void(0)\" style=\"margin-left: 5px; ;float : right;\" class=\"delete-btn\">Удалить<\/a>" +
                            "<a href=\"javascript:void(0)\" style=\"float : right;\" class=\"edit-btn\">Редактировать<\/a>";
                    }
                }
            ],
            "fnCreatedRow": function (nRow, data) {
                $(nRow).find(".edit-btn").data("book", data);
                $(nRow).find(".delete-btn").data("book", data);
            },
            "fnDrawCallback": function () {
                var sorting = that.booksList.dataTable().fnSettings().aaSorting[0];
                if (sessionStorage) {
                    sessionStorage.setItem('sortColumn', sorting[0]);
                    sessionStorage.setItem('sortOrder', sorting[1]);
                }
            }
        });

        this.modal = $("#book-modal").modal({ show: false });
        this.addBookBtn = $(".book-add").on("click", $.proxy(this.onAddBtnClick, this));
        this.bookArea = $(".book-area").bookControl()
            .on("save", $.proxy(this.onSave, this));

        this.modal.on('hide.bs.modal', function () {
            that.bookArea.bookControl("clear");
        });

        $(document).on("click", ".delete-btn", $.proxy(this.onDeleteBtnClick, this));
        $(document).on("click", ".edit-btn", $.proxy(this.onEditBtnClick, this));

        app.booksService.getAll().done($.proxy(this.onBooksLoaded, this));
    },

    onSave: function (e, book) {
        this.modal.modal("hide");
        var index = this.findBook(book);
        if (index >= 0) {
            this.booksList.fnUpdate(book, index);
        }
        else {
            this.booksList.fnAddData(book);
        }
    },

    findBook: function (book) {
        var that = this;
        var resultIndex = -1;
        this.booksList.find("tbody tr").each(function (index) {
            var currentBook = that.booksList.fnGetData(index);
            if (currentBook && currentBook.Id === book.Id) {
                resultIndex = index;
            }
        });
        return resultIndex;
    },

    onAddBtnClick: function () {
        this.bookArea.bookControl("new");
        this.modal.modal("show");
    },

    onEditBtnClick: function (e) {
        var target = $(e.target);
        var tr = target.closest("tr");
        var book = this.booksList.fnGetData(tr[0]);
        this.bookArea.bookControl("edit", book);
        this.modal.modal("show");
    },

    onDeleteBtnClick: function (e) {
        if (confirm("Удалить книгу?")) {
            var that = this;
            var target = $(e.target);
            var tr = target.closest("tr");
            var book = this.booksList.fnGetData(tr[0]);
            app.booksService.delete(book.Id).done(function () {
                that.booksList.fnDeleteRow(tr[0]);
            });
        }
    },

    onBooksLoaded: function (data) {
        var dataTable = this.booksList.dataTable();
        dataTable.fnAddData(data);
    }
};
