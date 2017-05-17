app.booksService = {
    getAll: function() {
        return $.get("/Books/Books/GetAll", function(data) {
        });
    },
    delete: function(id) {
        return jQuery.ajax({
                'type': 'POST',
                'url': "/Books/Books/Delete",
                'contentType': 'application/json',
                'data': JSON.stringify({ "id": id })
            }
        );
    }
};