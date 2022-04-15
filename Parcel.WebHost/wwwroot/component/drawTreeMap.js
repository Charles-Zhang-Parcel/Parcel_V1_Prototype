function aggregate(data, keyFields, accumulator) {
    var createNewObj = (ref, fields) => {
        return fields.reduce((result, key) => {
            return Object.assign(result, { [key] : ref[key] });
        }, {});
    }
    return Object.values(data.reduce((result, object, index, ref) => {
        let key = keyFields.map(key => object[key]).join('');
        let val = result[key] || createNewObj(object, keyFields);
        val[accumulator] = (val[accumulator] || 0) + object[accumulator];
        return Object.assign(result, { [key] : val });
    }, {}));
}

function randomColor() {
    return Math.floor(Math.random()*16777215).toString(16);
}

function drawCanvas(canvasSettings){
    const aggregateLevel1 = aggregate(canvasSettings.dataSource, ['outer'], 'value')
    
    const canvas = document.getElementById(canvasSettings.id);
    const ctx = canvas.getContext('2d');
    canvas.width = canvasSettings.width;
    canvas.height = canvasSettings.height;

    ctx.fillStyle = 'black';
    ctx.fillRect(0, 0, canvasSettings.width, canvasSettings.height);
    
    computeTreeMap(aggregateLevel1, canvasSettings.width, canvasSettings.height).forEach(element => {
        ctx.fillStyle = `#${randomColor()}`;
        ctx.fillRect(element.x, element.y, element.width, element.height);
    });
}

function computeTreeMap(sourceData, width, height) {
    return Treemap.getTreemap({
        data: sourceData,
        width: width, // the width and height of your treemap
        height: height,
    });
}

loadJs('https://unpkg.com/treemap-squarify@1.0.1/lib/bundle.min.js', () => {
    fetch(`/DataTable?tableName=TreeMap`)
        .then( request => request.text() )
        .then( csv => {
            const dataTable = Papa.parse(csv, {header: true, dynamicTyping: true, skipEmptyLines: true}).data
            
            // Aggregation
            const firstLevel = aggregate(dataTable, ['exemplar', 'ticker'], 'market_cap');
            const firstLevelMap = firstLevel.map(({exemplar: outer, ticker: inner, market_cap: value})=>({outer, inner, value}));
            initializeTreeMap(firstLevelMap)
        })
})

function initializeTreeMap(sourceData) {
    const defaultWidth = 1024;
    const defaultHeight = 768;
    const canvasSettings = {
        id: 'canvas',
        width: defaultWidth,
        height: defaultHeight,
        dataSource: sourceData
    };

    function reCompute() {
        canvasSettings.width = window.innerWidth * 0.8;
        canvasSettings.height = window.innerHeight * 0.8;
        
        drawCanvas(canvasSettings);
    }

    reCompute();

    window.addEventListener('resize', function(event) {
        reCompute();
    }, true);
}