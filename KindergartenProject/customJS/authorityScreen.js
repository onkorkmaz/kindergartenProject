﻿window.onload = function () {

    loadData();
};

function loadData() {

    var jsonData = "{  }";
    CallServiceWithAjax('/KinderGartenWebService.asmx/GetAuthorityScreenList', jsonData, successFunctionGetAuthorityScreenList, errorFunction);

}

function successFunctionGetAuthorityScreenList(obje) {

    if (!obje.HasError && obje.Result) {

        var entityList = obje.Result;
        if (entityList != null) {

            var tbody = "";
            for (var i in entityList) {

                tbody += "<tr>";
                tbody += "<td>";
                tbody += "<a href = \"#\"><img src =\"/img/icons/update1.png\" onclick='updateCurrentRecord(\"" + entityList[i].Id + "\")'/></a>";
                tbody += "<a href = \"#\"><img src =\"/img/icons/trush1.png\" onclick='deleteCurrentRecord(\"" + entityList[i].Id + "\")' /></a>";
                tbody += "</td>";

                tbody += "<td>" + entityList[i].Name + "</td>";

                tbody += "<td>" + entityList[i].Description + "</td>";

                if (entityList[i].IsActive)
                    tbody += "<td><img src='/img/icons/active.png' width='25' height ='25' /></td>";
                else
                    tbody += "<td><img src='/img/icons/passive.png' width='20' height ='20' /></td>";

                tbody += "</tr> ";
            }

            document.getElementById("tbAuthorityList").innerHTML = tbody;

        }
    }
    else {
        alert("Hata var !!! Error : " + obje.ErrorDescription);
    }
}

function validateAndSave()
{
    if (!validate())
        return false;

    var id = document.getElementById("hdnId").value;
    var name = document.getElementById("txtAuthorityName").value;
    var description = document.getElementById("txtDescription").value;
    var isActive = document.getElementById("chcIsActive").checked;


    var authorityScreenEntity = {};
    authorityScreenEntity["Name"] = name;
    authorityScreenEntity["IsActive"] = isActive;
    authorityScreenEntity["Description"] = description;


    var jsonData = "{ id:" + JSON.stringify(id) + ", authorityScreenEntity: " + JSON.stringify(authorityScreenEntity) + " }";
    CallServiceWithAjax('/KinderGartenWebService.asmx/InsertOrUpdateAuthorityScreen', jsonData, successFunctionInsertOrUpdateAuthorityScreen, errorFunction);

    return false;

}

function validate() {
    var errorMessage = "";

    var obje = document.getElementById("txtAuthorityName").value;
    if (IsNullOrEmpty(obje))
        errorMessage += "Yetki Adı boş bırakılamaz\n";

    if (!IsNullOrEmpty(errorMessage)) {
        alert(errorMessage);
        return false;
    }

    return true;
}

function successFunctionInsertOrUpdateAuthorityScreen(obje) {

    if (!obje.HasError && obje.Result) {
        loadData();
        callInsertOrUpdateInformationMessage("hdnId");

        setDefaultValues();
        
    }
    else {
        alert("Hata var !!! Error : " + obje.ErrorDescription);
    }
}

function deleteCurrentRecord(id) {

    if (confirm('Silme işlemine devam etmek istediğinize emin misiniz?')) {

        var jsonData = "{ id: " + JSON.stringify(id) + " }";
        CallServiceWithAjax('/KinderGartenWebService.asmx/DeleteAuthorityScreen', jsonData, successFunctionDeleteAuthorityScreen, errorFunction);
    }

}

function successFunctionDeleteAuthorityScreen(obje) {
    if (!obje.HasError && obje.Result) {
        loadData();
        callDeleteInformationMessage();
        setDefaultValues();

    }
    else {
        alert("Hata var !!! Error : " + obje.ErrorDescription);
    }
}

function updateCurrentRecord(id) {

    document.getElementById("hdnId").value = id;
    var jsonData = "{ id: " + JSON.stringify(id) + " }";
    CallServiceWithAjax('/KinderGartenWebService.asmx/GetAuthorityScreenWithId', jsonData, successFunctionGetAuthorityScreenWithId, errorFunction);

}

function successFunctionGetAuthorityScreenWithId(obje) {
    if (!obje.HasError && obje.Result != null) {
        var entity = obje.Result;
        document.getElementById("hdnId").value = entity.Id;
        document.getElementById("txtAuthorityName").value = entity.Name;
        document.getElementById("txtDescription").value = entity.Description;
        document.getElementById("chcIsActive").checked = entity.IsActive;
        document.getElementById("btnSubmit").value = "Güncelle";
        document.getElementById("btnSubmit").disabled = "";

    }
    else {
        alert("Hata var !!! Error : " + obje.ErrorDescription);
    }
}

function setDefaultValues() {
    document.getElementById("hdnId").value = "";
    document.getElementById("txtAuthorityName").value = "";
    document.getElementById("txtDescription").value = "";
    document.getElementById("chcIsActive").checked = true;
    document.getElementById("btnSubmit").value = "Kaydet";
}

function cancel() {
    setDefaultValues();
    return false;
}
