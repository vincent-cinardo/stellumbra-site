window.quillEditor = {
    instance: null,

    initialize: function (elementId) {
        this.instance = new Quill('#' + elementId, {
            theme: 'snow'
        });
    },

    getContent: function () {
        return this.instance.root.innerHTML;
    },

    setContent: function (html) {
        this.instance.root.innerHTML = html;
    }
};