﻿function GetStudentEntity(citizenshipNumber)
{
    if (!IsNullOrEmpty(citizenshipNumber)) {
        var jsonData = "{ citizenshipNumber: " + JSON.stringify(citizenshipNumber) + " }";
        CallServiceWithAjax('/KinderGartenWebService.asmx/GetStudentEntity',
            jsonData,
            successFunctionGetStudentEntity,
            errorFunction);
    }
}

function txtCitizenshipNumber_Change(citizenshipNumber) {
    GetStudentEntity(citizenshipNumber);
}

function fullName_Change() {
    var name = document.getElementById("txtName").value;
    var surname = document.getElementById("txtSurname").value;
    var middleName = document.getElementById("txtMiddleName").value;

    if (!IsNullOrEmpty(name) && !IsNullOrEmpty(surname)) {

        var fullName = replaceTurkichChar(name.toLocaleLowerCase('tr-TR'));

        if (!IsNullOrEmpty(middleName))
            fullName += " " + replaceTurkichChar(middleName.toLocaleLowerCase('tr-TR'));

        fullName += " " + replaceTurkichChar(surname.toLocaleLowerCase('tr-TR'));

        IsUserNameExist(fullName);

    }

}

function onChangeIsInterview() {
    var checked = document.getElementById("chcInterview").checked;
    var interviewDate = document.getElementById("interviewDate");

    if (checked) {
        interviewDate.style.display = "";

    }
    else {
        interviewDate.style.display = "none";
    }
}

function IsUserNameExist(fullName) {

    if (!IsNullOrEmpty(fullName)) {
        var jsonData = "{ fullName: " + JSON.stringify(fullName) + " }";
        CallServiceWithAjax('/KinderGartenWebService.asmx/GetStudentEntityWithFullName',
            jsonData,
            successFunctionGetStudentFullNameEntity,
            errorFunction);
    }

}

function successFunctionGetStudentFullNameEntity(result) {

    if (result != null && result.HasError) {
        alert("Hata var !!! Error : " + result.ErrorDescription);
    }

    else if (result != null && !result.HasError && result.Result != null) {
        alert("Girdiğiniz öğrenci sistemde mevcuttur");
        setStudent(result.Result);
    }
    else {
        document.getElementById("btnSubmit").value = "Kaydet";
    }
}

function setStudent(entity) {
    document.getElementById("hdnId").value = entity.Id;
    document.getElementById("txtName").value = entity.Name;
    document.getElementById("txtSurname").value = entity.Surname;
    document.getElementById("txtMiddleName").value = entity.MiddleName;
    document.getElementById("txtMotherName").value = entity.MotherName;
    document.getElementById("txtFatherName").value = entity.FatherName;
    document.getElementById("txtFatherPhoneNumber").value = entity.FatherPhoneNumber;
    document.getElementById("txtMotherPhoneNumber").value = entity.MotherPhoneNumber;
    document.getElementById("chcIsActive").checked = entity.IsActive;
    document.getElementById("txtSpokenPrice").value = entity.SpokenPrice;
    document.getElementById("txtNotes").value = entity.Notes;
    document.getElementById("txtBirthday").value = entity.BirthdayWithFormatyyyyMMdd;
    //document.getElementById("txtDateOfMeeting").value = entity.DateOfMeetingWithFormat2;
    document.getElementById("txtInterviewDate").value = entity.InterviewWithFormat;
    document.getElementById("chcInterview").checked = entity.IsInterview;


    document.getElementById("txtEmail").value = entity.Email;
    document.getElementById("btnSubmit").value = "Güncelle";
}

function successFunctionGetStudentEntity(result) {

    if (!result.HasError) {
        var entity = result.Result;
        if (entity != null) {
            alert("Girdiğiniz kimlik numarası sistemde mevcuttur");

            setStudent(entity);

        } else {
            document.getElementById("btnSubmit").value = "Kaydet";
        }
    }
    else {
        alert("Hata var !!! Error : " + result.ErrorDescription);
    }
}

function isEmailAddress(str) {

    var pattern = /^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/;
    return str.match(pattern);
}

function checkEmail(str) {
    var email = str;
    var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    if (!filter.test(email)) {
        return false;
    }
    return true;
}   

function validate() {
    var errorMessage = "";

    var obje = document.getElementById("txtTckn").value;
    //if (IsNullOrEmpty(obje))
    //    errorMessage += "Tc kimlik No boş bırakılamaz\n";

    obje = document.getElementById("txtName").value;
    if (IsNullOrEmpty(obje))
        errorMessage += "İsim boş bırakılamaz\n";

    obje = document.getElementById("txtSurname").value;
    if (IsNullOrEmpty(obje))
        errorMessage += "Soyisim boş bırakılamaz\n";


    var email = document.getElementById("txtEmail").value;
    if (!IsNullOrEmpty(email) && !checkEmail(email)) {
        errorMessage += "Email bilgisi doğru formatta girilmemiştir.\n";
    }

    var isInterview = document.getElementById("chcInterview").checked;
    if (isInterview) {
        var interviewDate = document.getElementById("txtInterviewDate").value;
        if (IsNullOrEmpty(interviewDate)) {
            errorMessage += "Görüşme tarihi boş geçilemez.\n";
        }
    }

    if (!IsNullOrEmpty(errorMessage)) {
        alert(errorMessage);
        return false;
    }
    return true;
}

function OnStudentStateChanged(value) {

    if (value == 0) {
        document.getElementById("divClassList").style.display = "";

        if (!IsNullOrEmpty(document.getElementById("hdnCurrentClassId").value)) {
            document.getElementById("drpClassList").value = document.getElementById("hdnCurrentClassId").value;

        }
    }
    else {
        document.getElementById("divClassList").style.display = "none";
        document.getElementById("drpClassList").value = -1;
        document.getElementById("lblMaxStudentCount").value = "";
    }
}

function OnClassListChanged(value) {

    if (!IsNullOrEmpty(value)) {
        var jsonData = "{ classId: " + JSON.stringify(value) + " }";
        CallServiceWithAjax('/KinderGartenWebService.asmx/CalculateRecordedStudentCount',
            jsonData,
            successFunctionCalculateRecordedStudentCount,
            errorFunction);
    }
}

function successFunctionCalculateRecordedStudentCount(result) {

    document.getElementById("lblMaxStudentCount").innerHTML = "";

    if (!IsNullOrEmpty(result)) {
        document.getElementById("lblMaxStudentCount").innerHTML = result;
    }
}

