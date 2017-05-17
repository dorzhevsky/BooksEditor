/*global jQuery, gSecurity, Helper*/
(function ($) {

    "use strict";

    //Виджет
    $.widget("app.bookControl", {

        //Опции
        options: {
            data: null
        }, 

        // Начало метода _create
        // Назначение            : Конструктор
        // Аргументы             : нет
        // Возвращаемое значение : нет     
        _create: function () {

            this.form = this.element.find("form");
            this.idHiddenField = this.element.find("input[name='Id']");
            this.titleTextBox = this.element.find("input[name='Title']");
            this.authorsList = this.element.find(".book-authors");
            this.publicationYearTextBox = this.element.find("input[name='PublicationYear']");
            this.pagesTextBox = this.element.find("input[name='Pages']");
            this.publicationTextBox = this.element.find("input[name='Publication']");
            this.isbnTextBox = this.element.find("input[name='Isbn']");
            this.bookImage = this.element.find(".book-image").bookImage();
            this.saveBtn = this.element.find(".save-btn").on("click", $.proxy(this.onSaveBtnClick, this));

            this.initForm();
        },
        // Конец метода _create

        clear: function () {
            this.validator.resetForm();

            this.titleTextBox.val("");
            this.publicationYearTextBox.val("");
            this.pagesTextBox.val("");
            this.publicationTextBox.val("");
            this.isbnTextBox.val("");
            this.idHiddenField.val("");

            this.authorsList.find(".list-item").remove();
            this.bookImage.bookImage("render", "");
        },

        new: function() {
            this.clear();
            this.addAuthorItem();
            this.bookImage.bookImage("render", "");
            this.authorsList.dynamiclist({ addCallbackFn: $.proxy(this.onAddAuthor, this) });
        },

        edit: function(book) {
            var that = this;
            this.clear();
            var data = book;
            if (data) {
                this.titleTextBox.val(data.Title);
                this.publicationYearTextBox.val(data.PublicationYear || "");
                this.pagesTextBox.val(data.Pages);
                this.publicationTextBox.val(data.Publication);
                this.isbnTextBox.val(data.Isbn);
                this.idHiddenField.val(data.Id);
                this.bookImage.bookImage("render", "/Books/Books/GetImage?id=" + data.Id);
                if (data.Authors && data.Authors.length > 0) {
                    _.each(data.Authors.reverse(), function (author, index) {
                        that.addAuthorItem(author);
                    });
                } else {
                    this.addAuthorItem();
                }
            }
            this.authorsList.dynamiclist({ addCallbackFn: $.proxy(this.onAddAuthor, this) });
        },

        onAddAuthor: function (item) {
            if (item.length > 0) {
                this.onAddAuthorItem(item);
            }
        },

        addAuthorItem: function (author) {

            var markup = "";
            markup += "<div class=\"list-item\">";
            markup += "  <div>";
            markup += "    <div style=\"width: 40%; display: inline-block;\">";
            markup += "      <input class=\"col-sm-5 form-control inline valid book-author-surname\" placeholder=\"Фамилия\" type=\"text\" \/>";
            markup += "    <\/div>";
            markup += "    <div style=\"width: 40%; display: inline-block\">";
            markup += "      <input class=\"col-sm-5 form-control inline valid book-author-name\" placeholder=\"Имя\" type=\"text\" \/>";
            markup += "    <\/div>";
            markup += "    <a href=\"javascript:void(0)\" class=\"list-remove\" style=\"float: right\" href=\"#\" >Удалить<\/a>";
            markup += "  <\/div>";
            markup += "<\/div>";

            var $item = $(markup);
            this.authorsList.prepend($item);

            this.onAddAuthorItem($item, author);
        },

        onAddAuthorItem: function ($item, author) {

            var time = new Date().getTime();

            var nameTextBox = $item.find(".book-author-name").attr("name", "AuthorName" + time);
            var surnameTextBox = $item.find(".book-author-surname").attr("name", "AuthorSurname" + time);

            if (author) {
                surnameTextBox.val(author.Surname);
                nameTextBox.val(author.Name);
            }

            nameTextBox.rules("add", {
                required: true,
                maxlength: 30,
                messages: {
                    required: "Введите имя автора",
                    maxlength: "Имя автора не должно превышать {0} символов"
                }
            });

            surnameTextBox.rules("add", {
                required: true,
                maxlength: 30,
                messages: {
                    required: "Введите фамилию автора",
                    maxlength: "Фамилия автора не должно превышать {0} символов"
                }
            });
        },

        initForm: function () {
            this.validator = this.element.find("form").validate({
                focusInvalid: true,
                onkeyup: false,
                onblur: false,
                rules: {
                    Title: {
                        required: true,
                        maxlength: 30
                    },
                    Pages: {
                            required: true,
                            number: true,
                            min: 0,
                            max: 10000
                    },
                    Publication: {
                        maxlength: 30
                    },
                    PublicationYear: {
                        number: true,
                        min: 1800
                    },
                    Isbn: {
                        isbn: true
                    },
                },
                messages: {
                    Title: {
                        required: "Введите название книги",
                        maxlength: "Название не должно превышать {0} символов"
                    },
                    Pages: {
                        required: "Введите количество страниц",
                        number: "Количество страниц должно быть числом",
                        min: "Количество страниц должно быть не меньше {0}",
                        max: "Количество страниц должно быть не больше {0}"
                    },
                    Publication: {
                        maxlength: "Название издательства не должно превышать {0} символов"
                    },
                    PublicationYear: {
                        number: "Год публикации должно быть числом",
                        min: "Год публикации должно быть не меньше {0}",
                    },
                    Isbn: {
                        isbn: "Не соответствует ISBN формату"
                    },
                }
            });
        },

        onImageClick: function() {
            this.imageField.trigger("click");
        },

        onImageChanged: function () {
            var that = this;
            var reader = new FileReader();
            reader.onload = function (e) {
                that.bookImage.bookImage("render", e.target.result);
            };
            reader.readAsDataURL(this.imageField[0].files[0]);
        },

        onSaveBtnClick: function () {
            var that = this;
            if (this.form.valid()) {
                this.adjustFormData();
                var formData = new FormData(this.form[0]);
                $.ajax({
                    url: '/Books/Books/Save',
                    type: 'POST',
                    data: formData,
                    contentType: false,
                    processData: false
                }).done(function (book) {
                    that.element.trigger("save", book);
                }).fail(function(xhr, status, err) {
                    if (xhr.status == 400)
                    {
                        var mes = Messenger().post({
                                message: xhr.responseText,
                                type: "error",
                                showCloseButton: true
                        });
                        setTimeout(function () {
                            mes.hide();
                        }, 5000);
                    }                    
                });
            }
        },

        adjustFormData: function() {
            this.authorsList.find(".list-item").each(function (index) {
                var $this = $(this);
                $this.find(".book-author-surname").attr("name", "Authors[" + index + "].Surname");
                $this.find(".book-author-name").attr("name", "Authors[" + index + "].Name");
            });
        }
    });
}(jQuery));