window.quill = {
    instance: null,
    options: {
        modules: {
            toolbar : "#toolbar",
        },
        theme: 'snow'
    },
    initialize: function (elementId) {
        this.instance = new Quill('#' + elementId, this.options);

        const quill = window.quill.instance;

        document.getElementById('boldBtn').onclick = () => quill.format('bold', true);
        document.getElementById('italicBtn').onclick = () => quill.format('italic', true);
        document.getElementById('underlineBtn').onclick = () => quill.format('underline', true);

        document.getElementById('orderedBtn').onclick = () => quill.format('list', 'ordered');
        document.getElementById('bulletBtn').onclick = () => quill.format('list', 'bullet');

        document.getElementById('linkBtn').onclick = () => {
            const range = quill.getSelection();
            if (!range || range.length == 0) {
                alert("Highlight a url first.")
                return;
            }
            const selectedText = quill.getText(range.index, range.length).trim();
            let url = selectedText;
            if (!/^https?:\/\//i.test(url)) {
                url = 'http://' + url;
            }

            // Apply the link format with the selected text as href
            quill.format('link', url);
        };

        document.getElementById('imageBtn').addEventListener('click', () => {
            document.getElementById('imageInput').click();
        });

        document.getElementById('imageInput').addEventListener('change', function () {
            const file = this.files[0];
            if (!file) return;

            const reader = new FileReader();
            reader.onload = function (e) {
                const range = quill.getSelection(true);
                quill.insertEmbed(range.index, 'image', e.target.result, Quill.sources.USER);
            };
            reader.readAsDataURL(file);
        });
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