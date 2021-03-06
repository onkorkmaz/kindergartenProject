﻿<%@ Page Title="Benim dünyam - Öğrenci Kayıt" Language="C#" MasterPageFile="~/kindergarten.Master" AutoEventWireup="true" CodeBehind="AddStudent.aspx.cs" Inherits="KindergartenProject.AddStudent" %>

<%@ Register Src="~/userControl/divInformation.ascx" TagPrefix="uc1" TagName="divInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="customJS/Student.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">

        <uc1:divInformation runat="server" ID="divInformation" InformationVisible="false" />

        <div class="col-12 col-xl-12">
            <div class="card">
                <div class="card-body">
                    <asp:Panel runat="server" ID="pnlBody">
                        <asp:HiddenField runat="server" ID="hdnId" />

                        <div class="mb-3 row">
                            <label class="col-form-label col-sm-2 text-sm-left">Kimlik Numarası</label>
                            <div class="col-sm-6">
                                <asp:TextBox runat="server" ID="txtTckn" CssClass="form-control" onchange="txtCitizenshipNumber_Change(this.value);" placeholder="TCKN" MaxLength="11" onkeypress="return isNumber(event)"></asp:TextBox>
                            </div>
                        </div>
                        <div class="mb-3 row">
                            <label class="col-form-label col-sm-2 text-sm-left">Adı</label>
                            <div class="col-sm-4">
                                <asp:TextBox runat="server" ID="txtName" CssClass="form-control" placeholder="Adı"></asp:TextBox>
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox runat="server" ID="txtMiddleName" CssClass="form-control" placeholder="İkinci Adı"></asp:TextBox>
                            </div>
                        </div>
                        <div class="mb-3 row">
                            <label class="col-form-label col-sm-2 text-sm-left">Soyadı</label>
                            <div class="col-sm-6">
                                <asp:TextBox runat="server" ID="txtSurname" CssClass="form-control" placeholder="Soyadı"></asp:TextBox>
                            </div>
                        </div>
                        <div class="mb-3 row">
                            <label class="col-form-label col-sm-2 text-sm-left">Doğum Tarihi</label>


                            <div class="col-sm-2">
                                <asp:TextBox runat="server" ID="txtDay" CssClass="form-control" placeholder="Gün" MaxLength="2" onkeypress="return isNumber(event)"></asp:TextBox>
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox runat="server" ID="txtMonth" CssClass="form-control" placeholder="Ay" MaxLength="2" onkeypress="return isNumber(event)"></asp:TextBox>
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox runat="server" ID="txtYear" CssClass="form-control" placeholder="Yıl" onkeypress="return isNumber(event)" MaxLength="4"></asp:TextBox>
                            </div>
                        </div>
                        <div class="mb-3 row">
                            <label class="col-form-label col-sm-2 text-sm-left">Baba Bilgileri</label>
                            <div class="col-sm-2">
                                <asp:TextBox runat="server" ID="txtFatherName" CssClass="form-control" placeholder="Baba Adı"></asp:TextBox>
                            </div>
                            <div class="col-sm-4">
                                <asp:TextBox runat="server" ID="txtFatherPhoneNumber" CssClass="form-control" onkeypress="return isNumber(event)" MaxLength="11" placeholder="Baba Tel"></asp:TextBox>

                            </div>
                        </div>
                        <div class="mb-3 row">
                            <label class="col-form-label col-sm-2 text-sm-left">Anne Bilgileri</label>
                            <div class="col-sm-2">
                                <asp:TextBox runat="server" ID="txtMotherName" CssClass="form-control" placeholder="Anne Adı"></asp:TextBox>
                            </div>

                            <div class="col-sm-4">
                                <asp:TextBox runat="server" ID="txtMotherPhoneNumber" CssClass="form-control" onkeypress="return isNumber(event)" MaxLength="11" placeholder="Anne Tel"></asp:TextBox>

                            </div>
                        </div>
                        <div class="mb-3 row">
                            <label class="col-form-label col-sm-2 text-sm-left">Öğrenci Durumu</label>
                            <div class="col-sm-2">
                                <asp:DropDownList runat="server" ID="drpStudentState" CssClass="form-control">
                                    <asp:ListItem Text="Kayıt" Selected="True" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Görüşme" Value="1"></asp:ListItem>
                                </asp:DropDownList>
                            </div>


                        </div>
                        <div class="mb-3 row">
                            <label class="col-form-label col-sm-2 text-sm-left">Aktif</label>
                            <div class="col-sm-6">
                                <asp:CheckBox runat="server" ID="chcIsActive" CssClass="form-check-input" Checked="true" />
                            </div>
                        </div>
                        <div class="mb-3 row">

                            <div class="col-sm-1">
                                <asp:Button runat="server" ID="btnSubmit" CssClass="btn btn-primary " Text="Kaydet" OnClientClick="javascript: return validate()" OnClick="btnSubmit_Click" />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button runat="server" ID="btnCancel" CssClass="btn btn-secondary " Text="İptal" OnClick="btnCancel_Click" />
                            </div>
                        </div>
                       
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
