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
    onSubmit: function () {
        if (!this.instance) {
            console.warn("Quill instance not initialized.");
            return [];
        }

        let imageIndex = 0;

        const rootClone = this.instance.root.cloneNode(true);

        // Find all images in the cloned root
        const imgs = rootClone.querySelectorAll('img');

        imgs.forEach(img => {
            const base64str = img.getAttribute('src');
            let format = null;

            if (base64str.startsWith('data:')) {
                const match = base64str.match(/^data:image\/(.*?);base64,/);
                if (match && match[1]) {
                    format = match[1];
                }
            }

            let filepath = window.apiRequests.addImage(base64str, format, imageIndex);
            imageIndex++;

            img.removeAttribute('src');
            img.setAttribute('data-newsrc', `/${filepath}`);

        });

        return rootClone.innerHTML.replace(/data-newsrc="(.*?)"/g, 'src="$1"');
    },
    setContent: function (html) {
        if (!this.instance) {
            console.warn("Quill instance not initialized.");
            return [];
        }
        this.instance.root.innerHTML = html;
    }
};