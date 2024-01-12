// Load specific JS files
// Reference: https://www.geekinsta.com/add-javascript-in-blazor-components/
function loadJs(sourceUrl, action = null) {
    if (sourceUrl.Length == 0) {
        console.error("Invalid source URL");
        return;
    }

    var tag = document.createElement('script');
    tag.src = sourceUrl;
    tag.type = "text/javascript";

    tag.onload = function () {
        console.log("Script loaded successfully");
        if(action) action();
    }

    tag.onerror = function () {
        console.error("Failed to load script");
    }

    document.body.appendChild(tag);
}