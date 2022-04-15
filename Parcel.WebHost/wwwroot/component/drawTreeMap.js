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
    const string = Math.floor(Math.random()*16777215).toString(16) 
    return `#` + string
}

function componentToHex(c) {
    var hex = c.toString(16);
    return hex.length == 1 ? "0" + hex : hex;
}

function rgbToHex(r, g, b) {
    return "#" + componentToHex(r) + componentToHex(g) + componentToHex(b);
}

function interpColor(ratio) { // Ratio between 0-1
    const red = {r: 255, g: 0, b: 0}
    const green = {r: 0, g: 255, b: 0}
    
    const r = red.r * (1 - ratio) + green.r * ratio;
    const g = red.g * (1 - ratio) + green.g * ratio;
    const b = red.b * (1 - ratio) + green.b * ratio;
    return rgbToHex(Math.floor(r), Math.floor(g), Math.floor(b));
}

function drawCanvas(canvasSettings){    
    const canvas = document.getElementById(canvasSettings.id);
    const ctx = canvas.getContext('2d');
    canvas.width = canvasSettings.width;
    canvas.height = canvasSettings.height;
    
    // Background
    ctx.fillStyle = 'black';
    ctx.fillRect(0, 0, canvasSettings.width, canvasSettings.height);
    ctx.strokeStyle = 'black';
    
    // Level 1
    const aggregateLevel1 = aggregate(canvasSettings.dataSource, ['outer'], 'value')
    const level1Max = Math.max.apply(Math, aggregateLevel1.map(o => o.value))
    const level1Min = Math.min.apply(Math, aggregateLevel1.map(o => o.value))
    computeTreeMap(aggregateLevel1, canvasSettings.width, canvasSettings.height).forEach(element => {
        // Level 1 Background
        ctx.fillStyle = interpColor((element.data.value - level1Min) / level1Max)
        ctx.lineWidth = 4;
        ctx.fillRect(element.x, element.y, element.width, element.height)
        ctx.strokeRect(element.x, element.y, element.width, element.height)

        // Level 1 Label
        const level1LabelHeight = 12
        ctx.fillStyle = "gray"
        ctx.fillRect(element.x, element.y, element.width, level1LabelHeight)
        ctx.font = '12px serif';
        ctx.textBaseline = "top";
        ctx.textAlign = 'start';
        ctx.fillStyle = 'white';
        ctx.fillText(element.data.outer, element.x + /*Gap*/ 3, element.y);
        ctx.lineWidth = 2;
        ctx.strokeRect(element.x, element.y, element.width, level1LabelHeight)

        // Level 2
        const aggregationLevel2 = canvasSettings.dataSource.filter(ds => ds.outer === element.data.outer)
        const level2Max = Math.max.apply(Math, aggregationLevel2.map(o => o.value))
        const level2Min = Math.min.apply(Math, aggregationLevel2.map(o => o.value))
        computeTreeMap(aggregationLevel2, element.width, element.height - level1LabelHeight).forEach(child => {
            // Level 2 Background
            ctx.fillStyle = interpColor((child.data.value - level2Min) / level2Max)
            ctx.lineWidth = 1;
            ctx.fillRect(element.x + child.x, element.y + child.y + level1LabelHeight, child.width, child.height)
            ctx.strokeRect(element.x + child.x, element.y + child.y + level1LabelHeight, child.width, child.height)

            // Level 2 Label
            ctx.font = '12px serif';
            ctx.textBaseline = "middle";
            ctx.textAlign = 'center';
            ctx.fillStyle = 'black';
            ctx.fillText(child.data.inner, element.x + child.x + /*Gap*/ 3 + child.width / 2, element.y + child.y + level1LabelHeight + child.height / 2);
        })
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