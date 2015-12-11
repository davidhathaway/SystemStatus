/// <reference path="../../typings/knockout/knockout.d.ts" />
/// <reference path="../../typings/jquery/jquery.d.ts" />
/// <reference path="../../typings/jquery.timeago/jquery.timeago.d.ts" />
/// <reference path="../../typings/signalr/signalr.d.ts" />
/// <reference path="../../typings/bootstrap/bootstrap.d.ts" />
/// <reference path="../../typings/flot/jquery.flot.d.ts" />
/// <reference path="../../typings/moment/moment.d.ts" />


declare var rawSystemStatusModel: any;

interface SystemStatusViewModel
{
    SystemGroupID: number;
    Name: string;
    AppStatuses: any;
    DrillDownUrl: string;
};

class SystemAppViewKoModel
{
    public items: KnockoutObservableArray<SystemStatusKoModel>;
    constructor() {
        this.items = ko.observableArray([]);
    }

  

    RefreshAll()
    {
        var url = <any>$(".app-list").data("refreshall");

        $.getJSON(url,(data, textStatus, jqXHR) => {
            if (data && data.length)
            {                
                this.Load(data);
            }
        }); 
    }

    Load(data: Array<SystemStatusViewModel>)
    {
        //load model
        this.items.removeAll();
        for (var i = 0; i < data.length; i++)
        {
            var itemModel = data[i];
            var appItem = new SystemStatusKoModel(this, itemModel);
            this.items.push(appItem);
        }
    }

    UpdateItem(model)
    {
        if (model && model.SystemID)
        {
            //find item
            var match = ko.utils.arrayFirst(this.items(), function (item) {
                return model.SystemGroupID === item.SystemGroupID;
            });

            if (match)
            {
               match.update(model);
            }
        }
    }

    CreateSystem()
    {
        var section = $("#system-create-section");

        var url = section.data("url");

        $("#system-create-section").empty();

        $("#system-create-section").load(url,() =>
        {
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
                }).done((data) =>
                {
                    if (data.Success)
                    {
                        dlg.modal("hide");
                        //get new app status view model and add it.
                        this.RefreshAll();
                    }
                    else {
                        //set errors
                        if (data.Errors && data.Errros.length) {
                            for (var e = 0; e < data.Errors.length; e++)
                            {
                                
                            }
                        }
                    }
                }); 
                      
            });
        });
    }

}

class SystemStatusKoModel
{
    public App: SystemAppViewKoModel;
    public SystemGroupID: number;
    public Name: string;
    public DrillDownUrl: string;

  

    public LastAppStatuses: KnockoutObservable<any>;
    public AppStatusClass: KnockoutComputed<string>;
    public AppStatusTextClass: KnockoutComputed<string>;

    constructor(app: SystemAppViewKoModel, model: SystemStatusViewModel)
    {
        this.App = app;
        this.SystemGroupID = model.SystemGroupID;
        this.Name = model.Name;
        this.DrillDownUrl = model.DrillDownUrl;

        this.LastAppStatuses = ko.observable(model.AppStatuses);

        this.AppStatusClass = ko.computed(() =>
        {
            var systemStatus = 0;

            var statuses = this.LastAppStatuses();
            if ($.isArray(statuses))
            {
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

        this.AppStatusTextClass = ko.computed(() => {
            return this.AppStatusClass() + "-text";

        }, this);

    }

    update(model: SystemStatusViewModel)
    {
      
    }

    ViewGroup()
    {
        //open group in new window?
        window.open(this.DrillDownUrl);
    }

}

$(function () {

    var appModel = new SystemAppViewKoModel();

    appModel.Load(rawSystemStatusModel);

    ko.applyBindings(appModel);

    var hub = $.connection.appEventHub;

    hub.client.updateAppStatus = function (model)
    {
        console.log(model);
        appModel.UpdateItem(model);
    };

    $.connection.hub.start().done(function (e)
    {
        console.log(e);

    });

});