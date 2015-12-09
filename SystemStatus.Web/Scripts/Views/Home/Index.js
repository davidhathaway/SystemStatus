/// <reference path="../../typings/knockout/knockout.d.ts" />
/// <reference path="../../typings/jquery/jquery.d.ts" />
/// <reference path="../../typings/jquery.timeago/jquery.timeago.d.ts" />
/// <reference path="../../typings/signalr/signalr.d.ts" />
/// <reference path="../../typings/bootstrap/bootstrap.d.ts" />
/// <reference path="../../typings/flot/jquery.flot.d.ts" />
/// <reference path="../../typings/moment/moment.d.ts" />
;
;
var AppViewKoModel = (function () {
    function AppViewKoModel() {
        this.items = ko.observableArray([]);
    }
    AppViewKoModel.prototype.RefreshAll = function () {
        var _this = this;
        var url = $(".app-list").data("refreshall");
        $.getJSON(url, function (data, textStatus, jqXHR) {
            if (data && data.length) {
                _this.Load(data);
            }
        });
    };
    AppViewKoModel.prototype.Load = function (data) {
        //load model
        this.items.removeAll();
        for (var i = 0; i < data.length; i++) {
            var itemModel = data[i];
            var appItem = new AppStatusKoModel(this, itemModel);
            this.items.push(appItem);
        }
    };
    AppViewKoModel.prototype.UpdateItem = function (model) {
        if (model && model.AppID) {
            //find item
            var match = ko.utils.arrayFirst(this.items(), function (item) {
                return model.AppID === item.AppID;
            });
            if (match) {
                match.update(model);
            }
        }
    };
    AppViewKoModel.prototype.CreateApp = function () {
        var _this = this;
        var section = $("#app-create-section");
        var url = section.data("url");
        $("#app-create-section").empty();
        $("#app-create-section").load(url, function () {
            var dlg = $("#app-create-modal");
            dlg.modal({ show: true });
            var form = $("#app-create-form");
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
    return AppViewKoModel;
})();
var AppStatusKoModel = (function () {
    function AppStatusKoModel(app, model) {
        var _this = this;
        this.App = app;
        this.AppID = model.AppID;
        this.Name = model.Name;
        this.MachineName = model.MachineName;
        this.Description = model.Description;
        this.LastAppStatusText = ko.observable(model.LastAppStatusText);
        this.LastEventTime = ko.observable(model.LastEventTime);
        this.LastAppStatus = ko.observable(model.LastAppStatus);
        this.LastEventValue = ko.observable(model.LastEventValue);
        this.AppEvents = ko.observableArray([]);
        this.Loading = false;
        var self = this;
        this.AppTitle = ko.computed(function () {
            if (self.MachineName && self.MachineName.length > 0) {
                return self.MachineName + " - " + self.Name;
            }
            return self.Name;
        });
        this.AppStatusText = ko.computed(function () {
            var timeAsDate = new Date(_this.LastEventTime());
            var timeago = $.timeago(timeAsDate);
            if (_this.LastEventValue()) {
                return _this.LastAppStatusText() + " - " + timeago + " - " + _this.LastEventValue();
            }
            else {
                return _this.LastAppStatusText() + " - " + timeago;
            }
        }, this);
        this.AppStatusClass = ko.computed(function () {
            var statusClass = "app-status-error";
            switch (_this.LastAppStatus()) {
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
    AppStatusKoModel.prototype.update = function (model) {
        this.LastAppStatusText(model.LastAppStatusText);
        this.LastEventTime(model.LastEventTime);
        this.LastAppStatus(model.LastAppStatus);
        this.LastEventValue(model.LastEventValue);
        //add to graph if visible
        var koModel = $("#app-drilldown-section").data("AppStatusKoModel");
        if (koModel && koModel === this) {
            this.loadDrillDownContent();
        }
    };
    AppStatusKoModel.prototype.drilldown = function () {
        var _this = this;
        if (!this.Loading) {
            this.Loading = true;
            //setup dlg
            var drillDownSection = $("#app-drilldown-section");
            var url = drillDownSection.data("url");
            var data = { id: this.AppID };
            $("#app-drilldown-section").empty();
            $("#app-drilldown-section").load(url, data, function () {
                _this.Loading = false;
                $("#app-drilldown-section").data("AppStatusKoModel", _this);
                $("#app-drilldown-chart").hide();
                var dlg = $("#app-drilldown-modal");
                dlg.modal({ show: true });
                //hook up form to get status of specific hook (first selected).
                $("#app-drilldown-eventHook").change(function () {
                    //load
                    _this.loadDrillDownContent();
                });
                _this.loadDrillDownContent();
            });
        }
    };
    AppStatusKoModel.prototype.loadDrillDownContent = function () {
        var _this = this;
        var selectedEventHook = $("#app-drilldown-eventHook").val();
        var form = $("#app-drilldown-form");
        var url = form.attr("action") + "/" + selectedEventHook;
        //get data.
        //$("#app-drilldown-chart").hide();
        //$("#app-drilldown-chart").empty();
        $.getJSON(url, function (outdata, textStatus, jqXHR) {
            if (outdata.Events && outdata.Events.length) {
                _this.loadChart(outdata.Events, outdata.MinValue, outdata.MaxValue);
                _this.loadTable(outdata);
            }
        });
    };
    AppStatusKoModel.prototype.loadChart = function (events, min, max) {
        if (!events || !events.length || events.length == 0)
            return;
        this.CurrentChartData = [];
        var appEvents = [];
        for (var i = 0; i < events.length; i++) {
            if (events[i].Value != null) {
                var time = new Date(events[i].EventTime).getTime();
                var value = events[i].Value;
                var row = [time, value];
                this.CurrentChartData.push(row);
            }
            appEvents.push(new AppEventKoModel(events[i]));
        }
        this.AppEvents(appEvents);
        if (this.CurrentChartData.length > 0) {
            var placeholder = $("#app-drilldown-chart");
            var series = [{
                data: this.CurrentChartData,
                lines: {
                    fill: true
                }
            }];
            this.Plot = $.plot(placeholder, series, {
                xaxis: {
                    mode: "time"
                },
                yaxis: {
                    min: min,
                    max: max
                },
                legend: {
                    show: true
                }
            });
            //tabulate data.
            $("#app-drilldown-chart").show();
        }
    };
    AppStatusKoModel.prototype.loadTable = function (data) {
        //set AppEvents
        var container = document.getElementById("app-drilldown-table");
        ko.cleanNode(container);
        ko.applyBindings(this, container);
    };
    return AppStatusKoModel;
})();
var AppEventKoModel = (function () {
    function AppEventKoModel(appEvent) {
        var _this = this;
        this.AppEventID = appEvent.AppEventID;
        this.Message = appEvent.Message;
        var eventTimeDate = new Date(appEvent.EventTime);
        this.EventTime = moment(eventTimeDate).format("DD/MM/YYYY hh:mm:ss");
        this.AppStatus = appEvent.AppStatus;
        this.Value = appEvent.Value;
        this.AppStatusText = ko.computed(function () {
            switch (_this.AppStatus) {
                case 0: return "None";
                case 1: return "Fast";
                case 2: return "Normal";
                case 3: return "Slow";
                case 4: return "Running";
                default: return "Error";
            }
        }, this);
    }
    return AppEventKoModel;
})();
$(function () {
    var appModel = new AppViewKoModel();
    appModel.Load(rawModel);
    ko.applyBindings(appModel);
    var hub = $.connection.appEventHub;
    hub.client.updateAppStatus = function (model) {
        console.log(model);
        appModel.UpdateItem(model);
    };
    $.connection.hub.logging = true;
    $.connection.hub.start().done(function (e) {
        console.log(e);
    });
});
//# sourceMappingURL=Index.js.map