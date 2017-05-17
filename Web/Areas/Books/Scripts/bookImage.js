(function ($) {

    $.widget("app.bookImage",
    {
        options:
        {
            size: "150px",
            color: "lightgray"
        },

        destroy: function () {
            this.photoImage.off(".bookImage");
            this.element.empty();
        },

        _create: function () {

            var that = this;

            var template = '<span class="fa fa-book"></span>' +
                           '<div style="display: none;">' +
                           '  <img alt=""/><br/>' +
                           '  <input type="hidden" name="ImageChanged" />' +
                           '  <input style="display: none;" type="file" name="Image" />' +
                           '  <a href=\"javascript:void(0)\">Удалить</a>' +
                           '<\/div>';

            this.element.html(template);

            this.photoPlaceholder = this.element.find("span").css({"font-size": this.options.size,"color": this.options.color});

            this.photoImageDiv = this.element.find("div");

            this.photoImage = this.element.find("img").css({"height": this.options.size});

            this.removeBtn = this.element.find("a").on("click", $.proxy(this.onRemoveBtnClick, this));

            this.imageField = this.element.find("input[name='Image']")
                                          .on("change", $.proxy(this.onImageChanged, this))
                                          .on("click", $.proxy(this.onImageFieldClick, this));

            this.imageChangedField = this.element.find("input[name='ImageChanged']");

            this.element.on("click", $.proxy(this.onImageClick, this));

            this.photoImage.on("load.employeePhoto", function () {
                that.photoImageDiv.show();
                that.photoPlaceholder.hide();
            }).on("error.employeePhoto", function () {
                that.photoImageDiv.hide();
                that.photoPlaceholder.show();
            });
        },

        onImageFieldClick: function(e)
        {
            e.stopPropagation();
        },

        onImageClick: function (e) {
            this.imageField.trigger("click");
        },

        onImageChanged: function () {
            var that = this;
            var reader = new FileReader();
            reader.onload = function (e) {
                that._render(e.target.result);
            };
            reader.readAsDataURL(this.imageField[0].files[0]);
            this.imageChangedField.val("true");
        },


        onRemoveBtnClick: function(e) {
            e.stopPropagation();
            this._render("");
            this.imageChangedField.val("true");
        },

        render: function (url) {
            this.imageField.val("");
            this.imageChangedField.val("false");
            this._render(url);
        },

        _render: function(url) {
            this.photoImage.attr("src", url);
        }
    });
})(jQuery);
