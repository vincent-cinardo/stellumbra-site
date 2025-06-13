window.quillEditor = {
    instance: null,
    options: {
        modules: {
            toolbar : [
                ['bold', 'italic', 'underline'],
                [{ 'list': 'ordered' }, { 'list': 'bullet' }],
                ['link', 'image']
            ],
        },
        theme: 'snow'
    },
    initialize: function (elementId) {
        this.instance = new Quill('#' + elementId, this.options);
    },
    getContent: function () {
        return this.instance.root.innerHTML;
    },
    setContent: function (html) {
        this.instance.root.innerHTML = html;
    }
};