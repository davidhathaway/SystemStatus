/// <reference path="../../typings/knockout/knockout.d.ts" />
/// <reference path="../../typings/jquery/jquery.d.ts" />
/// <reference path="../../typings/jquery.timeago/jquery.timeago.d.ts" />
/// <reference path="../../typings/signalr/signalr.d.ts" />
/// <reference path="../../typings/bootstrap/bootstrap.d.ts" />
/// <reference path="../../typings/flot/jquery.flot.d.ts" />
/// <reference path="../../typings/moment/moment.d.ts" />
;
var SystemAppViewKoModel = (function () {
    function SystemAppViewKoModel() {
        this.items = ko.observableArray([]);
    }
    SystemAppViewKoModel.prototype.RefreshAll = function () {
        var _this = this;
        var url = $(".app-list").data("refreshall");
        $.getJSON(url, function (data, textStatus, jqXHR) {
            if (data && data.length) {
                _this.Load(data);
            }
        });
    };
    SystemAppViewKoModel.prototype.Load = function (data) {
        //load model
        this.items.removeAll();
        for (var i = 0; i < data.length; i++) {
            var itemModel = data[i];
            var appItem = new SystemStatusKoModel(this, itemModel);
            this.items.push(appItem);
        }
    };
    SystemAppViewKoModel.prototype.UpdateItem = function (model) {
        if (model && model.SystemID) {
            //find item
            var match = ko.utils.arrayFirst(this.items(), function (item) {
                return model.SystemGroupID === item.SystemGroupID;
            });
            if (match) {
                match.update(model);
            }
        }
    };
    SystemAppViewKoModel.prototype.CreateSystem = function () {
        var _this = this;
        var section = $("#system-create-section");
        var url = section.data("url");
        $("#system-create-section").empty();
        $("#system-create-section").load(url, function () {
            var dlg = $("#system-create-modal");
            dlg.modal({ show: true });
            var form = $("#system-create-form");
            //hook save
            $(".create-dialog-save").click(function () {
                //submit form  
                $.ajax({
                    url: form.attr("action"),
                    type: "POST",
                    data: form.serialize()
                }).done(function (data) {
                    if (data.Success) {
                        dlg.modal("hide");
                        //get new app status view model and add it.
                        _this.RefreshAll();
                    }
                    else {
                        //set errors
                        if (data.Errors && data.Errros.length) {
                            for (var e = 0; e < data.Errors.length; e++) {
                            }
                        }
                    }
                });
            });
        });
    };
    return SystemAppViewKoModel;
})();
var SystemStatusKoModel = (function () {
    function SystemStatusKoModel(app, model) {
        var _this = this;
        this.App = app;
        this.SystemGroupID = model.SystemGroupID;
        this.Name = model.Name;
        this.DrillDownUrl = model.DrillDownUrl;
        this.LastAppStatuses = ko.observable(model.AppStatuses);
        this.AppStatusClass = ko.computed(function () {
            var systemStatus = 0;
            var statuses = _this.LastAppStatuses();
            if ($.isArray(statuses)) {
                systemStatus = 4;
                for (var i = 0; i < statuses.length; i++) {
                    var status = statuses[i];
                    if (status < systemStatus) {
                        systemStatus = status;
                    }
                }
            }
            var statusClass = "app-status-error";
            switch (systemStatus) {
                case 0:
                    statusClass = "app-status-none";
                    break;
                case 1:
                    statusClass = "app-status-fast";
                    break;
                case 2:
                    statusClass = "app-status-normal";
                    break;
                case 3:
                    statusClass = "app-status-slow";
                    break;
                case 4:
                    statusClass = "app-status-fast";
                    break;
            }
            return statusClass;
        }, this);
        this.AppStatusTextClass = ko.computed(function () {
            return _this.AppStatusClass() + "-text";
        }, this);
    }
    SystemStatusKoModel.prototype.update = function (model) {
    };
    SystemStatusKoModel.prototype.ViewGroup = function () {
        //open group in new window?
        window.open(this.DrillDownUrl);
    };
    return SystemStatusKoModel;
})();
$(function () {
    var appModel = new SystemAppViewKoModel();
    appModel.Load(rawSystemStatusModel);
    ko.applyBindings(appModel);
    var hub = $.connection.appEventHub;
    hub.client.updateAppStatus = function (model) {
        console.log(model);
        appModel.UpdateItem(model);
    };
    $.connection.hub.start().done(function (e) {
        console.log(e);
    });
});
//# sourceMappingURL=Index.js.map