﻿interface SignalR {
    appEventHub: any;
};

interface SystemGroupViewModel {
    SystemGroupID: number;
    Name: string;
    ParentID?: number;
    Apps: AppStatusViewModel[];
    Children: SystemStatusViewModel[];
    Parents: ParentViewModel[];
}

interface SubSystemViewModel
{
     ID :number;
     Text: string;
     AppStatus: number;
     IsSystem: boolean;
     DrillDownUrl: string;
     IsSystemCritical: boolean;
}

interface SystemStatusViewModel {
    SystemGroupID: number;
    Name: string;
    SubSystems: SubSystemViewModel[];
    DrillDownUrl: string;
    EventTime?: any;
    IsDown: boolean;
};

interface ParentViewModel
{
    ID : number;
    Name: string;
}

interface AppStatusViewModel {
    SystemGroupID: number;
    AppID: number;
    Name: string;
    Description: string;
    AgentName: string;
    LastAppStatusText: string;
    LastEventTime: string;
    LastAppStatus: number;
    LastEventValue: number;
};

interface AppEventViewModel {
    AppEventID: number;
    AppID: number;
    EventTime: any;
    Message: string;
    AppStatus: number;
    Value: number;
}

class AppViewKoModel {

    public Model: SystemGroupViewModel;
    public Apps: KnockoutObservableArray<AppStatusKoModel>;
    public Systems: KnockoutObservableArray<SystemStatusKoModel>;
    public Name: KnockoutObservable<string>;

    constructor() {
        this.Apps = ko.observableArray([]);
        this.Systems = ko.observableArray([]);
        this.Name = ko.observable(null);
    }

    EditSystem() {
        var section = $("#system-edit-section");

        var url = section.data("url");

        section.empty();

        section.load(url, () => {
            var dlg = $("#system-edit-modal");
            dlg.modal({ show: true });
            var form = $("#system-edit-form");
            //hook save
            $(".create-dialog-save").click(() => {
                //submit form  
                $.ajax({
                    url: form.attr("action"),
                    type: "POST",
                    data: form.serialize()
                }).done((data) => {
                    if (data.Success) {
                        dlg.modal("hide");
                        //get new app status view model and add it.
                        this.RefreshAll();
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
    }

    RefreshAll()
    {
        var url = <any>$(".app-list").data("refreshall");

        $.getJSON(url, (data, textStatus, jqXHR) => {
            if (data) {
                this.Load(data);
            }
        });
    }

    Load(data: SystemGroupViewModel)
    {
        this.Model = data;

        this.Name(data.Name);

        //load model
        this.Apps.removeAll();

        if (data.Apps) {
            for (var i = 0; i < data.Apps.length; i++) {
                var itemModel = data.Apps[i];
                var appItem = new AppStatusKoModel(this, itemModel);
                this.Apps.push(appItem);
            }
        }

        this.Systems.removeAll();

        if (data.Children) {
            for (var i = 0; i < data.Children.length; i++) {
                var sysModel = data.Children[i];
                var sysItem = new SystemStatusKoModel(this, sysModel);
                this.Systems.push(sysItem);
            }
        }
    }

    UpdateItem(model) {
        if (model && model.AppID) {
            //find item
            var match = ko.utils.arrayFirst(this.Apps(), function (item) {
                return model.AppID === item.AppID;
            });

            if (match) {
                match.update(model);
            }

            var systems = this.Systems();
            var appFoundInSubSystems = false;
            var appInSubSystems = null;
            for (var sys = 0; sys < systems.length; sys++)
            {
                var subsystems = systems[sys].SubSystems();
                for (var subsys = 0; subsys < subsystems.length; subsys++)
                {
                    var subSystem = subsystems[subsys];
                    var id = subSystem.ID();
                    var isApp = !subSystem.IsSystem();
                    if (isApp && id == model.AppID)
                    {
                        appFoundInSubSystems = true;
                        appInSubSystems = subSystem;
                        break;
                    }
                }

                if (appFoundInSubSystems)
                {
                    break;
                }
            }

            if (appFoundInSubSystems)
            {
                appInSubSystems.update(model);//will update graph
            }
        }
    }

    CreateSubSystem() {
        var section = $("#system-create-section");

        var url = section.data("url");

        $("#system-create-section").empty();

        $("#system-create-section").load(url, () => {
            var dlg = $("#system-create-modal");
            dlg.modal({ show: true });
            var form = $("#system-create-form");
            //hook save
            $(".create-dialog-save").click(() => {
                //submit form  
                $.ajax({
                    url: form.attr("action"),
                    type: "POST",
                    data: form.serialize()
                }).done((data) => {
                    if (data.Success) {
                        dlg.modal("hide");
                        //get new app status view model and add it.
                        this.RefreshAll();
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
    }

    CreateApp() {
        var section = $("#app-create-section");

        var url = section.data("url");

        $("#app-create-section").empty();

        $("#app-create-section").load(url, () => {
            var dlg = $("#app-create-modal");
            dlg.modal({ show: true });
            var form = $("#app-create-form");
            //hook save
            $(".create-dialog-save").click(() => {
                //submit form  
                $.ajax({
                    url: form.attr("action"),
                    type: "POST",
                    data: form.serialize()
                }).done((data) => {
                    if (data.Success) {
                        dlg.modal("hide");
                        //get new app status view model and add it.
                        this.RefreshAll();
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
    }

}

class AppStatusKoModel {
    public App: AppViewKoModel;
    public AppID: number;
    public AgentName: string;
    public Name: string;
    public Description: string;
    public LastAppStatusText: KnockoutObservable<string>;
    public LastEventTime: KnockoutObservable<string>;
    public LastAppStatus: KnockoutObservable<number>;
    public LastEventValue: KnockoutObservable<number>;
    public AppStatusText: KnockoutComputed<string>;
    public AppStatusClass: KnockoutComputed<string>;
    public AppStatusTextClass: KnockoutComputed<string>;
    public AppEvents: KnockoutObservableArray<AppEventKoModel>;
    public AppTitle: KnockoutComputed<string>;
    public Plot: any;
    public CurrentChartData: any;
    public Loading: boolean;

    constructor(app: AppViewKoModel, model: AppStatusViewModel) {
        this.App = app;
        this.AppID = model.AppID;
        this.Name = model.Name;
        this.AgentName = model.AgentName;
        this.Description = model.Description;
        this.LastAppStatusText = ko.observable(model.LastAppStatusText);
        this.LastEventTime = ko.observable(model.LastEventTime);
        this.LastAppStatus = ko.observable(model.LastAppStatus);
        this.LastEventValue = ko.observable(model.LastEventValue);
        this.AppEvents = ko.observableArray([]);
        this.Loading = false;
        var self = this;
        this.AppTitle = ko.computed(() => {
            if (self.AgentName && self.AgentName.length > 0) {
                return self.AgentName + " - " + self.Name;
            }
            return self.Name;
        });

        this.AppStatusText = ko.computed(() => {
            var timeAsDate = new Date(this.LastEventTime());
            var timeago = $.timeago(timeAsDate);

            if (this.LastEventValue()) {
                return this.LastAppStatusText() + " - " + timeago + " - " + this.LastEventValue();
            }
            else {
                return this.LastAppStatusText() + " - " + timeago;
            }

        }, this);

        this.AppStatusClass = ko.computed(() => {
            var statusClass = "app-status-none";
            switch (this.LastAppStatus()) {
                case 0:
                    statusClass = "app-status-error";
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

        this.AppStatusTextClass = ko.computed(() => {
            return this.AppStatusClass() + "-text";

        }, this);
    }

    edit()
    {
        var section = $("#app-edit-section");

        var url = section.data("url") + "/" + this.AppID;

        section.empty();

        var self = this;

        section.load(url, () => {
            var dlg = $("#app-edit-modal");
            dlg.modal({ show: true });
            var form = $("#app-edit-form");
            //hook save
            $(".create-dialog-save").click(() => {
                //submit form  
                $.ajax({
                    url: form.attr("action"),
                    type: "POST",
                    data: form.serialize()
                }).done((data) => {
                    if (data.Success) {
                        dlg.modal("hide");
                        //get new app status view model and add it.
                        self.App.RefreshAll();
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
    }

    getAppModel(): any {
        return $("#app-drilldown-section").data("AppStatusKoModel");
    }

    update(model: AppStatusViewModel) {
        this.LastAppStatusText(model.LastAppStatusText);
        this.LastEventTime(model.LastEventTime);
        this.LastAppStatus(model.LastAppStatus);
        this.LastEventValue(model.LastEventValue);

        //add to graph if visible
        var koModel = this.getAppModel();

        if (koModel && koModel === this) {
            this.loadDrillDownContent();
        }
    }

    drilldown() {

        var self = this;

        if (!this.Loading) {
            this.Loading = true;
            //setup dlg
            var drillDownSection = $("#app-drilldown-section");
            var url = <any>drillDownSection.data("url");
            var data = { id: self.AppID };

            $("#app-drilldown-section").empty();
            $("#app-drilldown-section").load(url, data, () => {
                this.Loading = false;
                $("#app-drilldown-section").data("AppStatusKoModel", self);
                $("#app-drilldown-chart").hide();
                var dlg = $("#app-drilldown-modal");
                dlg.modal({ show: true });
                self.loadDrillDownContent();
            });

        }
    }

    loadDrillDownContent() {
      
        var form = $("#app-drilldown-form");
        var url = form.attr("action");
       
        //get data.
        //$("#app-drilldown-chart").hide();
        //$("#app-drilldown-chart").empty();
        $.getJSON(url, (outdata, textStatus, jqXHR) => {
            if (outdata.Events && outdata.Events.length) {
                this.loadChart(outdata.Events, outdata.MinValue, outdata.MaxValue);
                this.loadTable(outdata);
            }
        });
    }

    loadChart(events, min: number, max: number) {
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

            this.Plot = $.plot(placeholder, series,
                {
                    xaxis:
                    {
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
    }

    loadTable(data) {
        //set AppEvents
        var container = document.getElementById("app-drilldown-table");
        ko.cleanNode(container);
        ko.applyBindings(this, container);
    }
}

class AppEventKoModel {
    public AppEventID: number;
    public Message: string;
    public EventTime: string;
    public AppStatus: number;
    public Value: number;
    public AppStatusText: KnockoutComputed<string>;
    constructor(appEvent: AppEventViewModel) {
        this.AppEventID = appEvent.AppEventID;
        this.Message = appEvent.Message;


        var eventTimeDate = new Date(appEvent.EventTime);
        this.EventTime = moment(eventTimeDate).format("DD/MM/YYYY hh:mm:ss");

        this.AppStatus = appEvent.AppStatus;
        this.Value = appEvent.Value;

        this.AppStatusText = ko.computed(() => {
            switch (this.AppStatus) {
                case 0: return "Error";
                case 1: return "Fast";
                case 2: return "Normal";
                case 3: return "Slow";
                case 4: return "Running";
                default: return "None"
            }
        }, this);

    }
}

class SystemAppViewKoModel {
    public Systems: KnockoutObservableArray<SystemStatusKoModel>;
    constructor() {
        this.Systems = ko.observableArray([]);
    }
    
    EditSystem()
    {
        var section = $("#system-edit-section");

        var url = section.data("url");

        section.empty();

        section.load(url, () => {
            var dlg = $("#system-edit-modal");
            dlg.modal({ show: true });
            var form = $("#system-edit-form");
            //hook save
            $(".create-dialog-save").click(() => {
                //submit form  
                $.ajax({
                    url: form.attr("action"),
                    type: "POST",
                    data: form.serialize()
                }).done((data) => {
                    if (data.Success) {
                        dlg.modal("hide");
                        //get new app status view model and add it.
                        this.RefreshAll();
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
    }

    RefreshAll() {
        var url = <any>$(".app-list").data("refreshall");

        $.getJSON(url, (data, textStatus, jqXHR) => {
            if (data && data.length) {
                this.Load(data);
            }
        });
    }

    Load(data: Array<SystemStatusViewModel>) {
        //load model
        this.Systems.removeAll();
        for (var i = 0; i < data.length; i++) {
            var itemModel = data[i];
            var appItem = new SystemStatusKoModel(this, itemModel);
            this.Systems.push(appItem);
        }
    }

    UpdateItem(model) {
        if (model && model.SystemID) {
            //find item
            var match = ko.utils.arrayFirst(this.Systems(), function (item) {
                return model.SystemGroupID === item.SystemGroupID;
            });

            if (match) {
                match.update(model);
            }
        }
    }

    CreateSystem() {
        var section = $("#system-create-section");

        var url = section.data("url");

        section.empty();

        section.load(url, () => {
            var dlg = $("#system-create-modal");
            dlg.modal({ show: true });
            var form = $("#system-create-form");
            //hook save
            $(".create-dialog-save").click(() => {
                //submit form  
                $.ajax({
                    url: form.attr("action"),
                    type: "POST",
                    data: form.serialize()
                }).done((data) => {
                    if (data.Success) {
                        dlg.modal("hide");
                        //get new app status view model and add it.
                        this.RefreshAll();
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
    }

}

class SystemStatusKoModel {
    public App: any;

    public SystemGroupID: number;

    public Name: string;

    public DrillDownUrl: string;

    public EventTime: KnockoutObservable<any>;

    public IsDown: KnockoutObservable<boolean>;

    public SubSystems: KnockoutObservableArray<SubSystemKoModel>;

    public StatusClass: KnockoutComputed<string>;

    public StatusTextClass: KnockoutComputed<string>;

    constructor(app: any, model: SystemStatusViewModel) {
        this.App = app;
        this.SystemGroupID = model.SystemGroupID;
        this.Name = model.Name;
        this.DrillDownUrl = model.DrillDownUrl;



        this.SubSystems = ko.observableArray([]);

        for (var i = 0; i < model.SubSystems.length; i++) {
            var itemModel = model.SubSystems[i];
            var appItem = new SubSystemKoModel(this, itemModel);
            this.SubSystems.push(appItem);
        }

       

        this.EventTime = ko.observable(model.EventTime);

        this.IsDown = ko.observable(model.IsDown);

        this.StatusClass = ko.computed(() =>
        {
            var statusClass = "app-status-error";

            if (!this.IsDown())
            {
                statusClass = "app-status-fast";
            }
  
            return statusClass;
        }, this);

        this.StatusTextClass = ko.computed(() => {
            return this.StatusClass() + "-text";

        }, this);

    }

    update(model: SystemStatusViewModel)
    {
        this.SubSystems.removeAll();

        for (var i = 0; i < model.SubSystems.length; i++) {
            var itemModel = model.SubSystems[i];
            var appItem = new SubSystemKoModel(this, itemModel);
            this.SubSystems.push(appItem);
        }

        this.IsDown(model.IsDown);
        this.EventTime(model.EventTime);
    }

    ViewGroup() {
        //open group in new window?
        window.open(this.DrillDownUrl);
    }

}

class SubSystemKoModel
{
    public IsSystemCritical: KnockoutObservable<boolean>;
    public ID: KnockoutObservable<number>;
    public IsSystem: KnockoutObservable<boolean>;
    public AppStatus: KnockoutObservable<number>;
    public DrillDownUrl: KnockoutObservable<string>;
    public Text: KnockoutObservable<string>;
    public StatusClass: KnockoutComputed<string>;
    public Plot: any;
    public CurrentChartData: any;
    public Loading: boolean;
    public AppEvents: KnockoutObservableArray<AppEventKoModel>;

    constructor(app: SystemStatusKoModel, model: SubSystemViewModel)
    {
        this.Loading = false;

        this.ID = ko.observable(model.ID);

        this.IsSystem = ko.observable(model.IsSystem);

        this.AppStatus = ko.observable(model.AppStatus);

        this.DrillDownUrl = ko.observable(model.DrillDownUrl);

        this.Text = ko.observable(model.Text);

        this.AppEvents = ko.observableArray([]);

        this.IsSystemCritical = ko.observable(model.IsSystemCritical);

        this.StatusClass = ko.computed(() =>
        {
            var statusClass = "app-status-none";

            switch (this.AppStatus()) {
                case 0:
                    statusClass = "app-status-error";
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
   
    }

    getAppModel(): any {
        return $("#app-drilldown-section").data("AppStatusKoModel");
    }

    update(model: AppStatusViewModel)
    {
        //update app status
        this.AppStatus(model.LastAppStatus); 

        //update graph if visible
        var koModel = this.getAppModel();

        if (koModel && koModel === this) {
            this.loadDrillDownContent();
        }
    }

    drilldown() {

        var self = this;

        if (!this.Loading) {
            this.Loading = true;
            //setup dlg
            var drillDownSection = $("#app-drilldown-section");
            var url = <any>drillDownSection.data("url");
            var data = { id: self.ID };

            $("#app-drilldown-section").empty();
            $("#app-drilldown-section").load(url, data, () => {
                this.Loading = false;
                $("#app-drilldown-section").data("AppStatusKoModel", self);
                $("#app-drilldown-chart").hide();
                var dlg = $("#app-drilldown-modal");
                dlg.modal({ show: true });
                self.loadDrillDownContent();
            });

        }
    }

    loadDrillDownContent() {

        var form = $("#app-drilldown-form");
        var url = form.attr("action");
       
        //get data.
        //$("#app-drilldown-chart").hide();
        //$("#app-drilldown-chart").empty();
        $.getJSON(url, (outdata, textStatus, jqXHR) => {
            if (outdata.Events && outdata.Events.length) {
                this.loadChart(outdata.Events, outdata.MinValue, outdata.MaxValue);
                this.loadTable(outdata);
            }
        });
    }

    loadChart(events, min: number, max: number) {
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

            this.Plot = $.plot(placeholder, series,
                {
                    xaxis:
                    {
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
    }

    loadTable(data) {
        //set AppEvents
        var container = document.getElementById("app-drilldown-table");
        ko.cleanNode(container);
        ko.applyBindings(this, container);
    }
}