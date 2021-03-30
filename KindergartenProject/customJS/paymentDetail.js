﻿window.onload = function () {

    var encyrptId = getParameterByName("Id");
    if (!IsNullOrEmpty(encyrptId)) {
        document.getElementById("hdnId").value = encyrptId;
        loadData();
    }
};




function loadData() {
    var encryptStudentId = getParameterByName("Id");
    var year = document.getElementById("drpYear").value;
    var jsonData = "{decryptStudentId: " + JSON.stringify(encryptStudentId) + ", year:" + year + " }";
    CallServiceWithAjax('KinderGartenWebService.asmx/GetStudentListAndPaymentTypeInfoForPaymentDetail', jsonData, successFunctionCurrentPage, errorFunction);

}

function drpYear_Changed() {
    loadData();
}

function successFunctionCurrentPage(obje) {

    if (obje.PaymentTypeList != null) {

        var paymentTypeList = obje.PaymentTypeList;
        var year = document.getElementById("drpYear").value;

        var count = 0;
        var tbody = "<table class='table mb - 0'><thead><tr><th scope='col'>Ay</th>";
        for (var i in paymentTypeList) {

            tbody += "<th scope='col'>" + paymentTypeList[i].Name + "</th>";
            count = count + 1;
        }

        tbody += "</tr></thead>";

        for (var j in months) {

            for (var k in obje.StudentList) {

                tbody += "<tr>";
                tbody += "<td>" + months[j][1] + "</td>";
                tbody += drawPaymentDetail(paymentTypeList, year, months[j][0], obje.StudentList[k]);
                tbody += "</tr>";
            }
        }

        tbody += "</table>";

        document.getElementById("divMain").innerHTML = tbody;
    }
}





