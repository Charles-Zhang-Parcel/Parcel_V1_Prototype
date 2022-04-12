loadJs('https://unpkg.com/treemap-squarify@1.0.1/lib/bundle.min.js', () => {
    var defaultWidth = 1024;
    var defaultHeight = 768;
    var canvasSettings = {
        width: defaultWidth,
        height: defaultHeight
    }

    function randomColor() {
        return Math.floor(Math.random()*16777215).toString(16);
    }

    function drawCanvas(id = 'canvas', settings = canvasSettings, hierachy = result){
        const canvas = document.getElementById(id);
        const ctx = canvas.getContext('2d');

        canvas.width = settings.width;
        canvas.height = settings.height;

        ctx.fillStyle = 'black';
        ctx.fillRect(0, 0, settings.width, settings.height);

        hierachy.forEach(element => {
            ctx.fillStyle = `#${randomColor()}`;
            ctx.fillRect(element.x, element.y, element.width, element.height);
        });
    }

    function computeTreeMap(settings = canvasSettings) {
        const result = Treemap.getTreemap({
            data: [ // your dataset
                { value: 10 },
                { value: 7 },
                { value: 4 },
                { value: 1 },
                { value: 5 },
                { value: 9 },
            ],
            width: settings.width, // the width and height of your treemap
            height: settings.height,
        });
        return result;
    }

    function reCompute() {
        canvasSettings.width = window.innerWidth * 0.8;
        canvasSettings.height = window.innerHeight * 0.8;

        const result = computeTreeMap(canvasSettings);
        drawCanvas('canvas', canvasSettings, result);
    }

    reCompute();

    window.addEventListener('resize', function(event) {
        reCompute();
    }, true);  
})