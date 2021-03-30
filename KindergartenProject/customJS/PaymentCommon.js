﻿function findPaymentEntity(source, month, paymentTypeId) {
    for (var i = 0; i < source.length; i++) {
        if (source[i].Month === month && source[i].PaymentType === paymentTypeId) {
            return source[i];
        }
    }
    return null;
}

function doPaymentOrUnPayment(id,encryptStudentId,year, month, txtAmountName, isPayment, paymentType) {

    if (isPayment == 0) {
        if (!confirm('Ödeme silme işlemine devam etmek istediğinize emin misiniz?')) {
            return;
        }
    }

    var amount = document.getElementById(txtAmountName).value;

    if (IsNullOrEmpty(amount) && amount <=0) {
        alert("Tutar girmelisiniz");
        document.getElementById(txtAmountName).focus();
    }
    else {
        var jsonData = "{id: " + JSON.stringify(id) + ", encryptStudentId:" + JSON.stringify(encryptStudentId) + " , year:" + JSON.stringify(year) + ",month:" + JSON.stringify(month) + ", amount :" + JSON.stringify(amount) + " ,isPayment:" + JSON.stringify(isPayment) + ",paymentType:" + JSON.stringify(paymentType) + "}";
        CallServiceWithAjax('KinderGartenWebService.asmx/DoPaymentOrUnPayment', jsonData, successFunctionDoPaymentOrUnPayment, errorFunction);
    }
}

function successFunctionDoPaymentOrUnPayment(result) {

    if (!result.HasError) {
        alert("İşlem başarılıdır.");

        var paymentEntity = result.Result;
        var uniqueName = "_" + paymentEntity.StudentId + "_" + paymentEntity.Year + "_" + paymentEntity.Month + "_" + paymentEntity.PaymentType;

        var tdPaymentName = "tdPaymentName" + uniqueName;
        var tdUnPaymentName = "tdUnPaymentName" + uniqueName;

        var hdnPaymentStatus = "hdnPaymentStatus" + uniqueName;

        var chcPayment = "chc" + uniqueName;
        var txtPayment = "txt" + uniqueName;


        if (paymentEntity.IsPayment) {
            document.getElementById(tdPaymentName).removeAttribute("style");
            document.getElementById(tdUnPaymentName).style.display = "none";
            document.getElementById(hdnPaymentStatus).value = true;
            document.getElementById(chcPayment).setAttribute("disabled", "disabled");
            document.getElementById(txtPayment).setAttribute("disabled", "disabled");

        }
        else {
            document.getElementById(tdUnPaymentName).removeAttribute("style");
            document.getElementById(tdPaymentName).style.display = "none";
            document.getElementById(hdnPaymentStatus).value = false;
            document.getElementById(chcPayment).removeAttribute("disabled");
            document.getElementById(txtPayment).removeAttribute("disabled");
        }
    }
    else {
        alert("Hata var !!!" + result.ErrorDescription);
    }
}

function textAmount_Change(id, encryptStudentId, studentId, year, month, paymentType, amount) {

    var uniqueName = "_" + studentId + "_" + year + "_" + month + "_" + paymentType;
    var txtAmountName = "txt" + uniqueName;

    var currentAmount = document.getElementById(txtAmountName).value

    if (currentAmount != amount) {
        var jsonData = "{id: " + JSON.stringify(id) + ", encryptStudentId:" + JSON.stringify(encryptStudentId) + " , year:" + JSON.stringify(year) + ",month:" + JSON.stringify(month) + ", currentAmount :" + JSON.stringify(currentAmount) + " ,paymentType:" + JSON.stringify(paymentType) + "}";

        CallServiceWithAjax('KinderGartenWebService.asmx/SetPaymentAmount', jsonData, successFunctionSetPaymentAmount, errorFunction);
    }
}

function successFunctionSetPaymentAmount(obje) {
    if (obje.HasError) {
        alert("Hata var !!!" + obje.ErrorDescription);
    }
    else {
        var result = obje.Result;
        var uniqueName = "_" + result.StudentId + "_" + result.Year + "_" + result.Month + "_" + result.PaymentType;
        var txtAmountName = "txt" + uniqueName;

        document.getElementById(txtAmountName).setAttribute("onchange", "textAmount_Change(" + result.Id + ", '" + result.EncryptStudentId + "', " + result.StudentId + ", " + result.Year +
            "," + result.Month + "," + result.PaymentType + "," + result.Amount + ")");

    }
}

function setPayableStatus(id, encryptStudentId,studentId, year, month, paymentType, amount) {

    var uniqueName = "_" + studentId + "_" + year + "_" + month + "_" + paymentType;

    var hdnPaymentStatus = "hdnPaymentStatus" + uniqueName;
    var chcPaymentName = "chc" + uniqueName;
    var chc = document.getElementById(chcPaymentName);
    var hdnPayment = document.getElementById(hdnPaymentStatus);

    if (hdnPayment.value == "true") {
        alert("İlk önce kaydı ödenmemiş yapmanız gerekmektedir.");
        chc.checked = false;
    }
    else {

        var isNotPayable = chc.checked;

        var jsonData = "{id: " + JSON.stringify(id) + ", encryptStudentId:" + JSON.stringify(encryptStudentId) + " , year:" + JSON.stringify(year) + ",month:" + JSON.stringify(month) + ", amount :" + JSON.stringify(amount) + " ,isNotPayable:" + JSON.stringify(isNotPayable) + ",paymentType:" + JSON.stringify(paymentType) + "}";

        CallServiceWithAjax('KinderGartenWebService.asmx/SetPayableStatus', jsonData, successFunctionSetPayableStatus, errorFunction);
    }
}

function successFunctionSetPayableStatus(result) {

    if (!result.HasError) {
        var paymentEntity = result.Result;
        var uniqueName = "_" + paymentEntity.StudentId + "_" + paymentEntity.Year + "_" + paymentEntity.Month + "_" + paymentEntity.PaymentType;

        var tdPaymentName = "tdPaymentName" + uniqueName;
        var tdUnPaymentName = "tdUnPaymentName" + uniqueName;
        var imgPaymentName = "imgPaymentName" + uniqueName;
        var imgUnPaymentName = "imgUnPaymentName" + uniqueName;
        var txtAmountName = "txt" + uniqueName;
        var chcPaymentName = "chc" + uniqueName;

        var isNotPayable = document.getElementById(chcPaymentName).checked;
        if (isNotPayable) {
            document.getElementById(txtAmountName).value = "";
            document.getElementById(txtAmountName).setAttribute("disabled", "disabled");
            document.getElementById(tdPaymentName).style.pointerEvents = 'none';
            document.getElementById(tdUnPaymentName).style.pointerEvents = 'none';

            document.getElementById(imgPaymentName).style.display = 'none';
            document.getElementById(imgUnPaymentName).style.display = 'none';


            //document.getElementById(tdImageName).style.display = 'none';
        } else {
            document.getElementById(txtAmountName).value = paymentEntity.Amount;
            document.getElementById(txtAmountName).removeAttribute("disabled");
            document.getElementById(tdPaymentName).style.pointerEvents = 'auto';
            document.getElementById(tdUnPaymentName).style.pointerEvents = 'auto';
            //document.getElementById(tdImageName).removeAttribute("style");

            document.getElementById(imgPaymentName).style.display = '';
            document.getElementById(imgUnPaymentName).style.display = '';

        }
    }
    else {
        alert("Hata var !!!" + result.ErrorDescription);
    }
}

function drawPaymentDetail(paymentTypeList, year, month, studentEntity) {

    var tbody = "";

    for (var i in paymentTypeList) {

        var displayAmount = paymentTypeList[i].Amount;
        var amount = paymentTypeList[i].Amount;
        var id = 0;
        var isNotPayable = false;
        var isPayment = false;
        var uniqueName = "_" + studentEntity.Id + "_" + year + "_" + month + "_" + paymentTypeList[i].Id;

        var tdPaymentName = "tdPaymentName" + uniqueName;
        var tdUnPaymentName = "tdUnPaymentName" + uniqueName;

        var tdUnPaymentStyle = "";
        var tdPaymentStyle = "style ='display:none;'";

        var hdnPaymentStatus = "hdnPaymentStatus" + uniqueName;
        

        var paymentOkInputStyle = "";
        var notPayableInputTextStyle = "";
        var notPayableCheckboxChecked = "";


        var imgDisplay = "";

        var paymentEntity = findPaymentEntity(studentEntity.PaymentList, month, paymentTypeList[i].Id);
        if (paymentEntity != null) {
            id = paymentEntity.Id;
            isNotPayable = paymentEntity.IsNotPayable;

            amount = paymentEntity.Amount;
            displayAmount = paymentEntity.Amount;

            if (paymentEntity.IsPayment) {
                tdUnPaymentStyle = "style ='display:none;'";
                tdPaymentStyle = "";
                isPayment = true;
                paymentOkInputStyle = " disabled='disabled';";
            }

            if (isNotPayable == true) {
                displayAmount = "";
                notPayableInputTextStyle = " disabled='disabled';";
                notPayableCheckboxChecked = " checked='checked;'";
                //document.getElementById(tdPaymentName).style.pointerEvents = 'none';
                //document.getElementById(tdUnPaymentName).style.pointerEvents = 'none';
                imgDisplay = "display:none;";

            }
        }
        else {
            if (paymentTypeList[i].Id == PaymentType.Okul && studentEntity.SpokenPrice > 0) {
                displayAmount = studentEntity.SpokenPrice;
                amount = studentEntity.SpokenPrice;
            }
        }
        var txtAmountName = "txt" + uniqueName;
        var chcPaymentName = "chc" + uniqueName;

        var imgUnPaymentName = "imgUnPaymentName" + uniqueName;
        var imgPaymentName = "imgPaymentName" + uniqueName;

        /*
        document.getElementById(chcPayment).setAttribute("disabled", "disabled");
        document.getElementById(txtPayment).setAttribute("disabled", "disabled");
        */
        var imgUnPayment =
            "<img style='cursor: pointer; " + imgDisplay+"' id = '" + imgUnPaymentName + "' width='18' height='18' title='Ödeme Yapmak için tıklayınız' src=\"img/icons/unPayment2.png\"/>";
        var imgPayment =
            "<img style='cursor: pointer; " + imgDisplay +"'  id= '" + imgPaymentName + "' title = 'Ödemeyi Silmek için tıklayınız' src=\"img/icons/greenSmile2.png\"/>";

        tbody += "<td><table cellpadding='4'><tr>";

        tbody += "<td style='display:none;'><input type='hidden' value='" + isPayment + "' id = '" + hdnPaymentStatus + "'/></td>";

        tbody += "<td><input " + paymentOkInputStyle + " " + notPayableCheckboxChecked+" type='checkbox' id='" + chcPaymentName + "' name='" +
            chcPaymentName + "' onclick =setPayableStatus(" + id + ",'" + studentEntity.EncryptId + "'," + studentEntity.Id + "," + year + "," + month + "," + paymentTypeList[i].Id + "," + amount + ") /></td>";

        tbody += "<td><input " + paymentOkInputStyle + " " + notPayableInputTextStyle + " size='3' CssClass='form - control' id='" +
            txtAmountName + "' name='" + txtAmountName + "' type='text' value='" + displayAmount + "' onkeypress='return isNumber(event)' onchange =textAmount_Change(" + id + ",'" + studentEntity.EncryptId + "'," + studentEntity.Id + "," + year +
            "," + month + "," + paymentTypeList[i].Id + "," + amount + ") /></td>";

        tbody += "<td " + tdPaymentStyle+" id='" + tdPaymentName +
            "' onclick =doPaymentOrUnPayment(" + id + ",'" + studentEntity.EncryptId + "'," + year +
            "," + month + ",'" + txtAmountName + "'," + false + "," + paymentTypeList[i].Id + ")>" + imgPayment + "</td>";

        tbody += "<td " + tdUnPaymentStyle+" id='" + tdUnPaymentName +
            "' onclick =doPaymentOrUnPayment(" + id + ",'" + studentEntity.EncryptId + "'," + year +
            "," + month + ",'" + txtAmountName + "'," + true + "," + paymentTypeList[i].Id + ")>" + imgUnPayment + "</td>";

        tbody += "</tr></table></td>";
    }
    return tbody;
}