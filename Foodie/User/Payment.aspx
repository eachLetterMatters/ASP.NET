<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="Payment.aspx.cs" Inherits="Foodie.User.Payment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .rounded {
            border-radius: 1rem;
        }

        .nav-pills .nav-link {
            color: #555;
        }

            .nav-pills .nav-link.acive {
                color: white;
            }

        .bold {
            font-weight: bold;
        }

        .card {
            padding: 40px 50px;
            border-radius: 20px;
            border: none;
            box-shadow: 1px 5px 10px 1px rgba(0,0,0,0.2);
        }
    </style>

    <script>
        // for disappearing alert messages
        window.onload = function () {
            setTimeout(function () {
                document.getElementById("<%=lblMsg.ClientID %>").style.display = "none";
            }, 5000);
        };
        // for tooltips
        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        })
    </script>

    <%--function preventing back button--%>
    <script type="text/javascript">
        function DisableBackButton() {
            window.history.forward()
        }
        DisableBackButton();
        window.onload = DisableBackButton;
        window.onpageshow = function (evt) { if (evt.persisted) DisableBackButton() }
        window.onunload = function () { void (0) }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <section class="book_section" style="background-image: url('../Images/payment-bg.png'); width: 100%; height: 100%; background-repeat: no-repeat; background-size: auto; background-attachment: fixed; background-position: left;">

        <div class="container py-5">
            <div class="align-self-end">
                <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>
            </div>

            <div class="row mb-4">
                <div class="col-lg-8 mx-auto text-center">
                    <h1 class="display-6">Order Payment</h1>
                </div>
            </div>

            <div class="row pb-5">
                <div class="col-lg-6 mx-auto">
                    <div class="card">
                        <div class="card-header">
                            <!-- Credit card form tabs -->
                            <div class="bg-white shadow-sm pt-4 pl-2 pr-2 pb-2">
                                <ul role="tablist" class="nav bg-light nav-pills rounded nav-fill mb-3">
                                    <li class="nav-item"><a data-toggle="pill" href="#credit-card" class="nav-link active "><i class="fas fa-credit-card mr-2"></i>Credit Card </a></li>
                                    <li class="nav-item"><a data-toggle="pill" href="#paypal" class="nav-link "><i class="fab fa-moeny mr-2"></i>Paypal </a></li>
                                    <li class="nav-item"><%--<a data-toggle="pill" href="#net-banking" class="nav-link "><i class="fas fa-mobile-alt mr-2"></i>Net Banking </a>--%></li>
                                </ul>
                            </div>
                            <!-- End -->
                            <!-- Credit card form content -->
                            <div class="tab-content">
                                <!-- credit card info-->
                                <div id="credit-card" class="tab-pane fade show active pt-3">
                                    <div role="form">
                                        <div class="form-group">
                                            <label for="txtName">
                                                <h6>Card Owner</h6>
                                            </label>
                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" ErrorMessage="Name is required" ControlToValidate="txtName"
                                                ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ValidationGroup="card">*</asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="revName" runat="server" ErrorMessage="Name must be in characters" ControlToValidate="txtName"
                                                ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ValidationExpression="^[a-zA-Z\s]+$" ValidationGroup="card">*
                                            </asp:RegularExpressionValidator>
                                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Card Owner Name"></asp:TextBox>
                                        </div>
                                        <div class="form-group">
                                            <label for="txtCardNo">
                                                <h6>Card number</h6>
                                            </label>
                                            <asp:RequiredFieldValidator ID="rfvCardNo" runat="server" ErrorMessage="Card number is required" ControlToValidate="txtCardNo"
                                                ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ValidationGroup="card">*</asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="revCardNo" runat="server" ErrorMessage="Card number must be in 16 digits" ControlToValidate="txtCardNo"
                                                ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ValidationExpression="[0-9]{16}" ValidationGroup="card">*
                                            </asp:RegularExpressionValidator>
                                            <div class="input-group">
                                                <asp:TextBox ID="txtCardNo" runat="server" CssClass="form-control" placeholder="Valid card number" TextMode="number"></asp:TextBox>
                                                <div class="input-group-append">
                                                    <span class="input-group-text text-muted">
                                                        <i class="fab fa-cc-visa mx-1"></i>
                                                        <i class="fab fa-cc-mastercard mx-1"></i>
                                                        <i class="fab fa-cc-amex mx-1"></i>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-sm-8">
                                                <div class="form-group">
                                                    <label>
                                                        <span class="hidden-xs">
                                                            <h6>Expiration Date</h6>
                                                        </span>
                                                    </label>
                                                    <asp:RequiredFieldValidator ID="rfvExpMonth" runat="server" ErrorMessage="Expiry month is required"
                                                        ControlToValidate="txtExpMonth" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ValidationGroup="card">
                                                        *</asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="revExpMonth" runat="server" ErrorMessage="Expiry month must be in 2 digits" ControlToValidate="txtExpMonth"
                                                        ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ValidationExpression="[0-9]{2}" ValidationGroup="card">*
                                                    </asp:RegularExpressionValidator>
                                                    <asp:RequiredFieldValidator ID="rfvExpYear" runat="server" ErrorMessage="Expiry year is required"
                                                        ControlToValidate="txtExpYear" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ValidationGroup="card">
                                                        *</asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="revExpYear" runat="server" ErrorMessage="Expiry year must be in 4 digits" ControlToValidate="txtExpYear"
                                                        ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ValidationExpression="[0-9]{4}" ValidationGroup="card">*
                                                    </asp:RegularExpressionValidator>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtExpMonth" runat="server" CssClass="form-control" placeholder="MM" TextMode="Number"></asp:TextBox>
                                                        <asp:TextBox ID="txtExpYear" runat="server" CssClass="form-control" placeholder="YYYY" TextMode="Number"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-4">
                                                <div class="form-group mb-4">
                                                    <label data-toggle="tooltip" title="Three digit CV code on the back of your card">
                                                        <h6>CVV <i class="fa fa-question-circle d-inline"></i></h6>
                                                    </label>
                                                    <asp:RequiredFieldValidator ID="rfvCvv" runat="server" ErrorMessage="CVV is required" ControlToValidate="txtCvv"
                                                        ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ValidationGroup="card">*</asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="revCvv" runat="server" ErrorMessage="Cvv must be in 3 numbers" ControlToValidate="txtCvv"
                                                        ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ValidationExpression="[0-9]{3}" ValidationGroup="card">*
                                                    </asp:RegularExpressionValidator>
                                                    <asp:TextBox ID="txtCvv" runat="server" CssClass="form-control" placeholder="CVV" TextMode="Number"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="txtAddress">
                                                <h6>Delivery Address</h6>
                                            </label>
                                            <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ErrorMessage="Address is required" ControlToValidate="txtAddress"
                                                ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ValidationGroup="card">*</asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" placeholder="Delivery Address"
                                                ValidationGroup="card" TextMode="Multiline"></asp:TextBox>
                                        </div>
                                        <div class="card-footer">
                                            <asp:LinkButton ID="lbCardSubmit" runat="server" CssClass="subscribe btn btn-info btn-block shadow" ValidationGroup="card"
                                                OnClick="lbCardSubmit_Click">Confirm Payment</asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                                <!-- End -->
                                <%--Cash on delivery info--%>
                                <div id="paypal" class="tab-pane fade pt-3">
                                    <div class="form-group">
                                        <label for="txtCODAddress">
                                            <h6>Delivery Address</h6>
                                        </label>
                                        <asp:TextBox ID="txtCODAddress" runat="server" CssClass="form-control" placeholder="Delivery Address" TextMode="MultiLine"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvCODAddress" runat="server" ErrorMessage="Address is required" ControlToValidate="txtCODAddress"
                                            ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ValidationGroup="cod">*</asp:RequiredFieldValidator>
                                    </div>
                                    <p>
                                        <asp:LinkButton ID="lbCodSubmit" runat="server" CssClass="btn btn-info" ValidationGroup="cod" OnClick="lbCodSubmit_Click">
                                            <i class="fa fa-cart-arrow-down mr-2"></i>Confirm order</asp:LinkButton>
                                    </p>
                                    <p class="text-muted">
                                        Note: At the receiving of your order, you need to do the full payment. After completing the payment, you can check your order status.
                                    </p>
                                </div>
                                <%--End--%>
                            </div>
                            <%--End--%>
                        </div>
                        <div class="card-footer">
                            <b class="badge badge-success badge-pill shadow-sm">Order Total: <%Response.Write(Session["grandTotalPrice"]); %> $</b>
                            <div class="pt-1">
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" ValidationGroup="card" HeaderText="Fix the following errors" />
                            </div>
                        </div>
                    </div>
            </div>
        </div>
    </section>
</asp:Content>
