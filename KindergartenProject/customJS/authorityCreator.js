﻿window.onload = function () {

    loadData();
};

function loadData() {
}

function btnAuthorityCreator_click() {

    var jsonData = "{  }";
    CallServiceWithAjax('/KinderGartenWebService.asmx/GetAuthorityGenerateList', jsonData, successFunctionGetAuthorityGenerateList, errorFunction);


    return false;
}

function successFunctionGetAuthorityGenerateList(obje) {
    document.getElementById("txtNotes").innerHTML = obje;
}
