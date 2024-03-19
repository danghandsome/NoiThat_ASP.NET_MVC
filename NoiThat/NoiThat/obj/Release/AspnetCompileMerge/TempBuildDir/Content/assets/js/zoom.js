let zoom = document.querySelector('.zoom');
let imgZoom = document.getElementById('imgZoom');
imgZoom.style.opacity = 0;
zoom.addEventListener('mousemove', (e) => {
	imgZoom.style.opacity = 1;
	let positionPx = e.x - zoom.getBoundingClientRect().left;
	let positionX = (positionPx / zoom.offsetWidth) * 100;

	let positionPy = e.y - zoom.getBoundingClientRect().top;
	let positionY = (positionPy / zoom.offsetHeight) * 100;

	imgZoom.style.setProperty('--zoom-x', positionX + '%');
	imgZoom.style.setProperty('--zoom-y', positionY + '%');
})
zoom.addEventListener('mouseout', (e) => {
	imgZoom.style.opacity = 0;
})