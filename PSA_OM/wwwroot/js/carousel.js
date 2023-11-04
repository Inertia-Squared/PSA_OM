let imageIndex = 0;
let spChild = document.createElement('span');
let carouselInterval;


const images = [
    { src: "images/abstract_apartment.png", alt: "Abstract Apartment", caption: "Abstract Apartment - $246.30/Night - 1 Bedroom" },
    { src: "images/adobe_abode.png", alt: "Adobe Abode", caption: "Adobe Abode - $356.30/Night - 2 Bedrooms" },
    { src: "images/black_bungalow.png", alt: "Black Bungalow", caption: "Black Bungalow - $366.80/Night - 3 Bedrooms" },
    { src: "images/quaint_quarters.png", alt: "Quaint Quarters", caption: "Quaint Quarters - $350.80/Night - 2 Bedrooms" },
    { src: "images/sunny_studio.png", alt: "Sunny Studio", caption: "Sunny Studio - $256.30/Night - 1 Bedroom" }
];

document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('caption').innerText = "";
    spChild.textContent = "Abstract Apartment - $246.30 - 1 Bedroom";
    document.getElementById('caption').appendChild(spChild);
}, false);

function updateContent(index) {
    const imageElement = document.getElementById('carouselImage');
    const captionElement = document.getElementById('caption');
    imageElement.src = images[index].src;
    imageElement.alt = images[index].alt;
    captionElement.classList.add('slide-text');
    spChild.textContent = images[imageIndex].caption;
    captionElement.appendChild(spChild);
}

function changeImage(i = 1) {
    imageIndex = (imageIndex + i) % images.length;
    updateContent(imageIndex);
}

function previousImage() {
    changeImage(-1);
    restartAnimation();
}

function nextImage() {
    changeImage();
    restartAnimation();
}

function restartAnimation() {
    updateContent(imageIndex);
    resetAnimation(imageElement);
    resetAnimation(captionElement);
    clearInterval(carouselInterval);
    carouselInterval = setInterval(changeImage, intervalDelay);
}

function resetAnimation(element) {
    element.style.animation = 'none';
    element.offsetHeight;
    element.style.animation = null;
}


carouselInterval = setInterval(changeImage, 3500);