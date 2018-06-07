/*
实现量图时，量出来的px值直接除以100，得出rem值，如20px就是0.2rem
*/
(function () {
    var design_height = 1280;//UI设计图的px高度
    var scale = 1.0;//后期调整这个值调整整体大小

    window.__remConfig_flag = (window.innerHeight / design_height) * 100 * scale;
    document.documentElement.style.fontSize = window.__remConfig_flag + "px";

})();

//根据rem值，获取实际的px值
function getPixelByRem(remValue)
{
    return window.__remConfig_flag * remValue;
}