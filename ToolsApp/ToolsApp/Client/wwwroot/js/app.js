window.toolsAppDemo = {};

window.toolsAppDemo.setFocus = function (control) {
  control.focus();
};

window.toolsAppDemo.setupColorsRefresh = function (dotNetHelper) {

  console.log("setup colors refresh");

  $("#refresh-colors-button").click(function () {
    console.log("called refresh colors button");
    return dotNetHelper.invokeMethodAsync("All").then(colors => console.log(colors));
  });

};