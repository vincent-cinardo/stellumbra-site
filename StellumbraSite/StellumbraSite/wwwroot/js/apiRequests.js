window.apiRequests = {
    // Use the image index for multiple images at the same time.
    addImage: function(base64str, format, imageIndex = 0) {

        if (format != 'png' && format != 'jpg' && format != 'jpeg') {
            throw new Error("Expected a png, jpg, or jpeg.");
        }

        let filepath = `Uploads/Images/img-${Date.now()}-${imageIndex}.${format}`;
        fetch('/api/Image/AddImage', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                base64str: base64str,
                filepath: filepath
            })
        });
        return filepath;
    }
}