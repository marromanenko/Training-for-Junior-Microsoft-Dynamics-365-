var createPickup = {

    OpenRecord: function () {
        var entityFormOptions = {};

        entityFormOptions["entityName"] = "mr_cartransferreport";
        entityFormOptions["useQuickCreateForm"] = true;
        
        var formParameters = {};
        formParameters["mr_date"] = new Date();
        formParameters["mr_name"] = "Pickup";
        formParameters["mr_type"] = false;

        Xrm.Navigation.openForm(entityFormOptions, formParameters).then(
            function (success) {
                Xrm.Page.getAttribute("mr_pickupreport").setValue(success.savedEntityReference);
            },
            function (error) {
                console.log(error)
            }
        );

    }
}

var createReturn = {

    OpenRecord: function () {

        var entityFormOptions = {};

        entityFormOptions["entityName"] = "mr_cartransferreport";

        entityFormOptions["useQuickCreateForm"] = true;

        var formParameters = {};

        formParameters["mr_date"] = new Date();
        formParameters["mr_name"] = "Return";
        formParameters["mr_type"] = true;

        Xrm.Navigation.openForm(entityFormOptions, formParameters).then(
            function (success) {
                Xrm.Page.getAttribute("mr_returnreport").setValue(success.savedEntityReference);
            },
            function (error) {
                console.log(error);
            }
        );
    }
}

var Sdk = window.Sdk || {};
(
    function () {
        this.NotificationOnForm = function (executionContext) {
            var formContext = executionContext.getFormContext();
            var status = formContext.getAttribute("statuscode").getValue();
            var paid = formContext.getAttribute("mr_paid").getValue();

            if (status === 315890001 && paid === false) {
                formContext.ui.setFormNotification("Car rent is not yet paid.Car cannot be rented", "INFO", "formnot1");
            }
            else {
                formContext.ui.clearFormNotification("formnot1");
            }
        }
    }
).call(Sdk);