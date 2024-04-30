document.addEventListener('DOMContentLoaded', (event) => {
    document.querySelector('.custom-file-input').addEventListener('change', function (e) {
        var fileName = document.getElementById("publisherLogoInput").files[0].name;
        var nextSibling = e.target.nextElementSibling
        nextSibling.innerText = fileName
    });

    document.querySelector('.custom-file-input').addEventListener('change', function (e) {
        var fileName = document.getElementById("publisherLogoInput").files[0].name;
        var nextSibling = e.target.nextElementSibling;
        nextSibling.innerText = fileName;

        // Image preview
        var reader = new FileReader();
        reader.onload = function (e) {
            document.getElementById('imagePreview').style.display = 'block';
            document.getElementById('imagePreview').src = e.target.result;
        }
        reader.readAsDataURL(this.files[0]);
    });
});
